using System;
using System.Collections;
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

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    /// <summary>
    /// Провайдер запросов к абстрактному источнику данных.
    /// </summary>
    public abstract class QueryProvider : IQueryProvider
    {
        protected object _queryDataSource;

        protected bool _queryIsFiction;

        protected bool _isResultEnumerable;

        #region Instance properties

        /// <summary>
        /// Контекст запросов.
        /// </summary>
        protected abstract QueryContext QueryContext { get; }

        #endregion

        #region IQueryProvider implemented methods

        /// <summary>
        /// Необобщённое создание запроса.
        /// </summary>
        /// <param name="queryExpression"></param>
        /// <returns></returns>
        public IQueryable CreateQuery(Expression queryExpression)
        {
            return null;
        }

        /// <summary>
        /// Обобщённое создание запроса.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="queryExpression"></param>
        /// <returns></returns>
        public abstract IQueryable<TData> CreateQuery<TData>(Expression queryExpression);

        /// <summary>
        /// Необобщённое выполнение запроса.
        /// </summary>
        /// <param name="queryExpression"></param>
        /// <returns></returns>
        public object Execute(Expression queryExpression)
        {
            return default(IQueryable);
        }

        /// <summary>
        /// Обобщённое выполнение запроса.
        /// </summary>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <param name="queryExpression"></param>
        /// <returns></returns>
        public virtual TQueryResult Execute<TQueryResult>(Expression queryExpression)
        {
            MethodCallChainBuilder methodCallChainBuilder = new MethodCallChainBuilder();
            methodCallChainBuilder.Build(queryExpression);
            List<Expression> methodCallChain = methodCallChainBuilder.MethodCallChain;
            object queryDataSource = _queryDataSource = methodCallChainBuilder.QueryDataSource;

            if (methodCallChain.Count == 0)
            {
                _isResultEnumerable = true;
                _queryIsFiction = true;
                return (TQueryResult)queryDataSource;
            }

            _queryIsFiction = false;
            IQueryPipelineMiddleware firstQueryExecutor = null;
            QueryContext.ResetFirstQueryExecutor();
            foreach (Expression queryMethod in methodCallChain)
                firstQueryExecutor = QueryContext.BuildSingleQueryExecutor(queryMethod as MethodCallExpression);

            bool isResultEnumerable = _isResultEnumerable = 
                typeof(TQueryResult).GetInterface(nameof(IEnumerable)) is not null;

            QueryPipelineExecutor queryPipelineExecutor = CreateQueryPipelineExecutor(
                queryDataSource,
                firstQueryExecutor);

            TQueryResult queryResult = isResultEnumerable ?
                ExecuteWithExpectedEnumerableResult() :
                queryPipelineExecutor.ExecuteWithExpectedAggregableResult<TQueryResult>();

            return queryResult;

            TQueryResult ExecuteWithExpectedEnumerableResult()
            {
                Type queryResultDataType = typeof(TQueryResult).GetGenericArguments().Single();

                MethodInfo executionMethodInfo = queryPipelineExecutor
                    .GetType()
                    .GetMethod(nameof(QueryPipelineExecutor.ExecuteWithExpectedEnumerableResult))
                    .MakeGenericMethod(queryResultDataType);

                return (TQueryResult)executionMethodInfo.Invoke(queryPipelineExecutor, null);
            }
        }

        /// <summary>
        /// Создание исполнителя конвейера запросов.
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="firstQueryExecutor"></param>
        /// <returns></returns>
        protected abstract QueryPipelineExecutor CreateQueryPipelineExecutor(
            object dataSource, 
            IQueryPipelineMiddleware firstQueryExecutor);

        #endregion

        /// <summary>
        /// Построитель цепочки вызовов методов запроса.
        /// </summary>
        private class MethodCallChainBuilder : ExpressionVisitor
        {
            /// <summary>
            /// Источник данных.
            /// </summary>
            public object QueryDataSource { get; private set; }

            /// <summary>
            /// Цепочка вызовов методов запроса.
            /// </summary>
            public readonly List<Expression> MethodCallChain = new List<Expression>();

            /// <summary>
            /// API для построения цепочки вызовов методов запроса и извлечения первоначального источника данных.
            /// </summary>
            /// <param name="expr"></param>
            public void Build(Expression expr) => Visit(expr);

            /// <summary>
            /// Посещение константного, самого последнего в цепочке, выражения, которое
            /// должно являться первоначальным источником данных.
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            protected override Expression VisitConstant(ConstantExpression node)
            {
                QueryDataSource = node.Value;

                return base.VisitConstant(node);
            }

            /// <summary>
            /// Посещение выражения вызова метода. 
            /// Добавляет выражение метода в начало цепочки вызово.
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                MethodCallChain.Insert(0, node);

                return Visit(node.Arguments[0]);
            }
        }

        /// <summary>
        /// Ивзлекатель первоначального источника данных из выражения запроса.
        /// </summary>
        /// <typeparam name="TDataSource"></typeparam>
        protected class DataSourceExtractor<TDataSource> : ExpressionVisitor
        {
            protected TDataSource _dataSource;

            /// <summary>
            /// API для извлечения первоначального источника данных.
            /// </summary>
            /// <param name="queryExpression"></param>
            /// <returns></returns>
            public TDataSource Extract(Expression queryExpression)
            {
                Visit(queryExpression);

                return _dataSource;
            }

            /// <summary>
            /// Посещение константного, самого последнего в цепочке, выражения, которое
            /// должно являться первоначальным источником данных.
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            protected override Expression VisitConstant(ConstantExpression node)
            {
                _dataSource = (TDataSource)node.Value;

                return base.VisitConstant(node);
            }

            /// <summary>
            /// Посещение выражения вызова метода. 
            /// Производится обход нулевого аргумента для приближения к источнику данных.
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                return Visit(node.Arguments[0]);
            }
        }

        /// <summary>
        /// Инспектор запросов. 
        /// Упрощает, отбрасывая избыточные, и проверяет на допустимость запросы.
        /// </summary>
        protected class QueryInspector : ExpressionVisitor
        {
            /// <summary>
            /// API для инспекции запроса.
            /// </summary>
            /// <param name="expr"></param>
            /// <returns></returns>
            public Expression Inspect(Expression expr) => Visit(expr);

            /// <summary>
            /// Посещение выражения вызова метода. 
            /// Инспектирование метода на предмет специфических проблем и возможных упрощений.
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                switch (node.Method.Name)
                {
                    case "Select":
                        {
                            CheckSelectQueryOnAcceptability(node);
                            return base.VisitMethodCall(node);
                        }
                    default:
                        {
                            return base.VisitMethodCall(node);
                        }
                }
            }

            /// <summary>
            /// Метод для инспеции метода Select.
            /// </summary>
            /// <param name="selectExpression"></param>
            protected virtual void CheckSelectQueryOnAcceptability(MethodCallExpression selectExpression)
            { }
        }
    }
}
