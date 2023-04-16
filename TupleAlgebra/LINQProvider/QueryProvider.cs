using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Diagnostics;
using System.Reflection;

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
        protected abstract QueryPipelineExecutor CreateQueryPipelineExecutor(
            IEnumerable dataSource,
            IQueryPipelineMiddleware firstPipelineMiddleware);

        /// <summary>
        /// Определение перечислимости результата. Основывается на типе последнего запроса в цепочке.
        /// Доступна кастомизация.
        /// </summary>
        /// <param name="methodCallChain"></param>
        /// <returns></returns>
        private bool IsResultEnumerable(List<MethodCallExpression> methodCallChain)
        {
            MethodInfo lastMethod = methodCallChain[methodCallChain.Count - 1].Method;

            return lastMethod.Name switch
            {
                "All" => false,
                "Any" => false,
                "Contains" => false,
                "Count" => false,
                "First" => false,
                _ => IsResultEnumerableCustom(lastMethod)
            };
        }

        /// <summary>
        /// Кастомное определение перечислимости результата.
        /// </summary>
        /// <param name="lastMethod"></param>
        /// <returns></returns>
        protected virtual bool IsResultEnumerableCustom(MethodInfo lastMethod)
        {
            return lastMethod.Name switch
            {
                "Aggregate" => false,
                _ => true
            };
        }

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
            List<MethodCallExpression> methodCallChain = methodCallChainBuilder.MethodCallChain;
            IEnumerable queryDataSource = _queryDataSource = methodCallChainBuilder.QueryDataSource;

            /*
             * Запрос считается фиктивным, если цепочка методов запроса не содержит ни одного метода 
             * (только константу источника данных). 
             */
            if (methodCallChain.Count == 0)
            {
                _isResultEnumerable = true;
                _queryIsFiction = true;
                return (TQueryResult)queryDataSource;
            }

            _queryIsFiction = false;
            IQueryPipelineMiddleware firstQueryExecutor = null;
            _queryContext.ResetFirstQueryExecutor();
            foreach (Expression queryMethod in methodCallChain)
                firstQueryExecutor = _queryContext.BuildSingleQueryExecutor(queryMethod as MethodCallExpression);

            _isResultEnumerable = IsResultEnumerable(methodCallChain); //typeof(TQueryResult).GetInterface(nameof(IEnumerable)) is not null;

            QueryPipelineExecutor queryPipelineExecutor = CreateQueryPipelineExecutor(
                queryDataSource,
                firstQueryExecutor);

            return _isResultEnumerable ?
                ExecuteWithExpectedEnumerableResult() :
                queryPipelineExecutor.ExecuteWithExpectedAggregableResult<TQueryResult>();

            /*
             * Вызов метода исполнения запроса с ожидаемым перечислимым результатом.
             */
            TQueryResult ExecuteWithExpectedEnumerableResult()
            {
                /*
                 * Предполагается, что TQueryResult - это либо IEnumerable, либо IEnumerable<>.
                 * Если задать другой перечислимый тип, то могут возникнуть две ошибки:
                 * 1) отсутствия параметров типа (необобщённая реализация IEnumerable<>);
                 * 2) неоднозначности параметров типа (реализация нескольких вариантов IEnumerable<>).
                 */
                Type queryResultDataType = typeof(TQueryResult).GetGenericArguments().SingleOrDefault() ?? typeof(object);

                MethodInfo executionMethodInfo = queryPipelineExecutor
                    .GetType()
                    .GetMethod(nameof(QueryPipelineExecutor.ExecuteWithExpectedEnumerableResult))
                    .MakeGenericMethod(queryResultDataType);

                return (TQueryResult)executionMethodInfo.Invoke(queryPipelineExecutor, null);
            }
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

        /// <summary>
        /// Ивзлекатель первоначального источника данных из выражения запроса.
        /// </summary>
        /// <typeparam name="TDataSource"></typeparam>
        protected class DataSourceExtractor<TDataSource> : ExpressionVisitor
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
            #region Instance methods

            /// <summary>
            /// API для инспекции запроса.
            /// </summary>
            /// <param name="expr"></param>
            /// <returns></returns>
            public Expression Inspect(Expression expr)
            {
                return Visit(expr);
            }

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
                switch (node.Method.Name)
                {
                    case "Select":
                        {
                            CheckSelectQueryOnAcceptability(node);

                            break;
                        }
                }

                return base.VisitMethodCall(node);
            }

            #endregion
        }

        #endregion
    }
}
