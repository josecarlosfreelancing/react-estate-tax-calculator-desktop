using System.Windows;

namespace EstateView.View
{
    /// <summary>
    /// Interaction logic for IntegerOptionView.xaml
    /// </summary>
    public partial class IntegerOptionView
    {
        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(IntegerOptionView));
        public static DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(int), typeof(IntegerOptionView), new PropertyMetadata(0));
        public static DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(int), typeof(IntegerOptionView), new PropertyMetadata(int.MaxValue));
        public static DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(int), typeof(IntegerOptionView), new PropertyMetadata(1));

        public IntegerOptionView()
        {
            this.InitializeComponent();
        }

        public int Value
        {
            get
            {
                return (int)this.GetValue(IntegerOptionView.ValueProperty);
            }
            set
            {
                this.SetValue(IntegerOptionView.ValueProperty, value);
            }
        }

        public int Minimum
        {
            get
            {
                return (int)this.GetValue(IntegerOptionView.MinimumProperty);
            }
            set
            {
                this.SetValue(IntegerOptionView.MinimumProperty, value);
            }
        }

        public int Maximum
        {
            get
            {
                return (int)this.GetValue(IntegerOptionView.MaximumProperty);
            }
            set
            {
                this.SetValue(IntegerOptionView.MaximumProperty, value);
            }
        }

        public int Increment
        {
            get
            {
                return (int)this.GetValue(IntegerOptionView.IncrementProperty);
            }
            set
            {
                this.SetValue(IntegerOptionView.IncrementProperty, value);
            }
        }
    }
}
