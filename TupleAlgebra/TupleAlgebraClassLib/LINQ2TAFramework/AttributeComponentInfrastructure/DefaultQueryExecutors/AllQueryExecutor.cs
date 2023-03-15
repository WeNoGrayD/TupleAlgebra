using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.DefaultQueryExecutors
{
    public class AllStreamingQueryExecutor<TData> : StreamingQueryExecutorWithAggregableResult<TData, bool>
    {
        private bool _success = true;

        public AllStreamingQueryExecutor(Func<TData, bool> dataPassingCondition) 
            : base(dataPassingCondition, null)
        {
            Transform = (TData data, bool didDataPass) => _success;
            InitBehavior(ExecuteOverDataInstanceHandlerWithFullCovering);
        }

        public override bool ExecuteOverDataInstanceHandlerWithFullCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) = ConsumeData(data);
            if (didDataPass)
            {
                OnDataPassed(_success);
            }
            else OnDataNotPassed(_success);

            return mustGoOn;
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            bool didDataPass = DataPassingCondition(data);
            _success &= didDataPass;

            return (didDataPass, didDataPass);
        }

        public override void AccumulateIfDataPassed(ref bool accumulator, bool outputData)
        {
            accumulator = _success;
        }

        public override void AccumulateIfDataNotPassed(ref bool accumulator, bool outputData)
        {
            accumulator = _success;
        }
    }
}
