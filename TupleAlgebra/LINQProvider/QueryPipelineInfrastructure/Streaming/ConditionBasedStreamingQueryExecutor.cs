using LINQProvider.QueryResultAccumulatorInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryPipelineInfrastructure.Streaming
{
    public abstract class ConditionBasedStreamingQueryExecutorWithAggregableResult<TData, TQueryResult>
        : StreamingQueryExecutorWithAggregableResult<TData, TQueryResult>
    {
        #region Instance fields

        protected Func<TData, bool> _condition;

        #endregion

        #region Constructors

        public ConditionBasedStreamingQueryExecutorWithAggregableResult(Func<TData, bool> condition)
            : base()
        {
            _condition = condition;

            return;
        }

        #endregion
    }

    public abstract class ConditionBasedStreamingQueryExecutorWithEnumerableOneToOneResult<TData>
        : StreamingQueryExecutorWithDirectEnumerableOneToOneResult<TData>
    {
        #region Instance fields

        protected Func<TData, bool> _condition;

        #endregion

        #region Constructors

        public ConditionBasedStreamingQueryExecutorWithEnumerableOneToOneResult()
            : base()
        {
            return;
        }

        public ConditionBasedStreamingQueryExecutorWithEnumerableOneToOneResult(Func<TData, bool> condition)
            : base()
        {
            _condition = condition;

            return;
        }

        #endregion
    }
}
