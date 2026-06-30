using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PDV.Views
{
    public partial class HeaderView : UserControl
    {
        public event Action ToggleMenuRequested;

        public HeaderView()
        {
            InitializeComponent();
        }

        private void ToggleMenu_Click(object sender, RoutedEventArgs e)
        {
            ToggleMenuRequested?.Invoke();
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && textBox.Text == "Buscar...")
            {
                textBox.Text = "";
                textBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Buscar...";
                textBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }
    }
}