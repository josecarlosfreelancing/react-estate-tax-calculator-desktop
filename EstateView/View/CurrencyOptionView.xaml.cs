using System.Windows;

namespace EstateView.View
{
    /// <summary>
    /// Interaction logic for CurrencyOptionView.xaml
    /// </summary>
    public partial class CurrencyOptionView : OptionView
    {
        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(decimal), typeof(CurrencyOptionView));
        public static DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(decimal), typeof(CurrencyOptionView), new PropertyMetadata(decimal.MinValue));
        public static DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(decimal), typeof(CurrencyOptionView), new PropertyMetadata(decimal.MaxValue));
        public static DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(decimal), typeof(CurrencyOptionView), new PropertyMetadata(1M));

        public CurrencyOptionView()
        {
            this.InitializeComponent();
        }

        public decimal Value
        {
            get
            {
                return (decimal)this.GetValue(CurrencyOptionView.ValueProperty);
            }
            set
            {
                this.SetValue(CurrencyOptionView.ValueProperty, value);
            }
        }

        public decimal Minimum
        {
            get
            {
                return (decimal)this.GetValue(CurrencyOptionView.MinimumProperty);
            }
            set
            {
                this.SetValue(CurrencyOptionView.MinimumProperty, value);
            }
        }
        
        public decimal Maximum
        {
            get
            {
                return (decimal)this.GetValue(CurrencyOptionView.MaximumProperty);
            }
            set
            {
                this.SetValue(CurrencyOptionView.MaximumProperty, value);
            }
        }

        public decimal Increment
        {
            get
            {
                return (decimal)this.GetValue(CurrencyOptionView.IncrementProperty);
            }
            set
            {
                this.SetValue(CurrencyOptionView.IncrementProperty, value);
            }
        }
    }
}
