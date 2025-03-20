using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    public abstract class EveryDataInstanceReader<TData, TQueryResult> 
        : ISingleQueryExecutor<TData, TQueryResult>
    {
        public Func<TData, bool> Predicate { get; private set; }

        public void PutData(TData data)
        {

        }

        public abstract TQueryResult GetResult();

        public abstract bool IsAlreadyOver();

        public void Accept(ISingleQueryExecutorVisitor visitor)
        {
            visitor.VisitEveryDataInstanceReader(this);

            return;
        }

        public void Accept(ISingleQueryExecutorVisitor<TData> visitor)
        {
            visitor.VisitEveryDataInstanceReader(this);

            return;
        }
    }
}
