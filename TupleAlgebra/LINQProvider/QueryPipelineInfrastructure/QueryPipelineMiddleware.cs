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
    public abstract class QueryPipelineMiddleware<TData, TQueryResult, TInnerQueryExecutor>
        : IQueryPipelineMiddleware<TData, TQueryResult>,
          IAcceptor<ISingleQueryExecutorVisitor<TData>>
        where TInnerQueryExecutor : SingleQueryExecutor<TData, TQueryResult>
    {
        #region Instance fields

        /// <summary>
        /// Конкретно типизированный исполнитель запроса, приписанный к данному компоненту конвейера.
        /// </summary>
        protected TInnerQueryExecutor _innerExecutorImpl;

        protected bool _resultProvided;

        #endregion

        #region Instance properties

        public SingleQueryExecutor<TData, TQueryResult> InnerExecutor => _innerExecutorImpl;

        public QuerySourceToResultMiltiplicity Multiplicity
        {
            get => InnerExecutor.Multiplicity;
        }

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

        public abstract bool MustGoOn { get; set; }

        public abstract IQueryPipelineEndpoint PipelineEndpoint { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="innerExecutor"></param>
        protected QueryPipelineMiddleware(TInnerQueryExecutor innerExecutor)
        {
            _innerExecutorImpl = innerExecutor;

            return;
        }

        #endregion

        #region Instance methods

        public void StepBack(ISingleQueryExecutorResultRequester queryPipelineExecutor)
        {
            //IQueryPipelineMiddleware mid = _node.Previous is not null ?
            //    PreviousMiddleware : queryPipelineExecutor.GotoPreviousPipelineTask();

            return;
        }

        #endregion

        #region IQueryPipelineAcceptor implementation

        public abstract TPipelineQueryResult AcceptToExecuteWithAggregableResult<TPipelineQueryResult>(
            ISingleQueryExecutorResultRequester resultRequster);

        public abstract IEnumerable<TPipelineQueryResultData> AcceptToExecuteWithEnumerableResult<TPipelineQueryResultData>(
            System.Collections.IEnumerable dataSource,
            ISingleQueryExecutorResultRequester resultRequster);

        #endregion

        #region IQueryPipelineMiddleware implementation

        /// <summary>
        /// Продолжение компонента конвейера следующим.
        /// </summary>
        /// <param name="continuingExecutor"></param>
        /// <returns></returns>
        public abstract IQueryPipelineMiddleware ContinueWith(
            IQueryPipelineEndpoint continuingExecutor,
            IQueryPipelineScheduler scheduler);

        /// <summary>
        /// Подготовка компонента конвейера к тому, что итоговым результатом конвейера запросов
        /// будет какое-либо агрегируемое значение. 
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        public abstract void PrepareToAggregableResult<TPipelineQueryResult>(
            ISingleQueryExecutorResultRequester pipelineQueryExecutor);

        /// <summary>
        /// Подготовка компонента конвейера к тому, что итоговым результатом конвейера запросов
        /// будет какое-либо перечислимое значение. 
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        public abstract void PrepareToEnumerableResult<TPipelineQueryResultData>(
            ISingleQueryExecutorResultRequester pipelineQueryExecutor);

        /// <summary>
        /// Получение агрегируемого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        public abstract TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorResultRequester pipelineQueryExecutor);

        /// <summary>
        /// Получение перечислимого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        public abstract IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorResultRequester pipelineQueryExecutor);

        #endregion

        #region IAcceptor<ISingleQueryExecutorVisitor<TData>> implementation

        public abstract void Accept(ISingleQueryExecutorVisitor<TData> visitor);

        #endregion
    }
}
