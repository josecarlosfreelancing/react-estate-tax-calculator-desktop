using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EstateView.Converter
{
    /// <summary>
    /// Produces an output value that is the negative of the input.
    /// </summary>
    /// <remarks>
    /// The built-in signed types are supported as well as a handful of other
    /// commonly used types such as <see cref="Point"/>, <see cref="TimeSpan"/>,
    /// <see cref="Thickness"/>, etc.
    /// </remarks>
    public sealed class NegateConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (value is double)
            {
                return this.Negate((double)value);
            }

            if (value is int)
            {
                return this.Negate((int)value);
            }

            if (value is bool)
            {
                return this.Negate((bool)value);
            }

            if (value is long)
            {
                return this.Negate((long)value);
            }

            if (value is IConvertible)
            {
                return this.Negate((IConvertible)value, culture);
            }

            if (value is TimeSpan)
            {
                return this.Negate((TimeSpan)value);
            }

            if (value is Point)
            {
                return this.Negate((Point)value);
            }

            if (value is Thickness)
            {
                return this.Negate((Thickness)value);
            }

            throw new ArgumentException("Cannot negate " + value.GetType() + ".", "value");
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Convert(value, targetType, parameter, culture);
        }

        /// <summary>
        /// Negates a <see cref="TimeSpan"/> value.
        /// </summary>
        /// <param name="value">The value to negate.</param>
        /// <returns>The negated value.</returns>
        private TimeSpan Negate(TimeSpan value)
        {
            return value.Negate();
        }

        /// <summary>
        /// Negates a <see cref="Point"/> value.
        /// </summary>
        /// <param name="value">The value to negate.</param>
        /// <returns>The negated value.</returns>
        private Point Negate(Point value)
        {
            return new Point(
                -value.X,
                -value.Y);
        }

        /// <summary>
        /// Negates a <see cref="Thickness"/> value.
        /// </summary>
        /// <param name="value">The value to negate.</param>
        /// <returns>The negated value.</returns>
        private Thickness Negate(Thickness value)
        {
            return new Thickness(
                -value.Left,
                -value.Top,
                -value.Right,
                -value.Bottom);
        }

        /// <summary>
        /// Negates a <see cref="Boolean"/> value.
        /// </summary>
        /// <param name="value">The value to negate.</param>
        /// <returns>The negated value.</returns>
        private bool Negate(bool value)
        {
            return !value;
        }

        /// <summary>
        /// Negates a <see cref="Int32"/> value.
        /// </summary>
        /// <param name="value">The value to negate.</param>
        /// <returns>The negated value.</returns>
        private int Negate(int value)
        {
            return -value;
        }

        /// <summary>
        /// Negates a <see cref="Int64"/> value.
        /// </summary>
        /// <param name="value">The value to negate.</param>
        /// <returns>The negated value.</returns>
        private long Negate(long value)
        {
            return -value;
        }

        /// <summary>
        /// Negates a <see cref="Double"/> value.
        /// </summary>
        /// <param name="value">The value to negate.</param>
        /// <returns>The negated value.</returns>
        private double Negate(double value)
        {
            return -value;
        }

        /// <summary>
        /// Negates a <see cref="IConvertible"/> value by round tripping through
        /// the System.Decimal type.
        /// </summary>
        /// <param name="value">The value to negate.</param>
        /// <param name="formatProvider">The culture information.</param>
        /// <returns>The negated value as the original input type.</returns>
        private object Negate(IConvertible value, IFormatProvider formatProvider)
        {
            TypeCode inputType = value.GetTypeCode();

            decimal input = value.ToDecimal(formatProvider);
            decimal output = Decimal.Negate(input);

            return System.Convert.ChangeType(output, inputType, formatProvider);
        }
    }
}
