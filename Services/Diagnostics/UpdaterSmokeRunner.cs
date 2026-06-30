using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;

namespace PDV.Services.Diagnostics
{
    public static class UpdaterSmokeRunner
    {
        private const string UpdaterExeName = "TechOne.PDV.Updater.exe";

        public static int Run(string runRoot)
        {
            Directory.CreateDirectory(runRoot);
            var logPath = Path.Combine(runRoot, "updater-smoke.txt");

            void Log(string msg)
                => File.AppendAllText(logPath, $"{DateTime.Now:HH:mm:ss} {msg}{Environment.NewLine}");

            try
            {
                Log("=== UpdaterSmokeRunner iniciado ===");

                var updaterExe = EncontrarOuCompilarAtualizador(runRoot, Log);
                Require(updaterExe != null,
                    "TechOne.PDV.Updater.exe nao encontrado e nao foi possivel compilar. " +
                    "Execute 'dotnet build Tools/TechOne.PDV.Updater/' e tente novamente.");

                Log($"Atualizador localizado: {updaterExe}");

                Cenario1_AtualizacaoAplicadaComArquivosProtegidos(updaterExe!, runRoot, Log);
                Log("Cenario 1 PASSOU: atualizacao aplicada, arquivos protegidos preservados.");

                Cenario2_SemAtualizacaoVersaoIgual(updaterExe!, runRoot, Log);
                Log("Cenario 2 PASSOU: sem atualizacao quando versao remota igual a local.");

                Cenario3_AtualizacaoDesabilitadaNoManifesto(updaterExe!, runRoot, Log);
                Log("Cenario 3 PASSOU: arquivos intocados quando atualizacao desabilitada no manifesto.");

                Cenario4_Sha256InvalidoArquivosPreservados(updaterExe!, runRoot, Log);
                Log("Cenario 4 PASSOU: arquivos preservados quando SHA256 invalido.");

                Cenario5_VersaoRemotaMenorQueAtual(updaterExe!, runRoot, Log);
                Log("Cenario 5 PASSOU: sem atualizacao quando versao remota e menor que a local.");

                Cenario6_ZipComSubdiretorioRaiz(updaterExe!, runRoot, Log);
                Log("Cenario 6 PASSOU: atualizacao aplicada com ZIP contendo subdiretorio raiz.");

                Cenario7_ArquivosNovosAdicionados(updaterExe!, runRoot, Log);
                Log("Cenario 7 PASSOU: novos arquivos do pacote adicionados ao app dir.");

                Log("=== Todos os 7 cenarios concluidos com sucesso. ===");
                File.AppendAllText(logPath, "RESULTADO: OK" + Environment.NewLine);
                return 0;
            }
            catch (Exception ex)
            {
                File.AppendAllText(logPath, $"ERRO: {ex}{Environment.NewLine}RESULTADO: FALHOU{Environment.NewLine}");
                return 1;
            }
        }

