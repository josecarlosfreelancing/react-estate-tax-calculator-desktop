using System.Windows;

namespace EstateView.View
{
    /// <summary>
    /// Interaction logic for PercentageOptionView.xaml
    /// </summary>
    public partial class PercentageOptionView
    {
        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(decimal), typeof(PercentageOptionView));
        public static DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(decimal), typeof(PercentageOptionView), new PropertyMetadata(0M));
        public static DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(decimal), typeof(PercentageOptionView), new PropertyMetadata(1M));
        public static DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(decimal), typeof(PercentageOptionView), new PropertyMetadata(0.01M));

        public PercentageOptionView()
        {
            this.InitializeComponent();
        }

        public decimal Value
        {
            get
            {
                return (decimal)this.GetValue(PercentageOptionView.ValueProperty);
            }
            set
            {
                this.SetValue(PercentageOptionView.ValueProperty, value);
            }
        }

        public decimal Minimum
        {
            get
            {
                return (decimal)this.GetValue(PercentageOptionView.MinimumProperty);
            }
            set
            {
                this.SetValue(PercentageOptionView.MinimumProperty, value);
            }
        }

        public decimal Maximum
        {
            get
            {
                return (decimal)this.GetValue(PercentageOptionView.MaximumProperty);
            }
            set
            {
                this.SetValue(PercentageOptionView.MaximumProperty, value);
            }
        }

        public decimal Increment
        {
            get
            {
                return (decimal)this.GetValue(PercentageOptionView.IncrementProperty);
            }
            set
            {
                this.SetValue(PercentageOptionView.IncrementProperty, value);
            }
        }
    }
}
