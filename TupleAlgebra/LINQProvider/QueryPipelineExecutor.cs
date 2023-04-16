using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace LINQProvider
{
    /// <summary>
    /// Исполнитель конвейера запросов.
    /// </summary>
    public abstract class QueryPipelineExecutor : ISingleQueryExecutorVisitor
    {
        #region Instance fields

        /// <summary>
        /// Источник данных конвейера запросов.
        /// </summary>
        private IEnumerable _dataSource;

        /// <summary>
        /// 
        /// </summary>
        private MethodInfo _pipelineQueryExecutionMethodInfo;

        /// <summary>
        /// Ссылка на первый компонент конвейера запросов.
        /// </summary>
        public IQueryPipelineMiddleware _firstPipelineMiddleware { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="firstPipelineMiddleware"></param>
        protected QueryPipelineExecutor(IEnumerable dataSource, IQueryPipelineMiddleware firstPipelineMiddleware)
        {
            _dataSource = dataSource;
            _firstPipelineMiddleware = firstPipelineMiddleware;
            firstPipelineMiddleware.GetPipelineEndpoint().InitializeAsQueryPipelineEndpoint();
        }

        #endregion

        #region Instance methods

        /// <summary>
        /// Получение обобщённого источника данных.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <returns></returns>
        private IEnumerable<TData> GetDataSource<TData>()
        {
            return _dataSource as IEnumerable<TData>;
        }

        #endregion

        #region ISingleQueryExecutorVisitor implementation

        /// <summary>
        /// Обобщённая установка источника данных.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="dataSource"></param>
        public void SetDataSource<TData>(IEnumerable<TData> dataSource)
        {
            _dataSource = dataSource;

            return;
        }

        /// <summary>
        /// Выполнение запроса с ожидаемым агрегируемым результатом.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <returns></returns>
        public TPipelineQueryResult ExecuteWithExpectedAggregableResult<TPipelineQueryResult>()
        {
            _pipelineQueryExecutionMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return _firstPipelineMiddleware.Accept<TPipelineQueryResult, TPipelineQueryResult>(false, this);
        }

        /// <summary>
        /// Выполнение запроса с ожидаемым перечислимым результатом.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <returns></returns>
        public IEnumerable<TPipelineQueryResultData> ExecuteWithExpectedEnumerableResult<TPipelineQueryResultData>()
        {
            _pipelineQueryExecutionMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return _firstPipelineMiddleware.Accept<TPipelineQueryResultData, IEnumerable<TPipelineQueryResultData>>(true, this);
        }

        /// <summary>
        /// Продолжение выполнение конвейера запросов со следующего компонента, как будто он первый.
        /// </summary>
        /// <typeparam name="TQueryResultData"></typeparam>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="nextMiddleware"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public TPipelineQueryResult ContinueQueryExecuting<TQueryResultData, TPipelineQueryResult>(
            IQueryPipelineMiddleware nextMiddleware,
            IEnumerable<TQueryResultData> dataSource)
        {
            // Установка текущего источника данных в конвейере значением результата выполненного запроса.
            SetDataSource(dataSource);
            _firstPipelineMiddleware = nextMiddleware;
            return (TPipelineQueryResult)_pipelineQueryExecutionMethodInfo.Invoke(this, null);
        }

        /// <summary>
        /// Посещение потокового исполнителя запросов.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <typeparam name="TPipelineQueryResultParam"></typeparam>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="isResultEnumerable"></param>
        /// <param name="queryExecutor"></param>
        /// <returns></returns>
        public TPipelineQueryResult VisitStreamingQueryExecutor<TData, TQueryResult, TPipelineQueryResultParam, TPipelineQueryResult>(
            bool isResultEnumerable,
            StreamingQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            return isResultEnumerable ?
            (TPipelineQueryResult)VisitStreamingQueryExecutorWithExpectedEnumerableResult<TData, TQueryResult, TPipelineQueryResultParam>(queryExecutor) :
            VisitStreamingQueryExecutorWithExpectedAggregableResult<TData, TQueryResult, TPipelineQueryResult>(queryExecutor);
        }

        /// <summary>
        /// Посещение буферизирующего исполнителя запросов.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <typeparam name="TPipelineQueryResultParam"></typeparam>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="isResultEnumerable"></param>
        /// <param name="queryExecutor"></param>
        /// <returns></returns>
        public TPipelineQueryResult VisitBufferingQueryExecutor<TData, TQueryResult, TPipelineQueryResultParam, TPipelineQueryResult>(
            bool isResultEnumerable, 
            BufferingQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            queryExecutor.LoadDataSource(GetDataSource<TData>());

            return isResultEnumerable ? 
                (TPipelineQueryResult)VisitBufferingQueryExecutorWithExpectedEnumerableResult<TData, TQueryResult, TPipelineQueryResultParam>(queryExecutor) :
                VisitBufferingQueryExecutorWithExpectedAggregableResult<TData, TQueryResult, TPipelineQueryResult>(queryExecutor);
        }

        /// <summary>
        /// Посещение буферизирующего исполнителя запросов с ожидаемым агрегируемым результатом.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="queryExecutor"></param>
        /// <returns></returns>
        public TPipelineQueryResult 
            VisitBufferingQueryExecutorWithExpectedAggregableResult<TData, TQueryResult, TPipelineQueryResult>(
            BufferingQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            return _firstPipelineMiddleware.GetAggregablePipelineQueryResult<TPipelineQueryResult>(this);
        }

        /// <summary>
        /// Посещение буферизирующего исполнителя запросов с ожидаемым перечислимым результатом.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="queryExecutor"></param>
        /// <returns></returns>
        public IEnumerable<TPipelineQueryResultData> 
            VisitBufferingQueryExecutorWithExpectedEnumerableResult<TData, TQueryResult, TPipelineQueryResultData>(
            BufferingQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            return _firstPipelineMiddleware.GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(this);
        }

        /// <summary>
        /// Посещение потокового исполнителя запросов с ожидаемым агрегируемым результатом.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="queryExecutor"></param>
        /// <returns></returns>
        public TPipelineQueryResult
            VisitStreamingQueryExecutorWithExpectedAggregableResult<TData, TQueryResult, TPipelineQueryResult>(
            StreamingQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            _firstPipelineMiddleware.PrepareToAggregableResult<TPipelineQueryResult>(this);
            queryExecutor.PrepareToQueryStart();
            foreach (TData data in GetDataSource<TData>())
                if (!queryExecutor.ExecuteOverDataInstance(data)) break;

            return _firstPipelineMiddleware.GetAggregablePipelineQueryResult<TPipelineQueryResult>(this);
        }

        /// <summary>
        /// Посещение потокового исполнителя запросов с ожидаемым перечислимым результатом.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="queryExecutor"></param>
        /// <returns></returns>
        public IEnumerable<TPipelineQueryResultData>
            VisitStreamingQueryExecutorWithExpectedEnumerableResult<TFirstData, TFirstQueryResult, TPipelineQueryResultData>(
            StreamingQueryExecutor<TFirstData, TFirstQueryResult> firstQueryExecutor)
        {
            _firstPipelineMiddleware.PrepareToEnumerableResult<TPipelineQueryResultData>(this);
            firstQueryExecutor.PrepareToQueryStart();
            bool mustGoOn;

            foreach (TFirstData data in GetDataSource<TFirstData>())
            {
                mustGoOn = firstQueryExecutor.ExecuteOverDataInstance(data);
                foreach (TPipelineQueryResultData resultData
                         in _firstPipelineMiddleware.GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(this))
                {
                    yield return resultData;
                    if (!(mustGoOn &= _firstPipelineMiddleware.MustGoOn)) yield break;
                }
                if (!(mustGoOn & _firstPipelineMiddleware.MustGoOn)) yield break;
            }

            yield break;
        }

        #endregion
    }
}
