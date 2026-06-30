using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace PDV.Views
{
    public partial class SangriaView : UserControl
    {
        public SangriaView()

        {
            InitializeComponent();
        }

        private void ApenasNumeros(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"[\d,\.]");
        }

        private void txtValor_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}