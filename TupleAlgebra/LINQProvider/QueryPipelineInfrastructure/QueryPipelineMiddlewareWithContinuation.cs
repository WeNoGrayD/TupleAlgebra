using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Buffering;
using LINQProvider.QueryPipelineInfrastructure.Streaming;

namespace LINQProvider.QueryPipelineInfrastructure
{
    /// <summary>
    /// Компонент конвейера запросов с продолжением конвейера.
    /// </summary>
    /// <typeparam name="TInnerQueryExecutor"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResultData"></typeparam>
    /// <typeparam name="TNextQueryResult"></typeparam>
    public abstract class QueryPipelineMiddlewareWithContinuation<TInnerQueryExecutor, TData, TQueryResultData>
        : QueryPipelineMiddleware<TInnerQueryExecutor, TData, IEnumerable<TQueryResultData>>,
          IQueryPipelineMiddlewareVisitor<TQueryResultData>
        where TInnerQueryExecutor : SingleQueryExecutor<TData, IEnumerable<TQueryResultData>>
    {
        #region Instance fields

        /// <summary>
        /// Флаг продолжения обхода по входным данным.
        /// </summary>
        private bool _mustGoOn = true;

        #endregion

        #region Instance properties

        /// <summary>
        /// Следующий компонент в конвейере.
        /// </summary>
        public IQueryPipelineMiddleware NextExecutor { get => _node.Next!.Value!; }

        public override bool ResultProvided
        {
            get => base.ResultProvided;
            set
            {
                _resultProvided = value;
            }
        }

        public override bool MustGoOn
        {
            get => _mustGoOn;
            protected set
            {
                _mustGoOn = value;
                if (!value) OnFailingQuery();
            }
        }

        public override IQueryPipelineEndpoint PipelineEndpoint 
        { get => (_node.List!.Last!.Value as IQueryPipelineEndpoint)!; }

        #endregion

        #region Events

        public event Action FailingQuery;

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="innerExecutor"></param>
        /// <param name="nextExecutor"></param>
        /// <exception cref="ArgumentException"></exception>
        protected QueryPipelineMiddlewareWithContinuation(
            LinkedListNode<IQueryPipelineMiddleware> node,
            TInnerQueryExecutor innerExecutor,
            IQueryPipelineMiddlewareWithContinuationAcceptor<TQueryResultData> nextExecutor)
            : base(node, innerExecutor)
        {
            _node.List.AddAfter(_node, nextExecutor.PipelineScheduleNode);
            innerExecutor.DataPassed += OnDataPassed;
            DataPassed += DataPassedHandler;
            // Обход следующего компонента конвейера для создания надлежащей связи между компонентами.
            nextExecutor.Accept(this);
        }

        #endregion

        #region Instance methods

        /*
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
        */

        /// <summary>
        /// Обработчик события пропуска промежуточных данных внутренним исполнителем запроса.
        /// </summary>
        /// <param name="outputData"></param>
        protected virtual void DataPassedHandler(IEnumerable<TQueryResultData> outputData)
        {
            ResultProvided = true;

            return;
        }

        #endregion

        #region IQueryPipelineMiddleware implementation

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
        /// Продолжение компонента конвейера следующим.
        /// </summary>
        /// <typeparam name="TContinuingQueryData"></typeparam>
        /// <typeparam name="TContinuingQueryResult"></typeparam>
        /// <param name="continuingExecutor"></param>
        /// <returns></returns>
        public override IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> ContinueWith(
            IQueryPipelineMiddleware continuingExecutor,
            IQueryPipelineScheduler scheduler)
        {
            NextMiddleware.ContinueWith(continuingExecutor, scheduler);
            //NextExecutor = NextExecutor.ContinueWith(continuingExecutor, pipelineQueryExecutor);

            return this;
        }

        public void OnFailingQuery()
        {
            FailingQuery?.Invoke();

            return;
        }

        #endregion

        #region IQueryPipelineMiddlewareVisitor implemention

        public abstract void VisitStreamingQueryExecutor<TNextQueryResult>(
            StreamingQueryExecutor<TQueryResultData, TNextQueryResult> streaming);

        public abstract void VisitBufferingQueryExecutor<TNextQueryResult>(
            BufferingQueryExecutor<TQueryResultData, TNextQueryResult> buffering);

        #endregion
    }
}
