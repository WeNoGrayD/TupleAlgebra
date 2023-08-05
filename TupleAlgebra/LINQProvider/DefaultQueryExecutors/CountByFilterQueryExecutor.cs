using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Buffering;
using LINQProvider.QueryPipelineInfrastructure.Streaming;

namespace LINQProvider.DefaultQueryExecutors
{
    public class CountByFilterStreamingQueryExecutor<TData> 
        : ConditionTransformBasedStreamingQueryExecutorWithAggregableResult<TData, int>
    {
        #region Instance fields

        private int _count = 0;

        #endregion

        #region Instance properties

        public override int Accumulator { get => _count; }

        #endregion

        #region Constructors

        public CountByFilterStreamingQueryExecutor(Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
        }

        #endregion

        #region Instance methods

        protected override void TransformResult(TData data)
        {
            _count++;

            return;
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            return (_condition(data), true);
        }

        #endregion
    }
}
