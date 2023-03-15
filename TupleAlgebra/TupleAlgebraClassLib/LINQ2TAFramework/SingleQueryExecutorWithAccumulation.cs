using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{

    /// <summary>
    /// Декоратор исполнителя запроса для аккумулирования результатов запроса. Конечная точка конвейера запросов.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResult"></typeparam>
    /// <typeparam name="TAccumulator"></typeparam>
    public abstract class SingleQueryExecutorWithAccumulation<TInnerQueryExecutor, TData, TQueryResult, TAccumulator>
        : SingleQueryExecutor<TData, TQueryResult>, IQueryPipelineEndpoint<TData, TQueryResult>
        where TInnerQueryExecutor : SingleQueryExecutor<TData, TQueryResult>
        where TAccumulator : TQueryResult
    {
        private TInnerQueryExecutor _innerExecutorImpl;

        protected TInnerQueryExecutor InnerExecutorImpl
        {
            get => _innerExecutorImpl;
            private set => _innerExecutorImpl = value;
        }

        private IAccumulatePositiveQueryResult<TQueryResult, TAccumulator> _innerExecutorAsAccumulating;

        public SingleQueryExecutor<TData, TQueryResult> InnerExecutor => _innerExecutorImpl;

        protected TAccumulator _accumulator;

        public TQueryResult Accumulator => _accumulator;

        protected SingleQueryExecutorWithAccumulation(
            TInnerQueryExecutor innerExecutor,
            IAccumulatePositiveQueryResult<TQueryResult, TAccumulator> innerExecutorAsAccumulating)
            : base()
        {
            InnerExecutorImpl = innerExecutor;
            _innerExecutorAsAccumulating = innerExecutorAsAccumulating;
        }

        public void InitializeAsQueryPipelineEndpoint()
        {
            _accumulator = _innerExecutorAsAccumulating.InitAccumulator();
            InnerExecutorImpl.DataPassed +=
                (TQueryResult outputData) => _innerExecutorAsAccumulating.AccumulateIfDataPassed(ref _accumulator, outputData);
            if (InnerExecutorImpl is IAccumulateNegativeQueryResult<TQueryResult, TAccumulator> fullCoveringReader)
                fullCoveringReader.DataNotPassed +=
                    (TQueryResult outputData) => fullCoveringReader.AccumulateIfDataNotPassed(ref _accumulator, outputData);
        }

        public IQueryPipelineMiddleware<TData, TQueryResult>
            ContinueWith<TContinuingQueryData, TContinuingQueryResult>(
                IQueryPipelineMiddleware<TContinuingQueryData, TContinuingQueryResult> continuingExecutor) =>
            SingleQueryExecutorWithContinuationFactory.Create(
                this as IQueryPipelineMiddleware<TData, IEnumerable<TContinuingQueryData>>, continuingExecutor)
            as IQueryPipelineMiddleware<TData, TQueryResult>;

        IQueryPipelineMiddleware IQueryPipelineMiddleware.ContinueWith(IQueryPipelineMiddleware continuingExecutor)
            => ContinueWith((dynamic)continuingExecutor);

        public IQueryPipelineEndpoint GetPipelineEndpoint() => this;

        public abstract TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipeline);

        public virtual TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor) =>
            GetPipelineQueryResult<TPipelineQueryResult>(pipelineQueryExecutor);

        public virtual IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor) =>
            GetPipelineQueryResult<IEnumerable<TPipelineQueryResultData>>(pipelineQueryExecutor);

        public virtual void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<TQueryResult> queryResultPassingHandler) =>
            InnerExecutor.DataPassed += queryResultPassingHandler;

        public override TPipelineQueryResult
            Accept<TPipelineQueryResultParam, TPipelineQueryResult>(
                bool isResultEnumerable, ISingleQueryExecutorVisitor queryPipeline) =>
                InnerExecutor.Accept<TPipelineQueryResultParam, TPipelineQueryResult>(isResultEnumerable, queryPipeline);
    }

    public class StreamingQueryExecutorWithAccumulationOfAggregableResult<TData, TQueryResult>
        : SingleQueryExecutorWithAccumulation<
            StreamingQueryExecutor<TData, TQueryResult>,
            TData,
            TQueryResult,
            TQueryResult>
    {
        public StreamingQueryExecutorWithAccumulationOfAggregableResult(
            StreamingQueryExecutorWithAggregableResult<TData, TQueryResult> innerExecutor)
            : base(innerExecutor, innerExecutor)
        { }

        public override TPipelineQueryResultData GetPipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipeline)
        {
            return (dynamic)_accumulator;
        }
        public override void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<TQueryResult> queryResultPassingHandler)
        {
            base.SubscribeOninnerExecutorEventsOnDataInstanceProcessing(queryResultPassingHandler);
            InnerExecutorImpl.DataNotPassed += queryResultPassingHandler;
        }
    }

    public class StreamingQueryExecutorWithAccumulationOfEnumerableResult<TData, TQueryResultData>
        : SingleQueryExecutorWithAccumulation<
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>>,
            TData,
            IEnumerable<TQueryResultData>,
            IEnumerable<TQueryResultData>>
    {
        public StreamingQueryExecutorWithAccumulationOfEnumerableResult(
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>> innerExecutor,
            IAccumulatePositiveQueryResult<IEnumerable<TQueryResultData>, IEnumerable<TQueryResultData>> innerExecutorAsAccumulating)
            : base(innerExecutor, innerExecutorAsAccumulating)
        { }

        public override TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipeline)
        {
            return (dynamic)_accumulator;
        }
        public override void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<IEnumerable<TQueryResultData>> queryResultPassingHandler)
        {
            base.SubscribeOninnerExecutorEventsOnDataInstanceProcessing(queryResultPassingHandler);
            InnerExecutorImpl.DataNotPassed += queryResultPassingHandler;
        }
    }

    public class BufferingQueryExecutorWithAccumulation<TData, TQueryResult, TAccumulator>
        : SingleQueryExecutorWithAccumulation<BufferingQueryExecutor<TData, TQueryResult>, TData, TQueryResult, TAccumulator>
        where TAccumulator : TQueryResult
    {
        public BufferingQueryExecutorWithAccumulation(
            BufferingQueryExecutor<TData, TQueryResult> innerExecutor,
            IAccumulatePositiveQueryResult<TQueryResult, TAccumulator> innerExecutorAsAccumulating)
            : base(innerExecutor, innerExecutorAsAccumulating)
        { }

        public override TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipeline)
        {
            InnerExecutorImpl.GetQueryResult();

            return (dynamic)_accumulator;
        }

        public override void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<TQueryResult> queryResultPassingHandler)
        {
            InnerExecutor.DataPassed += (TQueryResult outputData) =>
                queryResultPassingHandler(InnerExecutorImpl.GetQueryResult());
        }
    }

    public class BufferingQueryExecutorWithAccumulationOfEnumerableResult<TData, TQueryResultData>
        : SingleQueryExecutorWithAccumulation<BufferingQueryExecutorWithEnumerableResult<TData, TQueryResultData>,
                                              TData,
                                              IEnumerable<TQueryResultData>,
                                              IEnumerable<TQueryResultData>>
    {
        public BufferingQueryExecutorWithAccumulationOfEnumerableResult(
            BufferingQueryExecutorWithEnumerableResult<TData, TQueryResultData> innerExecutor,
            IAccumulatePositiveQueryResult<IEnumerable<TQueryResultData>, IEnumerable<TQueryResultData>> innerExecutorAsAccumulating)
            : base(innerExecutor, innerExecutorAsAccumulating)
        { }

        public override TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipeline)
        {
            InnerExecutorImpl.GetQueryResult();

            return (dynamic)_accumulator;
        }

        public override void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<IEnumerable<TQueryResultData>> queryResultPassingHandler)
        {
            InnerExecutor.DataPassed += (IEnumerable<TQueryResultData> outputData) =>
                queryResultPassingHandler(InnerExecutorImpl.GetQueryResult());
        }
    }
}
