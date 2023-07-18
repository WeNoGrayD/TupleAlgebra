using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Streaming;

namespace LINQProvider.DefaultQueryExecutors
{
    public class AnyStreamingQueryExecutor<TData> : StreamingQueryExecutorWithAggregableResult<TData, bool>
    {
        public AnyStreamingQueryExecutor(Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            bool didDataPass = DataPassingCondition(data);

            return (didDataPass, !didDataPass);
        }

        protected override bool ModifyIntermediateQueryResult(TData data)
        {
            return false;
        }

        public override void AccumulateIfDataPassed(ref bool accumulator, bool outputData)
        {
            accumulator = true;
        }
    }
}
