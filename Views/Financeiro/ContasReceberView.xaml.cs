    using PDV.ViewModels;
    using System.Windows;
    using System.Windows.Controls;

    namespace PDV.Views
    {
        public partial class ContasReceberView : UserControl
        {
            public ContasReceberView()
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
                if (DataContext is ContasReceberViewModel vm)
                    vm.Voltar();
            }
        }
    }