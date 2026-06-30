using PDV.Services;
using PDV.Services.Interfaces;

namespace PDV.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentContentView;
        private readonly IViewModelNavigationService _navigationService;
        private readonly IPdvOperationService? _pdvService;
        private readonly IAuthenticationService? _authenticationService;
        private readonly IAlertService? _alertService;

        public ViewModelBase CurrentContentView
        {
            get => _currentContentView;
            set => SetProperty(ref _currentContentView, value);
        }

        public MenuViewModel Menu { get; private set; } = new MenuViewModel();
        public HeaderViewModel Header { get; private set; } = new HeaderViewModel();

        public MainViewModel(
            IViewModelNavigationService navigationService,
            IPdvOperationService pdvService,
            IAuthenticationService authenticationService,
            IAlertService alertService)
        {
            _navigationService = navigationService;
            _pdvService = pdvService;
            _authenticationService = authenticationService;
            _alertService = alertService;

            InitializeComponents();
            ConnectNavigation();
            NavigateInitial();
        }

        public MainViewModel(IViewModelNavigationService navigationService = null)
        {
            _navigationService = navigationService ?? new ViewModelNavigationService();

            InitializeComponents();
            ConnectNavigation();
            _navigationService.NavigateTo("Home");
        }

        private void InitializeComponents()
        {
            Menu = new MenuViewModel(_navigationService, _pdvService, _authenticationService, _alertService);
            Header = new HeaderViewModel(_authenticationService);
        }

        private void ConnectNavigation()
        {
            Menu.MenuItemSelected += OnMenuItemSelected;
            _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnMenuItemSelected(string viewName)
        {
            _navigationService.NavigateTo(viewName);
        }

        private void OnCurrentViewModelChanged(ViewModelBase newViewModel)
        {
            CurrentContentView = newViewModel;
        }

        private void NavigateInitial()
        {
            if (_pdvService == null)
            {
                _navigationService.NavigateTo("Home");
                return;
            }

            _navigationService.NavigateTo(PdvAccessPolicy.GetInitialRoute(_authenticationService, _pdvService));
        }
    }
}
