using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.DefaultQueryExecutors
{
    public class SkipWhileStreamingQueryExecutor<TData> : StreamingQueryExecutorWithEnumerableOneToOneResult<TData, TData>
    {
        private bool _skippedHead = false;

        public SkipWhileStreamingQueryExecutor(Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition, (TData data) => data)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithNegativeCovering);
        }

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

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            throw new NotImplementedException();
        }
    }
}
