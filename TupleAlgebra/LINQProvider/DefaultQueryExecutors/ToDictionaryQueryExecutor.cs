using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.DefaultQueryExecutors
{
    public class ToDictionaryBufferingQueryExecutor<TData, TKey, TValue>
        : QueryPipelineInfrastructure.Buffering
            .BufferingQueryExecutorWithAggregableResult<TData, Dictionary<TKey, TValue>>
        where TKey : notnull
    {
        private Func<TData, TKey> _keySelector;

        private Func<TData, TValue> _valueSelector;

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        public ToDictionaryBufferingQueryExecutor(
            Func<TData, TKey> keySelector, 
            Func<TData, TValue> valueSelector = null!)
            : base()
        {
            _keySelector = keySelector;
            _valueSelector = valueSelector ?? (new Func<TData, TData>((d) => d) as Func<TData, TValue>)!;

            return;
        }

        public override Dictionary<TKey, TValue> Execute()
        {
            return Enumerable.ToDictionary(_dataSource, _keySelector, _valueSelector);
        }
    }
}
