using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Reflection;
using LINQProvider.QueryPipelineInfrastructure;

namespace LINQProvider
{
    /// <summary>
    /// Провайдер запросов к абстрактному источнику данных.
    /// </summary>
    public abstract class QueryProvider : IQueryProvider
    {
        #region Instance fields

        protected QueryContext _queryContext;

        /// <summary>
        /// Источник данных запроса.
        /// </summary>
        protected IEnumerable _queryDataSource;

        /// <summary>
        /// Флаг фиктивности, избыточности запроса.
        /// </summary>
        protected bool _queryIsFiction;

        /// <summary>
        /// Флаг перечислимости результата запроса.
        /// </summary>
        protected bool _isResultEnumerable;

        #endregion

        #region Constructors

        public QueryProvider()
        {
            _queryContext = CreateQueryContext();
        }

        #endregion

        #region Instance methods

        protected virtual QueryContext CreateQueryContext()
        {
            return new QueryContext();
        }

        /// <summary>
        /// Создание исполнителя конвейера запросов.
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="firstPipelineMiddleware"></param>
        /// <returns></returns>
        protected abstract QueryPipelineScheduler CreateQueryPipelineExecutor(
            QueryContext queryContext,
            IEnumerable<MethodCallExpression> methodCallChain,
            IEnumerable dataSource);

        #endregion

        #region IQueryProvider implementation

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
        public IQueryable<TData> CreateQuery<TData>(Expression queryExpression)
        {
            IQueryable dataSource = CreateDataSourceExtractor<TData>().Extract(queryExpression);

            /*
               Проверка выражения запроса на избыточность (а также корректность). 
               Если оно избыточно, то экземпляр MethodCallInspector вернёт
               неизбыточное выражение, равное константному выражению источника данных для запроса.
             */
            QueryInspector inspector = CreateQueryInspector(dataSource);
            queryExpression = inspector.Inspect(queryExpression);
            //if (queryExpression != inspected && inspector.ChangesWereMade)
            if (inspector.QueryIsFictional)
            {
                return (dataSource as IQueryable<TData>)!;
            }

            return CreateQueryImpl<TData>(queryExpression, dataSource);
        }

        protected virtual IDataSourceExtractor<IQueryable> CreateDataSourceExtractor<TData>()
        {
            return new DataSourceExtractor<IQueryable>();
        }

        protected virtual QueryInspector CreateQueryInspector(IQueryable dataSource)
        {
            return new QueryInspector(dataSource);
        }

        protected abstract IQueryable<TData> CreateQueryImpl<TData>(
            Expression queryExpression,
            IQueryable dataSource);

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
            IEnumerable<MethodCallExpression> methodCallChain = methodCallChainBuilder.MethodCallChain;
            IEnumerable queryDataSource = _queryDataSource = methodCallChainBuilder.QueryDataSource;

            /*
             * Запрос считается фиктивным, если цепочка методов запроса не содержит ни одного метода 
             * (только константу источника данных). 
             */
            if (methodCallChain.Count() == 0)
            {
                _isResultEnumerable = true;
                _queryIsFiction = true;
                return (TQueryResult)queryDataSource;
            }

            _queryIsFiction = false;

            QueryPipelineScheduler queryPipelineExecutor = CreateQueryPipelineExecutor(
                _queryContext,
                methodCallChain,
                queryDataSource);

            TQueryResult queryResult = queryPipelineExecutor.Execute<TQueryResult>();
            _isResultEnumerable = queryPipelineExecutor.IsResultEnumerable;

            return queryResult;
        }

        #endregion

        #region Inner classes

        /// <summary>
        /// Построитель цепочки вызовов методов запроса.
        /// </summary>
        private class MethodCallChainBuilder : ExpressionVisitor
        {
            #region Instance properties

            /// <summary>
            /// Источник данных.
            /// </summary>
            public IEnumerable QueryDataSource { get; private set; }

            /// <summary>
            /// Цепочка вызовов методов запроса.
            /// </summary>
            public readonly List<MethodCallExpression> MethodCallChain = new List<MethodCallExpression>();

            #endregion

            #region Instance methods

            /// <summary>
            /// API для построения цепочки вызовов методов запроса и извлечения первоначального источника данных.
            /// </summary>
            /// <param name="expr"></param>
            public void Build(Expression expr)
            {
                Visit(expr);

                return;
            }

            /// <summary>
            /// Посещение константного, самого последнего в цепочке, выражения, которое
            /// должно являться первоначальным источником данных.
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            protected override Expression VisitConstant(ConstantExpression node)
            {
                QueryDataSource = node.Value as IEnumerable;

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

            #endregion
        }

        protected interface IDataSourceExtractor<out TDataSource>
        {
            TDataSource Extract(Expression queryExpression);
        }

