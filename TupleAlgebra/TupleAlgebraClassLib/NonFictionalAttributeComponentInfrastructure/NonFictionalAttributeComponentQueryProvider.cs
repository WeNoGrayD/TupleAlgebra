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
using TupleAlgebraClassLib.LINQ2TAFramework;
using System.Reflection;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    /// <summary>
    /// Провайдер запросов к нефиктивной компоненте.
    /// </summary>
    public class NonFictionalAttributeComponentQueryProvider : IQueryProvider
    {
        #region Instance fields

        /// <summary>
        /// Источник данных.
        /// </summary>
       // public NonFictionalAttributeComponent<TValue> Queryable { get; set; }

        private LinkedList<object> _dataSources;

        private LinkedListNode<object> _firstFromBeginning;

        private LinkedListNode<object> _firstFromEnd;

        private QueryPipelineExecutor _queryPipelineExecutor;

        private NonFictionalAttributeComponentQueryContext _queryContext;

        #endregion

        #region Constructors

        public NonFictionalAttributeComponentQueryProvider(
            NonFictionalAttributeComponentQueryContext queryContext = null)
        {
            _dataSources = new LinkedList<object>();
            _queryContext = queryContext ?? new NonFictionalAttributeComponentQueryContext();
        }

        #endregion

        #region Instance methods

        public void AppendDataSource<TDataSourceValue>(
            NonFictionalAttributeComponent<TDataSourceValue> dataSource)
        {
            _dataSources.AddLast(dataSource);
        }

        public NonFictionalAttributeComponent<TDataSource> FirstDataSourceFromBeginning<TDataSource>()
        {
            if (_firstFromBeginning is null) _firstFromBeginning = _dataSources.First.Next;

            return _firstFromBeginning.Previous.Value as NonFictionalAttributeComponent<TDataSource>;
        }

        public NonFictionalAttributeComponent<TDataSource> FirstDataSourceFromEnd<TDataSource>()
        {
            if (_firstFromEnd is null) _firstFromEnd = _dataSources.Last.Previous;

            return _firstFromEnd.Next.Value as NonFictionalAttributeComponent<TDataSource>;
        }

        #endregion

        #region IQueryProvider implemented methods

        public IQueryable CreateQuery(Expression queryExpression)
        {
            return null;
        }

        public IQueryable<TQueryResult> CreateQuery<TQueryResult>(Expression queryExpression)
        {
            NonFictionalAttributeComponent<TQueryResult> queryableAC =
                FirstDataSourceFromBeginning<TQueryResult>();

            return CreateQuery(queryExpression, queryableAC);
        }

        public IQueryable<TQueryResult> CreateQuery<TQueryResult>(
            Expression queryExpression,
            NonFictionalAttributeComponent<TQueryResult> queryableAC)
        {
            Expression queryableExpression = queryableAC.Expression;

            /*
               Проверка выражения запроса на избыточность (а также корректность). 
               Если оно избыточно, то экземпляр MethodCallInspector вернёт
               неизбыточное выражение, равное константному выражению источника данных для запроса.
             */
            if (queryExpression != new QueryInspector().Visit(queryExpression))
                return queryableAC;

            AttributeComponentFactoryArgs<TQueryResult> factoryArgs =
                ConstructFactoryArgs(queryableAC, null, this, queryExpression);

            return queryableAC.ProduceNewOfSameNatureType(factoryArgs);
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

            if (methodCallChain.Count == 0)
                return (TQueryResult)(queryExpression as ConstantExpression).Value;

            Type queryProviderType = this.GetType(),
                 acDataType = typeof(TQueryResult).GetGenericArguments().Single(),
                 acType = typeof(AttributeComponent<>)
                    .MakeGenericType(acDataType);

            bool isAttributeComponent = typeof(TQueryResult).IsGenericType &&
                (typeof(TQueryResult) == acType || typeof(TQueryResult).IsSubclassOf(acType));
            //typeof(NonFictionalAttributeComponent<>).IsAssignableFrom(typeof(TQueryResult).GetGenericTypeDefinition());
            string constructFactoryArgsMethodName = 
                nameof(NonFictionalAttributeComponentQueryProvider.ConstructFactoryArgs),
                   factoryMethodName = nameof(QueryableAttributeComponent.ProduceNewOfSameNatureType);

            Func<IEnumerable<object>, IEnumerable<object>, IQueryable> nonFictionalFactoryFunc =
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

            _queryPipelineExecutor =
                new AttributeComponentQueryPipelineExecutor(
                    isAttributeComponent ? 
                        typeof(NonFictionalAttributeComponent<>).MakeGenericType(acDataType) 
                        : typeof(TQueryResult),
                    _dataSources.First.Value as IEnumerable<object>, 
                    isAttributeComponent);
            _queryPipelineExecutor.ProduceNonFictionalAttributeComponentEvent += 
                new QueryPipelineExecutor.ProduceNonFictionalAttributeComponent(nonFictionalFactoryFunc);

            /*
            int requiredETDepth = -_methodCallChain.Count;
            ExpressionTreeIncrementalModifier etModifier = 
                new ExpressionTreeIncrementalModifier(requiredETDepth);
            ConstantExpression queriedDataSourceExpr;
            foreach (NonFictionalAttributeComponent<TQueryResult> dataSource in _dataSources)
            {
                queriedDataSourceExpr = Expression.Constant(ExecuteLocalQuery(
                    dataSource, queryExpression));
                etModifier.UpdateAndIncrement(queriedDataSourceExpr);
            }
            */

            foreach (Expression queryMethod in methodCallChain)
                _queryPipelineExecutor.AddSingleQueryExecutor(
                    _queryContext.BuildSingleQueryExecutor(queryMethod as MethodCallExpression));


            _dataSources.Clear();

            return _queryPipelineExecutor.ExecutePipeline<TQueryResult>();
        }

        public object ExecuteLocalQuery(
            IQueryable dataSource,
            Expression queryExpression)
        {
            Type currentQueryReturnType = dataSource.Expression.Type;
            object currentQueryResult = dataSource.Provider.Execute(dataSource.Expression);

            return currentQueryResult;
        }

        protected virtual AttributeComponentFactoryArgs<TQueryResult> ConstructFactoryArgs<TQueryResult>(
            NonFictionalAttributeComponent<TQueryResult> component,
            IEnumerable<TQueryResult> values,
            NonFictionalAttributeComponentQueryProvider queryProvider,
            Expression queryExpression)
        {
            return null;
        }

        #endregion

        /// <summary>
        /// Построитель цепочки вызовов методов запросов.
        /// </summary>
        private class MethodCallChainBuilder : ExpressionVisitor
        {
            public readonly List<Expression> MethodCallChain = new List<Expression>();

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                MethodCallChain.Add(node);

                return Visit(node.Arguments[0]);
            }
        }

        private class DataSourceExtractor : ExpressionVisitor
        {
            protected override Expression VisitConstant(ConstantExpression node)
            {
                return base.VisitConstant(node);
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                return node.Arguments[0];
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

        /// <summary>
        /// Модификатор дерева выражения запроса.
        /// Заменяет самое глубоковложенное выражение одиночного запроса
        /// константным выражением результирующего источника данных.
        /// </summary>
        private class ExpressionTreeIncrementalModifier : ExpressionVisitor
        {
            private int _requiredDepth;
            private int _currentDepth;
            private ConstantExpression _dataSourceReplacingWith;

            public ExpressionTreeIncrementalModifier(int requiredDepth)
            {
                _requiredDepth = requiredDepth;
            }

            public void UpdateAndIncrement(ConstantExpression dataSourceReplacingWith)
            {
                _currentDepth = 0;
                _requiredDepth++;
                _dataSourceReplacingWith = dataSourceReplacingWith;
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (--_currentDepth == _requiredDepth)
                {
                    return _dataSourceReplacingWith;
                }
                else
                    return base.Visit(node.Arguments[0]);
            }
        }

        /*
        protected abstract void QueryableAny(Expression queryExpression);

        protected abstract void QueryableAll(Expression queryExpression);

        protected abstract void QueryableSelect(Expression queryExpression);
        */
    }

    public class NonFictionalAttributeComponentQueryContext 
    {
        public IQueryPipelineExecutorAcceptor BuildSingleQueryExecutor(
            MethodCallExpression node)
        {
            Type queryContextType = typeof(NonFictionalAttributeComponentQueryContext),
                 queryResultType = node.Type;
            MethodInfo queryMethod;
            string queryMethodName = node.Method.Name;

            switch (queryMethodName)
            {
                case nameof(Queryable.Any):
                    { break; }
                case nameof(Queryable.All):
                    { break; }
                case nameof(Queryable.Select):
                    { break; }
                case nameof(Queryable.SelectMany):
                    { break; }
                case nameof(Queryable.Where):
                    {
                        queryMethod = queryContextType
                            .GetMethod(queryMethodName + "Query")
                            .MakeGenericMethod(queryResultType.GenericTypeArguments[0]);
                        return queryMethod.Invoke(this, new object[] { node }) as IQueryPipelineExecutorAcceptor;
                    }
                case nameof(Queryable.First):
                    { break; }
                case nameof(Queryable.FirstOrDefault):
                    { break; }
                case nameof(Queryable.Single):
                    { break; }
                case nameof(Queryable.SingleOrDefault):
                    { break; }
            }

            return null;
        }

        public virtual ISingleQueryExecutor<TData, IEnumerable<TData>> WhereQuery<TData>(
            MethodCallExpression whereExpr)
        {
            var a = (whereExpr.Arguments[1] as UnaryExpression).Operand;

            Type t1 = typeof(TData),
                 t2 = a.GetType();

            Expression<Func<TData, bool>> whereQueryBody = 
                ((whereExpr.Arguments[1] as UnaryExpression).Operand as LambdaExpression) 
                as Expression<Func<TData, bool>>;

            return new WhereQueryExecutor<TData>(whereQueryBody.Compile());
        }

        private class WhereQueryExecutor<TData> : WholeDataSourceReader<TData, IEnumerable<TData>>
        {
            public WhereQueryExecutor(Func<TData, bool> predicate)
                : base(predicate)
            { }

            public override IEnumerable<TData> GetResult()
            {
                return _dataSource;
            }
        }
    }

    /*
    public class QueryPipelineExecutor<TData>
    {
        private List<Predicate<TData>> _pipeline;
        private List<Func<IEnumerable<TData>, IEnumerable>> _executionPipeline;

        public void AddSingleQueryExecutor(
            Predicate<TData> queryExecutor)
        {
            _pipeline.Add(queryExecutor);
        }

        private void ExecuteSingleQuery(
            Predicate<TData> queryExecutor)
        {

        }

        private bool ExecutePipeline(TData data)
        {
            foreach (Predicate<TData> predicate in _pipeline)
                if (!predicate(data)) return false;

            return true;
        }

        public TQueryResult Execute<TQueryResult>(
            IQueryable<TData> dataSource,
            bool isEnumerable)
        {
            ArrayList queryResults = null;

            foreach (TData data in dataSource)
            {
                if (ExecutePipeline(data)) ;
            }

            return (isEnumerable ? queryResults : queryResults.SingleOrDefault()) as TQueryResult;
        }
    }
    */
}
