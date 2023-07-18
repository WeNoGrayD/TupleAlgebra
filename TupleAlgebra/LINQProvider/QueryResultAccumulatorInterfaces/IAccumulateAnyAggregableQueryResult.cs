using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryResultAccumulatorInterfaces
{
    /// <summary>
    /// Интерфейс исполнителя запросов, который аккумулирует агрегируемые выходные данные
    /// в любом случае.
    /// </summary>
    /// <typeparam name="TQueryResult"></typeparam>
    public interface IAccumulateAnyAggregableQueryResult<TQueryResult>
        : IAccumulatePositiveAggregableQueryResult<TQueryResult>,
          IAccumulateNegativeQueryResult<TQueryResult, TQueryResult>
    { }
}
