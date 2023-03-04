using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    public abstract class WholeDataSourceReader<TData, TQueryResult> 
        : ISingleQueryExecutor<TData, TQueryResult>
    {
        public Func<TData, bool> Predicate { get; private set; }

        protected List<TData> _dataSource;

        public WholeDataSourceReader(Func<TData, bool> predicate)
        {
            Predicate = predicate;
            _dataSource = new List<TData>();
        }

        public void PutData(TData data)
        {
            _dataSource.Add(data);
        }

        public abstract TQueryResult GetResult();

        public void Accept(ISingleQueryExecutorVisitor visitor)
        {
            visitor.VisitWholeDataSourceReader(this);

            return;
        }

        public void Accept(ISingleQueryExecutorVisitor<TData> visitor)
        {
            visitor.VisitWholeDataSourceReader(this);

            return;
        }
    }
}
