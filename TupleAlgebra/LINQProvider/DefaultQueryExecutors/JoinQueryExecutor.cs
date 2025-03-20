using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Buffering;
using LINQProvider.QueryPipelineInfrastructure.Streaming;

namespace LINQProvider.DefaultQueryExecutors
{
    public class InnerJoinStreamingQueryExecutor<TOuterData, TInnerData, TKey, TQueryResultData>
        : TransformBasedStreamingQueryExecutorWithEnumerableOneToManyResult<TOuterData, TQueryResultData>
        where TKey : notnull
    {
        private IEnumerable<TInnerData> _innerEnumerable;

        private Func<TOuterData, TKey> _outerKeySelector;

        private Func<TInnerData, TKey> _innerKeySelector;

        private ILookup<TKey, TInnerData> _phonebook;

        private Func<TOuterData, TInnerData, TQueryResultData> _transform;

        public InnerJoinStreamingQueryExecutor(
            IEnumerable<TInnerData> innerEnumerable,
            Func<TOuterData, TKey> outerKeySelector,
            Func<TInnerData, TKey> innerKeySelector,
            Func<TOuterData, TInnerData, TQueryResultData> transform)
            : base()
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

        protected override IEnumerable<TQueryResultData> Transform(TOuterData outerData)
        {
            foreach (TInnerData matchedInnerData in _phonebook[_outerKeySelector(outerData)])
                yield return _transform(outerData, matchedInnerData);
        }

        /*
         * Данные считаются переданными, если в лукапе присутствует соответствующий ключ.
         */
        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TOuterData outerData) => 
            (_phonebook.Contains(_outerKeySelector(outerData)), true);
    }

    public class LeftOuterJoinStreamingQueryExecutor<TOuterData, TInnerData, TKey, TQueryResultData>
        : TransformBasedStreamingQueryExecutorWithEnumerableOneToManyResult<TOuterData, TQueryResultData?>
        where TKey : notnull
    {
        private IEnumerable<TInnerData> _innerEnumerable;

        private Func<TOuterData, TKey> _outerKeySelector;

        private Func<TInnerData, TKey> _innerKeySelector;

        private ILookup<TKey, TInnerData> _phonebook;

        private Func<TOuterData, TInnerData, TQueryResultData> _transform;

        public LeftOuterJoinStreamingQueryExecutor(
            IEnumerable<TInnerData> innerEnumerable,
            Func<TOuterData, TKey> outerKeySelector,
            Func<TInnerData, TKey> innerKeySelector,
            Func<TOuterData, TInnerData?, TQueryResultData> transform)
            : base()
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

        protected override IEnumerable<TQueryResultData> Transform(TOuterData outerData)
        {
            TKey outerKey = _outerKeySelector(outerData);
            if (_phonebook.Contains(outerKey))
            {
                foreach (TInnerData matchedInnerData in _phonebook[outerKey])
                {
                    yield return _transform(outerData, matchedInnerData);
                }
            }
            else yield return _transform(outerData, default(TInnerData)!);
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TOuterData outerData) => 
            (true, true);
    }

    public class RightOuterJoinStreamingQueryExecutor<TOuterData, TInnerData, TKey, TQueryResultData>
        : TransformBasedStreamingQueryExecutorWithEnumerableOneToManyResult<TOuterData, TQueryResultData>
        where TKey : notnull
    {
        private IEnumerable<TInnerData> _innerEnumerable;

        private Func<TOuterData, TKey> _outerKeySelector;

        private Func<TInnerData, TKey> _innerKeySelector;

        private ILookup<TKey, TInnerData> _phonebook;

        private Func<TOuterData, TInnerData, TQueryResultData> _transform;

        private HashSet<TKey> _actualOuterKeys;

        public RightOuterJoinStreamingQueryExecutor(
            IEnumerable<TInnerData> innerEnumerable,
            Func<TOuterData, TKey> outerKeySelector,
            Func<TInnerData, TKey> innerKeySelector,
            Func<TOuterData, TInnerData, TQueryResultData> transform)
            : base()
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

        protected override IEnumerable<TQueryResultData> Transform(TOuterData outerData)
        {
            TKey outerKey = _outerKeySelector(outerData);

            if (_actualOuterKeys is null)
                _actualOuterKeys = new HashSet<TKey>();
            _actualOuterKeys.Add(outerKey);

            foreach (TInnerData matchedInnerData in _phonebook[outerKey])
            {
                yield return _transform(outerData, matchedInnerData);
            }
        }

        public IEnumerable<TQueryResultData> AfterTraversingImpl()
        {
            foreach (IGrouping<TKey, TInnerData> group in _phonebook)
            {
                if (_actualOuterKeys.Contains(group.Key)) continue;

                foreach (TInnerData innerData in group)
                    yield return _transform(default(TOuterData)!, innerData);
            }
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TOuterData outerData) =>
            (_phonebook.Contains(_outerKeySelector(outerData)), true);
    }

    public class FullOuterJoinStreamingQueryExecutor<TOuterData, TInnerData, TKey, TQueryResultData>
        : TransformBasedStreamingQueryExecutorWithEnumerableOneToManyResult<TOuterData, TQueryResultData>
        where TKey : notnull
    {
        private IEnumerable<TInnerData> _innerEnumerable;

        private Func<TOuterData, TKey> _outerKeySelector;

        private Func<TInnerData, TKey> _innerKeySelector;

        private ILookup<TKey, TInnerData> _phonebook;

        private Func<TOuterData, TInnerData, TQueryResultData> _transform;

        private HashSet<TKey> _actualOuterKeys;

        public FullOuterJoinStreamingQueryExecutor(
            IEnumerable<TInnerData> innerEnumerable,
            Func<TOuterData, TKey> outerKeySelector,
            Func<TInnerData, TKey> innerKeySelector,
            Func<TOuterData, TInnerData, TQueryResultData> transform)
            : base()
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

        protected override IEnumerable<TQueryResultData> Transform(TOuterData outerData)
        {
            TKey outerKey = _outerKeySelector(outerData);

            if (_actualOuterKeys is null)
                _actualOuterKeys = new HashSet<TKey>();
            _actualOuterKeys.Add(outerKey);

            if (_phonebook.Contains(outerKey))
            {
                foreach (TInnerData matchedInnerData in _phonebook[outerKey])
                {
                    yield return _transform(outerData, matchedInnerData);
                }
            }
            else yield return _transform(outerData, default(TInnerData)!);
        }

        public IEnumerable<TQueryResultData> AfterTraversingImpl()
        {
            foreach (IGrouping<TKey, TInnerData> group in _phonebook)
            {
                if (_actualOuterKeys.Contains(group.Key)) continue;

                foreach (TInnerData innerData in group)
                    yield return _transform(default(TOuterData)!, innerData);
            }
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TOuterData outerData) =>
            (true, true);
    }
}
