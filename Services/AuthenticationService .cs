using PDV.Services.Interfaces;
using PDV.Services.Retaguarda;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace PDV.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
        private readonly HttpClient _httpClient;
        private RetaguardaAuthSession? _currentSession;

        public event EventHandler? CurrentSessionChanged;

        public RetaguardaAuthSession? CurrentSession
        {
            get => _currentSession;
            private set
            {
                _currentSession = value;
                CurrentSessionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public AuthenticationService()
        {
            var baseUrl = RetaguardaEndpointResolver.ObterBaseUrl();

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/"),
                Timeout = TimeSpan.FromSeconds(15)
            };
        }

        public async Task<RetaguardaAuthResponse?> CreateAccountAsync(RetaguardaCreateAccountRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/criar-conta", request, JsonOptions).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                string mensagem;
                try
                {
                    using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                    mensagem = doc.RootElement.TryGetProperty("Message", out var m)
                        ? m.GetString() ?? "Erro desconhecido"
                        : "Erro desconhecido";
                }
                catch
                {
                    mensagem = $"Erro {(int)response.StatusCode}";
                }
                throw new InvalidOperationException(mensagem);
            }

            var auth = await response.Content.ReadFromJsonAsync<RetaguardaAuthResponse>(JsonOptions).ConfigureAwait(false);
            if (auth != null)
            {
                if (!string.IsNullOrWhiteSpace(auth.Token))
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token);

                CurrentSession = new RetaguardaAuthSession
                {
                    Token = auth.Token ?? string.Empty,
                    ExpiraEm = auth.ExpiraEm,
                    Usuario = auth.Usuario,
                    Empresa = auth.Empresa
                };
            }

            return auth;
        }

        public async Task<bool> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return false;

            var response = await _httpClient.PostAsJsonAsync("auth/login", new RetaguardaLoginRequest
            {
                Email = email.Trim(),
                Senha = password
            }, JsonOptions).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                return false;

            var auth = await response.Content.ReadFromJsonAsync<RetaguardaAuthResponse>(JsonOptions).ConfigureAwait(false);
            if (auth == null)
                return false;

            if (string.IsNullOrWhiteSpace(auth.Token))
            {
                CurrentSession = new RetaguardaAuthSession
                {
                    Token = string.Empty,
                    ExpiraEm = auth.ExpiraEm,
                    Usuario = auth.Usuario,
                    Empresa = auth.Empresa
                };

                return true;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token);
            var meResponse = await _httpClient.GetAsync("auth/me").ConfigureAwait(false);
            if (!meResponse.IsSuccessStatusCode)
                return false;

            CurrentSession = new RetaguardaAuthSession
            {
                Token = auth.Token,
                ExpiraEm = auth.ExpiraEm,
                Usuario = auth.Usuario,
                Empresa = auth.Empresa
            };

            return true;
        }

        public async Task<RetaguardaPasswordRecoveryResponse> RecoverPasswordAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new RetaguardaPasswordRecoveryResponse
                {
                    Sucesso = false,
                    Mensagem = "Informe o e-mail cadastrado."
                };
            }

            var response = await _httpClient.PostAsJsonAsync("auth/recuperar-senha", new RetaguardaPasswordRecoveryRequest
            {
                Email = email.Trim()
            }, JsonOptions).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                return new RetaguardaPasswordRecoveryResponse
                {
                    Sucesso = false,
                    Mensagem = "Nao foi possivel solicitar a recuperacao de senha na retaguarda."
                };
            }

            return await response.Content.ReadFromJsonAsync<RetaguardaPasswordRecoveryResponse>(JsonOptions).ConfigureAwait(false)
                ?? new RetaguardaPasswordRecoveryResponse
                {
                    Sucesso = false,
                    Mensagem = "A retaguarda retornou uma resposta vazia."
                };
        }

        public async Task<bool> SendRegistrationEmailAsync(string cnpj, string login)
        {
            var normalizedCnpj = SomenteDigitos(cnpj);
            if (normalizedCnpj.Length != 14)
                return false;

            var response = await _httpClient.PostAsJsonAsync("empresa/envia-email-confirmacao", new RetaguardaCnpjRequest
            {
                Cnpj = normalizedCnpj,
                Login = login?.Trim() ?? string.Empty
            }, JsonOptions).ConfigureAwait(false);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ConfirmRegistrationCodeAsync(string cnpj, string login, string confirmationCode)
        {
            var normalizedCnpj = SomenteDigitos(cnpj);
            var code = confirmationCode?.Trim() ?? string.Empty;
            if (normalizedCnpj.Length != 14 || code.Length != 32)
                return false;

            using var request = new HttpRequestMessage(HttpMethod.Post, "empresa/confere-codigo-confirmacao")
            {
                Content = JsonContent.Create(new RetaguardaCnpjRequest
                {
                    Cnpj = normalizedCnpj,
                    Login = login?.Trim() ?? string.Empty
                }, options: JsonOptions)
            };
            request.Headers.TryAddWithoutValidation("codigo-confirmacao", code);

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                return false;

            var empresaConfirmada = await response.Content.ReadFromJsonAsync<RetaguardaEmpresaResponse>(JsonOptions).ConfigureAwait(false);
            AtualizarSessaoConfirmada(normalizedCnpj, login?.Trim() ?? string.Empty, empresaConfirmada);
            return true;
        }

        private void AtualizarSessaoConfirmada(string cnpj, string login, RetaguardaEmpresaResponse? empresaConfirmada)
        {
            if (CurrentSession == null)
                return;

            if (!string.Equals(SomenteDigitos(CurrentSession.Empresa.Cnpj), cnpj, StringComparison.Ordinal))
                return;

            if (!string.Equals(CurrentSession.Usuario.Login, login?.Trim() ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                return;

            CurrentSession = new RetaguardaAuthSession
            {
                Token = CurrentSession.Token,
                ExpiraEm = CurrentSession.ExpiraEm,
                Usuario = new RetaguardaUsuarioResponse
                {
                    Id = CurrentSession.Usuario.Id,
                    Nome = CurrentSession.Usuario.Nome,
                    Login = CurrentSession.Usuario.Login,
                    Perfil = CurrentSession.Usuario.Perfil,
                    Email = CurrentSession.Usuario.Email,
                    Confirmado = "S"
                },
                Empresa = new RetaguardaEmpresaResponse
                {
                    Id = empresaConfirmada?.Id > 0 ? empresaConfirmada.Id : CurrentSession.Empresa.Id,
                    Cnpj = empresaConfirmada?.Cnpj ?? CurrentSession.Empresa.Cnpj,
                    RazaoSocial = empresaConfirmada?.RazaoSocial ?? CurrentSession.Empresa.RazaoSocial,
                    NomeFantasia = empresaConfirmada?.NomeFantasia ?? CurrentSession.Empresa.NomeFantasia,
                    BancoOperacional = CurrentSession.Empresa.BancoOperacional,
                    Registrado = string.IsNullOrWhiteSpace(empresaConfirmada?.Registrado) ? "S" : empresaConfirmada.Registrado
                }
            };
        }

        private static string SomenteDigitos(string value)
        {
            return new string((value ?? string.Empty).Where(char.IsDigit).ToArray());
        }
    }
}
