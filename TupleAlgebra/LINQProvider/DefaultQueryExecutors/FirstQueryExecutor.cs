using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.DefaultQueryExecutors
{
    public class FirstStreamingQueryExecutor<TData> : StreamingQueryExecutorWithAggregableResult<TData, TData>
    {
        public FirstStreamingQueryExecutor()
            : base(null)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            return (true, false);
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
