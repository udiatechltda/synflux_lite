using System.Diagnostics;
using System.IO.Compression;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json;

namespace TechOne.PDV.Updater;

internal static class Program
{
    private const string ManifestPath = "/updates/pdv/latest";
    private const string UpdaterExeName = "TechOne.PDV.Updater.exe";
    private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(20) };

    private static async Task<int> Main(string[] args)
    {
        var options = UpdateOptions.Parse(args);
        var logger = new UpdateLogger(options.DataDir);

        try
        {
            logger.Info("Atualizador iniciado.");

            if (options.ParentPid is > 0)
                WaitForParentExit(options.ParentPid.Value, logger);

            if (!Directory.Exists(options.AppDir))
            {
                logger.Info($"Diretorio da aplicacao nao existe: {options.AppDir}");
                return 0;
            }

            if (!CanWriteToAppDir(options.AppDir, logger))
                return 0;

            var manifest = await LoadManifestAsync(options, logger);
            if (manifest is null)
                return 0;

            if (!manifest.Enabled)
            {
                logger.Info("Atualizacao desabilitada no manifesto.");
                return 0;
            }

            if (!HasNewerVersion(options.CurrentVersion, manifest.Version))
            {
                logger.Info($"Nenhuma atualizacao disponivel. Atual={options.CurrentVersion}; Remota={manifest.Version}.");
                return 0;
            }

            if (string.IsNullOrWhiteSpace(manifest.PackageUrl))
            {
                logger.Info("Manifesto informa nova versao, mas nao possui PackageUrl.");
                return 0;
            }

            var workDir = CreateWorkDirectory(options.DataDir);
            var packagePath = Path.Combine(workDir, "pdv-update.zip");
            var extractDir = Path.Combine(workDir, "extract");
            var backupDir = Path.Combine(workDir, "backup");

            await DownloadPackageAsync(manifest.PackageUrl, packagePath, logger);
            ValidateSha256(packagePath, manifest.Sha256, logger);

            ZipFile.ExtractToDirectory(packagePath, extractDir, overwriteFiles: true);
            ApplyUpdate(options.AppDir, extractDir, backupDir, logger);

            logger.Info($"Atualizacao aplicada com sucesso. Versao={manifest.Version}.");
            TryCleanup(workDir, logger);
            return 0;
        }
        catch (Exception ex)
        {
            logger.Error($"Falha na atualizacao: {ex}");
            return 0;
        }
    }

    private static async Task<UpdateManifest?> LoadManifestAsync(UpdateOptions options, UpdateLogger logger)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(options.ManifestFile))
            {
                logger.Info($"Lendo manifesto local: {options.ManifestFile}");
                await using var manifestStream = File.OpenRead(options.ManifestFile);
                return await JsonSerializer.DeserializeAsync<UpdateManifest>(manifestStream, JsonOptions());
            }

            if (string.IsNullOrWhiteSpace(options.BaseUrl))
            {
                logger.Info("BaseUrl nao informada.");
                return null;
            }

            var url = $"{options.BaseUrl.TrimEnd('/')}{ManifestPath}?currentVersion={Uri.EscapeDataString(options.CurrentVersion)}";
            logger.Info($"Consultando manifesto: {url}");
            await using var stream = await Http.GetStreamAsync(url);
            return await JsonSerializer.DeserializeAsync<UpdateManifest>(stream, JsonOptions());
        }
        catch (Exception ex)
        {
            logger.Error($"Nao foi possivel obter manifesto: {ex.Message}");
            return null;
        }
    }

    private static async Task DownloadPackageAsync(string packageUrl, string packagePath, UpdateLogger logger)
    {
        logger.Info($"Baixando pacote: {packageUrl}");
        Directory.CreateDirectory(Path.GetDirectoryName(packagePath)!);

        if (Uri.TryCreate(packageUrl, UriKind.Absolute, out var uri) && uri.IsFile)
        {
            File.Copy(uri.LocalPath, packagePath, overwrite: true);
            return;
        }

        await using var remote = await Http.GetStreamAsync(packageUrl);
        await using var local = File.Create(packagePath);
        await remote.CopyToAsync(local);
    }

    private static void ValidateSha256(string filePath, string? expectedHash, UpdateLogger logger)
    {
        if (string.IsNullOrWhiteSpace(expectedHash))
        {
            logger.Info("SHA256 nao informado; validacao de hash ignorada.");
            return;
        }

        using var stream = File.OpenRead(filePath);
        var actualHash = Convert.ToHexString(SHA256.HashData(stream));
        if (!actualHash.Equals(expectedHash.Trim(), StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException($"SHA256 invalido. Esperado={expectedHash}; Obtido={actualHash}");

        logger.Info("SHA256 validado com sucesso.");
    }

    private static void ApplyUpdate(string appDir, string extractDir, string backupDir, UpdateLogger logger)
    {
        Directory.CreateDirectory(backupDir);

        var sourceRoot = ResolveSourceRoot(extractDir);
        var files = Directory.GetFiles(sourceRoot, "*", SearchOption.AllDirectories)
            .Where(file => !ShouldSkipFile(file, sourceRoot))
            .ToArray();

        logger.Info($"Aplicando {files.Length} arquivo(s).");

        try
        {
            foreach (var sourceFile in files)
            {
                var relativePath = Path.GetRelativePath(sourceRoot, sourceFile);
                var targetFile = Path.Combine(appDir, relativePath);
                var backupFile = Path.Combine(backupDir, relativePath);

                Directory.CreateDirectory(Path.GetDirectoryName(targetFile)!);

                if (File.Exists(targetFile))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(backupFile)!);
                    File.Copy(targetFile, backupFile, overwrite: true);
                }

                CopyWithRetry(sourceFile, targetFile);
            }
        }
        catch
        {
            RestoreBackup(backupDir, appDir, logger);
            throw;
        }
    }

    private static void CopyWithRetry(string source, string dest, int attempts = 3)
    {
        for (var i = 0; i < attempts; i++)
        {
            try
            {
                File.Copy(source, dest, overwrite: true);
                return;
            }
            catch (IOException) when (i < attempts - 1)
            {
                Thread.Sleep(1000);
            }
        }
    }

    private static string ResolveSourceRoot(string extractDir)
    {
        var entries = Directory.GetFileSystemEntries(extractDir);
        if (entries.Length == 1 && Directory.Exists(entries[0]))
            return entries[0];

        return extractDir;
    }

    private static bool ShouldSkipFile(string filePath, string sourceRoot)
    {
        var relative = Path.GetRelativePath(sourceRoot, filePath);
        var fileName = Path.GetFileName(relative);

        return fileName.Equals(UpdaterExeName, StringComparison.OrdinalIgnoreCase)
            || fileName.Equals("pdv.sqlite", StringComparison.OrdinalIgnoreCase)
            || fileName.Equals("pdv.config.json", StringComparison.OrdinalIgnoreCase);
    }

    private static void RestoreBackup(string backupDir, string appDir, UpdateLogger logger)
    {
        logger.Info("Restaurando backup apos falha.");

        if (!Directory.Exists(backupDir))
            return;

        foreach (var backupFile in Directory.GetFiles(backupDir, "*", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(backupDir, backupFile);
            var targetFile = Path.Combine(appDir, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(targetFile)!);
            File.Copy(backupFile, targetFile, overwrite: true);
        }
    }

    private static bool HasNewerVersion(string currentVersion, string? remoteVersion)
    {
        if (string.IsNullOrWhiteSpace(remoteVersion))
            return false;

        if (!Version.TryParse(NormalizeVersion(currentVersion), out var current))
            current = new Version(0, 0, 0);

        if (!Version.TryParse(NormalizeVersion(remoteVersion), out var remote))
            return false;

        return remote > current;
    }

    private static string NormalizeVersion(string version)
    {
        var normalized = version.Trim();
        var dashIndex = normalized.IndexOf('-', StringComparison.Ordinal);
        if (dashIndex >= 0)
            normalized = normalized[..dashIndex];

        return normalized;
    }

    private static bool CanWriteToAppDir(string appDir, UpdateLogger logger)
    {
        try
        {
            var probe = Path.Combine(appDir, $".update-probe-{Guid.NewGuid():N}.tmp");
            File.WriteAllText(probe, "ok");
            File.Delete(probe);
            return true;
        }
        catch (Exception ex)
        {
            logger.Error($"Sem permissao para atualizar {appDir}: {ex.Message}");
            return false;
        }
    }

    private static void WaitForParentExit(int parentPid, UpdateLogger logger)
    {
        try
        {
            using var parent = Process.GetProcessById(parentPid);
            if (!parent.HasExited)
            {
                logger.Info($"Aguardando PDV encerrar. PID={parentPid}");
                parent.WaitForExit(30000);
            }
        }
        catch
        {
            // Processo ja encerrou.
        }

        // Aguarda o SO liberar handles de arquivo do processo encerrado.
        Thread.Sleep(2000);
    }

    private static string CreateWorkDirectory(string dataDir)
    {
        var root = Path.Combine(dataDir, "updates", DateTime.Now.ToString("yyyyMMdd-HHmmss"));
        Directory.CreateDirectory(root);
        return root;
    }

    private static void TryCleanup(string workDir, UpdateLogger logger)
    {
        try
        {
            Directory.Delete(workDir, recursive: true);
        }
        catch (Exception ex)
        {
            logger.Info($"Nao foi possivel limpar pasta temporaria {workDir}: {ex.Message}");
        }
    }

    private static JsonSerializerOptions JsonOptions()
    {
        return new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
}

internal sealed class UpdateManifest
{
    public bool Enabled { get; set; }
    public string? Version { get; set; }
    public string? PackageUrl { get; set; }
    public string? Sha256 { get; set; }
    public bool Required { get; set; }
    public string? ReleaseNotes { get; set; }
}

internal sealed class UpdateOptions
{
    public int? ParentPid { get; private init; }
    public string AppDir { get; private init; } = AppContext.BaseDirectory;
    public string DataDir { get; private init; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        "TechOne",
        "PDV");
    public string BaseUrl { get; private init; } = string.Empty;
    public string CurrentVersion { get; private init; } = "0.0.0";
    public string? ManifestFile { get; private init; }

    public static UpdateOptions Parse(string[] args)
    {
        var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        for (var i = 0; i < args.Length; i++)
        {
            if (!args[i].StartsWith("--", StringComparison.Ordinal))
                continue;

            var key = args[i][2..];
            if (i + 1 < args.Length && !args[i + 1].StartsWith("--", StringComparison.Ordinal))
                values[key] = args[++i];
            else
                values[key] = "true";
        }

        return new UpdateOptions
        {
            ParentPid = values.TryGetValue("parent-pid", out var pid) && int.TryParse(pid, out var parsedPid)
                ? parsedPid
                : null,
            AppDir = Path.GetFullPath(values.GetValueOrDefault("app-dir", AppContext.BaseDirectory)),
            DataDir = Path.GetFullPath(values.GetValueOrDefault("data-dir", Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "TechOne",
                "PDV"))),
            BaseUrl = values.GetValueOrDefault("base-url", string.Empty).TrimEnd('/'),
            CurrentVersion = values.GetValueOrDefault("current-version", "0.0.0"),
            ManifestFile = values.GetValueOrDefault("manifest-file")
        };
    }
}

internal sealed class UpdateLogger
{
    private readonly string _logPath;

    public UpdateLogger(string dataDir)
    {
        var logDir = Path.Combine(dataDir, "logs");
        Directory.CreateDirectory(logDir);
        _logPath = Path.Combine(logDir, "updater.log");
    }

    public void Info(string message)
    {
        Write("INFO", message);
    }

    public void Error(string message)
    {
        Write("ERRO", message);
    }

    private void Write(string level, string message)
    {
        File.AppendAllText(_logPath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}{Environment.NewLine}");
    }
}
