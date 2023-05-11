using System;

namespace EstateView.Attributes
{
    public class FormatStringAttribute : Attribute
    {
        public FormatStringAttribute(string formatString)
        {
            this.FormatString = formatString;
        }

        public string FormatString { get; private set; }
    }
}