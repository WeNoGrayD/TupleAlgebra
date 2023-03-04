using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    public interface IQueryPipelineExecutorAcceptor
    {
        void Accept(ISingleQueryExecutorVisitor visitor);
    }

    public interface IQueryPipelineExecutorAcceptor<TData>
    {
        void Accept(ISingleQueryExecutorVisitor<TData> visitor);
    }
}
