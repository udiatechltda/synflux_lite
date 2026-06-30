using PDV.Utilities.Converters;
using PDV.Services;
using PDV.Services.Interfaces;
using System;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;

namespace PDV.ViewModels
{
    public class CadastrosPanelViewModel : ViewModelBase
    {
        private readonly IViewModelNavigationService? _navigationService;
        private ViewModelBase _currentContentView;

        public ViewModelBase CurrentContentView
        {
            get => _currentContentView;
            set => SetProperty(ref _currentContentView, value);
        }

        public ICommand NavigateCommand { get; }

        public CadastrosPanelViewModel() : this(null) { }

        public CadastrosPanelViewModel(IViewModelNavigationService? navigationService)
        {
            _navigationService = navigationService;
            NavigateCommand = new RelayCommand<string>(ExecuteNavigate);
            
            // Assinar evento de navegação DENTRO do Painel de Cadastros
            var navService = navigationService ?? App.ServiceProvider?.GetService(typeof(IViewModelNavigationService)) as IViewModelNavigationService;
            if (navService != null)
            {
                navService.CurrentViewModelChanged += OnCurrentViewModelChanged;
            }
        }

        private void OnCurrentViewModelChanged(ViewModelBase newViewModel)
        {
            // Quando navegação acontece DENTRO dos cadastros, renderizar aqui
            // MAS: se o ViewModel é CadastrosPanelViewModel (navegação de volta para o painel), ignorar
            if (newViewModel is not CadastrosPanelViewModel)
            {
                CurrentContentView = newViewModel;
            }
        }

        private void ExecuteNavigate(string destination)
        {
            if (string.IsNullOrEmpty(destination)) return;

            var navService = _navigationService
                ?? App.ServiceProvider?.GetService(typeof(IViewModelNavigationService)) as IViewModelNavigationService;

            navService?.NavigateTo(destination);
        }
    }
}
