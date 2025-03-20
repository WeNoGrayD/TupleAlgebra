using LINQProvider.QueryPipelineInfrastructure.Buffering;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.LINQ2TAFramework.TupleObjectInfrastructure.QueryExecutors
{
    using static TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectHelper;

    public class TupleObjectAnyQueryExecutor<TEntity>
        : BufferingQueryExecutorWithAggregableResult<TEntity, bool>
        where TEntity : new()
    {
        private TupleObject<TEntity> _filter;

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        public TupleObjectAnyQueryExecutor(
            TupleObject<TEntity> filter)
            : base()
        {
            _filter = filter;
            return;
        }

        public override bool Execute()
        {
            if (_filter.IsFalse()) return false;

            return ExecuteImpl(_dataSource as TupleObject<TEntity>);
        }

        public bool ExecuteImpl(TupleObject<TEntity> dataSource)
        {
            return (dataSource as TupleObject<TEntity>).ExecuteQuery() switch
            {
                EmptyTupleObject<TEntity> => false,
                ConjunctiveTuple<TEntity> cTuple =>
                    ExecuteOverConjunctiveTuple(cTuple),
                DisjunctiveTuple<TEntity> dTuple =>
                    ExecuteOverDisjunctiveTuple(dTuple),
                ConjunctiveTupleSystem<TEntity> cSys =>
                    ExecuteOverConjunctiveTupleSystem(cSys),
                DisjunctiveTupleSystem<TEntity> dSys =>
                    ExecuteOverDisjunctiveTupleSystem(dSys),
                FullTupleObject<TEntity> => true,
                QueriedTupleObject<TEntity> => ExecuteImpl(dataSource.ExecuteQuery()),
                _ => false
            };
        }

        private bool ExecuteOverConjunctiveTuple(ConjunctiveTuple<TEntity> cTuple)
        {
            return ConjunctiveTupleObjectMatchAny(cTuple, _filter);
        }

        private bool ExecuteOverDisjunctiveTuple(DisjunctiveTuple<TEntity> dTuple)
        {
            return DisjunctiveTupleObjectMatchAny(dTuple, _filter);
        }

        private bool ExecuteOverConjunctiveTupleSystem(
            ConjunctiveTupleSystem<TEntity> cSys)
        {
            return ConjunctiveTupleObjectMatchAny(cSys, _filter);
        }

        private bool ExecuteOverDisjunctiveTupleSystem(
            DisjunctiveTupleSystem<TEntity> dSys)
        {
            return DisjunctiveTupleObjectMatchAny(dSys, _filter);
        }
    }
}
