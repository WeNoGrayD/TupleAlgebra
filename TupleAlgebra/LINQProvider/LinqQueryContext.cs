using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;
using LINQProvider.DefaultQueryExecutors;

namespace LINQProvider
{
    public class LinqQueryContext : QueryContext
    {
        protected static IDictionary<string, IList<MethodInfo>> _queryBuildingMethods;

        #region Constructors

        public LinqQueryContext() : base()
        {
            if (_queryBuildingMethods is null)
                InitQueryBuildingMethods();

            return;
        }

        #endregion

        #region Instance methods

        private void InitQueryBuildingMethods()
        {
            _queryBuildingMethods = new Dictionary<string, IList<MethodInfo>>();

            Regex queryBuildingMethodNamePattern = new Regex($@"^Build(?<Query>\w+)Query$");
            MethodInfo[] queryContextMethods = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
            MethodInfo queryMethod;
            Match queryMethodMatch;
            string queryName;

            for (int i = 0; i < queryContextMethods.Length; i++)
            {
                queryMethod = queryContextMethods[i];
                queryMethodMatch = queryBuildingMethodNamePattern.Match(queryMethod.Name);
                if (!queryMethodMatch.Success) continue;

                queryName = queryMethodMatch.Groups["Query"].Captures[0].Value;
                if (!_queryBuildingMethods.ContainsKey(queryName))
                    _queryBuildingMethods[queryName] = new List<MethodInfo>(1);
                _queryBuildingMethods[queryName].Add(queryMethod);
            }

            return;
        }

        protected override MethodInfo FindQueryBuildingMethod(
            System.Linq.Expressions.MethodCallExpression methodExpr,
            object[] methodParams)
        {
            MethodInfo queryMethod = methodExpr.Method;
            IList<MethodInfo> matchedMethods = _queryBuildingMethods[queryMethod.Name];
            MethodInfo targetMethod = null!;

            for (int i = 0; i < matchedMethods.Count; i++)
            {
                targetMethod = matchedMethods[i].MakeGenericMethod(methodExpr.Method.GetGenericArguments());

                if (!CheckCompatibilityWithTarget(targetMethod, methodParams)) continue;
                
                break;
            }

            return targetMethod!;
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
            new InnerJoinStreamingQueryExecutor<TOuterData, TInnerData, TKey, TQueryResultData>(
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

        protected virtual SingleQueryExecutor<TData, TData[]> BuildToArrayQuery<TData>()
            => new ToArrayBufferingQueryExecutor<TData>();

        protected virtual SingleQueryExecutor<TData, Dictionary<TKey, TValue>> BuildToDictionaryQuery<TData, TKey, TValue>(
            Func<TData, TKey> keySelector, Func<TData, TValue> valueSelector = null!)
            where TKey : notnull
            => new ToDictionaryBufferingQueryExecutor<TData, TKey, TValue>(keySelector, valueSelector);

        protected virtual SingleQueryExecutor<TData, List<TData>> BuildToListQuery<TData>()
            => new ToListBufferingQueryExecutor<TData>();

        #endregion
    }
}
