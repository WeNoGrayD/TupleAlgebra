using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider
{
    public interface IAccumulatePositiveQueryResult<TQueryResult, TAccumulator> //: IQueryPipelineMiddleware
        where TAccumulator : TQueryResult
    {
        event Action<TQueryResult> DataPassed;

        TAccumulator InitAccumulator();

        TAccumulator InitAccumulator(TQueryResult initialAccumulatorValue);

        void AccumulateIfDataPassed(ref TAccumulator accumulator, TQueryResult outputData);
    }

    public interface IAccumulateNegativeQueryResult<TQueryResult, TAccumulator> //: IQueryPipelineMiddleware
        where TAccumulator : TQueryResult
    {
        event Action<TQueryResult> DataNotPassed;

        void AccumulateIfDataNotPassed(ref TAccumulator accumulator, TQueryResult outputData);
    }

    public interface IAccumulatePositiveAggregableQueryResult<TQueryResult>
        : IAccumulatePositiveQueryResult<TQueryResult, TQueryResult>
    {
        TQueryResult IAccumulatePositiveQueryResult<TQueryResult, TQueryResult>.InitAccumulator()
            => InitAccumulator(default(TQueryResult));
    }

    public interface IAccumulateAnyAggregableQueryResult<TQueryResult>
        : IAccumulatePositiveAggregableQueryResult<TQueryResult>, IAccumulateNegativeQueryResult<TQueryResult, TQueryResult>
    { }

    public interface IAccumulatePositiveEnumerableQueryResult<TQueryResultData>
        : IAccumulatePositiveQueryResult<IEnumerable<TQueryResultData>, IEnumerable<TQueryResultData>>
    {
        IEnumerable<TQueryResultData> IAccumulatePositiveQueryResult<IEnumerable<TQueryResultData>, IEnumerable<TQueryResultData>>
            .InitAccumulator()
            => Enumerable.Empty<TQueryResultData>();

        IEnumerable<TQueryResultData> IAccumulatePositiveQueryResult<IEnumerable<TQueryResultData>, IEnumerable<TQueryResultData>>
            .InitAccumulator(IEnumerable<TQueryResultData> initialAccumulatorValue) =>
            new OneShotEnumerable<TQueryResultData>(initialAccumulatorValue);
    }

    public interface IAccumulateAnyEnumerableQueryResult<TQueryResultData>
        : IAccumulatePositiveEnumerableQueryResult<TQueryResultData>,
          IAccumulateNegativeQueryResult<IEnumerable<TQueryResultData>, IEnumerable<TQueryResultData>>
    { }
}
