using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace PDV.Views
{
    /// <summary>
    /// Interaction logic for MovimentoView.xaml
    /// </summary>
    public partial class MovimentoView : UserControl
    {
        public MovimentoView()
        {
            InitializeComponent();
        }

        private bool _isUpdating = false;

        private void ApenasNumeros(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(char.IsDigit);
        }

        private void txtValor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isUpdating) return;
            _isUpdating = true;

            var tb = (TextBox)sender;

            string apenasDigitos = new string(tb.Text.Where(char.IsDigit).ToArray());

            if (string.IsNullOrEmpty(apenasDigitos))
            {
                tb.Text = "";
                tb.CaretIndex = 0;
                _isUpdating = false;
                return;
            }

            long valor = long.Parse(apenasDigitos);
            decimal valorDecimal = valor / 100m;

            string formatado = valorDecimal.ToString("N2",
                new System.Globalization.CultureInfo("pt-BR"));

            tb.Text = formatado;
            tb.CaretIndex = tb.Text.Length;

            _isUpdating = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}