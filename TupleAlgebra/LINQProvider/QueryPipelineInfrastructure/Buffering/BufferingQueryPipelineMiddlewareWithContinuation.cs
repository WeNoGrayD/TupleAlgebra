using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure;

namespace LINQProvider.QueryPipelineInfrastructure.Buffering
{
    /*
    public class BufferingQueryPipelineMiddlewareWithContinuation<TData, TQueryResultData, TNextQueryResult>
        : QueryPipelineMiddlewareWithContinuation<
            BufferingQueryExecutorWithEnumerableResult<TData, TQueryResultData>,
            TData,
            TQueryResultData,
            TNextQueryResult>
    {
        public override bool MustGoOn { get => NextExecutor.MustGoOn; }

        public BufferingQueryPipelineMiddlewareWithContinuation(
            LinkedListNode<IQueryPipelineMiddleware> node,
            BufferingQueryExecutorWithEnumerableResult<TData, TQueryResultData> innerExecutor,
            IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> nextExecutor)
            : base(node, innerExecutor, nextExecutor)
        { }

        protected override void DataPassedHandler(IEnumerable<TQueryResultData> outputData)
        {
            return;
        }

        private TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipeline)
        {
            // Явное уведомление исполнителя запроса о требовании подытоживания запроса.
            IEnumerable<TQueryResultData> queryResult = _innerExecutorImpl.GetQueryResult();

            // Переход к следующему исполнителю запроса.
            return default(TPipelineQueryResult);
            //return pipeline.ContinueQueryExecuting<TQueryResultData, TPipelineQueryResult>(NextExecutor, queryResult);
        }

        /// <summary>
        /// Получение агрегируемого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        public override TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            return GetPipelineQueryResult<TPipelineQueryResult>(pipelineQueryExecutor);
        }

        /// <summary>
        /// Получение перечислимого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        public override IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            return GetPipelineQueryResult<IEnumerable<TPipelineQueryResultData>>(pipelineQueryExecutor);
        }
    }
    */
}
