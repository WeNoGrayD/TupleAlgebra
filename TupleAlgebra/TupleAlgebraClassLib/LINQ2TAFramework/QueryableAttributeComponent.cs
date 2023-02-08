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
        public static AttributeComponent<TValueNew> ProduceNewOfSameNatureType<TValueOld, TValueNew>(
               this NonFictionalAttributeComponent<TValueOld> component,
               AttributeComponentFactoryArgs<TValueNew> factoryArgs)
        {
            return component.ProduceNewOfSameNatureType(factoryArgs);
        }

        public static AttributeComponent<TQueryResult> Select<TValue, TQueryResult>(
            this NonFictionalAttributeComponent<TValue> sourceComponent,
            Expression<Func<TValue, TQueryResult>> selector,
            AttributeDomain<TQueryResult> queryResultDomain)
        {
            MethodInfo queryMethod = (MethodBase.GetCurrentMethod() as MethodInfo)
                .MakeGenericMethod(typeof(TValue), typeof(TQueryResult));

            Expression queryExpression = Expression.Call(
                null,
                queryMethod,
                Expression.Constant(sourceComponent),
                Expression.Quote(selector),
                Expression.Constant(queryResultDomain));

            return sourceComponent.Provider.CreateQuery<TQueryResult>(queryExpression) 
                as AttributeComponent<TQueryResult>;
        }

        public static AttributeComponent<TQueryResult> Select<TValue, TQueryResult>(
            this AttributeDomain<TValue> sourceComponent,
            Expression<Func<TValue, TQueryResult>> selector)
        {
            MethodInfo queryMethod = (MethodBase.GetCurrentMethod() as MethodInfo)
                .MakeGenericMethod(typeof(TValue), typeof(TQueryResult));

            Expression queryExpression = Expression.Call(
                null,
                queryMethod,
                Expression.Constant(sourceComponent),
                Expression.Quote(selector));

            return sourceComponent.Provider.CreateQuery<TQueryResult>(queryExpression)
                as AttributeComponent<TQueryResult>;
        }

        public static AttributeComponent<TQueryResult> Select<TValue, TQueryResult>(
            this NonFictionalAttributeComponent<TValue> sourceComponent,
            Expression<Func<TValue, TQueryResult>> selector)
        {
            MethodInfo queryMethod = (MethodBase.GetCurrentMethod() as MethodInfo)
                .MakeGenericMethod(typeof(TValue), typeof(TQueryResult));

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
