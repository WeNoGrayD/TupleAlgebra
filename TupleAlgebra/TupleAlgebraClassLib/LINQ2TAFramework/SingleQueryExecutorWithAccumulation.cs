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
        #region Instance fields

        /// <summary>
        /// Конкретно типизированный исполнитель запроса, приписанный к данному компоненту конвейера.
        /// </summary>
        private TInnerQueryExecutor _innerExecutorImpl;

        private IAccumulatePositiveQueryResult<TQueryResult, TAccumulator> _innerExecutorAsAccumulating;

        /// <summary>
        /// Аккумулятор результата конвейера запросов.
        /// </summary>
        protected TAccumulator _accumulator;

        #endregion

        #region Instance properties

        /// <summary>
        /// Конкретно типизированный внутренний исполнитель запроса.
        /// </summary>
        protected TInnerQueryExecutor InnerExecutorImpl
        {
            get => _innerExecutorImpl;
            private set => _innerExecutorImpl = value;
        }

        /// <summary>
        /// Внутренний сполнитель запроса.
        /// </summary>
        public SingleQueryExecutor<TData, TQueryResult> InnerExecutor => _innerExecutorImpl;

        /// <summary>
        /// Аккумулятор результата конвейера запросов.
        /// </summary>
        public TQueryResult Accumulator => _accumulator;


        /// <summary>
        /// Флаг продолжения обхода по входным данным.
        /// </summary>
        public virtual bool MustGoOn { get => true; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="innerExecutor"></param>
        /// <param name="innerExecutorAsAccumulating"></param>
        protected SingleQueryExecutorWithAccumulation(
            TInnerQueryExecutor innerExecutor,
            IAccumulatePositiveQueryResult<TQueryResult, TAccumulator> innerExecutorAsAccumulating)
            : base()
        {
            InnerExecutorImpl = innerExecutor;
            _innerExecutorAsAccumulating = innerExecutorAsAccumulating;
        }

        #endregion

        #region Instance methods

        /// <summary>
        /// Инициализация компонента в качестве конечного компонента конвейера запросов.
        /// </summary>
        public void InitializeAsQueryPipelineEndpoint()
        {
            _accumulator = _innerExecutorAsAccumulating.InitAccumulator();
            InnerExecutorImpl.DataPassed += DataPassedHandler;
            if (InnerExecutorImpl is IAccumulateNegativeQueryResult<TQueryResult, TAccumulator> fullCoveringReader)
                fullCoveringReader.DataNotPassed += 
                    (TQueryResult outputData) => DataNotPassedHandler(fullCoveringReader, outputData);
        }

        protected virtual void DataPassedHandler(TQueryResult outputData)
        {
            _innerExecutorAsAccumulating.AccumulateIfDataPassed(ref _accumulator, outputData);

            return;
        }

        protected virtual void DataNotPassedHandler(
            IAccumulateNegativeQueryResult<TQueryResult, TAccumulator> fullCoveringReader,
            TQueryResult outputData)
        {
            fullCoveringReader.AccumulateIfDataNotPassed(ref _accumulator, outputData);

            return;
        }

        /// <summary>
        /// Продолжение компонента конвейера следующим.
        /// </summary>
        /// <typeparam name="TContinuingQueryData"></typeparam>
        /// <typeparam name="TContinuingQueryResult"></typeparam>
        /// <param name="continuingExecutor"></param>
        /// <returns></returns>
        public IQueryPipelineMiddleware<TData, TQueryResult>
            ContinueWith<TContinuingQueryData, TContinuingQueryResult>(
                IQueryPipelineMiddleware<TContinuingQueryData, TContinuingQueryResult> continuingExecutor) =>
            SingleQueryExecutorWithContinuationFactory.Create(
                this as IQueryPipelineMiddleware<TData, IEnumerable<TContinuingQueryData>>, continuingExecutor)
            as IQueryPipelineMiddleware<TData, TQueryResult>;

        #endregion

        #region IQueryPipelineMiddleware implementation

        /// <summary>
        /// Продолжение компонента конвейера следующим (явная реализация).
        /// </summary>
        /// <param name="continuingExecutor"></param>
        /// <returns></returns>
        IQueryPipelineMiddleware IQueryPipelineMiddleware.ContinueWith(IQueryPipelineMiddleware continuingExecutor)
        {
            return ContinueWith((dynamic)continuingExecutor);
        }

        /// <summary>
        /// Получение конечного компонента конвейера запросов.
        /// </summary>
        /// <returns></returns>
        public IQueryPipelineEndpoint GetPipelineEndpoint()
        {
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryResultPassingHandler"></param>
        public virtual void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<TQueryResult> queryResultPassingHandler)
        {
            _innerExecutorImpl.DataPassed += queryResultPassingHandler;

            return;
        }

        /// <summary>
        /// Приём конвейера запросов для вызова метода запуска конвейера.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultParam"></typeparam>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="isResultEnumerable"></param>
        /// <param name="queryPipeline"></param>
        /// <returns></returns>
        public override TPipelineQueryResult
            Accept<TPipelineQueryResultParam, TPipelineQueryResult>(
                bool isResultEnumerable,
                ISingleQueryExecutorVisitor queryPipeline)
        {
            return _innerExecutorImpl.Accept<TPipelineQueryResultParam, TPipelineQueryResult>(isResultEnumerable, queryPipeline);
        }

        /// <summary>
        /// Получение результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        public abstract TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipeline);

        /// <summary>
        /// Получение агрегируемого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        public virtual TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
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
        public virtual IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            return GetPipelineQueryResult<IEnumerable<TPipelineQueryResultData>>(pipelineQueryExecutor);
        }

        #endregion
    }

    public class StreamingQueryExecutorWithAccumulationOfAggregableResult<TData, TQueryResult>
        : SingleQueryExecutorWithAccumulation<
            StreamingQueryExecutor<TData, TQueryResult>,
            TData,
            TQueryResult,
            TQueryResult>,
          IStreamingQueryPipelineMiddleware
    {
        public StreamingQueryExecutorWithAccumulationOfAggregableResult(
            StreamingQueryExecutorWithAggregableResult<TData, TQueryResult> innerExecutor)
            : base(innerExecutor, innerExecutor)
        {
            return;
        }

        public override TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipeline)
        {
            return (TPipelineQueryResult)(object)_accumulator;//(dynamic)_accumulator;
        }

        public override TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipeline)
        {
            return (this as StreamingQueryExecutorWithAccumulationOfAggregableResult<TData, TPipelineQueryResult>)._accumulator;
        }

        public TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipeline,
            ref bool mustGoOn)
        {
            return GetAggregablePipelineQueryResult<TPipelineQueryResult>(pipeline);
        }

        public (IEnumerable<TPipelineQueryResultData> QueryResult, bool MustGoOn)
            GetEnumerablePipelineQueryResult2<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipeline)
        {
            return (GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(pipeline), true);
        }

        public override void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<TQueryResult> queryResultPassingHandler)
        {
            base.SubscribeOninnerExecutorEventsOnDataInstanceProcessing(queryResultPassingHandler);
            InnerExecutorImpl.DataNotPassed += queryResultPassingHandler;

            return;
        }
    }

    public class StreamingQueryExecutorWithAccumulationOfEnumerableResult<TData, TQueryResultData>
        : SingleQueryExecutorWithAccumulation<
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>>,
            TData,
            IEnumerable<TQueryResultData>,
            IEnumerable<TQueryResultData>>,
          IStreamingQueryPipelineMiddleware
    {
        /// <summary>
        /// Флаг продолжения обхода по входным данным.
        /// </summary>
        private bool _mustGoOn = false;

        /// <summary>
        /// Флаг продолжения обхода по входным данным.
        /// </summary>
        //public override bool MustGoOn { get => _mustGoOn; }

        public StreamingQueryExecutorWithAccumulationOfEnumerableResult(
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>> innerExecutor,
            IAccumulatePositiveQueryResult<IEnumerable<TQueryResultData>, IEnumerable<TQueryResultData>> innerExecutorAsAccumulating)
            : base(innerExecutor, innerExecutorAsAccumulating)
        {
            return;
        }

        public override TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipeline)
        {
            return (TPipelineQueryResult)(object)_accumulator;//_accumulator;
        }

        public override IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipeline)
        {
            return (this as StreamingQueryExecutorWithAccumulationOfEnumerableResult<TData, TPipelineQueryResultData>)._accumulator;
        }

        public TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipeline,
            ref bool mustGoOn)
        {
            return GetAggregablePipelineQueryResult<TPipelineQueryResult>(pipeline);
        }

        public (IEnumerable<TPipelineQueryResultData> QueryResult, bool MustGoOn)
            GetEnumerablePipelineQueryResult2<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipeline)
        {
            return (GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(pipeline), true);
        }

        public override void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<IEnumerable<TQueryResultData>> queryResultPassingHandler)
        {
            base.SubscribeOninnerExecutorEventsOnDataInstanceProcessing(queryResultPassingHandler);
            InnerExecutorImpl.DataNotPassed += queryResultPassingHandler;

            return;
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
