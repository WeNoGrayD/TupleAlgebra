using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.DefaultQueryExecutors
{
    /*
    public class ToListBufferingQueryExecutor<TData> 
        : BufferingQueryExecutorWithAggregableResult<TData, List<TData>>
    {
        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        public ToListBufferingQueryExecutor()
            : base((_) => true)
        { }

        protected override List<TData> TraverseOverDataSource()
        {
            List<TData> accumulator = new List<TData>();

            foreach (TData data in DataSource)
                accumulator.Add(data);

            return accumulator;
        }
    }
    */
}
