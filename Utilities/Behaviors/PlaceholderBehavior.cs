using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PDV.Utilities.Behaviors
{
    public static class PlaceholderBehavior
    {
        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.RegisterAttached("PlaceholderText", typeof(string), typeof(PlaceholderBehavior),
                new PropertyMetadata(string.Empty, OnPlaceholderTextChanged));

        public static readonly DependencyProperty PlaceholderBrushProperty =
            DependencyProperty.RegisterAttached("PlaceholderBrush", typeof(Brush), typeof(PlaceholderBehavior),
                new PropertyMetadata(Brushes.Gray));

        public static string GetPlaceholderText(DependencyObject obj)
        {
            return (string)obj.GetValue(PlaceholderTextProperty);
        }

        public static void SetPlaceholderText(DependencyObject obj, string value)
        {
            obj.SetValue(PlaceholderTextProperty, value);
        }

        public static Brush GetPlaceholderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(PlaceholderBrushProperty);
        }

        public static void SetPlaceholderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(PlaceholderBrushProperty, value);
        }

        private static void OnPlaceholderTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if (e.NewValue is string placeholderText && !string.IsNullOrEmpty(placeholderText))
                {
                    textBox.GotFocus += RemovePlaceholder;
                    textBox.LostFocus += ShowPlaceholder;

                    if (string.IsNullOrEmpty(textBox.Text))
                    {
                        ShowPlaceholder(textBox, placeholderText);
                    }
                }
            }
            else if (d is PasswordBox passwordBox)
            {
                if (e.NewValue is string placeholderText && !string.IsNullOrEmpty(placeholderText))
                {
                    passwordBox.GotFocus += RemovePasswordPlaceholder;
                    passwordBox.LostFocus += ShowPasswordPlaceholder;

                    if (string.IsNullOrEmpty(passwordBox.Password))
                    {
                        ShowPasswordPlaceholder(passwordBox, placeholderText);
                    }
                }
            }
        }

        private static void ShowPlaceholder(TextBox textBox, string placeholderText)
        {
            textBox.Text = placeholderText;
            textBox.Foreground = GetPlaceholderBrush(textBox);
        }

        private static void RemovePlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string placeholderText = GetPlaceholderText(textBox);
                if (textBox.Text == placeholderText)
                {
                    textBox.Text = string.Empty;
                    textBox.Foreground = SystemColors.WindowTextBrush;
                }
            }
        }

        private static void ShowPlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string placeholderText = GetPlaceholderText(textBox);
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    ShowPlaceholder(textBox, placeholderText);
                }
            }
        }

        private static void ShowPasswordPlaceholder(PasswordBox passwordBox, string placeholderText)
        {
            // Para PasswordBox, precisamos usar um hack visual
            var placeholderTextBlock = GetPasswordPlaceholderTextBlock(passwordBox);
            if (placeholderTextBlock != null)
            {
                placeholderTextBlock.Text = placeholderText;
                placeholderTextBlock.Visibility = Visibility.Visible;
            }
        }

        private static void RemovePasswordPlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                var placeholderTextBlock = GetPasswordPlaceholderTextBlock(passwordBox);
                if (placeholderTextBlock != null)
                {
                    placeholderTextBlock.Visibility = Visibility.Collapsed;
                }
            }
        }

        private static void ShowPasswordPlaceholder(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                var placeholderTextBlock = GetPasswordPlaceholderTextBlock(passwordBox);
                if (placeholderTextBlock != null && string.IsNullOrEmpty(passwordBox.Password))
                {
                    placeholderTextBlock.Visibility = Visibility.Visible;
                }
            }
        }

        private static TextBlock GetPasswordPlaceholderTextBlock(PasswordBox passwordBox)
        {
            // Este método precisa ser implementado com um template customizado
            return null;
        }
    }
}