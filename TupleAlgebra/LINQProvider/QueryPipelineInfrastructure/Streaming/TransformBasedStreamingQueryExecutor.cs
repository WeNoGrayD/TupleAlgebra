using LINQProvider.QueryResultAccumulatorInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryPipelineInfrastructure.Streaming
{
    public abstract class TransformBasedStreamingQueryExecutorWithAggregableResult<TData, TQueryResult>
        : StreamingQueryExecutorWithAggregableResult<TData, TQueryResult>
    {
        #region Constructors

        public TransformBasedStreamingQueryExecutorWithAggregableResult()
            : base()
        {
            return;
        }

        #endregion

        #region Instance methods

        protected abstract void TransformResult(TData data);

        public override (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithPositiveCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) flags = ConsumeData(data);
            if (flags.didDataPass)
            {
                TransformResult(data);
                OnDataPassed(Accumulator);
            }

            return flags;
        }

        public override (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithFullCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) flags = ConsumeData(data);
            TransformResult(data);
            if (flags.didDataPass) OnDataPassed(Accumulator);
            else OnDataNotPassed(Accumulator);

            return flags;
        }

        #endregion
    }

    public abstract class TransformBasedStreamingQueryExecutorWithEnumerableOneToOneResult<TData, TQueryResultData>
        : StreamingQueryExecutorWithEnumerableOneToOneResult<TData, TQueryResultData>
    {
        #region instance fields

        protected Func<TData, TQueryResultData> _transform;

        #endregion

        #region Constructors

        public TransformBasedStreamingQueryExecutorWithEnumerableOneToOneResult()
            : base()
        {
            return;
        }

        public TransformBasedStreamingQueryExecutorWithEnumerableOneToOneResult(Func<
            TData, TQueryResultData> transform)
            : this()
        {
            _transform = transform;

            return;
        }

        #endregion

        #region Instance methods

        public override (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithPositiveCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) flags = ConsumeData(data);
            if (flags.didDataPass)
            {
                _queryResult[0] = _transform(data);
                OnDataPassed(_queryResult);
            }

            return flags;
        }

        public override (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithFullCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) flags = ConsumeData(data);
            _queryResult[0] = _transform(data);
            if (flags.didDataPass) OnDataPassed(_queryResult);
            else OnDataNotPassed(_queryResult);

            return flags;
        }

        #endregion
    }

    public abstract class TransformBasedStreamingQueryExecutorWithEnumerableOneToManyResult<TData, TQueryResultData>
        : StreamingQueryExecutorWithEnumerableOneToManyResult<TData, TQueryResultData>
    {
        #region Constructors

        public TransformBasedStreamingQueryExecutorWithEnumerableOneToManyResult()
            : base()
        {
            return;
        }

        #endregion

        #region Instance methods

        protected abstract IEnumerable<TQueryResultData> Transform(TData data);

        public override (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithPositiveCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) flags = ConsumeData(data);
            if (flags.didDataPass) OnDataPassed(Transform, data);

            return flags;
        }

        public override (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithFullCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) flags = ConsumeData(data);
            if (flags.didDataPass) OnDataPassed(Transform(data));
            else OnDataNotPassed(Transform(data));

            return flags;
        }

        #endregion
    }
}
