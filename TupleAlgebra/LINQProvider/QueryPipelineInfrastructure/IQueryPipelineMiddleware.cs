using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryPipelineInfrastructure
{
    /// <summary>
    /// Интерфейс компонента конвейера запросов.
    /// </summary>
    public interface IQueryPipelineMiddleware 
        : IQueryResultProvider
    {
        #region Properties

        /// <summary>
        /// Кратность источника данных запроса и его результат.
        /// </summary>
        QuerySourceToResultMiltiplicity Multiplicity { get; }

        /// <summary>
        /// Флаг предоставления результата данным компонентом конвейреа.
        /// </summary>
        bool ResultProvided { get; set; }

        /// <summary>
        /// Флаг продолжения обхода по входным данным.
        /// </summary>
        bool MustGoOn { get; set; }

        /// <summary>
        /// Конечный компонента конвейера запросов.
        /// </summary>
        /// <returns></returns>
        IQueryPipelineEndpoint PipelineEndpoint { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Продолжение компонента конвейера следующим.
        /// </summary>
        /// <param name="continuingExecutor"></param>
        /// <returns></returns>
        IQueryPipelineMiddleware ContinueWith(
            IQueryPipelineEndpoint continuingExecutor,
            IQueryPipelineScheduler scheduler);

        /// <summary>
        /// Подготовка компонента конвейера к тому, что итоговым результатом конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        void PrepareToAggregableResult<TPipelineQueryResult>(ISingleQueryExecutorResultRequester pipelineQueryExecutor);

        /// <summary>
        /// Подготовка компонента конвейера к тому, что итоговым результатом конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        void PrepareToEnumerableResult<TPipelineQueryResultData>(ISingleQueryExecutorResultRequester pipelineQueryExecutor);

        /// <summary>
        /// Получение агрегируемого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorResultRequester pipelineQueryExecutor);

        /// <summary>
        /// Получение перечислимого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorResultRequester pipelineQueryExecutor);

        #endregion
    }

    /// <summary>
    /// Интерфейс компонента конвейера запросов.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResult"></typeparam>
    public interface IQueryPipelineMiddleware<TData, TQueryResult> 
        : IQueryPipelineMiddleware
    {
        #region Properties

        /// <summary>
        /// Внутренний сполнитель запроса.
        /// </summary>
        SingleQueryExecutor<TData, TQueryResult> InnerExecutor { get; }

        #endregion
    }
}
