using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfCharts.Converters
{
    public class ConvertBoolToVisibility : MarkupExtension, IValueConverter
    {
        private static ConvertBoolToVisibility converter;

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var visibility = true;
            if (parameter != null) bool.TryParse(parameter as string, out visibility);
            return ((bool)value == visibility) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotImplementedException(); }

        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider) { return converter ?? (converter = new ConvertBoolToVisibility()); }
    }
}
