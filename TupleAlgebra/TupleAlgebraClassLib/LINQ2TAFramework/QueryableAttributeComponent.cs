using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using TupleAlgebraClassLib.AlgebraicTupleInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    public static class QueryableAttributeComponent
    {
        public static AttributeComponent<TQueryResult> Select<TData, TQueryResult>(
            this AttributeComponent<TData> sourceComponent,
            Expression<Func<TData, TQueryResult>> selector,
            AttributeDomain<TQueryResult> queryResultDomain)
        {
            MethodInfo queryMethod = (MethodBase.GetCurrentMethod() as MethodInfo)
                .MakeGenericMethod(typeof(TData), typeof(TQueryResult));

            Expression queryExpression = Expression.Call(
                null,
                queryMethod,
                Expression.Constant(sourceComponent),
                Expression.Quote(selector),
                Expression.Constant(queryResultDomain));

            return sourceComponent.Provider.CreateQuery<TQueryResult>(queryExpression) 
                as AttributeComponent<TQueryResult>;
        }

        public static AttributeComponent<TQueryResult> Select<TData, TQueryResult>(
            this AttributeDomain<TData> sourceDomain,
            Expression<Func<TData, TQueryResult>> selector)
        {
            /*
            MethodInfo mi = MethodBase.GetCurrentMethod() as MethodInfo;
            var meths = typeof(QueryableAttributeComponent).G
            MethodInfo queryMethod = typeof(QueryableAttributeComponent).GetMethod(
                MethodBase.GetCurrentMethod().Name,
                new Type[] { typeof(AttributeComponent<>), typeof(Expression<>) })
                .MakeGenericMethod(typeof(TData), typeof(TQueryResult));

            Expression queryExpression = Expression.Call(
                null,
                queryMethod,
                Expression.Constant(sourceDomain.Universum),
                Expression.Quote(selector));

            return sourceDomain.Provider.CreateQuery<TQueryResult>(queryExpression)
                as AttributeComponent<TQueryResult>;
                */
            return QueryableAttributeComponent.Select(sourceDomain.Universum, selector);
        }

        public static AttributeComponent<TQueryResult> Select<TData, TQueryResult>(
            this AttributeComponent<TData> sourceComponent,
            Expression<Func<TData, TQueryResult>> selector)
        {
            MethodInfo queryMethod = (MethodBase.GetCurrentMethod() as MethodInfo)
                .MakeGenericMethod(typeof(TData), typeof(TQueryResult));

            Expression queryExpression = Expression.Call(
                null,
                queryMethod,
                Expression.Constant(sourceComponent),
                Expression.Quote(selector));

            return sourceComponent.Provider.CreateQuery<TQueryResult>(queryExpression)
                as AttributeComponent<TQueryResult>;
        }
    }
}
