using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.DefaultQueryExecutors
{
    public class SkipWhileStreamingQueryExecutor<TData> : StreamingQueryExecutorWithEnumerableResult<TData, TData>
    {
        private bool _skippedHead = false;

        public SkipWhileStreamingQueryExecutor(Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition, (TData data, bool didDataPass) => data)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithNegativeCovering);
        }

        public override bool ExecuteOverDataInstanceHandlerWithNegativeCovering(TData data)
        {
            if (_skippedHead || !DataPassingCondition(data))
            {
                _skippedHead = true;
                ModifyIntermediateQueryResult(data, false);
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