        /// <summary>
        /// Ивзлекатель первоначального источника данных из выражения запроса.
        /// </summary>
        /// <typeparam name="TDataSource"></typeparam>
        protected class DataSourceExtractor<TDataSource> : ExpressionVisitor, IDataSourceExtractor<TDataSource>
            where TDataSource : IEnumerable
        {
            #region Instance fields

            /// <summary>
            /// Источник данных.
            /// </summary>
            protected TDataSource _dataSource;

            #endregion

            #region Instance methods

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

            #endregion
        }

        /// <summary>
        /// Инспектор запросов. 
        /// Упрощает, отбрасывая избыточные, и проверяет на допустимость запросы.
        /// </summary>
        protected class QueryInspector : ExpressionVisitor
        {
            #region Instance fields

            protected IQueryable _dataSource;

            protected Expression _expressionTreeRoot;

            #endregion

            #region Instance properties

            public bool ChangesWereMade { get; private set; }

            public bool QueryIsFictional { get => false; }

            #endregion

            #region Constructors

            public QueryInspector(IQueryable dataSource)
            {
                _dataSource = dataSource;
            }

            #endregion

            #region Instance methods

            /// <summary>
            /// API для инспекции запроса.
            /// </summary>
            /// <param name="expr"></param>
            /// <returns></returns>
            public Expression Inspect(Expression expr)
            {
                _expressionTreeRoot = expr;
                /*
                Expression inspected;

                do
                {
                    ChangesWereMade = false;
                    inspected = Visit(expr);
                }
                while (ChangesWereMade);

                return inspected;
                */
                return Visit(expr);
            }

            /*
            public override Expression Visit(Expression? node)
            {
                if (ChangesWereMade)
                    return null;

                return base.Visit(node);
            }
            */

            /// <summary>
            /// Метод для инспеции метода Select.
            /// </summary>
            /// <param name="selectExpression"></param>
            protected virtual void CheckSelectQueryOnAcceptability(MethodCallExpression selectExpression)
            {
                return;
            }

            /// <summary>
            /// Посещение выражения вызова метода. 
            /// Инспектирование метода на предмет специфических проблем и возможных упрощений.
            /// </summary>
            /// <param name="node"></param>
            /// <returns></returns>
            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                QueryExpressionTreeReorderer reorderer = null;
                QueryExpressionTreeReducer reductor = null;

                switch (node.Method.Name)
                {
                    case "Select":
                        {
                            CheckSelectQueryOnAcceptability(node);

                            break;
                        }
                    case "Where":
                        {
                            reductor = new ConditionBasedQueryExpressionTreeReducer();

                            break;
                        }
                    case "Skip":
                        {
                            reorderer = new SkipAndTakeQueryExpressionTreeReorderer(_dataSource);
                            reductor = new SkipQueryExpressionTreeReducer();

                            break;
                        }
                    case "Take":
                        {
                            reorderer = new SkipAndTakeQueryExpressionTreeReorderer(_dataSource);
                            reductor = new TakeQueryExpressionTreeReducer();

                            break;
                        }
                    case "SkipWhile":
                    case "TakeWhile":
                        {
                            reorderer = new SkipAndTakeQueryExpressionTreeReorderer(_dataSource);
                            reductor = new ConditionBasedQueryExpressionTreeReducer();

                            break;
                        }
                }

                bool changesWereMade = false;
                MethodCallExpression inspectedNode = null!;
                switch (reorderer, reductor)
                {
                    case (null, null): 
                        { 
                            break;
                        }
                    case (_, null):
                        {
                            MethodCallExpression reordered = reorderer.Reorder(node);

                            changesWereMade = reorderer.ReorderingWasMade;
                            inspectedNode = reordered;

                            break;
                        }
                    case (null, _):
                        {
                            MethodCallExpression reduced = reductor.Reduce(node);

                            changesWereMade = reductor.ReductionWasMade;
                            inspectedNode = reduced;

                            break;
                        }
                    case (_, _):
                        {
                            MethodCallExpression reordered = reorderer.Reorder(node);
                            MethodCallExpression reduced = reductor.Reduce(reordered);

                            changesWereMade = reorderer.ReorderingWasMade || reductor.ReductionWasMade;
                            inspectedNode = reduced;

                            break;
                        }
                }
                //ChangesWereMade |= changesWereMade;

                if (changesWereMade)
                    return VisitMethodCall(inspectedNode);

                return base.VisitMethodCall(node);
            }

            #endregion
        }

        /// <summary>
        /// Переупорядочиватель дерева выражений.
        /// Занимется перестановкой выражений запросов Skip и Take
        /// в таком порядке, чтобы результат конвейера запросов не нарушался, 
        /// но при этом конвейер запросов работал эффективнее.
        /// </summary>
        protected abstract class QueryExpressionTreeReorderer : ExpressionVisitor
        {
            #region Instance fields

