using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryPipelineInfrastructure
{
    /// <summary>
    /// Приниматель конвейера запросов.
    /// </summary>
    public interface IQueryResultProvider
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
        //TPipelineQueryResult Accept<TPipelineQueryResultParam, TPipelineQueryResult>(
        //    bool isResultEnumerable, ISingleQueryExecutorVisitor queryPipeline);

        TPipelineQueryResult AcceptToExecuteWithAggregableResult<TPipelineQueryResult>(
            ISingleQueryExecutorResultRequester resultRequster);

        IEnumerable<TPipelineQueryResultData> AcceptToExecuteWithEnumerableResult<TPipelineQueryResultData>(
            System.Collections.IEnumerable dataSource,
            ISingleQueryExecutorResultRequester resultRequster);

        #endregion
    }
}
