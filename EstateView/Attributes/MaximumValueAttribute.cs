using System;

namespace EstateView.Attributes
{
    public class MaximumValueAttribute : Attribute
    {
        public MaximumValueAttribute(int maximum)
        {
            this.Maximum = maximum;
        }

        public MaximumValueAttribute(double maximum)
        {
            this.Maximum = new decimal(maximum);
        }

        public MaximumValueAttribute(string propertyName)
        {
            this.PropertyName = propertyName;
        }

        public decimal Maximum { get; private set; }

        public string PropertyName { get; private set; }
    }
}