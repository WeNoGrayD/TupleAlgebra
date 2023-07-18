using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryResultAccumulatorInterfaces
{
    /// <summary>
    /// Интерфейс исполнителя запросов, который аккумулирует выходные данные
    /// при их непропуске.
    /// </summary>
    /// <typeparam name="TQueryResult"></typeparam>
    /// <typeparam name="TAccumulator"></typeparam>
    public interface IAccumulateNegativeQueryResult<TQueryResult, TAccumulator>
        where TAccumulator : TQueryResult
    {
        /// <summary>
        /// Событие пропуска выходных данных.
        /// </summary>
        event Action<TQueryResult> DataNotPassed;

        /// <summary>
        /// Аккумулирование выходных данных при их непропуске.
        /// </summary>
        /// <param name="accumulator"></param>
        /// <param name="outputData"></param>
        void AccumulateIfDataNotPassed(ref TAccumulator accumulator, TQueryResult outputData);
    }
}
