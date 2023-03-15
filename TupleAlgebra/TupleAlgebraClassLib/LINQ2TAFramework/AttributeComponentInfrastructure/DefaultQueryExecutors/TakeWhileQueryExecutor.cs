using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.DefaultQueryExecutors
{
    public class TakeWhileStreamingQueryExecutor<TData> : StreamingQueryExecutorWithEnumerableResult<TData, TData>
    {
        public TakeWhileStreamingQueryExecutor(Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition, (TData data, bool didDataPass) => data)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            bool didDataPass = DataPassingCondition(data);

            return (didDataPass, didDataPass);
        }
    }
}
