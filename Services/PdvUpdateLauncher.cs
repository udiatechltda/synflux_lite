using PDV.Services.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace PDV.Services
{
    public sealed class PdvUpdateLauncher : IPdvUpdateLauncher
    {
        private const string UpdaterFileName = "TechOne.PDV.Updater.exe";

        public void TryLaunchOnExit(bool hasOpenCash)
        {
            try
            {
                PdvRuntimePaths.EnsureDataDirectory();

                if (hasOpenCash)
                {
                    Log("Atualizacao no encerramento ignorada: caixa aberto.");
                    return;
                }

                var updaterPath = Path.Combine(AppContext.BaseDirectory, UpdaterFileName);
                if (!File.Exists(updaterPath))
                {
                    Log($"Atualizador nao encontrado em {updaterPath}.");
                    return;
                }

                var currentVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "0.0.0";
                var baseUrl = RetaguardaEndpointResolver.ObterBaseUrl();
                var processId = Environment.ProcessId;

                var startInfo = new ProcessStartInfo
                {
                    FileName = updaterPath,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = AppContext.BaseDirectory
                };
                startInfo.ArgumentList.Add("--on-exit");
                startInfo.ArgumentList.Add("--parent-pid");
                startInfo.ArgumentList.Add(processId.ToString());
                startInfo.ArgumentList.Add("--app-dir");
                startInfo.ArgumentList.Add(AppContext.BaseDirectory);
                startInfo.ArgumentList.Add("--data-dir");
                startInfo.ArgumentList.Add(PdvRuntimePaths.DataDirectory);
                startInfo.ArgumentList.Add("--base-url");
                startInfo.ArgumentList.Add(baseUrl);
                startInfo.ArgumentList.Add("--current-version");
                startInfo.ArgumentList.Add(currentVersion);

                Process.Start(startInfo);
                Log($"Atualizador disparado. VersaoAtual={currentVersion}; BaseUrl={baseUrl}.");
            }
            catch (Exception ex)
            {
                Log($"Falha ao disparar atualizador: {ex.Message}");
            }
        }

        private static void Log(string message)
        {
            try
            {
                var logDir = Path.Combine(PdvRuntimePaths.DataDirectory, "logs");
                Directory.CreateDirectory(logDir);
                File.AppendAllText(
                    Path.Combine(logDir, "updater-launcher.log"),
                    $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}");
            }
            catch
            {
                // Nao bloquear fechamento do PDV por falha de log.
            }
        }
    }
}
