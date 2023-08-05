using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryResultAccumulatorInfrastructure
{
    /// <summary>
    /// Интерфейс исполнителя запросов, который аккумулирует выходные данные
    /// </summary>
    /// <typeparam name="TQueryResult"></typeparam>
    /// <typeparam name="TAccumulator"></typeparam>
    public interface IAccumulateQueryResult<TQueryResult, out TAccumulator> 
        : QueryPipelineInfrastructure.IQueryMultiplicityFactor
        where TAccumulator : TQueryResult
    {
        TAccumulator Acc { get; }

        public delegate TAccumulator AccumulateHandler(TQueryResult data);

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

        void Accumulate(TQueryResult outputData);
    }
}
