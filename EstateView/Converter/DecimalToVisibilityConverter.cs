using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EstateView.Converter
{
    public class DecimalToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal? coercedValue = value as decimal?;
            return coercedValue.HasValue && coercedValue.Value > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}