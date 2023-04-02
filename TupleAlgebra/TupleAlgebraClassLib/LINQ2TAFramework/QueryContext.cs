using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.DefaultQueryExecutors;
using System.Collections;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    public abstract class QueryContext
    {
        private IQueryPipelineMiddleware _firstQueryExecutor;

        public void ResetFirstQueryExecutor() => _firstQueryExecutor = null;

        public IQueryPipelineMiddleware BuildSingleQueryExecutor(MethodCallExpression methodExpr) =>
            (methodExpr.Arguments.Count, methodExpr.Arguments) switch
            {
                (2, [_, UnaryExpression param1Expr]) => 
                    RecognizeUnderlyingExpressionAndBuildSingleQueryExecutor(
                        (dynamic)param1Expr.Operand, methodExpr),
                (3, [_, UnaryExpression param1Expr, UnaryExpression param2Expr]) =>
                    RecognizeUnderlyingExpressionAndBuildSingleQueryExecutor(
                        (dynamic)param1Expr.Operand, (dynamic)param2Expr.Operand, methodExpr),
                (5, [_, ConstantExpression param1Expr, UnaryExpression param2Expr, UnaryExpression param3Expr, UnaryExpression param4Expr]) => 
                    RecognizeUnderlyingExpressionAndBuildSingleQueryExecutor(
                        param1Expr, (dynamic)param2Expr.Operand, (dynamic)param3Expr.Operand, (dynamic)param4Expr.Operand, methodExpr),
                var methodInfo => BuildCustomSingleQueryExecutor(methodExpr, methodInfo)
            };

        /// <summary>
        /// Метод для построения пользовательских запросов с сигнатурами методов, отличными от нескольких стандартных.
        /// </summary>
        /// <param name="methodExpr"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual IQueryPipelineMiddleware BuildCustomSingleQueryExecutor(
            MethodCallExpression methodExpr, 
            (int Argc, System.Collections.ObjectModel.ReadOnlyCollection<Expression> Argv) info) =>
            throw new NotImplementedException();

        /// <summary>
        /// Распознавание стандартного запроса с одним параметром.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResultData"></typeparam>
        /// <param name="lambdaFunc"></param>
        /// <param name="methodExpr"></param>
        /// <returns></returns>
        protected IQueryPipelineMiddleware RecognizeUnderlyingExpressionAndBuildSingleQueryExecutor<TData, TQueryResultData>(
            Expression<Func<TData, TQueryResultData>> lambdaFunc,
            MethodCallExpression methodExpr)
        {
            string queryMethodName = methodExpr.Method.Name;
            MethodInfo queryBuildingMethod = typeof(QueryContext)
                .GetMethod("Build" + queryMethodName + "Query", BindingFlags.NonPublic | BindingFlags.Instance);

            queryBuildingMethod = queryBuildingMethod.GetGenericArguments().Length switch
            {
                1 => queryBuildingMethod.MakeGenericMethod(typeof(TData)),
                2 => queryBuildingMethod.MakeGenericMethod(typeof(TData), typeof(TQueryResultData)),
                _ => throw new Exception()
            };

            return BuildSingleQueryExecutorImpl(queryBuildingMethod, lambdaFunc.Compile());
        }

        /// <summary>
        /// Распознавание запросов соединений - внутренних и групповых.
        /// </summary>
        /// <typeparam name="TOuterData"></typeparam>
        /// <typeparam name="TInnerData"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResultData"></typeparam>
        /// <param name="innerEnumerable"></param>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="resultSelector"></param>
        /// <param name="methodExpr"></param>
        /// <returns></returns>
        protected IQueryPipelineMiddleware 
            RecognizeUnderlyingExpressionAndBuildSingleQueryExecutor<TOuterData, TInnerData, TKey, TProjection, TResultData>(
            ConstantExpression innerEnumerableExpr,
            Expression<Func<TOuterData, TKey>> outerKeySelectorExpr,
            Expression<Func<TInnerData, TKey>> innerKeySelectorExpr,
            Expression<Func<TOuterData, TProjection, TResultData>> resultSelectorExpr,
            MethodCallExpression methodExpr)
        {
            string queryMethodName = methodExpr.Method.Name;
            MethodInfo queryBuildingMethod = typeof(QueryContext)
                .GetMethod("Build" + queryMethodName + "Query", BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(typeof(TOuterData), typeof(TInnerData), typeof(TKey), typeof(TResultData));

            return BuildSingleQueryExecutorImpl(
                queryBuildingMethod,
                innerEnumerableExpr.Value, 
                outerKeySelectorExpr.Compile(), 
                innerKeySelectorExpr.Compile(), 
                resultSelectorExpr.Compile());
        }

        /// <summary>
        /// Распознавание перекрёстных соединений.
        /// </summary>
        /// <typeparam name="TOuterData"></typeparam>
        /// <typeparam name="TInnerData"></typeparam>
        /// <typeparam name="TProjection"></typeparam>
        /// <typeparam name="TResultData"></typeparam>
        /// <param name="innerDataSelectorExpr"></param>
        /// <param name="resultSelectorExpr"></param>
        /// <param name="methodExpr"></param>
        /// <returns></returns>
        protected IQueryPipelineMiddleware
            RecognizeUnderlyingExpressionAndBuildSingleQueryExecutor<TOuterData, TInnerData, TProjection, TResultData>(
            Expression<Func<TOuterData, IEnumerable<TInnerData>>> innerDataSelectorExpr,
            Expression<Func<TOuterData, TProjection, TResultData>> resultSelectorExpr,
            MethodCallExpression methodExpr)
        {
            string queryMethodName = methodExpr.Method.Name;
            MethodInfo queryBuildingMethod = typeof(QueryContext)
                .GetMethod("Build" + queryMethodName + "Query", BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(typeof(TOuterData), typeof(TInnerData), typeof(TResultData));

            return BuildSingleQueryExecutorImpl(
                queryBuildingMethod,
                innerDataSelectorExpr.Compile(),
                resultSelectorExpr.Compile());
        }

        private IQueryPipelineMiddleware BuildSingleQueryExecutorImpl(
            MethodInfo queryBuildingMethod, params object[] queryBuildingMethodParams)
        {
            IQueryPipelineAcceptor iqpa = 
                queryBuildingMethod.Invoke(this, queryBuildingMethodParams) as IQueryPipelineAcceptor;
            IQueryPipelineMiddleware iqpm = SingleQueryExecutorWithAccumulationFactory.Create((dynamic)iqpa);

            _firstQueryExecutor = _firstQueryExecutor is null ? iqpm : _firstQueryExecutor.ContinueWith((dynamic)iqpm);

            return _firstQueryExecutor;
        }

        protected virtual SingleQueryExecutor<TData, bool> BuildAllQuery<TData>(
            Func<TData, bool> predicate) => new AllStreamingQueryExecutor<TData>(predicate);

        protected virtual SingleQueryExecutor<TData, bool> BuildAnyQuery<TData>(
            Func<TData, bool> predicate) => new AnyStreamingQueryExecutor<TData>(predicate);

        protected virtual SingleQueryExecutor<TData, int> BuildCountQuery<TData>(
            Func<TData, bool> predicate) => new CountByFilterBufferingQueryExecutor<TData>(predicate);

        protected virtual SingleQueryExecutor<TData, TData> BuildFirstQuery<TData>(
            Func<TData, bool> predicate) => new FirstStreamingQueryExecutor<TData>(predicate);

        protected virtual SingleQueryExecutor<TOuterData, IEnumerable<TQueryResultData>>
            BuildGroupJoinQuery<TOuterData, TInnerData, TKey, TQueryResultData>(
                IEnumerable<TInnerData> innerEnumerable,
                Func<TOuterData, TKey> outerKeySelector,
                Func<TInnerData, TKey> innerKeySelector,
                Func<TOuterData, IEnumerable<TInnerData>, TQueryResultData> resultSelector) =>
            new GroupJoinStreamingQueryExecutor<TOuterData, TInnerData, TKey, TQueryResultData>(
                innerEnumerable,
                outerKeySelector,
                innerKeySelector,
                resultSelector);

        protected virtual SingleQueryExecutor<TOuterData, IEnumerable<TQueryResultData>> 
            BuildJoinQuery<TOuterData, TInnerData, TKey, TQueryResultData>(
                IEnumerable<TInnerData> innerEnumerable,
                Func<TOuterData, TKey> outerKeySelector,
                Func<TInnerData, TKey> innerKeySelector,
                Func<TOuterData, TInnerData, TQueryResultData> resultSelector) =>
            new JoinStreamingQueryExecutor<TOuterData, TInnerData, TKey, TQueryResultData>(
                innerEnumerable, 
                outerKeySelector, 
                innerKeySelector,
                resultSelector);

        protected virtual SingleQueryExecutor<TData, IEnumerable<TQueryResultData>> BuildSelectQuery<TData, TQueryResultData>(
            Func<TData, TQueryResultData> func) => new SelectStreamingQueryExecutor<TData, TQueryResultData>(func);

        protected virtual SingleQueryExecutor<TOuterData, IEnumerable<TQueryResultData>>
            BuildSelectManyQuery<TOuterData, TInnerData, TQueryResultData>(
            Func<TOuterData, IEnumerable<TInnerData>> innerDataSelector,
            Func<TOuterData, TInnerData, TQueryResultData> resultSelector) =>
            new SelectManyStreamingQueryExecutor<TOuterData, TInnerData, TQueryResultData>(
                innerDataSelector, resultSelector);

        protected virtual SingleQueryExecutor<TData, IEnumerable<TData>> BuildSkipWhileQuery<TData>(
            Func<TData, bool> predicate) => new SkipWhileStreamingQueryExecutor<TData>(predicate);

        protected virtual SingleQueryExecutor<TData, IEnumerable<TData>> BuildTakeWhileQuery<TData>(
            Func<TData, bool> predicate) => new TakeWhileStreamingQueryExecutor<TData>(predicate);

        protected virtual SingleQueryExecutor<TData, IEnumerable<TData>> BuildWhereQuery<TData>(
            Func<TData, bool> predicate) => new WhereStreamingQueryExecutor<TData>(predicate);
    }
}
