using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    public class BufferingQueryExecutorWithAccumulation<TData, TQueryResult, TAccumulator>
        : QueryPipelineMiddlewareWithAccumulation<BufferingQueryExecutor<TData, TQueryResult>, TData, TQueryResult, TAccumulator>
        where TAccumulator : TQueryResult
    {
        public BufferingQueryExecutorWithAccumulation(
            BufferingQueryExecutor<TData, TQueryResult> innerExecutor,
            IAccumulatePositiveQueryResult<TQueryResult, TAccumulator> innerExecutorAsAccumulating)
            : base(innerExecutor, innerExecutorAsAccumulating)
        { }

        public override TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipeline)
        {
            _innerExecutorImpl.GetQueryResult();

            return (dynamic)_accumulator;
        }

        public override IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            throw new NotImplementedException();
        }

        public override void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<TQueryResult> queryResultPassingHandler)
        {
            InnerExecutor.DataPassed += (TQueryResult outputData) =>
                queryResultPassingHandler(_innerExecutorImpl.GetQueryResult());
        }
    }

    public class BufferingQueryExecutorWithAccumulationOfEnumerableResult<TData, TQueryResultData>
        : QueryPipelineMiddlewareWithAccumulation<BufferingQueryExecutorWithEnumerableResult<TData, TQueryResultData>,
                                              TData,
                                              IEnumerable<TQueryResultData>,
                                              IEnumerable<TQueryResultData>>
    {
        public BufferingQueryExecutorWithAccumulationOfEnumerableResult(
            BufferingQueryExecutorWithEnumerableResult<TData, TQueryResultData> innerExecutor,
            IAccumulatePositiveQueryResult<IEnumerable<TQueryResultData>, IEnumerable<TQueryResultData>> innerExecutorAsAccumulating)
            : base(innerExecutor, innerExecutorAsAccumulating)
        { }

        public override IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipeline)
        {
            _innerExecutorImpl.GetQueryResult();

            return (dynamic)_accumulator;
        }

        public override TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            throw new NotImplementedException();
        }

        public override void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<IEnumerable<TQueryResultData>> queryResultPassingHandler)
        {
            InnerExecutor.DataPassed += (IEnumerable<TQueryResultData> outputData) =>
                queryResultPassingHandler(_innerExecutorImpl.GetQueryResult());
        }
    }
}
