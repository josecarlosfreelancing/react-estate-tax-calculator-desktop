using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using EstateView.Core;

namespace EstateView.ViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly Dictionary<string, object> propertyValues;

        public ViewModel()
        {
            this.propertyValues = new Dictionary<string, object>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected T GetValue<T>(Expression<Func<T>> propertyExpression)
        {
            string propertyName = ExpressionHelper.GetName(propertyExpression);
            object value;
            
            if (this.propertyValues.TryGetValue(propertyName, out value))
            {
                return (T)value;
            }

            return default(T);
        }

        protected void SetValue<T>(Expression<Func<T>> propertyExpression, T value)
        {
            string propertyName = ExpressionHelper.GetName(propertyExpression);
            this.propertyValues[propertyName] = value;
            this.NotifyPropertyChanged(propertyExpression);
        }

        protected virtual void NotifyPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            this.NotifyPropertyChanged(ExpressionHelper.GetName(propertyExpression));
        }

        protected virtual void NotifyPropertyChanged()
        {
            this.NotifyPropertyChanged(string.Empty);
        }

        protected void RelayPropertyChanged(INotifyPropertyChanged child)
        {
            child.PropertyChanged -= this.RelayPropertyChanged;
            child.PropertyChanged += this.RelayPropertyChanged;
        }

        private void RelayPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.NotifyPropertyChanged();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
