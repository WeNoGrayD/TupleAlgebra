using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryPipelineInfrastructure
{
    public interface IQueryPipelineMiddlewareAcceptor<TData>
    {
        /// <summary>
        /// Приём визитёра.
        /// </summary>
        void Accept(
            IQueryPipelineScheduler scheduler,
            IQueryPipelineMiddlewareVisitor<TData> visitor);
    }
}
