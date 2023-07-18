using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Streaming;

namespace LINQProvider.DefaultQueryExecutors
{
    public class AllStreamingQueryExecutor<TData> : StreamingQueryExecutorWithAggregableResult<TData, bool>
    {
        #region Instance fields
        
        private bool _success = true;

        #endregion

        public AllStreamingQueryExecutor(Func<TData, bool> dataPassingCondition) 
            : base(dataPassingCondition)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithFullCovering);
        }

        /*
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
        */

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            bool didDataPass = DataPassingCondition(data);
            _success &= didDataPass;

            return (didDataPass, didDataPass);
        }

        protected override bool ModifyIntermediateQueryResult(TData data)
        {
            return _success;
        }

        public override void AccumulateIfDataPassed(ref bool accumulator, bool outputData)
        {
            accumulator = outputData;
        }

        public override void AccumulateIfDataNotPassed(ref bool accumulator, bool outputData)
        {
            accumulator = outputData;
        }
    }
}
