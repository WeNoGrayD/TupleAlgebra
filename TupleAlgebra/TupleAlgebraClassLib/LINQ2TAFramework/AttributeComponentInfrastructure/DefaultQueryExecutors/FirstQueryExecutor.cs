using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.DefaultQueryExecutors
{
    public class FirstStreamingQueryExecutor<TData> : StreamingQueryExecutorWithAggregableResult<TData, TData>
    {
        public FirstStreamingQueryExecutor(Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition, (TData data, bool didDataPass) => data)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            bool didDataPass = DataPassingCondition(data);

            return (didDataPass, !didDataPass);
        }

        public override void AccumulateIfDataPassed(ref TData accumulator, TData outputData)
        {
            accumulator = outputData;
        }
    }
}
