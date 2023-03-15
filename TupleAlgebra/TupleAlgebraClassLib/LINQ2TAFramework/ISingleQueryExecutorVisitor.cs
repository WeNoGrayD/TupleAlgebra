using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    public interface ISingleQueryExecutorVisitor
    {
        IQueryPipelineMiddleware FirstQueryExecutor { get; set; }

        TPipelineQueryResult ExecuteWithExpectedAggregableResult<TPipelineQueryResult>();

        IEnumerable<TPipelineQueryResultData> ExecuteWithExpectedEnumerableResult<TPipelineQueryResultData>();

        void SetDataSource<TData>(IEnumerable<TData> dataSource);

        TPipelineQueryResult ContinueQueryExecuting<TQueryResultData, TPipelineQueryResult>(
            IQueryPipelineMiddleware nextMiddleware,
            IEnumerable<TQueryResultData> dataSource);

        TPipelineQueryResult VisitStreamingQueryExecutor<TData, TQueryResult, TPipelineQueryResultParam, TPipelineQueryResult>(
            bool iResultEnumerable, StreamingQueryExecutor<TData, TQueryResult> queryExecutor);

        TPipelineQueryResult VisitBufferingQueryExecutor<TData, TQueryResult, TPipelineQueryResultParam, TPipelineQueryResult>(
            bool iResultEnumerable, BufferingQueryExecutor<TData, TQueryResult> queryExecutor);

        TPipelineQueryResult VisitBufferingQueryExecutorWithExpectedAggregableResult<TData, TQueryResult, TPipelineQueryResult>(
            BufferingQueryExecutor<TData, TQueryResult> queryExecutor);

        IEnumerable<TPipelineQueryResultData>
            VisitBufferingQueryExecutorWithExpectedEnumerableResult<TData, TQueryResult, TPipelineQueryResultData>(
            BufferingQueryExecutor<TData, TQueryResult> queryExecutor);

        TPipelineQueryResult
            VisitStreamingQueryExecutorWithExpectedAggregableResult<TData, TQueryResult, TPipelineQueryResult>(
            StreamingQueryExecutor<TData, TQueryResult> queryExecutor);

        IEnumerable<TPipelineQueryResultData>
            VisitStreamingQueryExecutorWithExpectedEnumerableResult<TData, TQueryResult, TPipelineQueryResultData>(
            StreamingQueryExecutor<TData, TQueryResult> queryExecutor);
    }
}
