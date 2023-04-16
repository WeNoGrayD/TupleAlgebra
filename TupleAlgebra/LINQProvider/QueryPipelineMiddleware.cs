using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider
{
    /// <summary>
    /// Приниматель конвейера запросов.
    /// </summary>
    public interface IQueryPipelineAcceptor
    {
        #region Methods

        /// <summary>
        /// Приём конвейера запросов для дальнейшего вызова метода конвейера по выполнению запроса.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultParam"></typeparam>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="isResultEnumerable"></param>
        /// <param name="queryPipeline"></param>
        /// <returns></returns>
        TPipelineQueryResult Accept<TPipelineQueryResultParam, TPipelineQueryResult>(
            bool isResultEnumerable, ISingleQueryExecutorVisitor queryPipeline);

        #endregion
    }

    /// <summary>
    /// Интерфейс компонента конвейера запросов.
    /// </summary>
    public interface IQueryPipelineMiddleware : IQueryPipelineAcceptor
    {
        #region Properties

        /// <summary>
        /// Флаг продолжения обхода по входным данным.
        /// </summary>
        bool MustGoOn { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Продолжение компонента конвейера следующим.
        /// </summary>
        /// <param name="continuingExecutor"></param>
        /// <returns></returns>
        IQueryPipelineMiddleware ContinueWith(IQueryPipelineMiddleware continuingExecutor);

        /// <summary>
        /// Получение конечного компонента конвейера запросов.
        /// </summary>
        /// <returns></returns>
        IQueryPipelineEndpoint GetPipelineEndpoint();

        /// <summary>
        /// Подготовка компонента конвейера к тому, что итоговым результатом конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        void PrepareToAggregableResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipelineQueryExecutor);

        /// <summary>
        /// Подготовка компонента конвейера к тому, что итоговым результатом конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        void PrepareToEnumerableResult<TPipelineQueryResultData>(ISingleQueryExecutorVisitor pipelineQueryExecutor);

        /// <summary>
        /// Получение агрегируемого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor);

        /// <summary>
        /// Получение перечислимого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor);

        #endregion
    }

    /// <summary>
    /// Интерфейс компонента конвейера запросов.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResult"></typeparam>
    public interface IQueryPipelineMiddleware<TData, TQueryResult> : IQueryPipelineMiddleware
    {
        #region Properties

        /// <summary>
        /// Внутренний сполнитель запроса.
        /// </summary>
        SingleQueryExecutor<TData, TQueryResult> InnerExecutor { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Продолжение компонента конвейера следующим.
        /// </summary>
        /// <typeparam name="TContinuingQueryData"></typeparam>
        /// <typeparam name="TContinuingQueryResult"></typeparam>
        /// <param name="continuingExecutor"></param>
        /// <returns></returns>
        IQueryPipelineMiddleware<TData, TQueryResult> ContinueWith<TContinuingQueryData, TContinuingQueryResult>(
                IQueryPipelineMiddleware<TContinuingQueryData, TContinuingQueryResult> continuingExecutor);

        /// <summary>
        /// Подписка компонента конвейера запросов на событие пропуска промежуточных данных своего исполнителя запросов.
        /// </summary>
        /// <param name="queryResultPassingHandler"></param>
        void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(Action<TQueryResult> queryResultPassingHandler);

        #endregion
    }

    /// <summary>
    /// Интерфейс конечного компонента конвейера запросов.
    /// </summary>
    public interface IQueryPipelineEndpoint : IQueryPipelineMiddleware
    {
        #region Methods

        /// <summary>
        /// Инициализация компонента в качестве конечного компонента конвейера запросов.
        /// </summary>
        void InitializeAsQueryPipelineEndpoint();

        #endregion
    }

    /// <summary>
    /// Компонент конвейера запросов.
    /// </summary>
    /// <typeparam name="TInnerQueryExecutor"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResult"></typeparam>
    public abstract class QueryPipelineMiddleware<TInnerQueryExecutor, TData, TQueryResult>
        : IQueryPipelineMiddleware<TData, TQueryResult>
        where TInnerQueryExecutor : SingleQueryExecutor<TData, TQueryResult>
    {
        #region Instance fields

        /// <summary>
        /// Конкретно типизированный исполнитель запроса, приписанный к данному компоненту конвейера.
        /// </summary>
        protected TInnerQueryExecutor _innerExecutorImpl;

        #endregion

        #region Instance properties

        /// <summary>
        /// Внутренний сполнитель запроса.
        /// </summary>
        public SingleQueryExecutor<TData, TQueryResult> InnerExecutor => _innerExecutorImpl;

        /// <summary>
        /// Флаг продолжения обхода по входным данным.
        /// </summary>
        public abstract bool MustGoOn { get; }

        #endregion

        #region Instance events

        /// <summary>
        /// Событие пропуска данных на возможный следующий компонент конвейера запросов. 
        /// </summary>
        public event Action<TQueryResult> DataPassed;

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="innerExecutor"></param>
        protected QueryPipelineMiddleware(TInnerQueryExecutor innerExecutor)
        {
            _innerExecutorImpl = innerExecutor;
        }

        #endregion

        #region Instance methods

        /// <summary>
        /// Вызов события пропуска данных с готовым промежуточным результатом.
        /// </summary>
        /// <param name="outputData"></param>
        protected void OnDataPassed(TQueryResult outputData)
        {
            DataPassed?.Invoke(outputData);

            return;
        }

        #endregion

        #region IQueryPipelineAcceptor implementation

        /// <summary>
        /// Приём конвейера запросов для вызова метода запуска конвейера.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultParam"></typeparam>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="isResultEnumerable"></param>
        /// <param name="queryPipeline"></param>
        /// <returns></returns>
        public TPipelineQueryResult Accept<TPipelineQueryResultParam, TPipelineQueryResult>(
                bool isResultEnumerable, ISingleQueryExecutorVisitor queryPipeline)
        {
            return _innerExecutorImpl.Accept<TPipelineQueryResultParam, TPipelineQueryResult>(isResultEnumerable, queryPipeline);
        }

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
        public abstract IQueryPipelineEndpoint GetPipelineEndpoint();

        /// <summary>
        /// Подготовка компонента конвейера к тому, что итоговым результатом конвейера запросов
        /// будет какое-либо агрегируемое значение. 
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        public abstract void PrepareToAggregableResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipelineQueryExecutor);

        /// <summary>
        /// Подготовка компонента конвейера к тому, что итоговым результатом конвейера запросов
        /// будет какое-либо перечислимое значение. 
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        public abstract void PrepareToEnumerableResult<TPipelineQueryResultData>(ISingleQueryExecutorVisitor pipelineQueryExecutor);

        /// <summary>
        /// Получение агрегируемого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        public abstract TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor);

        /// <summary>
        /// Получение перечислимого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        public abstract IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor);

        #endregion

        #region IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> implementation

        /// <summary>
        /// Подписка компонента конвейера запросов на событие пропуска промежуточных данных своего исполнителя запросов.
        /// </summary>
        /// <param name="queryResultPassingHandler"></param>
        public virtual void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<TQueryResult> queryResultPassingHandler)
        {
            this.DataPassed += queryResultPassingHandler;

            return;
        }

        /// <summary>
        /// Продолжение компонента конвейера следующим.
        /// </summary>
        /// <typeparam name="TContinuingQueryData"></typeparam>
        /// <typeparam name="TContinuingQueryResult"></typeparam>
        /// <param name="continuingExecutor"></param>
        /// <returns></returns>
        public abstract IQueryPipelineMiddleware<TData, TQueryResult> ContinueWith<TContinuingQueryData, TContinuingQueryResult>(
            IQueryPipelineMiddleware<TContinuingQueryData, TContinuingQueryResult> continuingExecutor);

        #endregion
    }
}
