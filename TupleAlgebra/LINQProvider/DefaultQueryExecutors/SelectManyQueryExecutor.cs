using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Streaming;

namespace LINQProvider.DefaultQueryExecutors
{
    internal class SelectManyStreamingQueryExecutor<TOuterData, TInnerData, TQueryResultData>
        : TransformBasedStreamingQueryExecutorWithEnumerableOneToManyResult<TOuterData, TQueryResultData>
    {
        private Func<TOuterData, IEnumerable<TInnerData>> _innerDataSelector;

        private Func<TOuterData, TInnerData, TQueryResultData> _transform;

        public SelectManyStreamingQueryExecutor(
            Func<TOuterData, IEnumerable<TInnerData>> innerDataSelector,
            Func<TOuterData, TInnerData, TQueryResultData> transform)
            : base()
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
            _innerDataSelector = innerDataSelector;
            _transform = transform;
        }

        protected override IEnumerable<TQueryResultData> Transform(TOuterData outerData)
        {
            foreach (TInnerData innerData in _innerDataSelector(outerData))
                yield return _transform(outerData, innerData);
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TOuterData data) => (true, true);
    }
}
