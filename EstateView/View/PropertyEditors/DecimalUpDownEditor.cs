using EstateView.Attributes;
using System.Windows.Data;
using Xceed.Wpf.Toolkit;

namespace EstateView.View.PropertyEditors
{
    public class DecimalUpDownEditor : Xceed.Wpf.Toolkit.PropertyGrid.Editors.DecimalUpDownEditor
    {
        public override System.Windows.FrameworkElement ResolveEditor(Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem propertyItem)
        {
            System.Windows.FrameworkElement frameworkElement = base.ResolveEditor(propertyItem);

            foreach (object attribute in propertyItem.Instance.GetType().GetProperty(propertyItem.PropertyDescriptor.Name).GetCustomAttributes(true))
            {
                if (attribute is MinimumValueAttribute)
                {
                    this.Editor.Minimum = (attribute as MinimumValueAttribute).Minimum;
                }
                else if (attribute is MaximumValueAttribute)
                {
                    if (!string.IsNullOrEmpty((attribute as MaximumValueAttribute).PropertyName))
                    {
                        Binding binding = new Binding((attribute as MaximumValueAttribute).PropertyName);
                        binding.Source = propertyItem.Instance;
                        BindingOperations.SetBinding(this.Editor, DecimalUpDown.MaximumProperty, binding);
                    }
                    else
                    {
                        this.Editor.Maximum = (attribute as MaximumValueAttribute).Maximum;    
                    }
                }
                else if (attribute is IncrementValueAttribute)
                {
                    this.Editor.Increment = (attribute as IncrementValueAttribute).Increment;
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
