using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryResultAccumulatorInterfaces
{
    /// <summary>
    /// Интерфейс исполнителя запросов, который аккумулирует агрегируемые выходные данные
    /// при их пропуске.
    /// </summary>
    /// <typeparam name="TQueryResult"></typeparam>
    public interface IAccumulatePositiveAggregableQueryResult<TQueryResult>
        : IAccumulatePositiveQueryResult<TQueryResult, TQueryResult>
    {
        /// <summary>
        /// Инициализация аккумулятора значением по умолчанию.
        /// </summary>
        /// <returns></returns>
        TQueryResult IAccumulatePositiveQueryResult<TQueryResult, TQueryResult>.InitAccumulator()
            => InitAccumulator(default(TQueryResult));
    }
}
