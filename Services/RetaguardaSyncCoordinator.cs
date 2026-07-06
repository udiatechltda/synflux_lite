using Microsoft.Extensions.DependencyInjection;
using PDV.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace PDV.Services
{
    public sealed class RetaguardaSyncCoordinator : IRetaguardaSyncCoordinator, IDisposable
    {
        public event Action? OnRestoreCompleto;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILocalDatabaseService _databaseService;
        private readonly SemaphoreSlim _syncLock = new(1, 1);
        private readonly object _stateLock = new();
        private readonly string _syncStatePath;
        private readonly Dictionary<string, bool> _pendingByDatabase = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, long> _pendingVersionByDatabase = new(StringComparer.OrdinalIgnoreCase);

        private Timer? _timer;
        private bool _started;
        private bool _disposed;
        private bool _attemptScheduled;
        private bool _restoreConcluido;

        public RetaguardaSyncCoordinator(IServiceScopeFactory scopeFactory, ILocalDatabaseService databaseService)
        {
            _scopeFactory = scopeFactory;
            _databaseService = databaseService;

            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _syncStatePath = Path.Combine(appDataPath, "SynfluxPDV");
            Directory.CreateDirectory(_syncStatePath);
        }

        public bool SincronizacaoPendente
        {
            get
            {
                lock (_stateLock)
                    return EstaPendente(ObterChaveAtual());
            }
        }

        public void Start()
        {
            lock (_stateLock)
            {
                if (_started || _disposed)
                    return;

                _started = true;
            }

            NetworkChange.NetworkAvailabilityChanged += OnNetworkAvailabilityChanged;
            _timer = new Timer(_ =>
            {
                bool restorePendente;
                lock (_stateLock)
                    restorePendente = !_restoreConcluido;

                if (restorePendente)
                    _ = RestaurarDoServidorInternoAsync();
                else
                    _ = TentarSincronizarPendenteAsync("timer");
            }, null, TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(30));

            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(3)).ConfigureAwait(false);
                await RestaurarDoServidorInternoAsync().ConfigureAwait(false);
            });

            if (SincronizacaoPendente)
                AgendarTentativa("startup");
        }

        public void NotificarLoginRealizado()
        {
            lock (_stateLock)
                _restoreConcluido = false;

            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
                await RestaurarDoServidorInternoAsync().ConfigureAwait(false);
            });
        }

        public void SolicitarSincronizacao(string origem)
        {
            if (!EstaAtivo())
                return;

            MarcarPendente();
            AgendarTentativa(origem);
        }

        public Task<RetaguardaSyncResult> SincronizarAgoraAsync(string origem)
        {
            MarcarPendente();
            return TentarSincronizarPendenteAsync(origem);
        }

        private void OnNetworkAvailabilityChanged(object? sender, NetworkAvailabilityEventArgs e)
        {
            if (!e.IsAvailable)
                return;

            bool restorePendente;
            lock (_stateLock)
                restorePendente = !_restoreConcluido;

            if (restorePendente)
                _ = Task.Run(() => RestaurarDoServidorInternoAsync());

            AgendarTentativa("network-available");
        }

        private bool EstaAtivo()
        {
            lock (_stateLock)
                return _started && !_disposed;
        }

        private void MarcarPendente()
        {
            var chave = ObterChaveAtual();
            long versao;
            lock (_stateLock)
            {
                if (_disposed)
                    return;

                _pendingByDatabase[chave] = true;
                _pendingVersionByDatabase[chave] = _pendingVersionByDatabase.GetValueOrDefault(chave) + 1;
                versao = _pendingVersionByDatabase[chave];
            }

            try
            {
                File.WriteAllText(ObterArquivoPendente(chave), $"{DateTimeOffset.UtcNow:O}|{versao}");
            }
            catch
            {
                // O estado em memoria ainda garante retry durante a execucao atual.
            }
        }

        private void AgendarTentativa(string origem)
        {
            lock (_stateLock)
            {
                if (!_started || _disposed || _attemptScheduled)
                    return;

                _attemptScheduled = true;
            }

            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);

                lock (_stateLock)
                    _attemptScheduled = false;

                await TentarSincronizarPendenteAsync(origem).ConfigureAwait(false);
            });
        }

        private async Task RestaurarDoServidorInternoAsync()
        {
            if (!EstaAtivo())
                return;

            lock (_stateLock)
            {
                if (_restoreConcluido)
                    return;
            }

            if (!NetworkInterface.GetIsNetworkAvailable())
                return;

            if (!await _syncLock.WaitAsync(0).ConfigureAwait(false))
                return;

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var syncService = scope.ServiceProvider.GetRequiredService<IRetaguardaSyncService>();
                var resultado = await syncService.RestaurarDoServidorAsync().ConfigureAwait(false);

                if (resultado.Sincronizado)
                {
                    lock (_stateLock)
                        _restoreConcluido = true;

                    OnRestoreCompleto?.Invoke();
                }
            }
            catch
            {
                // falha no restore nao deve parar o app; sera tentado novamente no proximo tick
            }
            finally
            {
                _syncLock.Release();
            }
        }

        private async Task<RetaguardaSyncResult> TentarSincronizarPendenteAsync(string origem)
        {
            var chave = ObterChaveAtual();
            var versao = ObterVersaoPendente(chave);
            if (versao == 0)
                return SucessoSemPendencias();

            if (!NetworkInterface.GetIsNetworkAvailable())
                return Falha("Sem conexao de rede. Snapshot mantido pendente para sincronizar depois.");

            if (!await _syncLock.WaitAsync(0).ConfigureAwait(false))
                return Falha("Sincronizacao ja em andamento.");

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var syncService = scope.ServiceProvider.GetRequiredService<IRetaguardaSyncService>();
                var resultado = await syncService.SincronizarTudoAsync().ConfigureAwait(false);

                if (resultado.Sincronizado)
                    LimparPendenteSeVersaoNaoMudou(chave, versao);

                return resultado;
            }
            catch (Exception ex)
            {
                return Falha($"Falha ao sincronizar snapshot ({origem}): {ex.Message}");
            }
            finally
            {
                _syncLock.Release();
            }
        }

        private long ObterVersaoPendente(string chave)
        {
            lock (_stateLock)
            {
                if (_pendingByDatabase.GetValueOrDefault(chave))
                    return _pendingVersionByDatabase.GetValueOrDefault(chave);

                if (!File.Exists(ObterArquivoPendente(chave)))
                    return 0;

                _pendingByDatabase[chave] = true;
                _pendingVersionByDatabase[chave] = Math.Max(1, _pendingVersionByDatabase.GetValueOrDefault(chave));
                return _pendingVersionByDatabase[chave];
            }
        }

        private void LimparPendenteSeVersaoNaoMudou(string chave, long versaoSincronizada)
        {
            lock (_stateLock)
            {
                if (_pendingVersionByDatabase.GetValueOrDefault(chave) != versaoSincronizada)
                    return;

                _pendingByDatabase[chave] = false;
                _pendingVersionByDatabase[chave] = 0;
            }

            try
            {
                var arquivo = ObterArquivoPendente(chave);
                if (File.Exists(arquivo))
                    File.Delete(arquivo);
            }
            catch
            {
                // Na proxima inicializacao o timer tenta novamente. O snapshot e idempotente.
            }
        }

        private bool EstaPendente(string chave)
        {
            return _pendingByDatabase.GetValueOrDefault(chave) || File.Exists(ObterArquivoPendente(chave));
        }

        private string ObterChaveAtual()
        {
            var chave = _databaseService.CurrentDatabaseKey;
            return string.IsNullOrWhiteSpace(chave) ? "login" : SanitizarNomeArquivo(chave);
        }

        private string ObterArquivoPendente(string chave)
        {
            return Path.Combine(_syncStatePath, $"sync-pendente-{SanitizarNomeArquivo(chave)}.flag");
        }

        private static string SanitizarNomeArquivo(string value)
        {
            var sanitized = value ?? string.Empty;
            foreach (var invalid in Path.GetInvalidFileNameChars())
                sanitized = sanitized.Replace(invalid, '_');

            return string.IsNullOrWhiteSpace(sanitized) ? "login" : sanitized;
        }

        private static RetaguardaSyncResult SucessoSemPendencias()
        {
            return new RetaguardaSyncResult
            {
                Sincronizado = true,
                Mensagem = "Sem pendencias de sincronizacao."
            };
        }

        private static RetaguardaSyncResult Falha(string mensagem)
        {
            return new RetaguardaSyncResult
            {
                Sincronizado = false,
                Mensagem = mensagem
            };
        }

        public void Dispose()
        {
            lock (_stateLock)
            {
                if (_disposed)
                    return;

                _disposed = true;
            }

            NetworkChange.NetworkAvailabilityChanged -= OnNetworkAvailabilityChanged;
            _timer?.Dispose();
            _syncLock.Dispose();
        }
    }
}
