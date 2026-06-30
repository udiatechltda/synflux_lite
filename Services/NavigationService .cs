using PDV.Services.Interfaces;
using System;
using System.Windows;

namespace PDV.Services
{
    public class NavigationService : INavigationService
    {
        private Window _currentWindow;

        public void NavigateTo<T>() where T : Window
        {
            var newWindow = Activator.CreateInstance<T>();
            newWindow.Show();

            _currentWindow?.Close();
            _currentWindow = newWindow;
        }

        public void ShowDialog<T>() where T : Window
        {
            var dialog = Activator.CreateInstance<T>();
            dialog.Owner = Application.Current.MainWindow;
            dialog.ShowDialog();
        }

        public void CloseCurrent()
        {
            _currentWindow?.Close();
        }
    }
}