using System;
using System.IO;

namespace PDV.Services
{
    public static class PdvRuntimePaths
    {
        public const string CompanyName = "TechOne";
        public const string ProductName = "PDV";

        public static string DataDirectory
        {
            get
            {
                var configuredDirectory = Environment.GetEnvironmentVariable("PDV_DATA_DIR");
                if (!string.IsNullOrWhiteSpace(configuredDirectory))
                    return Path.GetFullPath(configuredDirectory);

                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    CompanyName,
                    ProductName);
            }
        }

        public static string LoginDatabasePath => Path.Combine(DataDirectory, "pdv.sqlite");

        public static string DataConfigPath => Path.Combine(DataDirectory, "pdv.config.json");

        public static void EnsureDataDirectory()
        {
            Directory.CreateDirectory(DataDirectory);
        }
    }
}
