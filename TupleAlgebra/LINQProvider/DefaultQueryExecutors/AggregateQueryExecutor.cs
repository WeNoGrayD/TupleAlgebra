using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.DefaultQueryExecutors
{
    public class AggregateStreamingQueryExecutor<TData, TQueryResult>
        : StreamingQueryExecutorWithAggregableResult<TData, TQueryResult>
    {
        private TQueryResult _queryResult;

        private Func<TQueryResult, TData, TQueryResult> _aggregatingFunc;

        public AggregateStreamingQueryExecutor(
            Func<TQueryResult, TData, TQueryResult> aggregatingFunc,
            TQueryResult seed = default(TQueryResult))
            : base((_) => true)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
            _queryResult = seed;
            _aggregatingFunc = aggregatingFunc;
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            bool didDataPass = DataPassingCondition(data);

            return (didDataPass, true);
        }

        protected override TQueryResult ModifyIntermediateQueryResult(TData data)
        {
            return _queryResult = _aggregatingFunc(_queryResult, data);
        }

        public override void AccumulateIfDataPassed(ref TQueryResult accumulator, TQueryResult outputData)
        {
            accumulator = _queryResult;
        }
    }
}