            protected IQueryable _dataSource;

            protected MethodCallExpression _targetMethodNode;

            private MethodCallExpression _lastLightweightMethodNode;

            private bool _reorderingMode;

            private bool _reorderingWasMade;

            #endregion

            #region Instance properties

            public bool ReorderingWasMade { get => _reorderingWasMade; }

            #endregion

            #region Constructors

            public QueryExpressionTreeReorderer(IQueryable dataSource)
            {
                _dataSource = dataSource;

                return;
            }

            #endregion

            #region Instance methods

            public MethodCallExpression Reorder(MethodCallExpression targetMethodNode)
            {
                _targetMethodNode = targetMethodNode;
                _lastLightweightMethodNode = null;
                _reorderingMode = true;
                _reorderingWasMade = false;

                Expression exprTreeContinuation = Visit(_targetMethodNode.Arguments[0]);

                if (!_reorderingWasMade) return _targetMethodNode;

                return (exprTreeContinuation as MethodCallExpression)!;
            }

            private Expression[] CreateReorderedMethodParams(Expression instance)
            {
                Expression[] methodParams = new Expression[_targetMethodNode.Arguments.Count];
                _targetMethodNode.Arguments.CopyTo(methodParams, 0);
                methodParams[0] = instance;

                return methodParams;
            }

            private MethodCallExpression ReorderImpl(Expression instance)
            {
                _reorderingWasMade = true;

                return Expression.Call(
                    null,
                    _targetMethodNode.Method
                        .GetGenericMethodDefinition()
                        .MakeGenericMethod(_lastLightweightMethodNode.Method.GetGenericArguments()[0]),
                    CreateReorderedMethodParams(instance));
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (_reorderingMode)
                {
                    /*
                     * Если _targetMethodNode может протолкнуться вниз node,
                     * то она проталкивается.
                     */
                    if (IsTargetHeavierThan(node))
                    {
                        _lastLightweightMethodNode = node;

                        return base.VisitMethodCall(node);
                    }

                    _reorderingMode = false;

                    /*
                     * Если _targetMethodNode не легче node и при этом уже хотя бы раз
                     * совершался обход узла с типом ExpressionMethodCall, 
                     * то производится перестановка предыдущего узла и _targetMethodNode.
                     * Режим перестановки прекращается.
                     */

                    if (_lastLightweightMethodNode is not null)
                    {
                        MethodCallExpression reorderedTarget = ReorderImpl(node);

                        return base.VisitMethodCall(reorderedTarget);
                    }

                    /*
                     * Если предыдущие условия провалены, это означает, что
                     * _targetMethodNode первым аргументом имеет
                     * выражение метода запроса, которое не легче его самого
                     * ("протолкнуть" вниз _targetMethodNode нельзя).
                     * Режим перестановки отменяется.
                     */
                }

                return base.VisitMethodCall(node);
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                if (_reorderingMode)
                {
                    if (_lastLightweightMethodNode is not null)
                    {
                        _reorderingMode = false;

                        return ReorderImpl(node);
                    }
                }

                return base.VisitConstant(node);
            }

            protected abstract bool IsTargetHeavierThan(MethodCallExpression node);

            #endregion
        }

        protected class SkipAndTakeQueryExpressionTreeReorderer : QueryExpressionTreeReorderer
        {
            #region Instance fields

            private System.Text.RegularExpressions.Regex _queriesLighterInWeight = new
                System.Text.RegularExpressions.Regex(
                    "^((Select)|(GroupJoin))$", 
                    System.Text.RegularExpressions.RegexOptions.Compiled);

            #endregion

            #region Constructors

            public SkipAndTakeQueryExpressionTreeReorderer(IQueryable dataSource) 
                : base(dataSource)
            { 
                return; 
            }

            #endregion

            #region Instance methods

            protected override bool IsTargetHeavierThan(MethodCallExpression node)
            {
                return _queriesLighterInWeight.IsMatch(node.Method.Name);
            }

            #endregion
        }

        protected abstract class QueryExpressionTreeReducer : ExpressionVisitor
        {
            #region Instance fields

            protected MethodCallExpression _targetMethodNode;

            protected Expression[] _targetMethodNodeParams;

            protected Stack<MethodCallExpression> _reducedNodes;

            #endregion

            #region Instance properties

            public bool ReductionWasMade { get; private set; }

            #endregion

            #region Constructos

            public QueryExpressionTreeReducer()
            {
                _reducedNodes = new Stack<MethodCallExpression>();
            }

            #endregion

            #region Instance methods

