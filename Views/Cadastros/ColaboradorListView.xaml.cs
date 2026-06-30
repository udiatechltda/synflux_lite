using System.Windows;
using System.Windows.Controls;

namespace PDV.Views.Cadastros
{
    public partial class ColaboradorListView : UserControl
    {
        public ColaboradorListView() { InitializeComponent(); }

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
