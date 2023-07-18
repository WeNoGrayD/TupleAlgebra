using LINQProvider.QueryPipelineInfrastructure.Streaming;
using LINQProvider.QueryPipelineInfrastructure.Buffering;
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
        void VisitStreamingQueryExecutor<TExecutorQueryResult>(
            StreamingQueryExecutor<TQueryResultData, TExecutorQueryResult> streaming);

        /// <summary>
        /// Обработка случая, когда следующий компонент конвейера запросов является
        /// буферизирующим компонентов.
        /// </summary>
        /// <param name="nextBuffering"></param>
        void VisitBufferingQueryExecutor<TExecutorQueryResult>(
            BufferingQueryExecutor<TQueryResultData, TExecutorQueryResult> buffering);
    }
}
