using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure;
using LINQProvider.QueryResultAccumulatorInfrastructure;

namespace LINQProvider.QueryPipelineInfrastructure.Buffering
{
    public class BufferingQueryPipelineMiddlewareWithAccumulation<TData, TQueryResult>
        : QueryPipelineMiddlewareWithAccumulation<
            TData, 
            TQueryResult, 
            BufferingQueryExecutor<TData, TQueryResult>>
    {
        #region Constructors

        public BufferingQueryPipelineMiddlewareWithAccumulation(
            BufferingQueryExecutor<TData, TQueryResult> innerExecutor)
            : base(innerExecutor)
        {
            return;
        }

        #endregion

        #region Instance methods

        public override void InitializeAsQueryPipelineStartupMiddleware()
        {
            return;
        }

        public override IQueryPipelineMiddleware<TData, TQueryResult> ContinueWith(
            IQueryPipelineEndpoint continuingExecutor,
            IQueryPipelineScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Инициализация компонента в качестве конечного компонента конвейера запросов.
        /// </summary>
        public override void InitializeAsQueryPipelineEndpoint()
        {
            return;
        }

        public override TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorResultRequester pipeline)
        {
            return (_innerExecutorImpl as BufferingQueryExecutor<TData, TPipelineQueryResult >)!.Execute();
        }

        public override IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorResultRequester pipeline)
        {
            return (_innerExecutorImpl.Execute() as IEnumerable<TPipelineQueryResultData>)!;
        }

        #endregion

        #region IQueryPipelineAcceptor implementation

        public override TPipelineQueryResult AcceptToExecuteWithAggregableResult<TPipelineQueryResult>(
            ISingleQueryExecutorResultRequester resultRequster)
        {
            return resultRequster.VisitBufferingQueryExecutorWithExpectedAggregableResult<TData, TQueryResult, TPipelineQueryResult>(
                _innerExecutorImpl);
        }

        public override IEnumerable<TPipelineQueryResultData> AcceptToExecuteWithEnumerableResult<TPipelineQueryResultData>(
            System.Collections.IEnumerable dataSource,
            ISingleQueryExecutorResultRequester resultRequster)
        {
            return resultRequster.VisitBufferingQueryExecutorWithExpectedEnumerableResult<
                TData, 
                TQueryResult, 
                TPipelineQueryResultData>(
                    dataSource,
                    this,
                    _innerExecutorImpl);
        }

        #endregion

        #region IQueryPipelineMiddlewareWithContinuationAcceptor<TQueryResultData>

        public override void Accept(ISingleQueryExecutorVisitor<TData> visitor)
        {
            visitor.VisitBufferingQueryExecutor(_innerExecutorImpl);

            return;
        }

        public override void Accept(
            IQueryPipelineScheduler scheduler,
            IQueryPipelineMiddlewareVisitor<TData> visitor)
        {
            visitor.VisitBufferingMiddleware(scheduler, this);

            return;
        }

        #endregion

        #region IQueryPipelineMiddleware implementation

        public override void PrepareToAggregableResult<TPipelineQueryResult>(ISingleQueryExecutorResultRequester pipelineQueryExecutor)
        { }

        public override void PrepareToEnumerableResult<TPipelineQueryResultData>(ISingleQueryExecutorResultRequester pipelineQueryExecutor)
        { }

        #endregion
    }

    public class BufferingQueryPipelineMiddlewareWithAccumulationOfEnumerableResult<TData, TQueryResultData>
        : BufferingQueryPipelineMiddlewareWithAccumulation<TData, IEnumerable<TQueryResultData>>
    {
        #region Constructors

        public BufferingQueryPipelineMiddlewareWithAccumulationOfEnumerableResult(
            BufferingQueryExecutorWithEnumerableResult<TData, TQueryResultData> innerExecutor)
            : base(innerExecutor)
        {
            return;
        }

        #endregion

        #region Instance methods

        public override IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> ContinueWith(
            IQueryPipelineEndpoint continuingExecutor,
            IQueryPipelineScheduler scheduler)
        {
            scheduler.PushMiddleware(continuingExecutor);

            return this;
        }

        #endregion
    }
}
