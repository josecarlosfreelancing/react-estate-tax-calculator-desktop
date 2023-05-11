using System.Windows;
using System.Windows.Media;

namespace EstateView.Utilities
{
    public static class ControlHelper
    {
        public static TChild GetChildOfType<TChild>(DependencyObject parent)
            where TChild : DependencyObject
        {
            if (parent == null)
            {
                return null;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                TChild result = (child as TChild) ?? ControlHelper.GetChildOfType<TChild>(child);
                if (result != null)
                {
                    return result;
                }
            }
            
            return null;
        }
    }
}