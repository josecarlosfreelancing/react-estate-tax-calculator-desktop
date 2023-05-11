using System;
using System.Windows.Data;

namespace EstateView.Converter
{
    public class DecimalToStringConverter : IValueConverter
    {
        public string PositiveStringFormat { get; set; }
        public string NegativeStringFormat { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            decimal decimalValue = (decimal)value;

            if (decimalValue < 0)
            {
                return string.Format(this.NegativeStringFormat, -decimalValue);
            }
            else
            {
                return string.Format(this.PositiveStringFormat, decimalValue);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
