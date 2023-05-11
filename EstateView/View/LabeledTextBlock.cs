using System.Windows;
using System.Windows.Controls;

namespace EstateView.View
{
    public class LabeledTextBlock : Control
    {
        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register("LabelText", typeof(string), typeof(LabeledTextBlock), new PropertyMetadata(null));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(LabeledTextBlock), new PropertyMetadata(null));

        public string LabelText
        {
            get { return (string)this.GetValue(LabeledTextBlock.LabelTextProperty); }
            set { SetValue(LabeledTextBlock.LabelTextProperty, value); }
        }

        public string Text
        {
            get { return (string)this.GetValue(LabeledTextBlock.TextProperty); }
            set { SetValue(LabeledTextBlock.TextProperty, value); }
        }

        public LabeledTextBlock()
        {
            this.DefaultStyleKey = typeof(LabeledTextBlock);
        }
    }
}
