using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.DefaultQueryExecutors
{
    public class ContainsStreamingQueryExecutor<TData> : AnyStreamingQueryExecutor<TData>
    {
        public ContainsStreamingQueryExecutor(TData sampleObj)
            : base((TData data) => sampleObj?.Equals(data) ?? data is null)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
        }
    }
}
