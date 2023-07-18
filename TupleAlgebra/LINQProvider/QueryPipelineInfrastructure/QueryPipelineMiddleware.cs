using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryPipelineInfrastructure
{
    /// <summary>
    /// Компонент конвейера запросов.
    /// </summary>
    /// <typeparam name="TInnerQueryExecutor"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResult"></typeparam>
    public abstract class QueryPipelineMiddleware<TInnerQueryExecutor, TData, TQueryResult>
        : IQueryPipelineMiddleware<TData, TQueryResult>,
          IQueryPipelineMiddlewareWithContinuationAcceptor<TData>
        where TInnerQueryExecutor : SingleQueryExecutor<TData, TQueryResult>
    {
        #region Instance fields

        /// <summary>
        /// Конкретно типизированный исполнитель запроса, приписанный к данному компоненту конвейера.
        /// </summary>
        protected TInnerQueryExecutor _innerExecutorImpl;

        protected bool _resultProvided;

        protected LinkedListNode<IQueryPipelineMiddleware> _node;

        #endregion

        #region Instance properties

        public LinkedListNode<IQueryPipelineMiddleware> PipelineScheduleNode { get => _node; }

        public SingleQueryExecutor<TData, TQueryResult> InnerExecutor => _innerExecutorImpl;

        public QuerySourceToResultMiltiplicity Multiplicity
        {
            get => InnerExecutor.Multiplicity;
        }

        public IQueryPipelineMiddleware PreviousMiddleware { get => _node.Previous!.Value; }

        public IQueryPipelineMiddleware NextMiddleware { get => _node.Next!.Value; }

        public virtual bool ResultProvided
        {
            get
            {
                bool resultProvidedBuf = _resultProvided;
                _resultProvided = false;

                return resultProvidedBuf;
            }
            set => _resultProvided = value;
        }

        public abstract bool MustGoOn { get; protected set; }

        public abstract IQueryPipelineEndpoint PipelineEndpoint { get; }

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
        protected QueryPipelineMiddleware(
            LinkedListNode<IQueryPipelineMiddleware> node, 
            TInnerQueryExecutor innerExecutor)
        {
            _node = node;
            _node.Value = this;
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

        public void StepBack(ISingleQueryExecutorVisitor queryPipelineExecutor)
        {
            //IQueryPipelineMiddleware mid = _node.Previous is not null ?
            //    PreviousMiddleware : queryPipelineExecutor.GotoPreviousPipelineTask();

            return;
        }

        #endregion

        #region IQueryPipelineAcceptor implementation

        public TPipelineQueryResult AcceptToExecuteWithAggregableResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor queryPipeline)
        {
            return _innerExecutorImpl.AcceptToExecuteWithAggregableResult<TPipelineQueryResult>(queryPipeline);
        }

        public IEnumerable<TPipelineQueryResultData> AcceptToExecuteWithEnumerableResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor queryPipeline)
        {
            return _innerExecutorImpl.AcceptToExecuteWithEnumerableResult<TPipelineQueryResultData>(queryPipeline);
        }

        #endregion

        #region IQueryPipelineMiddleware implementation

        /// <summary>
        /// Продолжение компонента конвейера следующим.
        /// </summary>
        /// <param name="continuingExecutor"></param>
        /// <returns></returns>
        public abstract IQueryPipelineMiddleware ContinueWith(
            IQueryPipelineMiddleware continuingExecutor,
            IQueryPipelineScheduler scheduler);

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
        public virtual void SubscribeOnInnerExecutorEventsOnDataInstanceProcessing(
            Action<TQueryResult> queryResultPassingHandler)
        {
            DataPassed += queryResultPassingHandler;

            return;
        }

        #endregion

        #region IQueryPipelineMiddlewareWithContinuationAcceptor<TData> implementation

        public abstract void Accept(IQueryPipelineMiddlewareVisitor<TData> visitor);

        #endregion
    }
}
