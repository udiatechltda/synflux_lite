using PDV.Services.Interfaces;
using PDV.Services.Retaguarda;
using System;
using System.IO;
using System.Linq;

namespace PDV.Services
{
    public sealed class LocalDatabaseService : ILocalDatabaseService
    {
        private readonly object _lock = new();
        private readonly string _tenantRoot;
        private string _currentDatabasePath;
        private string _currentDatabaseKey = "login";

        public LocalDatabaseService(string loginDatabasePath)
        {
            LoginDatabasePath = Path.GetFullPath(loginDatabasePath);
            _currentDatabasePath = LoginDatabasePath;

            var baseDirectory = Path.GetDirectoryName(LoginDatabasePath) ?? AppContext.BaseDirectory;
            _tenantRoot = Path.Combine(baseDirectory, "tenants");
        }

        public string CurrentDatabasePath
        {
            get
            {
                lock (_lock)
                    return _currentDatabasePath;
            }
        }

        public string CurrentDatabaseKey
        {
            get
            {
                lock (_lock)
                    return _currentDatabaseKey;
            }
        }

        public string LoginDatabasePath { get; }

        public string GetTenantDatabasePath(RetaguardaAuthSession session)
        {
            var cnpj = SomenteDigitos(session?.Empresa.Cnpj ?? string.Empty);
            if (cnpj.Length != 14)
                throw new InvalidOperationException("Sessao da retaguarda sem CNPJ valido.");

            return Path.Combine(_tenantRoot, cnpj, "pdv.sqlite");
        }

        public void UseTenantDatabase(RetaguardaAuthSession session)
        {
            var path = GetTenantDatabasePath(session);
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? _tenantRoot);

            lock (_lock)
            {
                _currentDatabasePath = path;
                _currentDatabaseKey = SomenteDigitos(session.Empresa.Cnpj);
            }
        }

        private static string SomenteDigitos(string value)
        {
            return new string((value ?? string.Empty).Where(char.IsDigit).ToArray());
        }
    }
}
