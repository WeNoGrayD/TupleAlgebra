using LINQProvider.QueryPipelineInfrastructure.Buffering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.LINQ2TAFramework.TupleObjectInfrastructure.QueryExecutors
{
    public class TupleObjectWhereQueryExecutor<TEntity>
        : BufferingQueryExecutorWithAggregableResult<TEntity, TupleObject<TEntity>>
        where TEntity : new()
    {
        private TupleObject<TEntity> _filter;

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        public TupleObjectWhereQueryExecutor(
            TupleObject<TEntity> filter)
            : base()
        {
            _filter = filter;

            return;
        }

        public override TupleObject<TEntity> Execute()
        {
            return (_dataSource as TupleObject<TEntity>) & _filter;
        }
    }
}
