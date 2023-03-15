using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    /// <summary>
    /// Декоратор исполнителя запроса для передачи результата запроса на следующего исполнителя запросов в конвейере.
    /// </summary>
    /// <typeparam name="TInnerQueryExecutor"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResultData"></typeparam>
    /// <typeparam name="TNextQueryResult"></typeparam>
    public abstract class SingleQueryExecutorWithContinuation<TInnerQueryExecutor, TData, TQueryResultData, TNextQueryResult>
        : SingleQueryExecutor<TData, IEnumerable<TQueryResultData>>,
          IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>>
        where TInnerQueryExecutor : SingleQueryExecutor<TData, IEnumerable<TQueryResultData>>
    {
        private TInnerQueryExecutor _innerExecutorImpl;

        protected TInnerQueryExecutor InnerExecutorImpl
        {
            get => _innerExecutorImpl;
            private set => _innerExecutorImpl = value;
        }

        public SingleQueryExecutor<TData, IEnumerable<TQueryResultData>> InnerExecutor => _innerExecutorImpl;

        public IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> NextExecutor { get; protected set; }

        protected SingleQueryExecutorWithContinuation(
            TInnerQueryExecutor innerExecutor,
            IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> nextExecutor)
            : base()
        {
            InnerExecutorImpl = innerExecutor;
            NextExecutor = nextExecutor;
            innerExecutor.DataPassed += (IEnumerable<TQueryResultData> outputData) => this.OnDataPassed(outputData);

            switch (NextExecutor.InnerExecutor)
            {
                case StreamingQueryExecutor<TQueryResultData, TNextQueryResult> streaming:
                    {
                        VisitNextStreamingQueryExecutorOnInit(streaming);
                        break;
                    }
                case BufferingQueryExecutor<TQueryResultData, TNextQueryResult> buffering:
                    {
                        VisitNextBufferingQueryExecutorOnInit(buffering);
                        break;
                    }
                default: throw new ArgumentException();
            }
        }

        public void UpdateNextQueryExecutor(IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> newNextExecutor)
            => NextExecutor = newNextExecutor;

        public abstract TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipeline);

        public IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>>
            ContinueWith<TContinuingQueryData, TContinuingQueryResult>(
                IQueryPipelineMiddleware<TContinuingQueryData, TContinuingQueryResult> continuingExecutor)
        {
            NextExecutor = NextExecutor.ContinueWith(continuingExecutor);

            return this;
        }

        public virtual TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor) =>
            NextExecutor.GetAggregablePipelineQueryResult<TPipelineQueryResult>(pipelineQueryExecutor);

        public virtual IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor) =>
            NextExecutor.GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(pipelineQueryExecutor);

        IQueryPipelineMiddleware IQueryPipelineMiddleware.ContinueWith(IQueryPipelineMiddleware continuingExecutor)
            => ContinueWith((dynamic)continuingExecutor);

        public IQueryPipelineEndpoint GetPipelineEndpoint() => NextExecutor.GetPipelineEndpoint();

        IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>>
            .ContinueWith<TContinuingQueryData, TContinuingQueryResult>(
                IQueryPipelineMiddleware<TContinuingQueryData, TContinuingQueryResult> continuingExecutor)
        {
            NextExecutor = NextExecutor.ContinueWith(continuingExecutor);

            return this;
        }

        public virtual void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<IEnumerable<TQueryResultData>> queryResultPassingHandler) =>
            InnerExecutor.DataPassed += queryResultPassingHandler;

        protected abstract void DataPassedHandler(IEnumerable<TQueryResultData> outputData);

        protected virtual void VisitNextStreamingQueryExecutorOnInit(
            StreamingQueryExecutor<TQueryResultData, TNextQueryResult> nextStreaming)
        { }

        protected virtual void VisitNextBufferingQueryExecutorOnInit(
            BufferingQueryExecutor<TQueryResultData, TNextQueryResult> nextBuffering) => nextBuffering.InitDataSource();

        public override TPipelineQueryResult
            Accept<TPipelineQueryResultParam, TPipelineQueryResult>(
                bool isResultEnumerable, ISingleQueryExecutorVisitor queryPipeline) =>
                InnerExecutor.Accept<TPipelineQueryResultParam, TPipelineQueryResult>(isResultEnumerable, queryPipeline);
    }

    public class StreamingQueryExecutorWithContinuation<TData, TQueryResultData, TNextQueryResult>
        : SingleQueryExecutorWithContinuation<
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>>,
            TData,
            TQueryResultData,
            TNextQueryResult>
    {
        public StreamingQueryExecutorWithContinuation(
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>> innerExecutor,
            IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> nextExecutor)
            : base(innerExecutor, nextExecutor)
        { }

        protected override void DataPassedHandler(IEnumerable<TQueryResultData> outputData)
        {
            return;
        }

        protected override void VisitNextStreamingQueryExecutorOnInit(StreamingQueryExecutor<TQueryResultData, TNextQueryResult> nextStreaming)
        {
            nextStreaming.PrepareToQueryStart();
            /*
            InnerExecutor.DataPassed += (intermediateResult) =>
            {
                foreach (TQueryResultData intermediateResultData in intermediateResult)
                    nextStreaming.ExecuteOverDataInstance(intermediateResultData);
            };
            */
            SubscribeOninnerExecutorEventsOnDataInstanceProcessing((intermediateResult) =>
            {
                foreach (TQueryResultData intermediateResultData in intermediateResult)
                    nextStreaming.ExecuteOverDataInstance(intermediateResultData);
            });
        }

        protected override void VisitNextBufferingQueryExecutorOnInit(BufferingQueryExecutor<TQueryResultData, TNextQueryResult> nextBuffering)
        {
            Type nextQueryResultType = typeof(TNextQueryResult);
            string ienumerableName = nameof(IEnumerable);
            if (nextQueryResultType.Name.StartsWith(ienumerableName) ||
                nextQueryResultType.GetInterface(ienumerableName) is not null)
            {
                throw new InvalidOperationException("Потоковой исполнитель запроса не поддерживает продолжение " +
                                                    "буферизированным исполнителем запроса с ожидаемым " +
                                                    "перечислимым результатом.");
            }

            base.VisitNextBufferingQueryExecutorOnInit(nextBuffering);
            InnerExecutor.DataPassed += (intermediateResult) =>
                nextBuffering.ExecuteOverDataInstance(intermediateResult.First());
        }

        public override void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<IEnumerable<TQueryResultData>> queryResultPassingHandler)
        {
            base.SubscribeOninnerExecutorEventsOnDataInstanceProcessing(queryResultPassingHandler);
            InnerExecutorImpl.DataNotPassed += queryResultPassingHandler;
        }

        public override TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipeline)
            => NextExecutor.GetPipelineQueryResult<TPipelineQueryResult>(pipeline);
    }

    public class StreamingQueryExecutorWithContinuationAndOneToManyResult<TData, TQueryResultData, TNextQueryResult>
    : SingleQueryExecutorWithContinuation<
        StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>>,
        TData,
        TQueryResultData,
        TNextQueryResult>
    {
        private event Action<IEnumerable<TQueryResultData>> DataNotPassed;

        private Action<TQueryResultData> _nextExecuteOverResutDataInstance;

        private IEnumerable<TQueryResultData> _intermediateQueryResult;

        public StreamingQueryExecutorWithContinuationAndOneToManyResult(
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>> innerExecutor,
            IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> nextExecutor)
            : base(innerExecutor, nextExecutor)
        {
            this.DataPassed += DataPassedHandler;
            innerExecutor.DataNotPassed += (IEnumerable<TQueryResultData> outputData) => this.OnDataNotPassed(outputData);
        }

        protected void OnDataNotPassed(IEnumerable<TQueryResultData> outputData) => DataNotPassed?.Invoke(outputData);

        protected override void DataPassedHandler(IEnumerable<TQueryResultData> outputData)
        {
            _intermediateQueryResult = outputData;

            /*
            IEnumerator<TQueryResultData> outputDataEnumerator = outputData.GetEnumerator();

            while(outputDataEnumerator.MoveNext())
            {
                _nextExecuteOverResutDataInstance(outputDataEnumerator.Current);
            }
            */

            return;
        }

        protected void DataNotPassedHandler(IEnumerable<TQueryResultData> outputData)
        {
            return;
        }

        public override TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor) 
        {
            foreach (TQueryResultData intermediateResultData in _intermediateQueryResult)
                _nextExecuteOverResutDataInstance(intermediateResultData);

            return NextExecutor.GetAggregablePipelineQueryResult<TPipelineQueryResult>(pipelineQueryExecutor);
        }

        public override IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            foreach (TQueryResultData intermediateResultData in _intermediateQueryResult)
            {
                _nextExecuteOverResutDataInstance(intermediateResultData);
                foreach (TPipelineQueryResultData queryResultData
                        in NextExecutor.GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(pipelineQueryExecutor))
                    yield return queryResultData;
            }
        }

        protected override void VisitNextStreamingQueryExecutorOnInit(
            StreamingQueryExecutor<TQueryResultData, TNextQueryResult> nextStreaming)
        {
            _nextExecuteOverResutDataInstance = (outputDataInstance) =>
                nextStreaming.ExecuteOverDataInstance(outputDataInstance);

            /*
            InnerExecutor.DataPassed += (intermediateResult) =>
            {
                IEnumerator<TQueryResultData> intermediateResultEnumerator = intermediateResult.GetEnumerator();
                bool isMovingOn;
                nextStreaming.DataPassed += (_) =>
                {
                    isMovingOn = intermediateResultEnumerator.MoveNext();
                    if (isMovingOn) nextStreaming.ExecuteOverDataInstance(intermediateResultEnumerator.Current);
                };
            };
            */
        }

        protected override void VisitNextBufferingQueryExecutorOnInit(
            BufferingQueryExecutor<TQueryResultData, TNextQueryResult> nextBuffering)
        {
            /*
            Type nextQueryResultType = typeof(TNextQueryResult);
            string ienumerableName = nameof(IEnumerable);
            if (nextQueryResultType.Name.StartsWith(ienumerableName) ||
                nextQueryResultType.GetInterface(ienumerableName) is not null)
            {
                throw new InvalidOperationException("Потоковой исполнитель запроса не поддерживает продолжение " +
                                                    "буферизированным исполнителем запроса с ожидаемым " +
                                                    "перечислимым результатом.");
            }

            _nextExecuteOverResutDataInstance = (outputDataInstance) => 
                nextBuffering.ExecuteOverDataInstance(outputDataInstance);
            
            base.VisitNextBufferingQueryExecutorOnInit(nextBuffering);
            InnerExecutor.DataPassed += (intermediateResult) =>
                nextBuffering.ExecuteOverDataInstance(intermediateResult.First());
            */
        }

        public override void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<IEnumerable<TQueryResultData>> queryResultPassingHandler)
        {
            base.SubscribeOninnerExecutorEventsOnDataInstanceProcessing(queryResultPassingHandler);
            InnerExecutorImpl.DataNotPassed += queryResultPassingHandler;
        }

        /// <summary>
        /// Предполагается несколько вариантов выходного результата:
        /// 1) одиночный;
        /// 2) множественный с отношением "1 выход на 1 вход";
        /// 3) множественный с отношением "N выходов на 1 вход".
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        public override TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipeline)
        {
            return default(TPipelineQueryResult);
            /*
            return typeof(TPipelineQueryResult).GetInteface(nameof(IEnumerable)) is null ?
                NextExecutor.GetPipelineQueryResult<TPipelineQueryResult>(pipeline) :
                (dynamic)GetPipelineQueryEnumerableResult(pipeline);
            */
        }
    }

    public class BufferingQueryExecutorWithContinuation<TData, TQueryResultData, TNextQueryResult>
        : SingleQueryExecutorWithContinuation<
            BufferingQueryExecutorWithEnumerableResult<TData, TQueryResultData>,
            TData,
            TQueryResultData,
            TNextQueryResult>
    {
        public BufferingQueryExecutorWithContinuation(
            BufferingQueryExecutorWithEnumerableResult<TData, TQueryResultData> innerExecutor,
            IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> nextExecutor)
            : base(innerExecutor, nextExecutor)
        { }

        protected override void DataPassedHandler(IEnumerable<TQueryResultData> outputData)
        {
            return;
        }

        public override TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipeline)
        {
            // Явное уведомление исполнителя запроса о требовании подытоживания запроса.
            IEnumerable<TQueryResultData> queryResult = InnerExecutorImpl.GetQueryResult();

            // Переход к следующему исполнителю запроса.
            return pipeline.ContinueQueryExecuting<TQueryResultData, TPipelineQueryResult>(NextExecutor, queryResult);
        }
    }
}
