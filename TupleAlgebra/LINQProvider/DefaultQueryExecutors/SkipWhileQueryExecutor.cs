using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Streaming;

namespace LINQProvider.DefaultQueryExecutors
{
    public class SkipWhileStreamingQueryExecutor<TData> : StreamingQueryExecutorWithEnumerableOneToOneResult<TData, TData>
    {
        private bool _skippedHead = false;

        public SkipWhileStreamingQueryExecutor(Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition, (TData data) => data)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithFullCovering);

            Action<IEnumerable<TData>> onDataPassed = null;
            onDataPassed = (_) =>
            {
                _skippedHead = true;
                DataPassed -= onDataPassed;
            };

            DataPassed += onDataPassed;
        }

        /*
        public override bool ExecuteOverDataInstanceHandlerWithNegativeCovering(TData data)
        {
            if (_skippedHead || !DataPassingCondition(data))
            {
                _skippedHead = true;
                ModifyIntermediateQueryResult(data);
                OnDataNotPassed(IntermediateQueryResult);
            }

            return true;
        }
        */

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            return ((_skippedHead || !DataPassingCondition(data)), true);
        }
    }
}
