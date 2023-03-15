using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.DefaultQueryExecutors
{
    public class SkipWhileStreamingQueryExecutor<TData> : StreamingQueryExecutorWithEnumerableResult<TData, TData>
    {
        public SkipWhileStreamingQueryExecutor(Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition, (TData data, bool didDataPass) => data)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithNegativeCovering);
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            bool didDataPass = DataPassingCondition(data);

            return (didDataPass, true);
        }
    }
}
