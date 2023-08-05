using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryPipelineInfrastructure.Streaming
{
    public interface IStreamingQueryExecutor<TData>
    {
        #region Instance properties 

        Func<TData, (bool ResultProvided, bool MustGoOn)> ExecuteOverDataInstance { get; }

        #endregion
    }

    public interface IStreamingQueryPipelineMiddleware<TData>
        : IQueryPipelineMiddleware
    {
        #region Instance properties

        LinkedListNode<IQueryPipelineMiddleware> PipelineScheduleNode { get; }

        IStreamingQueryExecutor<TData> Executor { get; }

        #endregion

        #region Instance methods

        void PreparePipelineResultProvider();

        #endregion
    }
}
