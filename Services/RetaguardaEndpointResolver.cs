using System;
using System.IO;
using System.Text.Json;

namespace PDV.Services
{
    public static class RetaguardaEndpointResolver
    {
        private const string DefaultUrl = "https://retaguardash.techone-it.com.br";

        public static string ObterBaseUrl()
        {
            var envUrl = Environment.GetEnvironmentVariable("PDV_RETAGUARDA_URL");
            if (!string.IsNullOrWhiteSpace(envUrl))
                return Normalizar(envUrl);

            var configUrl = LerConfigLocal();
            if (!string.IsNullOrWhiteSpace(configUrl))
                return Normalizar(configUrl);

            return DefaultUrl;
        }

        private static string? LerConfigLocal()
        {
            var candidates = new[]
            {
                PdvRuntimePaths.DataConfigPath,
                Path.Combine(AppContext.BaseDirectory, "pdv.config.json")
            };

            foreach (var configPath in candidates)
            {
                if (!File.Exists(configPath))
                    continue;

                try
                {
                    using var stream = File.OpenRead(configPath);
                    using var document = JsonDocument.Parse(stream);
                    if (document.RootElement.TryGetProperty("retaguardaUrl", out var url))
                        return url.GetString();
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        private static string Normalizar(string url)
        {
            return url.Trim().TrimEnd('/');
        }
    }
}
