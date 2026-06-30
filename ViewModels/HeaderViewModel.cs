using PDV.Services.Interfaces;
using PDV.Services.Retaguarda;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PDV.ViewModels
{
    public class HeaderViewModel : ViewModelBase
    {
        private static readonly Dictionary<string, string> PerfisDescricao = new()
        {
            ["admin"] = "Administrador",
            ["administrador"] = "Administrador",
            ["operario"] = "Operador",
            ["operador"] = "Operador",
            ["gerente"] = "Gerente",
            ["aprovador"] = "Aprovador",
            ["usuario"] = "Usuário",
            ["user"] = "Usuário"
        };

        private readonly IAuthenticationService? _authenticationService;
        private string _usuarioNome = "Usuário";
        private string _usuarioPerfil = "Usuário";

        public string UsuarioNome
        {
            get => _usuarioNome;
            private set => SetProperty(ref _usuarioNome, value);
        }

        public string UsuarioPerfil
        {
            get => _usuarioPerfil;
            private set => SetProperty(ref _usuarioPerfil, value);
        }

        public HeaderViewModel()
        {
            AtualizarUsuario(null);
        }

        public HeaderViewModel(IAuthenticationService? authenticationService)
        {
            _authenticationService = authenticationService;
            if (_authenticationService != null)
                _authenticationService.CurrentSessionChanged += (_, _) => AtualizarUsuario(_authenticationService.CurrentSession);

            AtualizarUsuario(_authenticationService?.CurrentSession);
        }

        private void AtualizarUsuario(RetaguardaAuthSession? session)
        {
            UsuarioNome = FormatarPrimeiraLetraMaiuscula(PrimeiroValorPreenchido(
                session?.Usuario.Nome,
                session?.Usuario.Login,
                session?.Usuario.Email,
                "Usuário"));

            UsuarioPerfil = FormatarPerfil(session?.Usuario.Perfil);
        }

        private static string PrimeiroValorPreenchido(params string?[] valores)
        {
            foreach (var valor in valores)
            {
                if (!string.IsNullOrWhiteSpace(valor))
                    return valor.Trim();
            }

            return "Usuário";
        }

        private static string FormatarPerfil(string? perfil)
        {
            var perfilPreenchido = PrimeiroValorPreenchido(perfil, "Usuário");
            var chave = Normalizar(perfilPreenchido);

            var perfilFormatado = PerfisDescricao.TryGetValue(chave, out var descricao)
                ? descricao
                : perfilPreenchido;

            return FormatarPrimeiraLetraMaiuscula(perfilFormatado);
        }

        private static string FormatarPrimeiraLetraMaiuscula(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var texto = value.Trim().ToLower(CultureInfo.CurrentCulture);
            return char.ToUpper(texto[0], CultureInfo.CurrentCulture) + texto[1..];
        }

        private static string Normalizar(string value)
        {
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
