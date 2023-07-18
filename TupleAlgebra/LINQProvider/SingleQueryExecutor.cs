using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure;

namespace LINQProvider
{
    [Flags]
    public enum QuerySourceToResultMiltiplicity : byte
    {
        ManyToOne = 1,
        OneToOne = 2,
        OneToMany = 6
    }

    /// <summary>
    /// Исполнитель запроса.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResult"></typeparam>
    public abstract class SingleQueryExecutor<TData, TQueryResult> : IQueryPipelineAcceptor
    {
        #region Instance properties

        /// <summary>
        /// Кратность источника данных запроса и его результат.
        /// </summary>
        public abstract QuerySourceToResultMiltiplicity Multiplicity { get; } 

        #endregion

        #region Instance events

        /// <summary>
        /// Событие пропуска данных на возможный следующий компонент конвейера запросов. 
        /// </summary>
        public event Action<TQueryResult> DataPassed;

        #endregion

        #region Instance methods

        /// <summary>
        /// Вызов события пропуска данных с готовым промежуточным результатом.
        /// </summary>
        /// <param name="outputData"></param>
        protected void OnDataPassed(TQueryResult outputData)
        {
            DataPassed?.Invoke(outputData);

            return;
        }

        /// <summary>
        /// Вызов события пропуска данных с отложенным преобразованим данных в промежуточный результат.
        /// </summary>
        /// <param name="outputDataSelector"></param>
        /// <param name="data"></param>
        /*
         * Отложенное преобразование данных используется для того, чтобы оно не выполнялось в том случае,
         * если данные не прошли.
         */
        protected void OnDataPassed(Func<TData, TQueryResult> outputDataSelector, TData data)
        {
            DataPassed?.Invoke(outputDataSelector(data));

            return;
        }

        #endregion

        #region IQueryPipelineAcceptor implementation

        public abstract TPipelineQueryResult AcceptToExecuteWithAggregableResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor queryPipeline);

        public abstract IEnumerable<TPipelineQueryResultData> AcceptToExecuteWithEnumerableResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor queryPipeline);

        #endregion
    }
}
