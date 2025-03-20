using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Buffering;
using LINQProvider.QueryPipelineInfrastructure.Streaming;

namespace LINQProvider.DefaultQueryExecutors
{
    public class SelectStreamingQueryExecutor<TData, TQueryResultData> 
        : TransformBasedStreamingQueryExecutorWithEnumerableOneToOneResult<TData, TQueryResultData>
    {
        public SelectStreamingQueryExecutor(Func<TData, TQueryResultData> transform)
            : base(transform)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data) => (true, true);
    }
}
