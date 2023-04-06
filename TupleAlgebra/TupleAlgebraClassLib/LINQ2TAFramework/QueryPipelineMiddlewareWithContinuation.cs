﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    /// <summary>
    /// Компонент конвейера запросов с продолжением конвейера.
    /// </summary>
    /// <typeparam name="TInnerQueryExecutor"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResultData"></typeparam>
    /// <typeparam name="TNextQueryResult"></typeparam>
    public abstract class QueryPipelineMiddlewareWithContinuation<TInnerQueryExecutor, TData, TQueryResultData, TNextQueryResult>
        : QueryPipelineMiddleware<TInnerQueryExecutor, TData, IEnumerable<TQueryResultData>>
        where TInnerQueryExecutor : SingleQueryExecutor<TData, IEnumerable<TQueryResultData>>
    {
        #region Instance properties

        /// <summary>
        /// Следующий компонент в конвейере.
        /// </summary>
        public IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> NextExecutor { get; protected set; }

        /// <summary>
        /// Флаг продолжения обхода по входным данным.
        /// </summary>
        public override bool MustGoOn { get => NextExecutor.MustGoOn; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="innerExecutor"></param>
        /// <param name="nextExecutor"></param>
        /// <exception cref="ArgumentException"></exception>
        protected QueryPipelineMiddlewareWithContinuation(
            TInnerQueryExecutor innerExecutor,
            IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> nextExecutor)
            : base(innerExecutor)
        {
            NextExecutor = nextExecutor;
            innerExecutor.DataPassed += OnDataPassed;

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

        #endregion

        #region Instance methods

        /// <summary>
        /// Обработка случая, когда следующий компонент конвейера запросов является
        /// потоковым компонентом.
        /// </summary>
        /// <param name="nextStreaming"></param>
        protected virtual void VisitNextStreamingQueryExecutorOnInit(
            StreamingQueryExecutor<TQueryResultData, TNextQueryResult> nextStreaming)
        {
            return;
        }

        /// <summary>
        /// Обработка случая, когда следующий компонент конвейера запросов является
        /// буферизирующим компонентов.
        /// </summary>
        /// <param name="nextBuffering"></param>
        protected virtual void VisitNextBufferingQueryExecutorOnInit(
            BufferingQueryExecutor<TQueryResultData, TNextQueryResult> nextBuffering)
        {
            nextBuffering.InitDataSource();

            return;
        }

        /// <summary>
        /// Обработчик события пропуска промежуточных данных внутренним исполнителем запроса.
        /// </summary>
        /// <param name="outputData"></param>
        protected abstract void DataPassedHandler(IEnumerable<TQueryResultData> outputData);

        #endregion

        #region IQueryPipelineMiddleware implementation

        /// <summary>
        /// Получение конечного компонента конвейера запросов.
        /// </summary>
        /// <returns></returns>
        public override IQueryPipelineEndpoint GetPipelineEndpoint()
        {
            return NextExecutor.GetPipelineEndpoint();
        }

        /// <summary>
        /// Подготовка компонента конвейера к тому, что итоговым результатом конвейера запросов.
        /// будет какое-либо агрегируемое значение. 
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        public override void PrepareToAggregableResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            NextExecutor.PrepareToAggregableResult<TPipelineQueryResult>(pipelineQueryExecutor);

            return;
        }

        /// <summary>
        /// Подготовка компонента конвейера к тому, что итоговым результатом конвейера запросов.
        /// будет какое-либо перечислимое значение. 
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        public override void PrepareToEnumerableResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            NextExecutor.PrepareToEnumerableResult<TPipelineQueryResultData>(pipelineQueryExecutor);

            return;
        }

        /// <summary>
        /// Получение агрегируемого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        public override TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            return NextExecutor.GetAggregablePipelineQueryResult<TPipelineQueryResult>(pipelineQueryExecutor);
        }

        /// <summary>
        /// Получение перечислимого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        public override IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            return NextExecutor.GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(pipelineQueryExecutor);
        }

        #endregion

        #region IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> implementation

        /// <summary>
        /// Подписка компонента конвейера запросов на событие пропуска промежуточных данных своего исполнителя запросов.
        /// </summary>
        /// <param name="queryResultPassingHandler"></param>
        public override void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<IEnumerable<TQueryResultData>> queryResultPassingHandler)
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
        public override IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>>
            ContinueWith<TContinuingQueryData, TContinuingQueryResult>(
                IQueryPipelineMiddleware<TContinuingQueryData, TContinuingQueryResult> continuingExecutor)
        {
            NextExecutor = NextExecutor.ContinueWith(continuingExecutor);

            return this;
        }

        #endregion
    }
}