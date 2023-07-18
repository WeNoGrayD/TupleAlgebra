using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Streaming;

namespace LINQProvider.DefaultQueryExecutors
{
    public class FirstByFilterStreamingQueryExecutor<TData> : StreamingQueryExecutorWithAggregableResult<TData, TData>
    {
        public FirstByFilterStreamingQueryExecutor(Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            bool didDataPass = DataPassingCondition(data);

            return (didDataPass, !didDataPass);
        }

        protected override TData ModifyIntermediateQueryResult(TData data)
        {
            return data;
        }

        public override void AccumulateIfDataPassed(ref TData accumulator, TData outputData)
        {
            accumulator = outputData;
        }
    }
}
