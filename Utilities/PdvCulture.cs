using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace PDV.Utilities
{
    public static class PdvCulture
    {
        public static readonly CultureInfo Culture = CultureInfo.GetCultureInfo("pt-BR");

        public static void Configure()
        {
            CultureInfo.DefaultThreadCurrentCulture = Culture;
            CultureInfo.DefaultThreadCurrentUICulture = Culture;

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(Culture.IetfLanguageTag)));

            DecimalTextBoxBehavior.RegisterGlobalFormatting();
        }
    }
}
