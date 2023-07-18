using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryPipelineInfrastructure
{
    /// <summary>
    /// Интерфейс конечного компонента конвейера запросов.
    /// </summary>
    public interface IQueryPipelineEndpoint : IQueryPipelineMiddleware
    {
        #region Methods

        /// <summary>
        /// Инициализация компонента в качестве конечного компонента конвейера запросов.
        /// </summary>
        void InitializeAsQueryPipelineEndpoint();

        #endregion
    }
}
