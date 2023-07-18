using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryPipelineInfrastructure
{
    public interface IQueryPipelineMiddlewareWithContinuationAcceptor<TData> : IQueryPipelineMiddleware
    {
        /// <summary>
        /// Приём визитёра.
        /// </summary>
        void Accept(IQueryPipelineMiddlewareVisitor<TData> visitor);
    }
}
