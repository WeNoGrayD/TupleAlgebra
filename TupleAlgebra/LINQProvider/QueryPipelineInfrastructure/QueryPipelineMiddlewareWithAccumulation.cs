using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Streaming;
using LINQProvider.QueryResultAccumulatorInfrastructure;

namespace LINQProvider.QueryPipelineInfrastructure
{
    /// <summary>
    /// Компонент конвейера запросов с аккумулированием результатов запроса. 
    /// </summary>
    /// <typeparam name="TInnerQueryExecutor"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResult"></typeparam>
    /// <typeparam name="TAccumulator"></typeparam>
    public abstract class QueryPipelineMiddlewareWithAccumulation<TData, TQueryResult, TInnerQueryExecutor>
        : QueryPipelineMiddleware<TData, TQueryResult, TInnerQueryExecutor>,
          IQueryPipelineEndpoint<TData, TQueryResult>
        where TInnerQueryExecutor : SingleQueryExecutor<TData, TQueryResult>
    {
        #region Instance fields

        #endregion

        #region Instance properties

        public override sealed bool MustGoOn { get => true; set { } }

        public override IQueryPipelineEndpoint PipelineEndpoint { get => this; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="innerExecutor"></param>
        /// <param name="innerExecutorAsAccumulating"></param>
        protected QueryPipelineMiddlewareWithAccumulation(TInnerQueryExecutor innerExecutor)
            : base(innerExecutor)
        {
            return;
        }

        #endregion

        #region Instance methods

        #endregion

        #region IQueryPipelineEndpoint implementation

        public abstract void InitializeAsQueryPipelineStartupMiddleware();

        /// <summary>
        /// Инициализация компонента в качестве конечного компонента конвейера запросов.
        /// </summary>
        public abstract void InitializeAsQueryPipelineEndpoint();

        #endregion

        #region IQueryPipelineMiddlewareAcceptor<TData> implementation

        public abstract void Accept(
            IQueryPipelineScheduler scheduler,
            IQueryPipelineMiddlewareVisitor<TData> visitor);

        #endregion
    }
}
