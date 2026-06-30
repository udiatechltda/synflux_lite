using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using PDV.Models;

namespace PDV.Utilities.Converters
{
    public class MenuItemToStyleConverter : IValueConverter
    {
        public static MenuItemToStyleConverter Instance = new MenuItemToStyleConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MenuItem selectedItem && parameter is FrameworkElement element)
            {
                var dataContext = element.DataContext as MenuItem;
                if (dataContext != null && dataContext == selectedItem)
                {
                    return element.FindResource("SelectedTopMenuButtonStyle");
                }
                return element.FindResource("TopMenuButtonStyle");
            }

            // Fallback caso o parameter não seja FrameworkElement
            return Application.Current.FindResource("TopMenuButtonStyle");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}