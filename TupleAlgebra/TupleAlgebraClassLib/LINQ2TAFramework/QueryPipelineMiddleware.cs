using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{

    public interface IQueryPipelineAcceptor
    {
        TPipelineQueryResult Accept<TPipelineQueryResultParam, TPipelineQueryResult>(
            bool isResultEnumerabl, ISingleQueryExecutorVisitor queryPipeline);
    }

    public interface IQueryPipelineMiddleware : IQueryPipelineAcceptor
    {
        IQueryPipelineMiddleware ContinueWith(IQueryPipelineMiddleware continuingExecutor) => default(IQueryPipelineMiddleware);

        IQueryPipelineEndpoint GetPipelineEndpoint();

        TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipelineQueryExecutor);

        TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor);

        IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor);
    }

    public interface IQueryPipelineMiddleware<TData, TQueryResult> : IQueryPipelineMiddleware
    {
        SingleQueryExecutor<TData, TQueryResult> InnerExecutor { get; }

        IQueryPipelineMiddleware<TData, TQueryResult> ContinueWith<TContinuingQueryData, TContinuingQueryResult>(
                IQueryPipelineMiddleware<TContinuingQueryData, TContinuingQueryResult> continuingExecutor);
        void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<TQueryResult> queryResultPassingHandler);

        //void SubscribeOninnerExecutorEventsOnDataInstanceProcessing<TQueryResultData>(
        //QueryPipelineExecutor.LoadPipelineAccumulatorHandler<TQueryResultData> queryResultPassingHandler);

    }

    public interface IQueryPipelineEndpoint : IQueryPipelineMiddleware
    {
        void InitializeAsQueryPipelineEndpoint();
    }

    public interface IQueryPipelineEndpoint<TData, TQueryResult>
        : IQueryPipelineEndpoint, IQueryPipelineMiddleware<TData, TQueryResult>
    {
        TQueryResult Accumulator { get; }
    }
}