        // Cenario 1: Arquivos atualizados corretamente; pdv.sqlite, pdv.config.json e o proprio updater sao preservados.
        private static void Cenario1_AtualizacaoAplicadaComArquivosProtegidos(string updaterExe, string runRoot, Action<string> log)
        {
            var (root, appDir, dataDir) = CriarDiretoriosCenario(runRoot, "cenario-1");

            WriteFile(appDir, "PDV.exe", "pdv-v1");
            WriteFile(appDir, "PDV.dll", "dll-v1");
            WriteFile(appDir, "Newtonsoft.Json.dll", "json-v1");
            WriteFile(appDir, "pdv.sqlite", "SQLITE-ORIGINAL");
            WriteFile(appDir, "pdv.config.json", "{\"retaguardaUrl\":\"https://original\"}");
            WriteFile(appDir, UpdaterExeName, "UPDATER-ORIGINAL");

            var zipPath = Path.Combine(root, "update.zip");
            CriarZip(zipPath, new[]
            {
                ("PDV.exe", "pdv-v2"),
                ("PDV.dll", "dll-v2"),
                ("Newtonsoft.Json.dll", "json-v2"),
                ("pdv.sqlite", "SQLITE-MODIFICADO"),
                ("pdv.config.json", "{\"retaguardaUrl\":\"https://modificado\"}"),
                (UpdaterExeName, "UPDATER-MODIFICADO")
            });

            var sha256 = ComputarSha256(zipPath);
            var manifestPath = EscreverManifesto(root, enabled: true, version: "2.0.0",
                packageUrl: FileUri(zipPath), sha256: sha256);

            ExecutarAtualizador(updaterExe, appDir, dataDir, currentVersion: "1.0.0", manifestFile: manifestPath);

            var updaterLog = LerLogAtualizador(dataDir);
            Require(updaterLog.Contains("Atualizacao aplicada com sucesso"),
                "Cenario 1: log deve conter 'Atualizacao aplicada com sucesso'.");

            Require(ReadFile(appDir, "PDV.exe") == "pdv-v2", "Cenario 1: PDV.exe deve ser v2.");
            Require(ReadFile(appDir, "PDV.dll") == "dll-v2", "Cenario 1: PDV.dll deve ser v2.");
            Require(ReadFile(appDir, "Newtonsoft.Json.dll") == "json-v2", "Cenario 1: Newtonsoft.Json.dll deve ser v2.");

            Require(ReadFile(appDir, "pdv.sqlite") == "SQLITE-ORIGINAL",
                "Cenario 1: pdv.sqlite deve ser preservado (arquivo protegido).");
            Require(ReadFile(appDir, "pdv.config.json") == "{\"retaguardaUrl\":\"https://original\"}",
                "Cenario 1: pdv.config.json deve ser preservado (arquivo protegido).");
            Require(ReadFile(appDir, UpdaterExeName) == "UPDATER-ORIGINAL",
                "Cenario 1: TechOne.PDV.Updater.exe deve ser preservado (nao pode substituir a si mesmo).");
        }

        // Cenario 2: Versao remota igual a atual → nenhum arquivo tocado.
        private static void Cenario2_SemAtualizacaoVersaoIgual(string updaterExe, string runRoot, Action<string> log)
        {
            var (root, appDir, dataDir) = CriarDiretoriosCenario(runRoot, "cenario-2");
            WriteFile(appDir, "PDV.exe", "pdv-v1");

            var manifestPath = EscreverManifesto(root, enabled: true, version: "1.0.0");
            ExecutarAtualizador(updaterExe, appDir, dataDir, currentVersion: "1.0.0", manifestFile: manifestPath);

            var updaterLog = LerLogAtualizador(dataDir);
            Require(updaterLog.Contains("Nenhuma atualizacao disponivel"),
                "Cenario 2: log deve indicar 'Nenhuma atualizacao disponivel'.");
            Require(ReadFile(appDir, "PDV.exe") == "pdv-v1",
                "Cenario 2: PDV.exe nao deve ser modificado quando versoes sao iguais.");
        }

        // Cenario 3: Enabled=false no manifesto → nenhum arquivo tocado.
        private static void Cenario3_AtualizacaoDesabilitadaNoManifesto(string updaterExe, string runRoot, Action<string> log)
        {
            var (root, appDir, dataDir) = CriarDiretoriosCenario(runRoot, "cenario-3");
            WriteFile(appDir, "PDV.exe", "pdv-v1");

            var manifestPath = EscreverManifesto(root, enabled: false, version: "9.0.0");
            ExecutarAtualizador(updaterExe, appDir, dataDir, currentVersion: "1.0.0", manifestFile: manifestPath);

            var updaterLog = LerLogAtualizador(dataDir);
            Require(updaterLog.Contains("desabilitada"),
                "Cenario 3: log deve indicar 'Atualizacao desabilitada no manifesto'.");
            Require(ReadFile(appDir, "PDV.exe") == "pdv-v1",
                "Cenario 3: PDV.exe nao deve ser modificado quando atualizacao esta desabilitada.");
        }