            public MethodCallExpression Reduce(MethodCallExpression targetMethodNode)
            {
                _targetMethodNode = targetMethodNode;
                _targetMethodNodeParams = new Expression[targetMethodNode.Arguments.Count];
                targetMethodNode.Arguments.CopyTo(_targetMethodNodeParams, 0);
                _reducedNodes.Clear();

                ReductionWasMade = false;

                return Visit(targetMethodNode.Arguments[0]) as MethodCallExpression;
            }

            protected Expression Reduce()
            {
                if (_reducedNodes.Count == 0)
                {
                    return _targetMethodNode;
                }

                ReductionWasMade = true;
                _targetMethodNodeParams[0] = _reducedNodes.Peek().Arguments[0];

                return ReduceImpl();
            }

            protected abstract Expression ReduceImpl();

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node.Method == _targetMethodNode.Method)
                {
                    _reducedNodes.Push(node);

                    return Visit(node.Arguments[0]);
                }

                return Reduce();
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                return Reduce();
            }

            #endregion

            #region Inner classes

            protected class ParameterExpressionReplacer : ExpressionVisitor
            {
                private IDictionary<string?, ParameterExpression> _replacementParams;

                public Expression Replace(
                    Expression exprTree, 
                    IDictionary<string?, ParameterExpression> replacementParams)
                {
                    _replacementParams = replacementParams;

                    return Visit(exprTree);
                }

                protected override Expression VisitParameter(ParameterExpression node)
                {
                    return _replacementParams[node.Name];
                }
            }

            #endregion
        }

        protected class SkipQueryExpressionTreeReducer : QueryExpressionTreeReducer
        {
            #region Instance methods

            protected override Expression ReduceImpl()
            {
                int skippedDataCount = _targetMethodNodeParams[1].AsConstantOfType<int>();
                
                foreach (MethodCallExpression reducedNode in _reducedNodes)
                {
                    skippedDataCount += reducedNode.Arguments[1].AsConstantOfType<int>();
                }
                _targetMethodNodeParams[1] = Expression.Constant(skippedDataCount);

                return Expression.Call(
                    null,
                    _targetMethodNode.Method, 
                    _targetMethodNodeParams);
            }

            #endregion
        }

        protected class TakeQueryExpressionTreeReducer : QueryExpressionTreeReducer
        {
            #region Instance methods

            protected override Expression ReduceImpl()
            {
                Expression exprWithMinTakenDataCount = _targetMethodNode;
                int minTakenDataCount = _targetMethodNodeParams[1].AsConstantOfType<int>(),
                    reducedNodeTakenDataCount;

                foreach (MethodCallExpression reducedNode in _reducedNodes)
                {
                    reducedNodeTakenDataCount = reducedNode.Arguments[1].AsConstantOfType<int>();
                    if (reducedNodeTakenDataCount < minTakenDataCount)
                    {
                        minTakenDataCount = reducedNodeTakenDataCount;
                        exprWithMinTakenDataCount = reducedNode;
                    }
                }

                return exprWithMinTakenDataCount;
            }

            #endregion
        }

        protected class ConditionBasedQueryExpressionTreeReducer : QueryExpressionTreeReducer
        {
            #region Instance methods

            protected override Expression ReduceImpl()
            {
                LambdaExpression reducedNodeCondition = _targetMethodNode.ExpandArgumentAsLambda(1);
                Expression andExpr = reducedNodeCondition.Body;
                ParameterExpressionReplacer replacer = new ParameterExpressionReplacer();
                ParameterExpression conditionParam = reducedNodeCondition.Parameters[0];
                IDictionary<string?, ParameterExpression> replacementParams =
                    new Dictionary<string?, ParameterExpression>();
                Expression operandBody, replaced;

                /*
                 * Реверсирование стека необходимо для того, чтобы условия выполнялись в порядке приоритетности.
                 * В стеке первый элемент - узел с более ранним выражением запроса.
                 * Выражение "and" строится от конца.
                 */
                foreach (MethodCallExpression reducedNode in _reducedNodes.Reverse())
                {
                    reducedNodeCondition = reducedNode.ExpandArgumentAsLambda(1);
                    replacementParams[reducedNodeCondition.Parameters[0].Name] = conditionParam;

                    operandBody = reducedNodeCondition.Body;
                    replaced = replacer.Replace(operandBody, replacementParams);

                    andExpr = Expression.MakeBinary(ExpressionType.AndAlso, replaced, andExpr);
                }
                andExpr = Expression.Lambda(andExpr, true, conditionParam);
                _targetMethodNodeParams[1] = andExpr;

                return Expression.Call(
                    null,
                    _targetMethodNode.Method,
                    _targetMethodNodeParams);
            }

            #endregion
        }

        #endregion
    }
}
