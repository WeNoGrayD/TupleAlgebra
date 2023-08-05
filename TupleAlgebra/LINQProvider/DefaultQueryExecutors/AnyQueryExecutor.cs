using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Streaming;

namespace LINQProvider.DefaultQueryExecutors
{
    public class AnyStreamingQueryExecutor<TData> 
        : ConditionBasedStreamingQueryExecutorWithAggregableResult<TData, bool>
    {
        #region Instance fields

        private bool _success = false;

        #endregion

        #region Instance properties

        public override bool Accumulator { get => _success; }

        #endregion

        #region Constructors

        public AnyStreamingQueryExecutor(Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
        }

        #endregion

        #region Instance methods

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            bool didDataPass = _condition(data);
            _success |= didDataPass;

            return (didDataPass, !didDataPass);
        }

        #endregion
    }
}
