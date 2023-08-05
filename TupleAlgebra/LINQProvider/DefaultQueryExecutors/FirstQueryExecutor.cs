using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Streaming;

namespace LINQProvider.DefaultQueryExecutors
{
    public class FirstStreamingQueryExecutor<TData> : StreamingQueryExecutorWithAggregableResult<TData, TData>
    {
        #region Instance fields

        private TData _first;

        #endregion

        #region Instance properties

        public override TData Accumulator { get => _first; }

        #endregion

        #region Constructors

        public FirstStreamingQueryExecutor()
            : base()
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
        }

        #endregion

        #region Instance methods

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            _first = data;

            return (true, false);
        }

        #endregion
    }
}
