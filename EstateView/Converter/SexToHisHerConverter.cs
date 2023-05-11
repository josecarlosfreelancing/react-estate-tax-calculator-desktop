using System;
using System.Globalization;
using System.Windows.Data;
using EstateView.Core.Model;

namespace EstateView.Converter
{
    public class SexToHisHerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Sex? coercedValue = value as Sex?;
            
            if (!coercedValue.HasValue)
            {
                throw new InvalidOperationException("Must bind to a property of type Sex");
            }

            return coercedValue.Value == Sex.Male ? "his" : "her";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}