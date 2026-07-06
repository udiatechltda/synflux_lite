using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PDV.Services.Interfaces;
using PDV.ViewModels;

namespace PDV.Views
{
    public partial class LoginView : UserControl
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAlertService _alertService;
        private readonly ILocalTenantService _localTenantService;
        private readonly IRetaguardaSyncCoordinator _syncCoordinator;

        public LoginView(IAuthenticationService authenticationService, IAlertService alertService, ILocalTenantService localTenantService, IRetaguardaSyncCoordinator syncCoordinator)
        {
            _authenticationService = authenticationService;
            _alertService = alertService;
            _localTenantService = localTenantService;
            _syncCoordinator = syncCoordinator;
            InitializeComponent();
            DataContext = new LoginViewModel(authenticationService, alertService, localTenantService, syncCoordinator);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UsuarioInput.Focus();
        }

        private void UsuarioInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrEmpty(UsuarioInput.Text))
            {
                PasswordInput.Focus();
            }
        }

        private void PasswordInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && DataContext is LoginViewModel viewModel && viewModel.LoginCommand.CanExecute(null))
            {
                viewModel.LoginCommand.Execute(null);
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.Senha = ((PasswordBox)sender).Password;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void EsqueceuSenha_Click(object sender, MouseButtonEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
                mainWindow.NavigateToEsqueceuSenha();
        }

        private void Cadastrar_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
                mainWindow.NavigateToCadastroInicial();
        }

        private void LoadingOverlay_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
