using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.DefaultQueryExecutors
{
    public class ContainsStreamingQueryExecutor<TData> : StreamingQueryExecutorWithAggregableResult<TData, bool>
    {
        public ContainsStreamingQueryExecutor(TData sampleObj)
            : base((TData data) => sampleObj.Equals(data), (TData data, bool didDataPass) => didDataPass)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            bool didDataPass = DataPassingCondition(data);

            return (didDataPass, !didDataPass);
        }

        public override void AccumulateIfDataPassed(ref bool accumulator, bool outputData)
        {
            accumulator = true;
        }
    }
}
