using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Streaming;

namespace LINQProvider.DefaultQueryExecutors
{
    public class SkipWhileStreamingQueryExecutor<TData> 
        : ConditionBasedStreamingQueryExecutorWithEnumerableOneToOneResult<TData>
    {
        private bool _skippedHead = false;

        public SkipWhileStreamingQueryExecutor(Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithFullCovering);

            Action<IEnumerable<TData>> onDataPassed = null!;
            onDataPassed = (_) =>
            {
                _skippedHead = true;
                DataPassed -= onDataPassed;
            };

            DataPassed += onDataPassed;
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            return ((_skippedHead || !_condition(data)), true);
        }
    }
}