        // Cenario 4: SHA256 errado → atualizador rejeita antes de extrair; arquivos originais intactos.
        private static void Cenario4_Sha256InvalidoArquivosPreservados(string updaterExe, string runRoot, Action<string> log)
        {
            var (root, appDir, dataDir) = CriarDiretoriosCenario(runRoot, "cenario-4");
            WriteFile(appDir, "PDV.exe", "pdv-v1");
            WriteFile(appDir, "PDV.dll", "dll-v1");

            var zipPath = Path.Combine(root, "update.zip");
            CriarZip(zipPath, new[] { ("PDV.exe", "pdv-v2"), ("PDV.dll", "dll-v2") });

            const string sha256Errado = "0000000000000000000000000000000000000000000000000000000000000000";
            var manifestPath = EscreverManifesto(root, enabled: true, version: "2.0.0",
                packageUrl: FileUri(zipPath), sha256: sha256Errado);

            ExecutarAtualizador(updaterExe, appDir, dataDir, currentVersion: "1.0.0", manifestFile: manifestPath);

            var updaterLog = LerLogAtualizador(dataDir);
            Require(updaterLog.Contains("SHA256 invalido") || updaterLog.Contains("Falha na atualizacao"),
                "Cenario 4: log deve indicar erro de SHA256.");
            Require(ReadFile(appDir, "PDV.exe") == "pdv-v1",
                "Cenario 4: PDV.exe deve ser preservado apos SHA256 invalido.");
            Require(ReadFile(appDir, "PDV.dll") == "dll-v1",
                "Cenario 4: PDV.dll deve ser preservado apos SHA256 invalido.");
        }

        // Cenario 5: Versao remota menor que a atual → sem atualizacao.
        private static void Cenario5_VersaoRemotaMenorQueAtual(string updaterExe, string runRoot, Action<string> log)
        {
            var (root, appDir, dataDir) = CriarDiretoriosCenario(runRoot, "cenario-5");
            WriteFile(appDir, "PDV.exe", "pdv-v2");

            var manifestPath = EscreverManifesto(root, enabled: true, version: "0.5.0");
            ExecutarAtualizador(updaterExe, appDir, dataDir, currentVersion: "2.0.0", manifestFile: manifestPath);

            var updaterLog = LerLogAtualizador(dataDir);
            Require(updaterLog.Contains("Nenhuma atualizacao disponivel"),
                "Cenario 5: log deve indicar 'Nenhuma atualizacao disponivel'.");
            Require(ReadFile(appDir, "PDV.exe") == "pdv-v2",
                "Cenario 5: PDV.exe nao deve ser modificado quando versao remota e menor.");
        }

        // Cenario 6: ZIP com subdiretorio raiz (ex: "app-2.0.0/PDV.exe") → atualizador desaninha automaticamente.
        private static void Cenario6_ZipComSubdiretorioRaiz(string updaterExe, string runRoot, Action<string> log)
        {
            var (root, appDir, dataDir) = CriarDiretoriosCenario(runRoot, "cenario-6");
            WriteFile(appDir, "PDV.exe", "pdv-v1");
            WriteFile(appDir, "config.ini", "config-v1");

            var zipPath = Path.Combine(root, "update.zip");
            CriarZipComSubdir(zipPath, "pdv-2.0.0", new[]
            {
                ("PDV.exe", "pdv-v2"),
                ("config.ini", "config-v2")
            });
            var sha256 = ComputarSha256(zipPath);
            var manifestPath = EscreverManifesto(root, enabled: true, version: "2.0.0",
                packageUrl: FileUri(zipPath), sha256: sha256);

            ExecutarAtualizador(updaterExe, appDir, dataDir, currentVersion: "1.0.0", manifestFile: manifestPath);

            var updaterLog = LerLogAtualizador(dataDir);
            Require(updaterLog.Contains("Atualizacao aplicada com sucesso"),
                "Cenario 6: log deve confirmar sucesso com ZIP aninhado.");
            Require(ReadFile(appDir, "PDV.exe") == "pdv-v2",
                "Cenario 6: PDV.exe deve ser v2 mesmo com subdiretorio no ZIP.");
            Require(ReadFile(appDir, "config.ini") == "config-v2",
                "Cenario 6: config.ini deve ser v2 mesmo com subdiretorio no ZIP.");
        }

