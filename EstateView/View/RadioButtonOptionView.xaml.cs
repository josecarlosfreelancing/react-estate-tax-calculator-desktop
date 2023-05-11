using System;
using System.Collections.Generic;
using System.Windows;
using EstateView.ViewModel;

namespace EstateView.View
{
    /// <summary>
    /// Interaction logic for RadioButtonOptionView.xaml
    /// </summary>
    public partial class RadioButtonOptionView
    {
        public static DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(RadioButtonOptionView));
        public static DependencyProperty OptionsProperty = DependencyProperty.Register("Options", typeof(IEnumerable<ValueObject>), typeof(RadioButtonOptionView));

        public RadioButtonOptionView()
        {
            this.InitializeComponent();
            this.GroupName = Guid.NewGuid().ToString();
        }

        public string GroupName { get; private set; }

        public object Value
        {
            get { return this.GetValue(RadioButtonOptionView.ValueProperty); }
            set { this.SetValue(RadioButtonOptionView.ValueProperty, value); }
        }

        public IEnumerable<ValueObject> Options
        {
            get { return (IEnumerable<ValueObject>)this.GetValue(RadioButtonOptionView.OptionsProperty); }
            set { this.SetValue(RadioButtonOptionView.OptionsProperty, value); }
        }
    }
}
