using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using LINQProvider.DefaultQueryExecutors;
using System.Collections;
using LINQProvider.QueryPipelineInfrastructure;
using System.ComponentModel;

namespace LINQProvider
{
    public class QueryContext
    {
        public IQueryPipelineAcceptor BuildSingleQueryExecutor(MethodCallExpression methodExpr)
        {
            int argc = methodExpr.Arguments.Count - 1;
            if (argc > 0)
            {
                object[] arguments = new object[argc];
                Expression argExpr;
                for (int i = 0; i < argc; i++)
                {
                    argExpr = methodExpr.Arguments[i + 1];
                    arguments[i] = argExpr switch
                    {
                        ConstantExpression cExpr => cExpr.Expand(),
                        UnaryExpression unaryExpr => unaryExpr.ExpandAsDelegate(),
                        _ => throw new NotImplementedException()
                    };
                }

                return RecognizeUnderlyingExpressionAndBuildSingleQueryExecutor(methodExpr, arguments);
            }

            return RecognizeUnderlyingExpressionAndBuildSingleQueryExecutor(methodExpr);
        }

        /// <summary>
        /// Метод для построения пользовательских запросов с сигнатурами методов, отличными от нескольких стандартных.
        /// </summary>
        /// <param name="methodExpr"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual IQueryPipelineAcceptor BuildCustomSingleQueryExecutor(
            MethodCallExpression methodExpr, 
            (int Argc, System.Collections.ObjectModel.ReadOnlyCollection<Expression> Argv) info) =>
            throw new NotImplementedException();

        protected IQueryPipelineAcceptor RecognizeUnderlyingExpressionAndBuildSingleQueryExecutor(
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

        private IQueryPipelineAcceptor BuildSingleQueryExecutorImpl(
            MethodInfo queryBuildingMethod,
            params object[] queryBuildingMethodParams)
        {
            return (queryBuildingMethod.Invoke(this, queryBuildingMethodParams) as IQueryPipelineAcceptor)!;
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
