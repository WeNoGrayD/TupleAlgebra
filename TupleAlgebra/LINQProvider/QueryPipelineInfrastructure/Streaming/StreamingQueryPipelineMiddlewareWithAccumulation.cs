using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure;
using LINQProvider.QueryPipelineInfrastructure.Buffering;
using LINQProvider.QueryResultAccumulatorInfrastructure;

namespace LINQProvider.QueryPipelineInfrastructure.Streaming
{
    /// <summary>
    /// Компонент конвейера запросов с аккумулированием  результата потокового исполнителя.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResultData"></typeparam>
    public class StreamingQueryPipelineMiddlewareWithAccumulation<TData, TQueryResult>
        : QueryPipelineMiddlewareWithAccumulation<
            TData,
            TQueryResult,
            StreamingQueryExecutor<TData, TQueryResult>>,
          IStreamingQueryPipelineMiddleware<TData>
    {
        #region Instance fields

        protected LinkedListNode<IQueryPipelineMiddleware> _node;

        #endregion

        #region Instance properties

        public LinkedListNode<IQueryPipelineMiddleware> PipelineScheduleNode { get => _node; }

        public IQueryPipelineMiddleware PreviousMiddleware { get => _node.Previous!.Value; }

        public IStreamingQueryExecutor<TData> Executor { get => _innerExecutorImpl; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="innerExecutor"></param>
        /// <param name="innerExecutorAsAccumulating"></param>
        public StreamingQueryPipelineMiddlewareWithAccumulation(
            LinkedListNode<IQueryPipelineMiddleware> node,
            StreamingQueryExecutor<TData, TQueryResult> innerExecutor)
            : base(innerExecutor)
        {
            node.Value = this;
            _node = node;

            return;
        }

        #endregion

        #region IQueryPipelineEndpoint implementation

        /// <summary>
        /// Получение агрегируемого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorResultRequester pipelineQueryExecutor)
        {
            return (_innerExecutorImpl as StreamingQueryExecutor<TData, TPipelineQueryResult>)!
                .Accumulator;
        }

        /// <summary>
        /// Получение перечислимого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        public override IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorResultRequester pipeline)
        {
            return (_innerExecutorImpl.Accumulator as IEnumerable<TPipelineQueryResultData>)!;
        }

        #endregion
        public override void InitializeAsQueryPipelineStartupMiddleware()
        {
            /*
             * Создаётся новая задача, в эту задачу добавляется первым звеном звено-продолжение конвейера.
             */
            LinkedList<IQueryPipelineMiddleware> newPipelineTask = new LinkedList<IQueryPipelineMiddleware>();
            newPipelineTask.AddFirst(_node);

            return;
        }

        /// <summary>
        /// Продолжение компонента конвейера следующим.
        /// </summary>
        /// <typeparam name="TContinuingQueryData"></typeparam>
        /// <typeparam name="TContinuingQueryResult"></typeparam>
        /// <param name="continuingExecutor"></param>
        /// <returns></returns>
        public override IQueryPipelineMiddleware<TData, TQueryResult> ContinueWith(
            IQueryPipelineEndpoint continuingExecutor,
            IQueryPipelineScheduler scheduler)
        {
            throw new NotImplementedException();
        }

        public void PreparePipelineResultProvider()
        {
            return;
        }

        /// <summary>
        /// Инициализация компонента в качестве конечного компонента конвейера запросов.
        /// </summary>
        public override void InitializeAsQueryPipelineEndpoint()
        {
            _innerExecutorImpl.AccumulateQueryResult();
        }

        #region IQueryPipelineAcceptor implementation

        public override TPipelineQueryResult AcceptToExecuteWithAggregableResult<TPipelineQueryResult>(
            ISingleQueryExecutorResultRequester resultRequster)
        {
            return resultRequster.VisitStreamingQueryExecutorWithExpectedAggregableResult<TData, TQueryResult, TPipelineQueryResult>(
                _innerExecutorImpl);
        }

        public override IEnumerable<TPipelineQueryResultData> AcceptToExecuteWithEnumerableResult<TPipelineQueryResultData>(
            System.Collections.IEnumerable dataSource,
            ISingleQueryExecutorResultRequester resultRequster)
        {
            return resultRequster.VisitStreamingQueryExecutorWithExpectedEnumerableResult<
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
            visitor.VisitStreamingQueryExecutor(_innerExecutorImpl);

            return;
        }

        public override void Accept(
            IQueryPipelineScheduler scheduler,
            IQueryPipelineMiddlewareVisitor<TData> visitor)
        {
            visitor.VisitStreamingMiddleware(scheduler, this);

            return;
        }

        #endregion

        #region IQueryPipelineMiddleware implementation

        public override void PrepareToAggregableResult<TPipelineQueryResult>(ISingleQueryExecutorResultRequester pipelineQueryExecutor)
        {
            _innerExecutorImpl.PrepareToQueryStart();

            return;
        }

        public override void PrepareToEnumerableResult<TPipelineQueryResultData>(ISingleQueryExecutorResultRequester pipelineQueryExecutor)
        {
            _innerExecutorImpl.PrepareToQueryStart();

            return;
        }

        #endregion
    }

    /// <summary>
    /// Компонент конвейера запросов с аккумулированием перечислимого результата потокового исполнителя.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResultData"></typeparam>
    public class StreamingQueryPipelineMiddlewareWithEnumerableResultAccumulation<TData, TQueryResultData>
        : StreamingQueryPipelineMiddlewareWithAccumulation<TData, IEnumerable<TQueryResultData>>,
          IQueryPipelineMiddlewareVisitor<TQueryResultData>
    {
        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="innerExecutor"></param>
        /// <param name="innerExecutorAsAccumulating"></param>
        public StreamingQueryPipelineMiddlewareWithEnumerableResultAccumulation(
            LinkedListNode<IQueryPipelineMiddleware> node,
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>> innerExecutor)
            : base(node, innerExecutor)
        {
            return;
        }

        #endregion

        /// <summary>
        /// Продолжение компонента конвейера следующим.
        /// </summary>
        /// <param name="continuingExecutor"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public override IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> ContinueWith(
            IQueryPipelineEndpoint continuingExecutor,
            IQueryPipelineScheduler scheduler)
        {
            /*
             * Происходит посещение присоединяемого к конвейеру компонента.
             * В результате в ассоциируемом с данным (this) компонентом узле задачи конвейера
             * _node свойство Value содержит ссылку на компонент в той же позиции внутри задачи. 
             */
            (continuingExecutor as IQueryPipelineMiddlewareAcceptor<TQueryResultData>)!
                .Accept(scheduler, this);

            return (_node.Value as IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>>)!;
        }

        /// <summary>
        /// Посещение потокового компонента конвейера для продолжения им данного.
        /// </summary>
        /// <typeparam name="TExecutorQueryResult"></typeparam>
        /// <param name="scheduler"></param>
        /// <param name="nextExecutor"></param>
        public void VisitStreamingMiddleware<TExecutorQueryResult>(
            IQueryPipelineScheduler scheduler,
            StreamingQueryPipelineMiddlewareWithAccumulation<TQueryResultData, TExecutorQueryResult> 
                nextExecutor)
        {
            /*
             * В свойстве _node.Value сохраняется новый компонент с продолжением.
             */
            _node.Value = scheduler.MiddlewareWithContinuationFactory.Create(
                _node,
                this,
                nextExecutor);

            return;
        }

        /// <summary>
        /// Посещение буферизирующего компонента конвейера для продолжения им данного.
        /// </summary>
        /// <typeparam name="TExecutorQueryResult"></typeparam>
        /// <param name="scheduler"></param>
        /// <param name="nextExecutor"></param>
        public void VisitBufferingMiddleware<TExecutorQueryResult>(
            IQueryPipelineScheduler scheduler,
            BufferingQueryPipelineMiddlewareWithAccumulation<TQueryResultData, TExecutorQueryResult>
                nextExecutor)
        {
            /*
             * В конвейер добавляется новая задача с continuingExecutor в качестве первого компонента.
             */
            scheduler.PushMiddleware(scheduler.MiddlewareWithAccumulationFactory.Create(nextExecutor.InnerExecutor));

            return;
        }
    }
}
