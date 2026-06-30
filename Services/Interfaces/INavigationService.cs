using System;
using System.Windows;

namespace PDV.Services.Interfaces
{
    public interface INavigationService
    {
        void NavigateTo<T>() where T : Window;
        void ShowDialog<T>() where T : Window;
        void CloseCurrent();
    }
}