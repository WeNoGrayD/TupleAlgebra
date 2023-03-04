using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.DefaultQueryExecutors
{
    public class WhereQueryExecutor<TData> : WholeDataSourceReader<TData, IEnumerable<TData>>
    {
        public WhereQueryExecutor(Func<TData, bool> predicate)
            : base(predicate)
        { }

        public override IEnumerable<TData> GetResult()
        {
            return _dataSource;
        }
    }
}
