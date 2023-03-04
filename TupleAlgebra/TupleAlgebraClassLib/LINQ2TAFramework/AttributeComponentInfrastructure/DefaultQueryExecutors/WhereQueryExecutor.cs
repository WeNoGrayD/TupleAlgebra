using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.DefaultQueryExecutors
{
    /*
    public class WhereQueryExecutor<TData> : WholeDataSourceReader<TData, IEnumerable<TData>>
    {
        public WhereQueryExecutor(Func<TData, bool> predicate)
            : base(predicate)
        { }

        public override IEnumerable<TData> Execute()
        {
            return _dataSource;
        }
    }*/

    /*
    public class WhereQueryExecutor<TData> : EveryDataInstanceReader2WithEnumerableResult<TData, TData>
    {
        public WhereQueryExecutor(Predicate<TData> dataPassingCondition)
            : base(dataPassingCondition, (TData data) => data)
        { }

        protected override (bool MustGoOn, bool DidDataPass) ConsumeData(TData data) => (true, DataPassingCondition(data));
    }
    */
    public class WhereQueryExecutor<TData> : WholeDataSourceReader2WithEnumerableResult<TData, TData>
    {
        public WhereQueryExecutor(Predicate<TData> dataPassingCondition)
            : base(dataPassingCondition)
        { }

        protected override IEnumerable<TData> TraverseOverDataSource()
        {
            foreach (TData data in DataSource)
                if (DataPassingCondition(data)) 
                    yield return data;
        }
    }
}
