using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EstateView.Converter
{
    public class IntToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int? coercedValue = value as int?;
            return coercedValue.HasValue && coercedValue.Value > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}