        // Cenario 7: Pacote inclui arquivos novos que nao existiam no app dir → devem ser criados.
        private static void Cenario7_ArquivosNovosAdicionados(string updaterExe, string runRoot, Action<string> log)
        {
            var (root, appDir, dataDir) = CriarDiretoriosCenario(runRoot, "cenario-7");
            WriteFile(appDir, "PDV.exe", "pdv-v1");

            var zipPath = Path.Combine(root, "update.zip");
            CriarZip(zipPath, new[]
            {
                ("PDV.exe", "pdv-v2"),
                ("NovoPlugin.dll", "plugin-v1"),
                ("assets/logo.png", "logo-bytes")
            });
            var sha256 = ComputarSha256(zipPath);
            var manifestPath = EscreverManifesto(root, enabled: true, version: "2.0.0",
                packageUrl: FileUri(zipPath), sha256: sha256);

            ExecutarAtualizador(updaterExe, appDir, dataDir, currentVersion: "1.0.0", manifestFile: manifestPath);

            var updaterLog = LerLogAtualizador(dataDir);
            Require(updaterLog.Contains("Atualizacao aplicada com sucesso"),
                "Cenario 7: log deve confirmar sucesso.");
            Require(ReadFile(appDir, "PDV.exe") == "pdv-v2",
                "Cenario 7: PDV.exe deve ser v2.");
            Require(File.Exists(Path.Combine(appDir, "NovoPlugin.dll")),
                "Cenario 7: NovoPlugin.dll deve ter sido criado.");
            Require(ReadFile(appDir, "NovoPlugin.dll") == "plugin-v1",
                "Cenario 7: NovoPlugin.dll deve ter conteudo correto.");
            Require(File.Exists(Path.Combine(appDir, "assets", "logo.png")),
                "Cenario 7: assets/logo.png deve ter sido criado em subdiretorio.");
        }

        // --- Infraestrutura ---

        private static (string root, string appDir, string dataDir) CriarDiretoriosCenario(string runRoot, string nome)
        {
            var root = Path.Combine(runRoot, nome);
            var appDir = Path.Combine(root, "app");
            var dataDir = Path.Combine(root, "data");
            Directory.CreateDirectory(appDir);
            Directory.CreateDirectory(dataDir);
            return (root, appDir, dataDir);
        }

        private static void WriteFile(string dir, string name, string content)
            => File.WriteAllText(Path.Combine(dir, name), content);

        private static string ReadFile(string dir, string name)
            => File.ReadAllText(Path.Combine(dir, name));

        private static string LerLogAtualizador(string dataDir)
        {
            var logPath = Path.Combine(dataDir, "logs", "updater.log");
            return File.Exists(logPath) ? File.ReadAllText(logPath) : string.Empty;
        }

        private static void CriarZip(string zipPath, (string name, string content)[] entries)
        {
            using var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create);
            foreach (var (name, content) in entries)
            {
                var entry = zip.CreateEntry(name);
                using var writer = new StreamWriter(entry.Open());
                writer.Write(content);
            }
        }

        private static void CriarZipComSubdir(string zipPath, string subdir, (string name, string content)[] entries)
        {
            using var zip = ZipFile.Open(zipPath, ZipArchiveMode.Create);
            foreach (var (name, content) in entries)
            {
                var entry = zip.CreateEntry($"{subdir}/{name}");
                using var writer = new StreamWriter(entry.Open());
                writer.Write(content);
            }
        }

