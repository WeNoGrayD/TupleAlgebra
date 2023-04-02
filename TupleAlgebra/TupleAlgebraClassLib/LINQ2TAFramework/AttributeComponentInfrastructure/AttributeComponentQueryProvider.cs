using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Diagnostics;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.SetOperationExecutersContainers;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Reflection;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure
{
    /// <summary>
    /// Провайдер запросов к компоненте атрибута.
    /// </summary>
    public abstract class AttributeComponentQueryProvider : QueryProvider
    {
        /// <summary>
        /// Создание запроса с заданным выражением.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="queryExpression"></param>
        /// <returns></returns>
        public override IQueryable<TData> CreateQuery<TData>(Expression queryExpression)
        {
            AttributeComponent<TData> queryableAC = 
                new DataSourceExtractor<AttributeComponent<TData>>().Extract(queryExpression);

            return CreateQueryImpl(queryExpression, queryableAC);
        }

        /// <summary>
        /// Имплементированное обобщённое создание запроса.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="queryExpression"></param>
        /// <param name="queryableAC"></param>
        /// <returns></returns>
        protected IQueryable<TData> CreateQueryImpl<TData>(
            Expression queryExpression,
            AttributeComponent<TData> queryableAC)
        {
            /*
               Проверка выражения запроса на избыточность (а также корректность). 
               Если оно избыточно, то экземпляр MethodCallInspector вернёт
               неизбыточное выражение, равное константному выражению источника данных для запроса.
             */
            if (queryExpression != new QueryInspector().Inspect(queryExpression))
                return queryableAC;

            AttributeComponentFactoryArgs<TData> factoryArgs = queryableAC.ZipInfo(null);
            factoryArgs.QueryExpression = queryExpression;

            return queryableAC.Reproduce(factoryArgs);
        }

        public override TQueryResult Execute<TQueryResult>(Expression queryExpression)
        {
            TQueryResult queryResult = base.Execute<TQueryResult>(queryExpression);
            if (_isResultEnumerable && !_queryIsFiction)
                return (TQueryResult)WrapEnumerableResultWithAttributeComponent((dynamic)_queryDataSource, queryResult);
            else return queryResult;
        }

        protected object WrapEnumerableResultWithAttributeComponent<TQueryResultData>(
            AttributeComponent<TQueryResultData> queryableAC,
            IEnumerable<TQueryResultData> queryResult)
        {
            AttributeComponentFactoryArgs<TQueryResultData> factoryArgs = queryableAC.ZipInfo(queryResult);
            return queryableAC.Reproduce(factoryArgs);
        }

        /// <summary>
        /// Инспектор запросов к компоненте атрибута.
        /// </summary>
        protected class AttributeComponentQueryInspector : QueryInspector
        {
            /// <summary>
            /// Метод для инспеции метода Select.
            /// </summary>
            /// <param name="selectExpression"></param>
            /// <exception cref="InvalidOperationException"></exception>
            protected override void CheckSelectQueryOnAcceptability(MethodCallExpression selectExpression)
            {
                if (selectExpression.Arguments[1].NodeType != ExpressionType.Quote)
                    throw new InvalidOperationException(
                        $"Выражение {selectExpression.ToString()} недопустимо:\n" +
                        "выражение вида AttributeComponent.Select может содержать только" +
                        "проекцию вида { (el) => el }.");
            }
        }
    }
}
