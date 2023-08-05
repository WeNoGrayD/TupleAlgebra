using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryPipelineInfrastructure
{
    /// <summary>
    /// Интерфейс необобщённого конечного компонента конвейера запросов.
    /// </summary>
    public interface IQueryPipelineEndpoint : IQueryPipelineMiddleware
    {
        #region Methods

        void InitializeAsQueryPipelineStartupMiddleware();

        /// <summary>
        /// Инициализация компонента в качестве конечного компонента конвейера запросов.
        /// </summary>
        void InitializeAsQueryPipelineEndpoint();

        #endregion
    }

    /// <summary>
    /// Интерфейс обобщённого конечного компонента конвейера запросов.
    /// </summary>
    public interface IQueryPipelineEndpoint<TData, TQueryResult> 
        : IQueryPipelineMiddleware<TData, TQueryResult>,
          IQueryPipelineEndpoint,
          IQueryPipelineMiddlewareAcceptor<TData>
    { }
}
