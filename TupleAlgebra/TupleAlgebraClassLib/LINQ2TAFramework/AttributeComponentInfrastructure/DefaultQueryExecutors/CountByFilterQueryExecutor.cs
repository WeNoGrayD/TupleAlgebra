using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.DefaultQueryExecutors
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
            : base(dataPassingCondition, (TData data, bool didDataPass) => 0)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
            this.DataPassed += (_) => _count++;
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            bool didDataPass = DataPassingCondition(data);

            return (didDataPass, true);
        }

        public override void AccumulateIfDataPassed(ref int accumulator, int outputData)
        {
            accumulator = _count;
        }
    }
}
