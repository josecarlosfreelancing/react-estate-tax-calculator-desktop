using System;

namespace EstateView.Attributes
{
    public class IncrementValueAttribute : Attribute
    {
        public IncrementValueAttribute(int increment)
        {
            this.Increment = increment;
        }

        public IncrementValueAttribute(double increment)
        {
            this.Increment = new decimal(increment);
        }

        public decimal Increment { get; private set; }
    }
}