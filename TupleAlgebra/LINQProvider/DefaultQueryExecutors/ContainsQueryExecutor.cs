using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.DefaultQueryExecutors
{
    public class ContainsStreamingQueryExecutor<TData> : AnyStreamingQueryExecutor<TData>
    {
        #region Constructors

        public ContainsStreamingQueryExecutor(TData sampleObj)
            : base(sampleObj is null ? 
                   (TData data) => data is null : 
                   (TData data) => sampleObj.Equals(data))
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
        }

        #endregion
    }
}
