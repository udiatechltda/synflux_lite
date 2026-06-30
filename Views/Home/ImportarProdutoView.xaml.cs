using PDV.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PDV.Views
{
    public partial class ImportarProdutoView : UserControl
    {
        public ImportarProdutoView()
        {
            InitializeComponent();
            DataContext = new ImportarProdutoViewModel();
            Loaded += (_, _) => TxtPesquisa.Focus();
        }

        public ImportarProdutoView(ImportarProdutoViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            Loaded += (_, _) => TxtPesquisa.Focus();
        }

        private void BtnScrollEsquerda_Click(object sender, RoutedEventArgs e)
        {
            svDataGrid.ScrollToHorizontalOffset(svDataGrid.HorizontalOffset - 150);
        }

        private void BtnScrollDireita_Click(object sender, RoutedEventArgs e)
        {
            svDataGrid.ScrollToHorizontalOffset(svDataGrid.HorizontalOffset + 150);
        }
    }
}
