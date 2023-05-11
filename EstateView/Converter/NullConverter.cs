using System;
using System.Globalization;
using System.Windows.Data;

namespace EstateView.Converter
{
    public class NullConverter : IValueConverter
    {
        public object ValueIfNull { get; set; }

        public object ValueIfNotNull { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? this.ValueIfNull : this.ValueIfNotNull;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}