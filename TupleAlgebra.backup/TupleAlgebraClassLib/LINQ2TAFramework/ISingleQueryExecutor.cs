using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    public interface ISingleQueryExecutor<TData, TQueryResult> 
        : IQueryPipelineExecutorAcceptor, IQueryPipelineExecutorAcceptor<TData>
    {
        Func<TData, bool> Predicate { get; }

        void PutData(TData data);

        TQueryResult GetResult();
    }
}
