using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Streaming;

namespace LINQProvider.DefaultQueryExecutors
{
    public class AggregateStreamingQueryExecutor<TData, TQueryResult>
        : TransformBasedStreamingQueryExecutorWithAggregableResult<TData, TQueryResult>
    {
        #region Instance fields

        /// <summary>
        /// Результат агрегирования данных.
        /// </summary>
        private TQueryResult _queryResult;

        /// <summary>
        /// Функция агрегирования данных.
        /// </summary>
        private Func<TQueryResult, TData, TQueryResult> _aggregatingFunc;

        #endregion

        #region Instance properties

        public override TQueryResult Accumulator { get => _queryResult; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="aggregatingFunc"></param>
        /// <param name="seed"></param>
        public AggregateStreamingQueryExecutor(
            Func<TQueryResult, TData, TQueryResult> aggregatingFunc,
            TQueryResult seed = default(TQueryResult)!)
            : base()
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
            _queryResult = seed;
            _aggregatingFunc = aggregatingFunc;
        }

        #endregion

        #region Instance methods

        protected override void TransformResult(TData data)
        {
            _queryResult = _aggregatingFunc(_queryResult, data);

            return;
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data) => (true, true);

        #endregion
    }
}
