using System;

namespace EstateView.Attributes
{
    public class MinimumValueAttribute : Attribute
    {
        public MinimumValueAttribute(int minimum)
        {
            this.Minimum = minimum;
        }

        public MinimumValueAttribute(double minimum)
        {
            this.Minimum = new decimal(minimum);
        }

        public decimal Minimum { get; private set; }
    }
}
