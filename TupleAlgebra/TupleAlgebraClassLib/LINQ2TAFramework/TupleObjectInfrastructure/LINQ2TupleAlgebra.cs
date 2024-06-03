using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.LINQ2TAFramework.TupleObjectInfrastructure
{
    public static class LINQ2TupleAlgebra
    {
        private static Expression BuildCoveredExpression(
            IQueryable source,
            MethodInfo queryMethodInfo,
            Type[] genericArguments,
            params Expression[] queryMethodParams)
        {
            Expression[] queryMethodParamsBuf = new Expression[queryMethodParams.Length + 1];
            queryMethodParamsBuf[0] = source.Expression;
            int i = 1;
            foreach (Expression paramExpr in queryMethodParams.Select(
                param => param switch
                {
                    LambdaExpression lambda => Expression.Quote(lambda),
                    Expression constant => constant
                })
                )
            {
                queryMethodParamsBuf[i] = paramExpr;
            }

            return Expression.Call(
                null,
                queryMethodInfo.MakeGenericMethod(genericArguments),
                queryMethodParamsBuf);
        }

        public static TupleObject<TQueryEntity> Select<TEntity, TQueryEntity>(
            this TupleObject<TEntity> tuple,
            Expression<Func<TEntity, TQueryEntity>> selectorExpr)
            where TEntity : new()
            where TQueryEntity : new()
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return tuple.Provider.CreateQuery<TQueryEntity>(
                BuildCoveredExpression(
                    tuple,
                    queryMethodInfo,
                    [typeof(TEntity), typeof(TQueryEntity)],
                    selectorExpr)) as TupleObject<TQueryEntity>;
        }

        public static TupleObject<TEntity> Where<TEntity>(
            this TupleObject<TEntity> tuple,
            Expression<Func<TEntity, bool>> filterExpr)
            where TEntity : new()
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return tuple.Provider.CreateQuery<TEntity>(
                BuildCoveredExpression(
                    tuple,
                    queryMethodInfo,
                    [typeof(TEntity)],
                    filterExpr)) as TupleObject<TEntity>;
        }
    }
}
