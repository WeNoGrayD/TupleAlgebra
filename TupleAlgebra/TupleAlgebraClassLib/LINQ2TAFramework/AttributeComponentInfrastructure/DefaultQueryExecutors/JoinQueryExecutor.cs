using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.DefaultQueryExecutors
{
    public class JoinBufferingQueryExecutor<TOuterData, TInnerData, TKey, TQueryResultData>
        : BufferingQueryExecutorWithEnumerableResult<TOuterData, TQueryResultData>
        where TKey : notnull
    {
        private IEnumerable<TInnerData> _innerEnumerable;

        private Func<TOuterData, TKey> _outerKeySelector;

        private Func<TInnerData, TKey> _innerKeySelector;

        private Func<TOuterData, TInnerData, TQueryResultData> _transform;

        public JoinBufferingQueryExecutor(
            IEnumerable<TInnerData> innerEnumerable,
            Func<TOuterData, TKey> outerKeySelector,
            Func<TInnerData, TKey> innerKeySelector,
            Func<TOuterData, TInnerData, TQueryResultData> transform)
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
                foreach (TInnerData matchedInnerData in phonebook[_outerKeySelector(outerData)])
                    yield return _transform(outerData, matchedInnerData);
        }
    }

    public class JoinStreamingQueryExecutor<TOuterData, TInnerData, TKey, TQueryResultData>
        : StreamingQueryExecutorWithEnumerableOneToManyResult<TOuterData, TQueryResultData>
        where TKey : notnull
    {
        private IEnumerable<TInnerData> _innerEnumerable;

        private Func<TOuterData, TKey> _outerKeySelector;

        private Func<TInnerData, TKey> _innerKeySelector;

        private ILookup<TKey, TInnerData> _phonebook;

        private Func<TOuterData, TInnerData, TQueryResultData> _transform;

        public JoinStreamingQueryExecutor(
            IEnumerable<TInnerData> innerEnumerable,
            Func<TOuterData, TKey> outerKeySelector,
            Func<TInnerData, TKey> innerKeySelector,
            Func<TOuterData, TInnerData, TQueryResultData> transform)
            : base((_) => true)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
            _innerEnumerable = innerEnumerable;
            _outerKeySelector = outerKeySelector;
            _innerKeySelector = innerKeySelector;
            _transform = transform;
        }

        public override void PrepareToQueryStart()
        {
            _phonebook = _innerEnumerable.ToLookup(_innerKeySelector);
        }

        protected override IEnumerable<TQueryResultData> Match(TOuterData outerData)
        {
            foreach (TInnerData matchedInnerData in _phonebook[_outerKeySelector(outerData)])
                yield return _transform(outerData, matchedInnerData);
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TOuterData data) => (true, true);
    }
}
