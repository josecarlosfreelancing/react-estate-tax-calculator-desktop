using System;
using System.Linq.Expressions;

namespace EstateView.Core
{
    public static class ExpressionHelper
    {
        public static string GetName<T>(Expression<Func<T>> expression)
        {
            MemberExpression body = expression.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)expression.Body;
                body = (MemberExpression)ubody.Operand;
            }

            return body.Member.Name;
        }
    }
}