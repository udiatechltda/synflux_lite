using System.Windows;
using System.Windows.Controls;
using PDV.ViewModels;
using PDV.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace PDV.Views
{
    public partial class MainView : UserControl
    {
        private bool menuAberto = true;

        public MainView()
        {
            InitializeComponent();
            
            // Resolve MainViewModel com injeção de dependência - usar SINGLETON do container
            var mainViewModel = App.ServiceProvider?.GetService<MainViewModel>();
            var navigationService = App.ServiceProvider?.GetService<IViewModelNavigationService>();
            this.DataContext = mainViewModel ?? new MainViewModel(navigationService);

            HeaderView.BtnMenu.Click += ToggleMenu_Click;

            // MENU INICIAL
            MenuViewControl_Loaded();
        }

        private void ToggleMenu_Click(object sender, RoutedEventArgs e)
        {
            if (menuAberto)
            {
                MenuColumn.Width = new GridLength(0);
                menuAberto = false;
            }
            else
            {
                MenuColumn.Width = new GridLength(200);
                menuAberto = true;
            }
        }

        // MENU PADRÃO AO INICIAR
        private void MenuViewControl_Loaded()
        {
            MenuViewControl.OpcoesCaixaPanel.Visibility = Visibility.Visible;
            MenuViewControl.ModulosPanel.Visibility = Visibility.Collapsed;
            MenuViewControl.OutrosPanel.Visibility = Visibility.Collapsed;
        }

        // BOTÃO 1 → OPÇÕES DE CAIXA
        private void BtnCaixa_Click(object sender, RoutedEventArgs e)
        {
            MenuViewControl.OpcoesCaixaPanel.Visibility = Visibility.Visible;
            MenuViewControl.ModulosPanel.Visibility = Visibility.Collapsed;
            MenuViewControl.OutrosPanel.Visibility = Visibility.Collapsed;
        }

        // BOTÃO 2 → MÓDULOS
        private void BtnModulos_Click(object sender, RoutedEventArgs e)
        {
            MenuViewControl.OpcoesCaixaPanel.Visibility = Visibility.Collapsed;
            MenuViewControl.ModulosPanel.Visibility = Visibility.Visible;
            MenuViewControl.OutrosPanel.Visibility = Visibility.Collapsed;
        }

        // BOTÃO 3 → OUTROS
        private void BtnOutros_Click(object sender, RoutedEventArgs e)
        {
            MenuViewControl.OpcoesCaixaPanel.Visibility = Visibility.Collapsed;
            MenuViewControl.ModulosPanel.Visibility = Visibility.Collapsed;
            MenuViewControl.OutrosPanel.Visibility = Visibility.Visible;
        }

        private void HeaderView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void MenuView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void MenuViewControl_Loaded_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
