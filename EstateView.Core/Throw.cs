using System;
using System.Linq.Expressions;

namespace EstateView.Core
{
    public static class Throw
    {
        public static void IfNull(Expression<Func<object>> expression)
        {
            object value = expression.Compile().Invoke();

            if (value == null)
            {
                string name = ExpressionHelper.GetName(expression);
                throw new ArgumentNullException(name);
            }
        }

        public static void If<T>(Expression<Func<T>> getValueExpression, Func<T, bool> testValueMethod)
        {
            T value = getValueExpression.Compile().Invoke();

            if (testValueMethod(value))
            {
                string name = ExpressionHelper.GetName(getValueExpression);
                throw new ArgumentException(name);
            }
        }

        public static void IfNot<T>(Expression<Func<T>> getValueExpression, Func<T, bool> testValueMethod)
        {
            Throw.If(getValueExpression, x => !testValueMethod(x));
        }
    }
}
