using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryResultAccumulatorInterfaces
{
    /// <summary>
    /// Интерфейс исполнителя запросов, который аккумулирует перечислимые выходные данные
    /// в любом случае.
    /// </summary>
    /// <typeparam name="TQueryResultData"></typeparam>
    public interface IAccumulateAnyEnumerableQueryResult<TQueryResultData>
        : IAccumulatePositiveEnumerableQueryResult<TQueryResultData>,
          IAccumulateNegativeQueryResult<IEnumerable<TQueryResultData>, IEnumerable<TQueryResultData>>
    { }
}
