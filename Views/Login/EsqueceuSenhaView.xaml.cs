using PDV.Services.Interfaces;
using PDV.ViewModels.Login;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PDV.Views
{
    public partial class EsqueceuSenhaView : UserControl
    {
        public EsqueceuSenhaView(
            IAuthenticationService authenticationService,
            IAlertService alertService,
            Action? onCancel = null)
        {
            InitializeComponent();
            DataContext = new EsqueceuSenhaViewModel(authenticationService, alertService, onCancel);
            Loaded += (_, _) => EmailUsuarioInput.Focus();
        }

        private void EmailUsuarioInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && DataContext is EsqueceuSenhaViewModel viewModel && viewModel.ProximoCommand.CanExecute(null))
            {
                viewModel.ProximoCommand.Execute(null);
            }
        }

        private void Cancelar_Click(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is EsqueceuSenhaViewModel viewModel && viewModel.CancelarCommand.CanExecute(null))
            {
                viewModel.CancelarCommand.Execute(null);
            }
        }
    }
}
