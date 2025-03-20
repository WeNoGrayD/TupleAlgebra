using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Streaming;

namespace LINQProvider.DefaultQueryExecutors
{
    public class FirstByFilterStreamingQueryExecutor<TData> 
        : ConditionTransformBasedStreamingQueryExecutorWithAggregableResult<TData, TData>
    {
        #region Instance fields

        private TData _first;

        #endregion

        #region Instance properties

        public override TData Accumulator { get => _first; }

        #endregion

        #region Constructors

        public FirstByFilterStreamingQueryExecutor(Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
        }

        #endregion

        #region Instance methods

        protected override void TransformResult(TData data)
        {
            _first = data;

            return;
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            bool didDataPass = _condition(data);

            return (didDataPass, !didDataPass);
        }

        #endregion
    }
}