        private static string ComputarSha256(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            return Convert.ToHexString(SHA256.HashData(stream));
        }

        private static string FileUri(string path)
            => new Uri(Path.GetFullPath(path)).AbsoluteUri;

        private static string EscreverManifesto(string dir, bool enabled, string version,
            string? packageUrl = null, string? sha256 = null)
        {
            var manifest = new
            {
                enabled,
                version,
                packageUrl = packageUrl ?? string.Empty,
                sha256 = sha256 ?? string.Empty,
                required = false,
                releaseNotes = string.Empty
            };
            var path = Path.Combine(dir, "manifest.json");
            File.WriteAllText(path, JsonSerializer.Serialize(manifest));
            return path;
        }

        private static void ExecutarAtualizador(string updaterExe, string appDir, string dataDir,
            string currentVersion, string? manifestFile = null, string? baseUrl = null)
        {
            var psi = new ProcessStartInfo
            {
                FileName = updaterExe,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            psi.ArgumentList.Add("--app-dir");
            psi.ArgumentList.Add(appDir);
            psi.ArgumentList.Add("--data-dir");
            psi.ArgumentList.Add(dataDir);
            psi.ArgumentList.Add("--current-version");
            psi.ArgumentList.Add(currentVersion);

            if (manifestFile != null)
            {
                psi.ArgumentList.Add("--manifest-file");
                psi.ArgumentList.Add(manifestFile);
            }

            if (baseUrl != null)
            {
                psi.ArgumentList.Add("--base-url");
                psi.ArgumentList.Add(baseUrl);
            }

            using var process = Process.Start(psi)!;
            var encerrou = process.WaitForExit(30_000);
            if (!encerrou)
            {
                process.Kill();
                throw new TimeoutException("Atualizador demorou mais de 30s para encerrar. Verifique se ha deadlock.");
            }
        }

        private static string? EncontrarOuCompilarAtualizador(string runRoot, Action<string> Log)
        {
            var nextToApp = Path.Combine(AppContext.BaseDirectory, UpdaterExeName);
            if (File.Exists(nextToApp)) return nextToApp;

            var repoRoot = EncontrarRaizRepo();
            var devPath = Path.Combine(repoRoot, "Tools", "TechOne.PDV.Updater",
                "bin", "Debug", "net8.0-windows", "win-x64", UpdaterExeName);
            if (File.Exists(devPath)) return devPath;

            var projPath = Path.Combine(repoRoot, "Tools", "TechOne.PDV.Updater", "TechOne.PDV.Updater.csproj");
            if (!File.Exists(projPath))
            {
                Log("Projeto TechOne.PDV.Updater nao encontrado no repositorio.");
                return null;
            }

            Log("Compilando TechOne.PDV.Updater (isso pode levar alguns segundos)...");
            var buildLogPath = Path.Combine(runRoot, "updater-build.log");
            var buildPsi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"build \"{projPath}\" -c Debug",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var buildProc = Process.Start(buildPsi)!;
            var buildOutput = buildProc.StandardOutput.ReadToEnd() + buildProc.StandardError.ReadToEnd();
            buildProc.WaitForExit(120_000);
            File.WriteAllText(buildLogPath, buildOutput);

            if (buildProc.ExitCode != 0)
            {
                Log($"Compilacao falhou (exit {buildProc.ExitCode}). Detalhes em: {buildLogPath}");
                return null;
            }

            Log("Compilacao concluida com sucesso.");
            return File.Exists(devPath) ? devPath : null;
        }

        private static string EncontrarRaizRepo()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "PDV.csproj")))
                    return dir.FullName;
                dir = dir.Parent!;
            }
            return AppContext.BaseDirectory;
        }

        private static void Require(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException(message);
        }
    }
}
