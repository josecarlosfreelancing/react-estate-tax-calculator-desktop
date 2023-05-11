using EstateView.Attributes;

namespace EstateView.View.PropertyEditors
{
    public class IntegerUpDownEditor : Xceed.Wpf.Toolkit.PropertyGrid.Editors.IntegerUpDownEditor
    {
        public override System.Windows.FrameworkElement ResolveEditor(Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem propertyItem)
        {
            System.Windows.FrameworkElement frameworkElement = base.ResolveEditor(propertyItem);

            foreach (object attribute in propertyItem.Instance.GetType().GetProperty(propertyItem.PropertyDescriptor.Name).GetCustomAttributes(true))
            {
                if (attribute is MinimumValueAttribute)
                {
                    this.Editor.Minimum = (int?)(attribute as MinimumValueAttribute).Minimum;
                }
                else if (attribute is MaximumValueAttribute)
                {
                    this.Editor.Maximum = (int?)(attribute as MaximumValueAttribute).Maximum;
                }
                else if (attribute is IncrementValueAttribute)
                {
                    this.Editor.Increment = (int?)(attribute as IncrementValueAttribute).Increment;
                }
                else if (attribute is FormatStringAttribute)
                {
                    this.Editor.FormatString = (attribute as FormatStringAttribute).FormatString;
                }
            }

            return frameworkElement;
        }
    }
}