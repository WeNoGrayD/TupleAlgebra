using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Buffering;
using LINQProvider.QueryPipelineInfrastructure.Streaming;

namespace LINQProvider.DefaultQueryExecutors
{
    public class GroupJoinStreamingQueryExecutor<TOuterData, TInnerData, TKey, TQueryResultData>
        : TransformBasedStreamingQueryExecutorWithEnumerableOneToOneResult<TOuterData, TQueryResultData>
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
            : base()
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);

            _innerEnumerable = innerEnumerable;
            _outerKeySelector = outerKeySelector;
            _innerKeySelector = innerKeySelector;
            _transform = (TOuterData outerData) => transform(outerData, _phonebook[_outerKeySelector(outerData)]);
        }

        public override void PrepareToQueryStart()
        {
            _phonebook = _innerEnumerable.ToLookup(_innerKeySelector);
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TOuterData outerData) => 
            (_phonebook.Contains(_outerKeySelector(outerData)), true);
    }
}
