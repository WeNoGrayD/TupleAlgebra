using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.DefaultQueryExecutors
{
    public class SkipStreamingQueryExecutor<TData> : StreamingQueryExecutorWithEnumerableOneToOneResult<TData, TData>
    {
        private bool _skippedHead = false;

        private int _skippedCount;

        public SkipStreamingQueryExecutor(int skippingCount)
            : base(null, (data) => data)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithNegativeCovering);
            _skippedCount = skippingCount;
        }

        public override bool ExecuteOverDataInstanceHandlerWithNegativeCovering(TData data)
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

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            throw new NotImplementedException();
        }
    }
}
