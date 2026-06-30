using System.Windows;
using PDV.Services.Interfaces;

namespace PDV.Views
{
    public partial class PdvFeatureEditDialog : Window
    {
        public PdvFeatureEditDialog(PdvFeatureEditModel model)
        {
            InitializeComponent();
            DataContext = model;
        }

        public PdvFeatureEditModel Model => (PdvFeatureEditModel)DataContext;

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
