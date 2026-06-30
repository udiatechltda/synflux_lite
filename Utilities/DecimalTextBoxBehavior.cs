using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PDV.Utilities
{
    public static class DecimalTextBoxBehavior
    {
        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.RegisterAttached(
                "Format",
                typeof(string),
                typeof(DecimalTextBoxBehavior),
                new PropertyMetadata(null));

        public static string? GetFormat(DependencyObject obj)
        {
            return (string?)obj.GetValue(FormatProperty);
        }

        public static void SetFormat(DependencyObject obj, string? value)
        {
            obj.SetValue(FormatProperty, value);
        }

        public static void RegisterGlobalFormatting()
        {
            EventManager.RegisterClassHandler(
                typeof(TextBox),
                UIElement.LostFocusEvent,
                new RoutedEventHandler(OnTextBoxLostFocus));
        }

        private static void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is not TextBox textBox)
                return;

            var format = GetFormat(textBox);
            if (string.IsNullOrWhiteSpace(format))
                return;

            textBox.Dispatcher.BeginInvoke(
                () => FormatText(textBox, format),
                DispatcherPriority.ContextIdle);
        }

        private static void FormatText(TextBox textBox, string format)
        {
            var text = textBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(text))
                return;

            textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();

            if (!decimal.TryParse(text, NumberStyles.Number, PdvCulture.Culture, out var value))
                return;

            textBox.SetCurrentValue(TextBox.TextProperty, value.ToString(format, PdvCulture.Culture));
        }
    }
}
