using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.DefaultQueryExecutors
{
    public class ToArrayBufferingQueryExecutor<TData>
        : QueryPipelineInfrastructure.Buffering
            .BufferingQueryExecutorWithAggregableResult<TData, TData[]>
    {
        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        public ToArrayBufferingQueryExecutor()
            : base()
        { }

        public override TData[] Execute()
        {
            return Enumerable.ToArray(_dataSource);
        }
    }
}
