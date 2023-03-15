using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    public abstract class SingleQueryExecutor<TData, TQueryResult> : IQueryPipelineAcceptor
    {
        public event Action<TQueryResult> DataPassed;

        public SingleQueryExecutor()
        { }

        public abstract TPipelineQueryResult
            Accept<TPipelineQueryResultParam, TPipelineQueryResult>(
            bool isResultEnumerabl, ISingleQueryExecutorVisitor queryPipeline);

        protected void OnDataPassed(TQueryResult outputData) => DataPassed?.Invoke(outputData);

        protected void OnDataPassed(Func<TData, bool, TQueryResult> outputDataSelector, TData data) =>
            DataPassed?.Invoke(outputDataSelector(data, true));
    }
}
