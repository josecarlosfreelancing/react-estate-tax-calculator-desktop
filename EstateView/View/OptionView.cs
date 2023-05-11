using System.Windows;
using System.Windows.Controls;

namespace EstateView.View
{
    public class OptionView : ContentControl
    {
        public static DependencyProperty LabelTextProperty = DependencyProperty.Register("LabelText", typeof(string), typeof(OptionView));

        static OptionView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OptionView), new FrameworkPropertyMetadata(typeof(OptionView)));
        }
        
        public string LabelText
        {
            get { return (string)this.GetValue(OptionView.LabelTextProperty); }
            set { this.SetValue(OptionView.LabelTextProperty, value); }
        }
    }
}
