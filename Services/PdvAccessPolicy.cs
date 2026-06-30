using System.Globalization;
using System.Text;
using PDV.Services.Interfaces;

namespace PDV.Services
{
    public static class PdvAccessPolicy
    {
        private static readonly HashSet<string> OperadorPerfis = new(StringComparer.OrdinalIgnoreCase)
        {
            "operador",
            "caixa",
            "vendedor"
        };

        private static readonly HashSet<string> OperadorRotas = new(StringComparer.OrdinalIgnoreCase)
        {
            "AberturaCaixa",
            "Home",
            "PDV",
            "Movimento",
            "Suprimento",
            "Sangria",
            "Vendas",
            "Ajuda",
            "Contato",
            "Sair"
        };

        private static readonly HashSet<string> RotasComCaixaAberto = new(StringComparer.OrdinalIgnoreCase)
        {
            "Home",
            "PDV",
            "Movimento",
            "Suprimento",
            "Sangria"
        };

        public static bool IsOperador(IAuthenticationService? authenticationService)
        {
            var perfil = Normalizar(authenticationService?.CurrentSession?.Usuario.Perfil);
            return string.IsNullOrWhiteSpace(perfil) || OperadorPerfis.Contains(perfil);
        }

        public static bool CanAccessRoute(IAuthenticationService? authenticationService, string viewName)
        {
            return !IsOperador(authenticationService) || OperadorRotas.Contains(viewName);
        }

        public static bool RequiresOpenCash(string viewName) => RotasComCaixaAberto.Contains(viewName);

        public static string GetInitialRoute(IAuthenticationService? authenticationService, IPdvOperationService pdvService)
        {
            if (IsOperador(authenticationService))
                return pdvService.ExisteMovimentoAberto() ? "Home" : "AberturaCaixa";

            return "Dashboard";
        }

        private static string Normalizar(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var normalized = value.Trim().ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder(normalized.Length);

            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    builder.Append(c);
            }

            return builder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
