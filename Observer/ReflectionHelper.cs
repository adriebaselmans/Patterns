using System;
using System.Linq.Expressions;

namespace Observer
{
    public static class ReflectionHelper
    {
        public static string GetPropertyName(Expression<Func<object>> exp)
        {
            var body = exp.Body as MemberExpression;

            if (body != null)
            {
                return body.Member.Name;
            }
            else
            {
                var ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;

                if (body != null)
                {
                    return body.Member.Name;
                }
            }

            return null;
        }        
    }
}