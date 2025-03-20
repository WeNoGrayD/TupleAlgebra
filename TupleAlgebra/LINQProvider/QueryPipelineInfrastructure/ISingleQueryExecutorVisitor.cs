using LINQProvider.QueryPipelineInfrastructure.Streaming;
using LINQProvider.QueryPipelineInfrastructure.Buffering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryPipelineInfrastructure
{
    public interface ISingleQueryExecutorVisitor<TQueryResultData>
    {
        /// <summary>
        /// Посещение потокового исполнителя запроса.
        /// </summary>
        /// <param name="nextStreaming"></param>
        void VisitStreamingQueryExecutor<TExecutorQueryResult>(
            StreamingQueryExecutor<TQueryResultData, TExecutorQueryResult> streaming);

        /// <summary>
        /// Посещение буферизирующего исполнителя запроса.
        /// </summary>
        /// <param name="nextBuffering"></param>
        void VisitBufferingQueryExecutor<TExecutorQueryResult>(
            BufferingQueryExecutor<TQueryResultData, TExecutorQueryResult> buffering);
    }
}
