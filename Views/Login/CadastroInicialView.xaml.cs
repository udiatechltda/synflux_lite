using PDV.Services.Interfaces;
using PDV.ViewModels.Login;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PDV.Views
{
    public partial class CadastroInicialView : UserControl
    {
        public CadastroInicialView(
            IAuthenticationService authenticationService,
            IAlertService alertService,
            Action? onCancel = null,
            Action? onCompleted = null)
        {
            InitializeComponent();
            DataContext = new CadastroInicialViewModel(authenticationService, alertService, onCancel, onCompleted);
        }

        private void SenhaInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is CadastroInicialViewModel viewModel)
                viewModel.Senha = ((PasswordBox)sender).Password;
        }

        private void ConfirmarSenhaInput_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is CadastroInicialViewModel viewModel)
                viewModel.ConfirmarSenha = ((PasswordBox)sender).Password;
        }

        private void Cancelar_Click(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is CadastroInicialViewModel viewModel && viewModel.CancelarCommand.CanExecute(null))
                viewModel.CancelarCommand.Execute(null);
        }
    }
}
