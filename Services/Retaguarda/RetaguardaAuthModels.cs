using System;
using System.Text.Json.Serialization;

namespace PDV.Services.Retaguarda
{
    public sealed class RetaguardaLoginRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("senha")]
        public string Senha { get; set; } = string.Empty;
    }

    public sealed class RetaguardaCreateAccountRequest
    {
        [JsonPropertyName("cnpj")]
        public string Cnpj { get; set; } = string.Empty;

        [JsonPropertyName("razaoSocial")]
        public string RazaoSocial { get; set; } = string.Empty;

        [JsonPropertyName("nomeFantasia")]
        public string NomeFantasia { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("usuarioNome")]
        public string UsuarioNome { get; set; } = string.Empty;

        [JsonPropertyName("login")]
        public string Login { get; set; } = string.Empty;

        [JsonPropertyName("senha")]
        public string Senha { get; set; } = string.Empty;

        [JsonPropertyName("perfil")]
        public string Perfil { get; set; } = string.Empty;
    }

    public sealed class RetaguardaPasswordRecoveryRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
    }

    public sealed class RetaguardaPasswordRecoveryResponse
    {
        [JsonPropertyName("sucesso")]
        public bool Sucesso { get; set; }

        [JsonPropertyName("senhaAlterada")]
        public bool SenhaAlterada { get; set; }

        [JsonPropertyName("mensagem")]
        public string Mensagem { get; set; } = string.Empty;

        [JsonPropertyName("senhaTemporaria")]
        public string? SenhaTemporaria { get; set; }
    }

    public sealed class RetaguardaCnpjRequest
    {
        [JsonPropertyName("cnpj")]
        public string Cnpj { get; set; } = string.Empty;

        [JsonPropertyName("login")]
        public string Login { get; set; } = string.Empty;
    }

    public sealed class RetaguardaAuthResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("expiraEm")]
        public DateTime ExpiraEm { get; set; }

        [JsonPropertyName("usuario")]
        public RetaguardaUsuarioResponse Usuario { get; set; } = new();

        [JsonPropertyName("empresa")]
        public RetaguardaEmpresaResponse Empresa { get; set; } = new();
    }

    public sealed class RetaguardaUsuarioResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [JsonPropertyName("login")]
        public string Login { get; set; } = string.Empty;

        [JsonPropertyName("perfil")]
        public string Perfil { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("confirmado")]
        public string Confirmado { get; set; } = string.Empty;
    }

    public sealed class RetaguardaEmpresaResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("cnpj")]
        public string Cnpj { get; set; } = string.Empty;

        [JsonPropertyName("razaoSocial")]
        public string RazaoSocial { get; set; } = string.Empty;

        [JsonPropertyName("nomeFantasia")]
        public string NomeFantasia { get; set; } = string.Empty;

        [JsonPropertyName("bancoOperacional")]
        public string BancoOperacional { get; set; } = string.Empty;

        [JsonPropertyName("registrado")]
        public string Registrado { get; set; } = string.Empty;
    }

    public sealed class RetaguardaAuthSession
    {
        public string Token { get; init; } = string.Empty;
        public DateTime ExpiraEm { get; init; }
        public RetaguardaUsuarioResponse Usuario { get; init; } = new();
        public RetaguardaEmpresaResponse Empresa { get; init; } = new();
    }
}
