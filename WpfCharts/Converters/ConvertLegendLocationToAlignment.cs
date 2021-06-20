using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfCharts.Converters
{
    public class ConvertLegendLocationToAlignment : MarkupExtension, IValueConverter
    {
        private static ConvertLegendLocationToAlignment instance;

        #region Overrides of MarkupExtension

        /// <summary>
        /// When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension. 
        /// </summary>
        /// <returns>
        /// The object value to set on the property where the extension is applied. 
        /// </returns>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return instance ?? (instance = new ConvertLegendLocationToAlignment());
        }

        #endregion

        #region Implementation of IValueConverter

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is LegendLocation)) return null;
            var legendLocation = (LegendLocation) value;
            var horAlignment = string.IsNullOrEmpty((string) parameter) || ((string) parameter).StartsWith("H");
            if (horAlignment)
                switch (legendLocation)
                {
                    case LegendLocation.TopLeft:
                    case LegendLocation.BottomLeft:
                        return HorizontalAlignment.Left;
                    case LegendLocation.TopRight:
                    case LegendLocation.BottomRight:
                        return HorizontalAlignment.Right;
                }
            else
                switch (legendLocation)
                {
                    case LegendLocation.TopLeft:
                    case LegendLocation.TopRight:
                        return VerticalAlignment.Top;
                    case LegendLocation.BottomLeft:
                    case LegendLocation.BottomRight:
                        return VerticalAlignment.Bottom;
                }
            return null;
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}