using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace LINQProvider
{
    public static class ExpressionHelper
    {
        public static object Expand(this ConstantExpression constantExpr) =>
            constantExpr.Value!;

        public static Delegate ExpandAsDelegate(this UnaryExpression unaryExpr) =>
            (unaryExpr.Operand as LambdaExpression)!.Compile();

        public static T AsConstantOfType<T>(this Expression paramExpr)
        {
            return (T)(paramExpr as ConstantExpression)!.Value!;
        }

        public static LambdaExpression ExpandArgumentAsLambda(
            this MethodCallExpression methodExpr,
            int parameterIndex)
        {
            return ((methodExpr.Arguments[parameterIndex] as UnaryExpression)!.Operand as LambdaExpression)!;
        }
    }
}
