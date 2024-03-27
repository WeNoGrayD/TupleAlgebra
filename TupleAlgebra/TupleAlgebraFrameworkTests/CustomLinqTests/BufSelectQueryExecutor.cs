using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraFrameworkTests.CustomLinqTests
{
    internal class BufSelectQueryExecutor<TData, TQueryResultData>
        : LINQProvider.QueryPipelineInfrastructure.Buffering
            .BufferingQueryExecutorWithEnumerableResult<TData, TQueryResultData>
    {
        private Func<TData, TQueryResultData> _transform;

        public BufSelectQueryExecutor(Func<TData, TQueryResultData> transform) : base()
        {
            _transform = transform;
            return;
        }

        public override IEnumerable<TQueryResultData> Execute()
        {
            foreach (TData data in _dataSource) yield return _transform(data);

            yield break;
        }
    }
}
