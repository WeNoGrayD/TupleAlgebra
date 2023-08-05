using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.DefaultQueryExecutors
{
    public class ToListBufferingQueryExecutor<TData> 
        : QueryPipelineInfrastructure.Buffering
            .BufferingQueryExecutorWithAggregableResult<TData, List<TData>>
    {
        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        public ToListBufferingQueryExecutor()
            : base()
        { }

        public override List<TData> Execute()
        {
            return new List<TData>(_dataSource);
        }
    }
}
