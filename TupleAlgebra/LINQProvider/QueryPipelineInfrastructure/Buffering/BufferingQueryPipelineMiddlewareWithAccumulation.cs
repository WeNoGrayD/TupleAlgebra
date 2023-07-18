using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure;
using LINQProvider.QueryResultAccumulatorInterfaces;

namespace LINQProvider.QueryPipelineInfrastructure.Buffering
{
    public class BufferingQueryPipelineMiddlewareWithAccumulation<TData, TQueryResult, TAccumulator>
        : QueryPipelineMiddlewareWithAccumulation<BufferingQueryExecutor<TData, TQueryResult>, TData, TQueryResult, TAccumulator>
        where TAccumulator : TQueryResult
    {
        public BufferingQueryPipelineMiddlewareWithAccumulation(
            LinkedListNode<IQueryPipelineMiddleware> node,
            BufferingQueryExecutor<TData, TQueryResult> innerExecutor,
            IAccumulatePositiveQueryResult<TQueryResult, TAccumulator> innerExecutorAsAccumulating)
            : base(node, innerExecutor, innerExecutorAsAccumulating)
        { }

        public override IQueryPipelineMiddleware<TData, TQueryResult>
            ContinueWith<TContinuingQueryData, TContinuingQueryResult>(
                IQueryPipelineMiddleware<TContinuingQueryData, TContinuingQueryResult> continuingExecutor,
                IQueryPipelineScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public override TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipeline)
        {
            _innerExecutorImpl.GetQueryResult();

            return (TPipelineQueryResult)(object)_accumulator!;
        }

        public override IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            throw new NotImplementedException();
        }

        public override void SubscribeOnInnerExecutorEventsOnDataInstanceProcessing(
            Action<TQueryResult> queryResultPassingHandler)
        {
            InnerExecutor.DataPassed += (outputData) =>
                queryResultPassingHandler(_innerExecutorImpl.GetQueryResult());
        }
    }

    public class BufferingQueryExecutorWithAccumulationOfEnumerableResult<TData, TQueryResultData>
        : QueryPipelineMiddlewareWithAccumulation<BufferingQueryExecutorWithEnumerableResult<TData, TQueryResultData>,
                                              TData,
                                              IEnumerable<TQueryResultData>,
                                              IEnumerable<TQueryResultData>>
    {
        public BufferingQueryExecutorWithAccumulationOfEnumerableResult(
            LinkedListNode<IQueryPipelineMiddleware> node,
            BufferingQueryExecutorWithEnumerableResult<TData, TQueryResultData> innerExecutor,
            IAccumulatePositiveQueryResult<IEnumerable<TQueryResultData>, IEnumerable<TQueryResultData>> innerExecutorAsAccumulating)
            : base(node, innerExecutor, innerExecutorAsAccumulating)
        { }

        public override IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>>
            ContinueWith<TContinuingQueryData, TContinuingQueryResult>(
                IQueryPipelineMiddleware<TContinuingQueryData, TContinuingQueryResult> continuingExecutor,
                IQueryPipelineScheduler scheduler)
        {
            scheduler.PushMiddleware(continuingExecutor);

            return this;
        }

        public override IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipeline)
        {
            _innerExecutorImpl.GetQueryResult();

            return _accumulator as IEnumerable<TPipelineQueryResultData>;
        }

        public override TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            throw new NotImplementedException();
        }

        public override void SubscribeOnInnerExecutorEventsOnDataInstanceProcessing(
            Action<IEnumerable<TQueryResultData>> queryResultPassingHandler)
        {
            InnerExecutor.DataPassed += (outputData) =>
                queryResultPassingHandler(_innerExecutorImpl.GetQueryResult());
        }
    }
}
