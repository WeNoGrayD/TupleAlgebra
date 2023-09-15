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
    using static QueryContextHelper;

    public class LinqQueryContext : QueryContext
    {
        #region Constants

        protected static Regex _queryMethodPattern { get; private set; } 
            = new Regex($@"^Build(?<Query>\w+)Query$");

        #endregion

        #region Constructors

        static LinqQueryContext()
        {
            IDictionary<string, IList<MethodInfo>> queryMethodPatterns =
                GetQueryMethodPatterns<LinqQueryContext>(_queryMethodPattern);
            Helper.RegisterType<LinqQueryContext>(
                queryMethodGroups: queryMethodPatterns);

            return;
        }

        public LinqQueryContext() : base()
        {
            return;
        }

        #endregion

        #region Instance methods

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
