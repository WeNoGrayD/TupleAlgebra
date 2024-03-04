using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Diagnostics;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using LINQProvider;
using System.Reflection;
using TupleAlgebraClassLib.AttributeComponents;
using LINQProvider.QueryPipelineInfrastructure;
using System.Collections;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure
{
    /// <summary>
    /// Провайдер запросов к компоненте атрибута.
    /// </summary>
    public abstract class AttributeComponentQueryProvider : QueryProvider
    {
        #region Istance methods

        /// <summary>
        /// Оборачивание перечислимого нефиктивного результата запроса в компоненту атрибута.
        /// </summary>
        /// <typeparam name="TQueryResultData"></typeparam>
        /// <param name="queryableAC"></param>
        /// <param name="queryResult"></param>
        /// <returns></returns>
        protected object WrapEnumerableResultWithAttributeComponent<TQueryResultData>(
            AttributeComponent<TQueryResultData> queryableAC,
            IEnumerable<TQueryResultData> queryResult)
        {
            AttributeComponentFactoryArgs factoryArgs = queryableAC.ZipInfo(
                queryResult,
                includeDomain: true);

            return queryableAC.Reproduce<TQueryResultData>(factoryArgs);
        }

        #endregion

        #region IQueryProvider implementation

        /// <summary>
        /// Имплементированное обобщённое создание запроса.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="queryExpression"></param>
        /// <param name="queryableAC"></param>
        /// <returns></returns>
        protected override IQueryable<TData> CreateQueryImpl<TData>(
            Expression queryExpression,
            IQueryable dataSource)
        {
            AttributeComponent<TData> queryableAC = dataSource as AttributeComponent<TData>;

            AttributeComponentFactoryArgs factoryArgs = queryableAC.ZipInfo<TData>(
                null,
                includeDomain: true);
            factoryArgs.QueryExpression = queryExpression;
            /*
             * Указывается флаг "компонента является продуктом запроса",
             * чтобы фабрика компонент атрибутов не проводила лишних проверок.
             */
            factoryArgs.IsQuery = true;

            return queryableAC.Reproduce<TData>(factoryArgs);
        }

        protected override IDataSourceExtractor<IQueryable<TData>> CreateDataSourceExtractor<TData>()
        {
            return new DataSourceExtractor<AttributeComponent<TData>>();
        }

        /// <summary>
        /// Обобщённое выполнение запроса.
        /// </summary>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <param name="queryExpression"></param>
        /// <returns></returns>
        public override TQueryResult Execute<TQueryResult>(Expression queryExpression)
        {
            TQueryResult queryResult = base.Execute<TQueryResult>(queryExpression);
            /*
             * Результат запроса оборачивается в компоненту атрибута, если он перечислимый и нефиктивный.
             */
            if (_isResultEnumerable && !_queryIsFiction)
                return (TQueryResult)WrapEnumerableResultWithAttributeComponent((dynamic)_queryDataSource, queryResult);
            
            return queryResult;
        }

        #endregion

        #region Inner classes

        /// <summary>
        /// Инспектор запросов к компоненте атрибута.
        /// </summary>
        protected class AttributeComponentQueryInspector : QueryInspector
        {
            #region Constructors

            public AttributeComponentQueryInspector(IQueryable dataSource)
                : base(dataSource)
            {
                return;
            }

            #endregion

            #region Instance methods

            /// <summary>
            /// Метод для инспеции метода Select.
            /// </summary>
            /// <param name="selectExpression"></param>
            /// <exception cref="InvalidOperationException"></exception>
            /*
            protected override void CheckSelectQueryOnAcceptability(MethodCallExpression selectExpression)
            {
                if (selectExpression.Arguments[1].NodeType != ExpressionType.Quote)
                    throw new InvalidOperationException(
                        $"Выражение {selectExpression.ToString()} недопустимо:\n" +
                        "выражение вида AttributeComponent.Select может содержать только" +
                        "проекцию вида { (el) => el }.");
            }
            */

            #endregion
        }

        #endregion
    }
}
