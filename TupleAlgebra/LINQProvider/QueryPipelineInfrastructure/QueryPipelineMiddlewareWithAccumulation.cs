﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryResultAccumulatorInterfaces;

namespace LINQProvider.QueryPipelineInfrastructure
{
    /// <summary>
    /// Компонент конвейера запросов с аккумулированием результатов запроса. 
    /// </summary>
    /// <typeparam name="TInnerQueryExecutor"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResult"></typeparam>
    /// <typeparam name="TAccumulator"></typeparam>
    public abstract class QueryPipelineMiddlewareWithAccumulation<TInnerQueryExecutor, TData, TQueryResult, TAccumulator>
        : QueryPipelineMiddleware<TInnerQueryExecutor, TData, TQueryResult>, IQueryPipelineEndpoint
        where TInnerQueryExecutor : SingleQueryExecutor<TData, TQueryResult>
        where TAccumulator : TQueryResult
    {
        #region Instance fields

        /// <summary>
        /// 
        /// </summary>
        private IAccumulatePositiveQueryResult<TQueryResult, TAccumulator> _innerExecutorAsAccumulating;

        /// <summary>
        /// Аккумулятор результата конвейера запросов.
        /// </summary>
        protected TAccumulator _accumulator;

        #endregion

        #region Instance properties

        public override sealed bool MustGoOn { get => true; protected set { } }

        public override IQueryPipelineEndpoint PipelineEndpoint { get => this; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="innerExecutor"></param>
        /// <param name="innerExecutorAsAccumulating"></param>
        protected QueryPipelineMiddlewareWithAccumulation(
            LinkedListNode<IQueryPipelineMiddleware> node,
            TInnerQueryExecutor innerExecutor,
            IAccumulatePositiveQueryResult<TQueryResult, TAccumulator> innerExecutorAsAccumulating)
            : base(node, innerExecutor)
        {
            _innerExecutorAsAccumulating = innerExecutorAsAccumulating;
        }

        #endregion

        #region Instance methods

        /// <summary>
        /// Обработчик пропуска данных внутренним исполнителем запроса.
        /// </summary>
        /// <param name="outputData"></param>
        protected virtual void DataPassedHandler(TQueryResult outputData)
        {
            _innerExecutorAsAccumulating.AccumulateIfDataPassed(ref _accumulator, outputData);

            return;
        }

        /// <summary>
        /// Обработчик непропуска данных внутренним исполнителем запроса.
        /// </summary>
        /// <param name="fullCoveringReader"></param>
        /// <param name="outputData"></param>
        protected virtual void DataNotPassedHandler(
            IAccumulateNegativeQueryResult<TQueryResult, TAccumulator> negativeCoveringReader,
            TQueryResult outputData)
        {
            negativeCoveringReader.AccumulateIfDataNotPassed(ref _accumulator, outputData);

            return;
        }

        #endregion

        #region IQueryPipelineMiddleware implementation

        /// <summary>
        /// Подготовка компонента конвейера к тому, что итоговым результатом конвейера запросов
        /// будет какое-либо агрегируемое значение. 
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        public override void PrepareToAggregableResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipelineQueryExecutor)
        { }

        /// <summary>
        /// Подготовка компонента конвейера к тому, что итоговым результатом конвейера запросов
        /// будет какое-либо перечислимое значение. 
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        public override void PrepareToEnumerableResult<TPipelineQueryResultData>(ISingleQueryExecutorVisitor pipelineQueryExecutor)
        { }

        /// <summary>
        /// Подписка компонента конвейера запросов на событие пропуска промежуточных данных своего исполнителя запросов.
        /// </summary>
        /// <param name="queryResultPassingHandler"></param>
        public override void SubscribeOnInnerExecutorEventsOnDataInstanceProcessing(
            Action<TQueryResult> queryResultPassingHandler)
        {
            _innerExecutorImpl.DataPassed += queryResultPassingHandler;

            return;
        }

        #endregion

        #region IQueryPipelineEndpoint implementation

        /// <summary>
        /// Инициализация компонента в качестве конечного компонента конвейера запросов.
        /// </summary>
        public void InitializeAsQueryPipelineEndpoint()
        {
            _accumulator = _innerExecutorAsAccumulating.InitAccumulator();
            _innerExecutorImpl.DataPassed += DataPassedHandler;
            /*
            if (_innerExecutorImpl is IAccumulateNegativeQueryResult<TQueryResult, TAccumulator> negativeCoveringReader)
                negativeCoveringReader.DataNotPassed +=
                    (TQueryResult outputData) => DataNotPassedHandler(negativeCoveringReader, outputData);
            */
        }

        #endregion
    }
}
