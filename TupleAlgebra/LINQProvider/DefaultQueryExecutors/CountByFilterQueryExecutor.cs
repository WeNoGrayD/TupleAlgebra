using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.DefaultQueryExecutors
{
    public class CountByFilterBufferingQueryExecutor<TData> :
        BufferingQueryExecutorWithAggregableResult<TData, int>
    {
        public CountByFilterBufferingQueryExecutor(Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition)
        { }

        protected override int TraverseOverDataSource()
        {
            int count = 0;
            foreach (TData data in DataSource)
                if (DataPassingCondition(data)) count++;

            return count;
        }
    }

    public class CountByFilterStreamingQueryExecutor<TData> 
        : StreamingQueryExecutorWithAggregableResult<TData, int>
    {
        private int _count = 0;

        public CountByFilterStreamingQueryExecutor(Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            bool didDataPass = DataPassingCondition(data);

            return (didDataPass, true);
        }

        protected override int ModifyIntermediateQueryResult(TData data)
        {
            return ++_count;
        }

        public override void AccumulateIfDataPassed(ref int accumulator, int outputData)
        {
            accumulator = _count;
        }
    }
}
