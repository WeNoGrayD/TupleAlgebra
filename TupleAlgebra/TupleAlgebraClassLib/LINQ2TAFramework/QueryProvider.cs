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
    /// Провайдер запросов к нефиктивной компоненте.
    /// </summary>
    public abstract class QueryProvider : IQueryProvider
    {
        #region Instance properties

        protected abstract QueryContext QueryContext { get; }

        #endregion

        #region IQueryProvider implemented methods

        public IQueryable CreateQuery(Expression queryExpression)
        {
            return null;
        }

        public abstract IQueryable<TData> CreateQuery<TData>(Expression queryExpression);

        public object Execute(Expression queryExpression)
        {
            return default(IQueryable);
        }

        public TQueryResult Execute<TQueryResult>(Expression queryExpression)
        {
            MethodCallChainBuilder methodCallChainBuilder = new MethodCallChainBuilder();
            methodCallChainBuilder.Visit(queryExpression);
            List<Expression> methodCallChain = methodCallChainBuilder.MethodCallChain;
            object queryDataSource = methodCallChainBuilder.QueryDataSource;

            if (methodCallChain.Count == 0) return (TQueryResult)queryDataSource;

            IQueryPipelineMiddleware firstQueryExecutor = null;
            QueryContext.ResetFirstQueryExecutor();
            foreach (Expression queryMethod in methodCallChain)
                firstQueryExecutor = QueryContext.BuildSingleQueryExecutor(queryMethod as MethodCallExpression);

            bool isResultEnumerable = typeof(TQueryResult).GetInterface(nameof(IEnumerable)) is not null;

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

        protected abstract QueryPipelineExecutor CreateQueryPipelineExecutor(
            object dataSource, 
            IQueryPipelineMiddleware firstQueryExecutor);

        public object ExecuteLocalQuery(
            IQueryable dataSource,
            Expression queryExpression)
        {
            Type currentQueryReturnType = dataSource.Expression.Type;
            object currentQueryResult = dataSource.Provider.Execute(dataSource.Expression);

            return currentQueryResult;
        }

        /*
        protected abstract AttributeComponentFactoryArgs<TQueryResult> ConstructFactoryArgs<TQueryResult>(
            AttributeComponent<TQueryResult> component,
            IEnumerable<TQueryResult> values,
            AttributeComponentQueryProvider queryProvider,
            Expression queryExpression);*/

        #endregion

        /// <summary>
        /// Построитель цепочки вызовов методов запросов.
        /// </summary>
        private class MethodCallChainBuilder : ExpressionVisitor
        {
            public object QueryDataSource { get; private set; }

            public readonly List<Expression> MethodCallChain = new List<Expression>();

            protected override Expression VisitConstant(ConstantExpression node)
            {
                QueryDataSource = node.Value;

                return base.VisitConstant(node);
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                MethodCallChain.Insert(0, node);

                return Visit(node.Arguments[0]);
            }
        }

        protected class DataSourceExtractor<TDataSource> : ExpressionVisitor
        {
            protected TDataSource _dataSource;

            public TDataSource Extract(Expression queryExpression)
            {
                Visit(queryExpression);

                return _dataSource;
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                _dataSource = (TDataSource)node.Value;

                return base.VisitConstant(node);
            }

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
            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                switch (node.Method.Name)
                {
                    case nameof(QueryableAttributeComponent.Select):
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

            protected virtual void CheckSelectQueryOnAcceptability(MethodCallExpression selectExpression)
            { }
        }
    }
}
