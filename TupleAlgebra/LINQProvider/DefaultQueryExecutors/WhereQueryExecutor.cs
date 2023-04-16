using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.DefaultQueryExecutors
{
    public class WhereQueryBufferingExecutor<TData> : BufferingQueryExecutorWithEnumerableResult<TData, TData>
    {
        public WhereQueryBufferingExecutor(Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition)
        { }

        protected override IEnumerable<TData> TraverseOverDataSource()
        {
            foreach (TData data in DataSource)
                if (DataPassingCondition(data)) 
                    yield return data;
        }
    }

    public class WhereStreamingQueryExecutor<TData> : StreamingQueryExecutorWithEnumerableOneToOneResult<TData, TData>
    {
        public WhereStreamingQueryExecutor(Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition, (TData data) => data)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data) =>
            (DataPassingCondition(data), true);
    }
}
