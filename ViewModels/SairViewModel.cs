using PDV.Commands;
using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Windows;
using System.Windows.Input;

namespace PDV.ViewModels
{
    public class SairViewModel : ViewModelBase
    {
        private readonly IViewModelNavigationService? _navigationService;
        private readonly IPdvUpdateLauncher? _updateLauncher;
        private readonly PdvCashSessionState? _cashSessionState;

        public ICommand VoltarCommand { get; }
        public ICommand ConfirmarSairCommand { get; }
        public ICommand CancelarCommand { get; }

        public SairViewModel()
        {
            VoltarCommand = new RelayCommand(ExecuteVoltar);
            ConfirmarSairCommand = new RelayCommand(ExecuteConfirmarSair);
            CancelarCommand = new RelayCommand(ExecuteCancelar);
        }

        public SairViewModel(
            IViewModelNavigationService navigationService,
            IPdvUpdateLauncher updateLauncher,
            PdvCashSessionState cashSessionState) : this()
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _updateLauncher = updateLauncher;
            _cashSessionState = cashSessionState;
        }

        private void ExecuteVoltar()
        {
            _navigationService?.NavigateTo<HomeViewModel>();
        }

        private void ExecuteConfirmarSair()
        {
            var result = MessageBox.Show(
                "Deseja realmente sair da aplicação?",
                "Confirmar Saída",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question,
                MessageBoxResult.No);

            if (result == MessageBoxResult.Yes)
            {
                _updateLauncher?.TryLaunchOnExit(_cashSessionState?.HasOpenCash ?? false);
                Application.Current.Shutdown();
            }
        }

        private void ExecuteCancelar()
        {
            _navigationService?.NavigateTo<HomeViewModel>();
        }
    }
}