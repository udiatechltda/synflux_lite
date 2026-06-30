using System;
using System.Threading.Tasks;
using PDV.Services.Retaguarda;

namespace PDV.Services.Interfaces
{
    public interface IAuthenticationService
    {
        event EventHandler? CurrentSessionChanged;
        RetaguardaAuthSession? CurrentSession { get; }
        Task<RetaguardaAuthResponse?> CreateAccountAsync(RetaguardaCreateAccountRequest request);
        Task<bool> AuthenticateAsync(string username, string password);
        Task<RetaguardaPasswordRecoveryResponse> RecoverPasswordAsync(string usernameOrEmail);
        Task<bool> SendRegistrationEmailAsync(string cnpj, string login);
        Task<bool> ConfirmRegistrationCodeAsync(string cnpj, string login, string confirmationCode);
    }
}
