using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Streaming;

namespace LINQProvider.DefaultQueryExecutors
{
    public class SkipStreamingQueryExecutor<TData> : StreamingQueryExecutorWithEnumerableOneToOneResult<TData, TData>
    {
        private bool _skippedHead = false;

        private int _skippedCount;

        public SkipStreamingQueryExecutor(int skippingCount)
            : base(null, (data) => data)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithFullCovering);
            _skippedCount = skippingCount;
            
            Action<IEnumerable<TData>> onDataPassed = null;
            onDataPassed = (_) =>
            {
                _skippedHead = true;
                DataPassed -= onDataPassed;
            };

            DataPassed += onDataPassed;
            DataNotPassed += (_) => _skippedCount--;
        }

        /*
        public override (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithPositiveCovering(TData data)
        {
            if (_skippedHead || _skippedCount == 0)
            {
                _skippedHead = true;
                ModifyIntermediateQueryResult(data);
                OnDataNotPassed(IntermediateQueryResult);
            }
            else _skippedCount--;

            return true;
        }
        */

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            return ((_skippedHead || _skippedCount == 0), true);
        }
    }
}
