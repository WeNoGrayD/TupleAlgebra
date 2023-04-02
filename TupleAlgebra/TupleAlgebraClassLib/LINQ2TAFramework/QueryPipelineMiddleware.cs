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
        bool MustGoOn { get; }

        IQueryPipelineMiddleware ContinueWith(IQueryPipelineMiddleware continuingExecutor) => default(IQueryPipelineMiddleware);

        IQueryPipelineEndpoint GetPipelineEndpoint();

        void PrepareToAggregableResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipelineQueryExecutor) { }

        void PrepareToEnumerableResult<TPipelineQueryResultData>(ISingleQueryExecutorVisitor pipelineQueryExecutor) { }

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
    }

    public interface IQueryPipelineEndpoint : IQueryPipelineMiddleware
    {
        void InitializeAsQueryPipelineEndpoint();
    }

    public interface IStreamingQueryPipelineMiddleware
    {
        TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipeline,
            ref bool mustGoOn);

        (IEnumerable<TPipelineQueryResultData> QueryResult, bool MustGoOn) 
            GetEnumerablePipelineQueryResult2<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipeline);
    }

    public interface IQueryPipelineEndpoint<TData, TQueryResult>
        : IQueryPipelineEndpoint, IQueryPipelineMiddleware<TData, TQueryResult>
    {
        TQueryResult Accumulator { get; }
    }
}
