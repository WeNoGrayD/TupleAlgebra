using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryResultAccumulatorInterfaces
{
    /// <summary>
    /// Интерфейс исполнителя запросов, который аккумулирует выходные данные
    /// при их пропуске.
    /// </summary>
    /// <typeparam name="TQueryResult"></typeparam>
    /// <typeparam name="TAccumulator"></typeparam>
    public interface IAccumulatePositiveQueryResult<TQueryResult, TAccumulator> //: IQueryPipelineMiddleware
        where TAccumulator : TQueryResult
    {
        /// <summary>
        /// Событие пропуска выходных данных.
        /// </summary>
        event Action<TQueryResult> DataPassed;

        /// <summary>
        /// Инициализация аккумулятора значением по умолчанию.
        /// </summary>
        /// <returns></returns>
        TAccumulator InitAccumulator();

        /// <summary>
        /// Инициализация аккумулятора выходными данными.
        /// </summary>
        /// <param name="initialAccumulatorValue"></param>
        /// <returns></returns>
        TAccumulator InitAccumulator(TQueryResult outputData);

        /// <summary>
        /// Аккумулирование выходных данных при их пропуске.
        /// </summary>
        /// <param name="accumulator"></param>
        /// <param name="outputData"></param>
        void AccumulateIfDataPassed(ref TAccumulator accumulator, TQueryResult outputData);
    }
}
