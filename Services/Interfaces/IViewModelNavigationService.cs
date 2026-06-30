using PDV.ViewModels;
using System;

namespace PDV.Services.Interfaces
{
    public interface IViewModelNavigationService
    {
        event Action<ViewModelBase> CurrentViewModelChanged;
        ViewModelBase CurrentViewModel { get; }
        void NavigateTo<T>() where T : ViewModelBase;
        void NavigateTo(string viewName);
        void NavigateTo(ViewModelBase viewModel);
    }
}