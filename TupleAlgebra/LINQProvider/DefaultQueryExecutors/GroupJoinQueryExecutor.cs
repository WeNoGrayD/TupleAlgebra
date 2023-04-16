using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.DefaultQueryExecutors
{
    public class GroupJoinBufferingQueryExecutor<TOuterData, TInnerData, TKey, TQueryResultData>
        : BufferingQueryExecutorWithEnumerableResult<TOuterData, TQueryResultData>
        where TKey : notnull
    {
        private IEnumerable<TInnerData> _innerEnumerable;

        private Func<TOuterData, TKey> _outerKeySelector;

        private Func<TInnerData, TKey> _innerKeySelector;

        private Func<TOuterData, IEnumerable<TInnerData>, TQueryResultData> _transform;

        public GroupJoinBufferingQueryExecutor(
            IEnumerable<TInnerData> innerEnumerable,
            Func<TOuterData, TKey> outerKeySelector,
            Func<TInnerData, TKey> innerKeySelector,
            Func<TOuterData, IEnumerable<TInnerData>, TQueryResultData> transform)
            : base((_) => true)
        {
            _innerEnumerable = innerEnumerable;
            _outerKeySelector = outerKeySelector;
            _innerKeySelector = innerKeySelector;
            _transform = transform;
        }

        protected override IEnumerable<TQueryResultData> TraverseOverDataSource()
        {
            ILookup<TKey, TInnerData> phonebook = _innerEnumerable.ToLookup(_innerKeySelector);

            foreach (TOuterData outerData in DataSource)
                yield return _transform(outerData, phonebook[_outerKeySelector(outerData)]);
        }
    }

    public class GroupJoinStreamingQueryExecutor<TOuterData, TInnerData, TKey, TQueryResultData>
        : StreamingQueryExecutorWithEnumerableOneToOneResult<TOuterData, TQueryResultData>
        where TKey : notnull
    {
        private IEnumerable<TInnerData> _innerEnumerable;

        private Func<TOuterData, TKey> _outerKeySelector;

        private Func<TInnerData, TKey> _innerKeySelector;

        private ILookup<TKey, TInnerData> _phonebook;

        public GroupJoinStreamingQueryExecutor(
            IEnumerable<TInnerData> innerEnumerable,
            Func<TOuterData, TKey> outerKeySelector,
            Func<TInnerData, TKey> innerKeySelector,
            Func<TOuterData, IEnumerable<TInnerData>, TQueryResultData> transform)
            : base((_) => true, null)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithFullCovering);
            _innerEnumerable = innerEnumerable;
            _outerKeySelector = outerKeySelector;
            _innerKeySelector = innerKeySelector;
            _transform = (TOuterData outerData) => transform(outerData, _phonebook[_outerKeySelector(outerData)]);
        }

        public override void PrepareToQueryStart()
        {
            _phonebook = _innerEnumerable.ToLookup(_innerKeySelector);
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TOuterData data) =>
            (DataPassingCondition(data), true);
    }
}
