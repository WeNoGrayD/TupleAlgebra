using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.DefaultQueryExecutors
{
    public class AggregateStreamingQueryExecutor<TData, TQueryResult>
        : StreamingQueryExecutorWithAggregableResult<TData, TQueryResult>
    {
        private TQueryResult _queryResult = default(TQueryResult);

        public AggregateStreamingQueryExecutor(
            Func<TData, bool> dataPassingCondition,
            Func<TData, bool, TQueryResult> transform,
            Func<TQueryResult, TQueryResult, TQueryResult> aggregatingFunc)
            : base(dataPassingCondition, transform)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
            this.DataPassed += (TQueryResult intermediateResult) => 
                _queryResult = aggregatingFunc(_queryResult, intermediateResult);
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            bool didDataPass = DataPassingCondition(data);

            return (didDataPass, true);
        }

        public override void AccumulateIfDataPassed(ref TQueryResult accumulator, TQueryResult outputData)
        {
            accumulator = _queryResult;
        }
    }
}
