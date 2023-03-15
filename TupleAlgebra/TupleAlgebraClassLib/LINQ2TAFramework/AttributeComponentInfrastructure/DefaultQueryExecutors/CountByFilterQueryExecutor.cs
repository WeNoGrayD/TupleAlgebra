using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.DefaultQueryExecutors
{
    public class CountByFilterBufferingQueryExecutor<TData> :
        BufferingQueryExecutorWithAggregableResult<TData, int>
    {
        public CountByFilterBufferingQueryExecutor(Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition)
        { }

        protected override int TraverseOverDataSource()
        {
            int count = 0;
            foreach (TData data in DataSource)
                if (DataPassingCondition(data)) count++;

            return count;
        }
    }
}
