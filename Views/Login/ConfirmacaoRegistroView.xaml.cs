using PDV.Services.Interfaces;
using PDV.ViewModels.Login;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PDV.Views
{
    public partial class ConfirmacaoRegistroView : Window
    {
        public ConfirmacaoRegistroView(
            IAuthenticationService authenticationService,
            IAlertService alertService,
            string cnpj,
            string login)
        {
            InitializeComponent();
            var viewModel = new ConfirmacaoRegistroViewModel(authenticationService, alertService, this, cnpj, login);
            viewModel.RegistroConfirmado += FecharAposConfirmacao;
            DataContext = viewModel;
        }

        private void FecharAposConfirmacao()
        {
            Dispatcher.BeginInvoke(() =>
            {
                try
                {
                    if (IsVisible)
                        DialogResult = true;
                }
                catch (InvalidOperationException)
                {
                    if (IsVisible)
                        Close();
                }
            });
        }

        private void CodigoConfirmacaoInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is ConfirmacaoRegistroViewModel viewModel)
                viewModel.TentarConfirmarAutomaticamente();
        }

        private void Fechar_Click(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is ConfirmacaoRegistroViewModel viewModel && viewModel.FecharCommand.CanExecute(null))
                viewModel.FecharCommand.Execute(null);
        }
    }
}
