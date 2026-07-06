using System.Windows;
using System.Windows.Controls;
using PDV.Services;
using PDV.Services.Interfaces;
using PDV.Views;

namespace PDV
{
    public partial class MainWindow : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILocalTenantService _localTenantService;
        private readonly PdvCashSessionState _cashSessionState;
        private readonly IRetaguardaSyncCoordinator _syncCoordinator;
        private readonly IPdvUpdateLauncher _updateLauncher;
        public IAlertService AlertService { get; }

        public MainWindow(
            IAuthenticationService authenticationService,
            IAlertService alertService,
            ILocalTenantService localTenantService,
            PdvCashSessionState cashSessionState,
            IRetaguardaSyncCoordinator syncCoordinator,
            IPdvUpdateLauncher updateLauncher)
        {
            _authenticationService = authenticationService;
            _localTenantService = localTenantService;
            _cashSessionState = cashSessionState;
            _syncCoordinator = syncCoordinator;
            _updateLauncher = updateLauncher;
            AlertService = alertService;
            InitializeComponent();

            DataContext = this;
            NavigateToLogin();
        }

        public void NavigateToLogin()
        {
            MainContent.Content = new LoginView(_authenticationService, AlertService, _localTenantService, _syncCoordinator);
        }

        public void NavigateToCadastroInicial()
        {
            MainContent.Content = new CadastroInicialView(
                _authenticationService,
                AlertService,
                NavigateToLogin,
                NavigateToLogin);
        }

        public void NavigateToEsqueceuSenha()
        {
            MainContent.Content = new EsqueceuSenhaView(
                _authenticationService,
                AlertService,
                NavigateToLogin);
        }

        public void NavigateToHome()
        {
            MainContent.Content = new HomeView();
        }

        public void NavigateToMain()
        {
            MainContent.Content = new MainView();
        }

        private void DismissAlert_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Models.AlertModel alert)
            {
                AlertService.DismissAlert(alert.Id);
            }
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                DragMove();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!_cashSessionState.HasOpenCash)
                return;

            var result = MessageBox.Show(
                "Existe um caixa aberto. Deseja realmente fechar o sistema?",
                "Caixa aberto",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning,
                MessageBoxResult.No);

            if (result != MessageBoxResult.Yes)
                e.Cancel = true;
        }

        protected override void OnClosed(System.EventArgs e)
        {
            base.OnClosed(e);
            _updateLauncher.TryLaunchOnExit(_cashSessionState.HasOpenCash);
        }

    }
}
