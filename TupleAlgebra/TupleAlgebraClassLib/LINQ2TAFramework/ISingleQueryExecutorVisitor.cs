using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    public interface ISingleQueryExecutorVisitor
    {
        void VisitWholeDataSourceReader<TData, TQueryResult>(
            WholeDataSourceReader<TData, TQueryResult> reader);

        void VisitEveryDataInstanceReader<TData, TQueryResult>(
            EveryDataInstanceReader<TData, TQueryResult> reader);
    }
}
