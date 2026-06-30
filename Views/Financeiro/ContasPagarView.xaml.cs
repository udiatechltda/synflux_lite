using PDV.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PDV.Views
{
    public partial class ContasPagarView : UserControl
    {
        public ContasPagarView()
        {
            InitializeComponent();
        }

        private void BtnToggleTotais_Click(object sender, RoutedEventArgs e)
        {
            PanelTotais.Visibility = PanelTotais.Visibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        private void BtnVoltar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ContasPagarViewModel vm)
                vm.Voltar();
        }
    }
}