using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using LINQProvider.DefaultQueryExecutors;
using System.Collections;

namespace LINQProvider
{
    public class QueryContext
    {
        private IQueryPipelineMiddleware _firstQueryExecutor;

        public void ResetFirstQueryExecutor() => _firstQueryExecutor = null;

        public IQueryPipelineMiddleware BuildSingleQueryExecutor(MethodCallExpression methodExpr) =>
            (methodExpr.Arguments.Count, methodExpr.Arguments) switch
            {
                (1, _) => RecognizeUnderlyingExpressionAndBuildSingleQueryExecutor(methodExpr),
                (2, [_, UnaryExpression param1Expr]) => 
                    RecognizeUnderlyingExpressionAndBuildSingleQueryExecutor(
                        methodExpr, (param1Expr.Operand as LambdaExpression).Compile()),
                (2, [_, ConstantExpression param1Expr]) =>
                    RecognizeUnderlyingExpressionAndBuildSingleQueryExecutor(
                        methodExpr, param1Expr.Value),
                (3, [_, UnaryExpression param1Expr, UnaryExpression param2Expr]) =>
                    RecognizeUnderlyingExpressionAndBuildSingleQueryExecutor(
                        methodExpr, 
                        (param1Expr.Operand as LambdaExpression).Compile(), 
                        (param2Expr.Operand as LambdaExpression).Compile()),
                (3, [_, ConstantExpression param1Expr, UnaryExpression param2Expr]) =>
                    RecognizeUnderlyingExpressionAndBuildSingleQueryExecutor(
                        methodExpr, 
                        param1Expr.Value, 
                        (param2Expr.Operand as LambdaExpression).Compile()),
                (5, [_, ConstantExpression param1Expr, UnaryExpression param2Expr, UnaryExpression param3Expr, UnaryExpression param4Expr]) => 
                    RecognizeUnderlyingExpressionAndBuildSingleQueryExecutor(
                        methodExpr, 
                        param1Expr.Value, 
                        (param2Expr.Operand as LambdaExpression).Compile(), 
                        (param3Expr.Operand as LambdaExpression).Compile(), 
                        (param4Expr.Operand as LambdaExpression).Compile()),
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

        protected IQueryPipelineMiddleware RecognizeUnderlyingExpressionAndBuildSingleQueryExecutor(
            MethodCallExpression methodExpr,
            params object[] methodParams)
        {
            string queryMethodName = methodExpr.Method.Name;
            MethodInfo queryBuildingMethod = typeof(QueryContext)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(method => method.Name == "Build" + queryMethodName + "Query" && 
                       method.GetParameters().Length == methodParams.Length)
                .Single()
                .MakeGenericMethod(methodExpr.Method.GetGenericArguments());

            return BuildSingleQueryExecutorImpl(queryBuildingMethod, methodParams);
        }

        private IQueryPipelineMiddleware BuildSingleQueryExecutorImpl(
            MethodInfo queryBuildingMethod, params object[] queryBuildingMethodParams)
        {
            IQueryPipelineAcceptor iqpa = 
                queryBuildingMethod.Invoke(this, queryBuildingMethodParams) as IQueryPipelineAcceptor;
            IQueryPipelineMiddleware iqpm = QueryPipelineMiddlewareWithAccumulationFactory.Create((dynamic)iqpa);

            _firstQueryExecutor = _firstQueryExecutor is null ? iqpm : _firstQueryExecutor.ContinueWith((dynamic)iqpm);

            return _firstQueryExecutor;
        }

        protected virtual SingleQueryExecutor<TData, TQueryResult> BuildAggregateQuery<TData, TQueryResult>(
            TQueryResult seed,
            Func<TQueryResult, TData, TQueryResult> aggregatingFunc) 
            => new AggregateStreamingQueryExecutor<TData, TQueryResult>(aggregatingFunc, seed);

        protected virtual SingleQueryExecutor<TData, bool> BuildAllQuery<TData>(
            Func<TData, bool> predicate) => new AllStreamingQueryExecutor<TData>(predicate);

        protected virtual SingleQueryExecutor<TData, bool> BuildAnyQuery<TData>(
            Func<TData, bool> predicate) => new AnyStreamingQueryExecutor<TData>(predicate);

        protected virtual SingleQueryExecutor<TData, bool> BuildContainsQuery<TData>(
            TData sampleObj) => new ContainsStreamingQueryExecutor<TData>(sampleObj);

        protected virtual SingleQueryExecutor<TData, int> BuildCountQuery<TData>() =>
            new CountByFilterStreamingQueryExecutor<TData>((_) => true);

        protected virtual SingleQueryExecutor<TData, int> BuildCountQuery<TData>(
            Func<TData, bool> predicate) => new CountByFilterStreamingQueryExecutor<TData>(predicate);

        protected virtual SingleQueryExecutor<TData, TData> BuildFirstQuery<TData>(
            Func<TData, bool> predicate) => new FirstByFilterStreamingQueryExecutor<TData>(predicate);

        protected virtual SingleQueryExecutor<TData, TData> BuildFirstQuery<TData>() => 
            new FirstStreamingQueryExecutor<TData>();
        

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

        protected virtual SingleQueryExecutor<TData, IEnumerable<TData>> BuildSkipQuery<TData>(
            int skippingCount) => new SkipStreamingQueryExecutor<TData>(skippingCount);

        protected virtual SingleQueryExecutor<TData, IEnumerable<TData>> BuildSkipWhileQuery<TData>(
            Func<TData, bool> predicate) => new SkipWhileStreamingQueryExecutor<TData>(predicate);

        protected virtual SingleQueryExecutor<TData, IEnumerable<TData>> BuildTakeQuery<TData>(
            int takingCount) => new TakeStreamingQueryExecutor<TData>(takingCount);

        protected virtual SingleQueryExecutor<TData, IEnumerable<TData>> BuildTakeWhileQuery<TData>(
            Func<TData, bool> predicate) => new TakeWhileStreamingQueryExecutor<TData>(predicate);

        protected virtual SingleQueryExecutor<TData, IEnumerable<TData>> BuildWhereQuery<TData>(
            Func<TData, bool> predicate) => new WhereStreamingQueryExecutor<TData>(predicate);
    }
}
