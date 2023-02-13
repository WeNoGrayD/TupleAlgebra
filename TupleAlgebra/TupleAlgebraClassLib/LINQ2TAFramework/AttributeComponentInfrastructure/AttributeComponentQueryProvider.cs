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

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure
{
    /// <summary>
    /// Провайдер запросов к нефиктивной компоненте.
    /// </summary>
    public abstract class AttributeComponentQueryProvider : IQueryProvider
    {
        #region Instance properties

        protected abstract AttributeComponentQueryContext QueryContext { get; }

        #endregion

        #region IQueryProvider implemented methods

        public IQueryable CreateQuery(Expression queryExpression)
        {
            return null;
        }

        public IQueryable<TQueryResult> CreateQuery<TQueryResult>(Expression queryExpression)
        {
            AttributeComponent<TQueryResult> queryableAC =
                new DataSourceExtractor().Extract<TQueryResult>(queryExpression);

            return CreateQuery(queryExpression, queryableAC);
        }

        public IQueryable<TQueryResult> CreateQuery<TQueryResult>(
            Expression queryExpression,
            AttributeComponent<TQueryResult> queryableAC)
        {
            Expression queryableExpression = queryableAC.Expression;

            /*
               Проверка выражения запроса на избыточность (а также корректность). 
               Если оно избыточно, то экземпляр MethodCallInspector вернёт
               неизбыточное выражение, равное константному выражению источника данных для запроса.
             */
            if (queryExpression != new QueryInspector().Visit(queryExpression))
                return queryableAC;

            AttributeComponentFactoryArgs<TQueryResult> factoryArgs = queryableAC.ZipInfo(null);
            factoryArgs.QueryExpression = queryExpression;

            return queryableAC.Reproduce(factoryArgs);
        }

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

            Type acDataType = queryDataSource.GetType().GetGenericArguments().Single(),
                 acType = typeof(AttributeComponent<>).MakeGenericType(acDataType);

            MethodInfo execute = this.GetType()
                .GetMethod(nameof(AttributeComponentQueryProvider.ExecuteExact), 
                           BindingFlags.Instance | BindingFlags.NonPublic)
                           //new Type[] { acType, typeof(List<Expression>) })
                .MakeGenericMethod(acDataType, typeof(TQueryResult));

            return (TQueryResult)execute.Invoke(this, new object[] { queryDataSource, methodCallChain});// Execute(queryDataSource, methodCallChain);
        }

        protected TQueryResult ExecuteExact<TData, TQueryResult>(
            AttributeComponent<TData> queryDataSource,
            List<Expression> methodCallChain) 
        {
            bool isAttributeComponent = typeof(TQueryResult).IsGenericType &&
                (typeof(TQueryResult) == typeof(AttributeComponent<TData>) || 
                 typeof(TQueryResult).IsSubclassOf(typeof(AttributeComponent<TData>)));

            QueryPipelineExecutor queryPipelineExecutor =
                AttributeComponentQueryPipelineExecutor2.Construct<TData>(
                    isAttributeComponent ?
                        typeof(AttributeComponent<TData>)//typeof(NonFictionalAttributeComponent<>).MakeGenericType(acDataType)
                        : typeof(TQueryResult),
                    queryDataSource,
                    isAttributeComponent);

            foreach (Expression queryMethod in methodCallChain)
                queryPipelineExecutor.AddSingleQueryExecutor(
                    QueryContext.BuildSingleQueryExecutor2(queryMethod as MethodCallExpression));

            return queryPipelineExecutor.ExecutePipeline<TQueryResult>();

            /*
            Type queryProviderType = this.GetType(),
                 acDataType = typeof(TQueryResult).GetGenericArguments().Single(),
                 acType = typeof(AttributeComponent<>)
                    .MakeGenericType(acDataType);
            bool isAttributeComponent = typeof(TQueryResult).IsGenericType &&
                (typeof(TQueryResult) == acType || typeof(TQueryResult).IsSubclassOf(acType));
            string constructFactoryArgsMethodName =
                nameof(AttributeComponentQueryProvider.ConstructFactoryArgs),
                   factoryMethodName = nameof(QueryableAttributeComponent.Reproduce);

            Func<IEnumerable<TData>, IEnumerable<TData>, IQueryable> nonFictionalFactoryFunc =
                (dataSource, resultData) =>
                {
                    Type nfacType =
                        typeof(NonFictionalAttributeComponent<>).MakeGenericType(acDataType);
                    object factoryArgs =
                        queryProviderType.GetMethod(constructFactoryArgsMethodName, BindingFlags.NonPublic | BindingFlags.Instance)
                                         .MakeGenericMethod(acDataType)
                                         .Invoke(this, new object[] { dataSource, resultData, this, null });

                    IQueryable q = typeof(QueryableAttributeComponent)
                        .GetMethod(factoryMethodName)
                        .MakeGenericMethod(acDataType, acDataType)
                        .Invoke(dataSource, new object[] { dataSource, factoryArgs }) as IQueryable;

                    return q;
                };

            AttributeComponentQueryPipelineExecutor<TData> queryPipelineExecutor =
                new AttributeComponentQueryPipelineExecutor<TData>(
                    isAttributeComponent ?
                        typeof(AttributeComponent<TData>)//typeof(NonFictionalAttributeComponent<>).MakeGenericType(acDataType)
                        : typeof(TQueryResult),
                    queryDataSource,
                    isAttributeComponent);
            queryPipelineExecutor.ProduceNonFictionalAttributeComponentEvent +=
                new AttributeComponentQueryPipelineExecutor<TData>
                    .ProduceNonFictionalAttributeComponent(nonFictionalFactoryFunc);

            foreach (Expression queryMethod in methodCallChain)
                queryPipelineExecutor.AddSingleQueryExecutor(
                    QueryContext.BuildSingleQueryExecutor<TData>(queryMethod as MethodCallExpression));

            return queryPipelineExecutor.ExecutePipeline<TQueryResult>();
            */
        }

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
                MethodCallChain.Add(node);

                return Visit(node.Arguments[0]);
            }
        }

        private class DataSourceExtractor : ExpressionVisitor
        {
            private object _dataSource;

            public AttributeComponent<TQueryDataResult> Extract<TQueryDataResult>(
                Expression queryExpression)
            {
                Visit(queryExpression);

                return _dataSource as AttributeComponent<TQueryDataResult>;
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                _dataSource = node.Value;

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
        private class QueryInspector : ExpressionVisitor
        {
            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                switch (node.Method.Name)
                {
                    case nameof(QueryableAttributeComponent.Select):
                        {
                            CheckSelectQueryOnAcceptability(node);
                            return base.Visit(node.Arguments[0]);
                        }
                    default:
                        {
                            return base.VisitMethodCall(node);
                        }
                }
            }

            private void CheckSelectQueryOnAcceptability(MethodCallExpression selectExpression)
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
