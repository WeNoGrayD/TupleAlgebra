using LINQProvider.QueryPipelineInfrastructure.Buffering;
using LINQProvider.QueryPipelineInfrastructure.Streaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryPipelineInfrastructure
{
    public interface IQueryPipelineMiddlewareVisitor<TQueryResultData>
    {
        /// <summary>
        /// Обработка случая, когда следующий компонент конвейера запросов является
        /// потоковым компонентом.
        /// </summary>
        /// <param name="nextStreaming"></param>
        void VisitStreamingMiddleware<TExecutorQueryResult>(
            IQueryPipelineScheduler scheduler,
            StreamingQueryPipelineMiddlewareWithAccumulation<TQueryResultData, TExecutorQueryResult> 
                streaming);

        /// <summary>
        /// Обработка случая, когда следующий компонент конвейера запросов является
        /// буферизирующим компонентов.
        /// </summary>
        /// <param name="nextBuffering"></param>
        void VisitBufferingMiddleware<TExecutorQueryResult>(
            IQueryPipelineScheduler scheduler,
            BufferingQueryPipelineMiddlewareWithAccumulation<TQueryResultData, TExecutorQueryResult> buffering);
    }
}
