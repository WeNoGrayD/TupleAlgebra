using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryPipelineInfrastructure.Streaming
{
    public abstract class ConditionTransformBasedStreamingQueryExecutorWithAggregableResult<TData, TQueryResult>
        : StreamingQueryExecutorWithAggregableResult<TData, TQueryResult>
    {
        #region Instance fields

        protected Func<TData, bool> _condition;

        #endregion

        #region Constructors

        public ConditionTransformBasedStreamingQueryExecutorWithAggregableResult(Func<TData, bool> condition)
            : base()
        {
            _condition = condition;

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
}
