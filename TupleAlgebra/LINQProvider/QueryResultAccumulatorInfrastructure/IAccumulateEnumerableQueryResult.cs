﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryResultAccumulatorInfrastructure
{
    /// <summary>
    /// Интерфейс исполнителя запросов, который аккумулирует перечислимые выходные данные
    /// при их пропуске.
    /// </summary>
    /// <typeparam name="TQueryResultData"></typeparam>
    public interface IAccumulateEnumerableQueryResult<TQueryResultData>
        : IAccumulateQueryResult<IEnumerable<TQueryResultData>, IEnumerable<TQueryResultData>>
    {
        /// <summary>
        /// Инициализация аккумулятора значением по умолчанию - пустым перечислением.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TQueryResultData> IAccumulateQueryResult<IEnumerable<TQueryResultData>, IEnumerable<TQueryResultData>>
            .InitAccumulator() => Enumerable.Empty<TQueryResultData>();

        /// <summary>
        /// Инициализация аккумулятора выходными данными.
        /// </summary>
        /// <param name="initialAccumulatorValue"></param>
        /// <returns></returns>
        IEnumerable<TQueryResultData> IAccumulateQueryResult<IEnumerable<TQueryResultData>, IEnumerable<TQueryResultData>>
            .InitAccumulator(IEnumerable<TQueryResultData> initialAccumulatorValue) => initialAccumulatorValue;
    }
}
