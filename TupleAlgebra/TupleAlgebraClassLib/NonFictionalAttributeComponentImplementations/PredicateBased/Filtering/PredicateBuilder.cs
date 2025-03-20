using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Diagnostics.CodeAnalysis;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering
{
    public class PredicateBuilder<TData> : ExpressionVisitor
    {
        #region Static fields

        private static ParameterExpression _dataInstanceParam;

        #endregion

        #region Delegates

        private delegate BinaryExpression PredicateConcatHandler(
            Expression op1,
            Expression op2);

        #endregion

        #region Constructors

        static PredicateBuilder()
        {
            _dataInstanceParam = Expression.Parameter(typeof(TData));

            return;
        }

        #endregion

        #region Instance methods

        public static Expression<Func<TData, bool>> Complement(
            Expression<Func<TData, bool>> first)
        {
            return BuildLambdaFrom(
                Expression.Not(first), 
                first.Parameters[0]);
        }

        public static Expression<Func<TData, bool>> Intersect(
            Expression<Func<TData, bool>> first,
            Expression<Func<TData, bool>> second)
        {
            return Concat(first, second, Expression.And);
        }

        public static Expression<Func<TData, bool>> Union(
            Expression<Func<TData, bool>> first,
            Expression<Func<TData, bool>> second)
        {
            return Concat(first, second, Expression.Or);
        }

        public static Expression<Func<TData, bool>> Except(
            Expression<Func<TData, bool>> first,
            Expression<Func<TData, bool>> second)
        {
            return Concat(first, second, BuildExcept());
        }

        private static PredicateConcatHandler BuildExcept()
        {
            return (first, second) => Expression.And(
                first,
                Expression.Not(second));
        }

        public static Expression<Func<TData, bool>> SymmetricExcept(
            Expression<Func<TData, bool>> first,
            Expression<Func<TData, bool>> second)
        {
            return Concat(first, second, Expression.ExclusiveOr);
        }

        private static Expression<Func<TData, bool>> Concat(
            Expression<Func<TData, bool>> first,
            Expression<Func<TData, bool>> second,
            PredicateConcatHandler concatHandler)
        {
            ParameterExpression dataInstanceParam =
                first.Parameters[0];

            return BuildLambdaFrom(
                concatHandler(first.Body, second.Body),
                dataInstanceParam);
        }

        private static Expression<Func<TData, bool>> BuildLambdaFrom(
            Expression body,
            ParameterExpression dataInstanceParam)
        {
            return Expression.Lambda<Func<TData, bool>>(
                body,
                true,
                dataInstanceParam);
        }

        public Expression<Func<TData, bool>> Prepare(
            Expression<Func<TData, bool>> expr)
        {
            return Visit(expr) as Expression<Func<TData, bool>>;
        }

        protected override Expression VisitParameter(
            ParameterExpression node)
        {
            return _dataInstanceParam;
        }

        #endregion
    }
}
