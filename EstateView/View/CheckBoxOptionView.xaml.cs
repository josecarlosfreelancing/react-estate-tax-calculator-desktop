using System.Windows;

namespace EstateView.View
{
    /// <summary>
    /// Interaction logic for CheckBoxOptionView.xaml
    /// </summary>
    public partial class CheckBoxOptionView
    {
        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(bool), typeof(CheckBoxOptionView));

        public CheckBoxOptionView()
        {
            this.InitializeComponent();
        }

        public string Value
        {
            get { return (string)this.GetValue(CheckBoxOptionView.ValueProperty); }
            set { this.SetValue(CheckBoxOptionView.ValueProperty, value); }
        }
    }
}
