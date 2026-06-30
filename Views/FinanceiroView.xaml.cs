    using PDV.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PDV.Views
{
    public partial class FinanceiroView : UserControl
    {
        public FinanceiroView()
        {
            InitializeComponent();
        }

        private void BtnContasPagar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is FinanceiroViewModel vm)
                vm.AbrirContasPagar();
        }

        private void BtnContasReceber_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is FinanceiroViewModel vm)
                vm.AbrirContasReceber();
        }
    }
}