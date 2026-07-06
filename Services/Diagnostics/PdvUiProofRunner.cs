using Microsoft.Extensions.DependencyInjection;
using PDV.Services.Interfaces;
using PDV.Services.Retaguarda;
using PDV.ViewModels;
using PDV.ViewModels.Login;
using PDV.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace PDV.Services.Diagnostics
{
    public static class PdvUiProofRunner
    {
        private const string ConfirmationKey = "id#UAq2&[L5fri/GF1:2Vs5r|)z)ZU*F";

        public static int Run(IServiceProvider provider)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");

            var runRoot = PrepareRunRoot();
            var logPath = Path.Combine(runRoot, "prova-ui.log");

            void Log(string message) =>
                File.AppendAllText(logPath, $"{DateTime.Now:HH:mm:ss} {message}{Environment.NewLine}");

            try
            {
                var auth = provider.GetRequiredService<IAuthenticationService>();
                var alerts = provider.GetRequiredService<IAlertService>();
                var tenant = provider.GetRequiredService<ILocalTenantService>();
                var cashSessionState = provider.GetRequiredService<PdvCashSessionState>();

                var stamp = DateTime.Now.ToString("ddHHmmssff", CultureInfo.InvariantCulture);
                var cnpj = $"77{stamp}00";
                var login = "admin";
                var senha = "admin123";
                var usuario = $"{cnpj}|{login}";
                var codigo = CodigoUsuario(cnpj, login);

                Log($"Criando cadastro pendente: {usuario}");
                var conta = auth.CreateAccountAsync(new RetaguardaCreateAccountRequest
                {
                    Cnpj = cnpj,
                    RazaoSocial = "Empresa Prova PDV",
                    NomeFantasia = "Empresa Prova PDV",
                    Email = $"suporte+pdv{stamp}@techone-it.com.br",
                    UsuarioNome = "Administrador",
                    Login = login,
                    Senha = senha,
                    Perfil = "Administrador"
                }).GetAwaiter().GetResult();

                if (conta == null)
                    throw new InvalidOperationException("Nao foi possivel criar a conta pendente na retaguarda.");

                var mainWindow = new MainWindow(auth, alerts, tenant, cashSessionState, provider.GetRequiredService<IRetaguardaSyncCoordinator>())
                {
                    WindowState = WindowState.Maximized,
                    Topmost = true
                };

                mainWindow.Show();
                mainWindow.Activate();
                PreencherLogin(mainWindow, usuario, senha);

                using var recorder = ScreenRecorder.Start(runRoot, Log);
                recorder.Capture(mainWindow, "login-preenchido", 3);

                var pendingHandled = false;
                var codeHandled = false;
                var loginStarted = false;
                var loginCompleted = false;
                var sawFalsePendingAfterSuccess = false;

                alerts.Alerts.CollectionChanged += (_, args) =>
                {
                    if (args.NewItems == null)
                        return;

                    foreach (var item in args.NewItems.OfType<Models.AlertModel>())
                    {
                        Log($"ALERTA: {item.Type} - {item.Message}");
                        if (loginCompleted &&
                            item.Message.Contains("Cadastro ainda pendente", StringComparison.OrdinalIgnoreCase))
                        {
                            sawFalsePendingAfterSuccess = true;
                        }
                    }
                };

                var timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(400)
                };

                timer.Tick += (_, _) =>
                {
                    if (!loginStarted)
                        return;

                    var confirmacao = Application.Current.Windows
                        .OfType<ConfirmacaoRegistroView>()
                        .FirstOrDefault();

                    if (confirmacao?.DataContext is not ConfirmacaoRegistroViewModel vm)
                        return;

                    confirmacao.Activate();

                    if (!pendingHandled && !vm.PendenteDeConfirmacao && vm.IsNotLoading)
                    {
                        vm.AceitaTermos = true;
                        if (vm.ConfirmarTermosCommand.CanExecute(null))
                        {
                            recorder.Capture(confirmacao, "registro-termos", 3);
                            pendingHandled = true;
                            vm.ConfirmarTermosCommand.Execute(null);
                            Log("Termos aceitos na tela real de registro.");
                        }
                    }

                    if (pendingHandled && !codeHandled && vm.PendenteDeConfirmacao && vm.IsNotLoading)
                    {
                        vm.CodigoConfirmacao = codigo;
                        if (vm.ConfirmarCodigoCommand.CanExecute(null))
                        {
                            recorder.Capture(confirmacao, "registro-codigo", 3);
                            codeHandled = true;
                            vm.ConfirmarCodigoCommand.Execute(null);
                            Log("Codigo informado na tela real de registro.");
                        }
                    }
                };

                timer.Start();

                var loginViewModel = ObterLoginViewModel(mainWindow)
                    ?? throw new InvalidOperationException("Nao foi possivel localizar o LoginViewModel.");

                Task.Delay(1400).ContinueWith(_ =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        loginStarted = true;
                        Log("Executando login pelo botao/command real do PDV.");
                        loginViewModel.LoginCommand.Execute(null);
                    });
                });

                var deadline = DateTime.Now.AddSeconds(45);
                while (DateTime.Now < deadline)
                {
                    DoEvents();
                    if (ConteudoPrincipal(mainWindow) is MainView)
                    {
                        loginCompleted = true;
                        recorder.Capture(mainWindow, "pdv-principal", 5);
                        Log("PDV abriu tela principal apos confirmacao.");
                        break;
                    }

                    Thread.Sleep(80);
                }

                timer.Stop();
                DoEvents();
                recorder.Capture(mainWindow, "pdv-principal-final", 3);
                Thread.Sleep(2500);

                if (!loginCompleted)
                    throw new InvalidOperationException("O PDV nao abriu a tela principal dentro do tempo esperado.");

                if (sawFalsePendingAfterSuccess)
                    throw new InvalidOperationException("A mensagem falsa de cadastro pendente apareceu depois da confirmacao.");

                Log($"VIDEO={recorder.VideoPath}");
                Log("RESULTADO=APROVADO");
                mainWindow.Close();
                return 0;
            }
            catch (Exception ex)
            {
                Log($"ERRO={ex}");
                return 1;
            }
        }

        private static string PrepareRunRoot()
        {
            var repoRoot = FindRepoRoot();
            var stamp = DateTime.Now.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture);
            var runRoot = Path.Combine(repoRoot, "artifacts", "pdv-ui-proof", stamp);
            Directory.CreateDirectory(runRoot);
            return runRoot;
        }

        private static void PreencherLogin(MainWindow mainWindow, string usuario, string senha)
        {
            if (ConteudoPrincipal(mainWindow) is not LoginView loginView ||
                loginView.DataContext is not LoginViewModel vm)
            {
                return;
            }

            vm.Email = usuario;
            vm.Senha = senha;

            var usuarioInput = FindVisualChild<TextBox>(loginView, "UsuarioInput");
            if (usuarioInput != null)
                usuarioInput.Text = usuario;

            var passwordInput = FindVisualChild<PasswordBox>(loginView, "PasswordInput");
            if (passwordInput != null)
                passwordInput.Password = senha;
        }

        private static LoginViewModel? ObterLoginViewModel(MainWindow mainWindow)
        {
            return ConteudoPrincipal(mainWindow) is LoginView loginView
                ? loginView.DataContext as LoginViewModel
                : null;
        }

        private static object? ConteudoPrincipal(MainWindow mainWindow)
        {
            return (mainWindow.FindName("MainContent") as ContentControl)?.Content;
        }

        private static T? FindVisualChild<T>(DependencyObject parent, string name) where T : FrameworkElement
        {
            var children = System.Windows.Media.VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < children; i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(parent, i);
                if (child is T typed && typed.Name == name)
                    return typed;

                var nested = FindVisualChild<T>(child, name);
                if (nested != null)
                    return nested;
            }

            return null;
        }

        private static string CodigoUsuario(string cnpj, string login)
        {
            return Md5(cnpj + ":" + login.Trim().ToLowerInvariant() + ConfirmationKey);
        }

        private static string Md5(string value)
        {
            var bytes = Encoding.ASCII.GetBytes(value);
            var hash = MD5.HashData(bytes);
            var builder = new StringBuilder(hash.Length * 2);
            foreach (var b in hash)
                builder.Append(b.ToString("x2", CultureInfo.InvariantCulture));
            return builder.ToString();
        }

        private static void DoEvents()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new DispatcherOperationCallback(_ =>
                {
                    frame.Continue = false;
                    return null;
                }),
                null);
            Dispatcher.PushFrame(frame);
        }

        private static string FindRepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "PDV.csproj")))
                    return dir.FullName;
                dir = dir.Parent;
            }

            return AppContext.BaseDirectory;
        }

        private sealed class ScreenRecorder : IDisposable
        {
            private const int Width = 640;
            private const int Height = 360;
            private const int Fps = 4;
            private readonly AviWriter _writer;
            private readonly Action<string> _log;
            public string VideoPath { get; }

            private ScreenRecorder(AviWriter writer, string videoPath, Action<string> log)
            {
                _writer = writer;
                VideoPath = videoPath;
                _log = log;
            }

            public static ScreenRecorder Start(string runRoot, Action<string> log)
            {
                var videoPath = Path.Combine(runRoot, "pdv-funcionando.avi");
                var writer = new AviWriter(videoPath, Width, Height, Fps);
                log($"Gravacao WPF iniciada: {videoPath}");
                return new ScreenRecorder(writer, videoPath, log);
            }

            public void Capture(FrameworkElement source, string stage, int repeat)
            {
                try
                {
                    var frame = RenderFrame(source);
                    for (var i = 0; i < repeat; i++)
                        _writer.AddFrame(frame);
                    _log($"Frame gravado: {stage}");
                }
                catch (Exception ex)
                {
                    _log($"Erro ao gravar frame {stage}: {ex.Message}");
                    throw;
                }
            }

            public void Dispose()
            {
                try
                {
                    _writer.Dispose();
                    _log($"Gravacao finalizada: {VideoPath}");
                }
                catch (Exception ex)
                {
                    _log($"Erro ao finalizar gravacao: {ex.Message}");
                }
            }

            private static byte[] RenderFrame(FrameworkElement source)
            {
                FrameworkElement visual = source;
                Window? originalWindow = null;
                object? originalWindowContent = null;
                if (source is Window window && window.Content is FrameworkElement content)
                {
                    originalWindow = window;
                    originalWindowContent = window.Content;
                    window.Content = null;
                    visual = content;
                }

                var host = new Border
                {
                    Width = Width,
                    Height = Height,
                    Background = Brushes.White
                };

                if (visual.Parent is Panel panel)
                    panel.Children.Remove(visual);
                else if (visual.Parent is ContentControl contentControl)
                    contentControl.Content = null;

                host.Child = visual;
                host.Measure(new Size(Width, Height));
                host.Arrange(new Rect(0, 0, Width, Height));
                host.UpdateLayout();

                var bitmap = new RenderTargetBitmap(Width, Height, 96, 96, PixelFormats.Pbgra32);
                bitmap.Render(host);

                var stride = Width * 4;
                var pixels = new byte[stride * Height];
                bitmap.CopyPixels(pixels, stride, 0);

                host.Child = null;
                if (originalWindow != null)
                    originalWindow.Content = originalWindowContent;

                var rowSize = ((Width * 24 + 31) / 32) * 4;
                var frame = new byte[rowSize * Height];
                for (var y = 0; y < Height; y++)
                {
                    var srcRow = y * stride;
                    var dstRow = (Height - 1 - y) * rowSize;
                    for (var x = 0; x < Width; x++)
                    {
                        var src = srcRow + (x * 4);
                        var dst = dstRow + (x * 3);
                        frame[dst] = pixels[src];
                        frame[dst + 1] = pixels[src + 1];
                        frame[dst + 2] = pixels[src + 2];
                    }
                }

                return frame;
            }
        }

        private sealed class AviWriter : IDisposable
        {
            private readonly BinaryWriter _writer;
            private readonly int _width;
            private readonly int _height;
            private readonly int _fps;
            private readonly int _frameSize;
            private readonly List<(int Offset, int Size)> _index = new();
            private readonly long _riffSizePosition;
            private readonly long _hdrlListSizePosition;
            private readonly long _avihFramesPosition;
            private readonly long _strhLengthPosition;
            private readonly long _moviListSizePosition;
            private readonly long _moviDataStart;

            public AviWriter(string path, int width, int height, int fps)
            {
                _width = width;
                _height = height;
                _fps = fps;
                _frameSize = ((width * 24 + 31) / 32) * 4 * height;
                _writer = new BinaryWriter(File.Create(path), Encoding.ASCII);

                WriteFourCc("RIFF");
                _riffSizePosition = _writer.BaseStream.Position;
                WriteInt32(0);
                WriteFourCc("AVI ");

                WriteFourCc("LIST");
                _hdrlListSizePosition = _writer.BaseStream.Position;
                WriteInt32(0);
                WriteFourCc("hdrl");

                WriteFourCc("avih");
                WriteInt32(56);
                WriteInt32(1_000_000 / fps);
                WriteInt32(_frameSize * fps);
                WriteInt32(0);
                WriteInt32(0x10);
                _avihFramesPosition = _writer.BaseStream.Position;
                WriteInt32(0);
                WriteInt32(0);
                WriteInt32(1);
                WriteInt32(_frameSize);
                WriteInt32(width);
                WriteInt32(height);
                WriteInt32(0);
                WriteInt32(0);
                WriteInt32(0);
                WriteInt32(0);

                WriteFourCc("LIST");
                var strlSizePosition = _writer.BaseStream.Position;
                WriteInt32(0);
                WriteFourCc("strl");

                WriteFourCc("strh");
                WriteInt32(56);
                WriteFourCc("vids");
                WriteFourCc("DIB ");
                WriteInt32(0);
                WriteInt16(0);
                WriteInt16(0);
                WriteInt32(0);
                WriteInt32(1);
                WriteInt32(fps);
                WriteInt32(0);
                _strhLengthPosition = _writer.BaseStream.Position;
                WriteInt32(0);
                WriteInt32(_frameSize);
                WriteInt32(-1);
                WriteInt32(0);
                WriteInt32(0);
                WriteInt32(0);
                WriteInt32(width);
                WriteInt32(height);

                WriteFourCc("strf");
                WriteInt32(40);
                WriteInt32(40);
                WriteInt32(width);
                WriteInt32(height);
                WriteInt16(1);
                WriteInt16(24);
                WriteInt32(0);
                WriteInt32(_frameSize);
                WriteInt32(0);
                WriteInt32(0);
                WriteInt32(0);
                WriteInt32(0);

                PatchSize(strlSizePosition);
                PatchSize(_hdrlListSizePosition);

                WriteFourCc("LIST");
                _moviListSizePosition = _writer.BaseStream.Position;
                WriteInt32(0);
                WriteFourCc("movi");
                _moviDataStart = _writer.BaseStream.Position;
            }

            public void AddFrame(byte[] frame)
            {
                if (frame.Length != _frameSize)
                    throw new InvalidOperationException("Frame com tamanho invalido para o AVI.");

                var offset = (int)(_writer.BaseStream.Position - _moviDataStart);
                WriteFourCc("00db");
                WriteInt32(frame.Length);
                _writer.Write(frame);
                if ((frame.Length & 1) == 1)
                    _writer.Write((byte)0);
                _index.Add((offset, frame.Length));
            }

            public void Dispose()
            {
                PatchInt32(_avihFramesPosition, _index.Count);
                PatchInt32(_strhLengthPosition, _index.Count);
                PatchSize(_moviListSizePosition);

                WriteFourCc("idx1");
                WriteInt32(_index.Count * 16);
                foreach (var entry in _index)
                {
                    WriteFourCc("00db");
                    WriteInt32(0x10);
                    WriteInt32(entry.Offset);
                    WriteInt32(entry.Size);
                }

                PatchInt32(_riffSizePosition, (int)(_writer.BaseStream.Length - 8));
                _writer.Dispose();
            }

            private void PatchSize(long sizePosition)
            {
                PatchInt32(sizePosition, (int)(_writer.BaseStream.Position - sizePosition - 4));
            }

            private void PatchInt32(long position, int value)
            {
                var current = _writer.BaseStream.Position;
                _writer.BaseStream.Position = position;
                WriteInt32(value);
                _writer.BaseStream.Position = current;
            }

            private void WriteFourCc(string value) => _writer.Write(Encoding.ASCII.GetBytes(value));
            private void WriteInt16(short value) => _writer.Write(value);
            private void WriteInt32(int value) => _writer.Write(value);
        }
    }
}
