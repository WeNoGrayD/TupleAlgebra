using LINQProvider.QueryPipelineInfrastructure.Buffering;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.LINQ2TAFramework.TupleObjectInfrastructure.QueryExecutors
{
    using static TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectHelper;

    public class TupleObjectAllQueryExecutor<TEntity>
        : BufferingQueryExecutorWithAggregableResult<TEntity, bool>
        where TEntity : new()
    {
        private TupleObject<TEntity> _filter;

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        public TupleObjectAllQueryExecutor(
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
            return ConjunctiveTupleObjectMatchAll(cTuple, _filter);
        }

        private bool ExecuteOverDisjunctiveTuple(DisjunctiveTuple<TEntity> dTuple)
        {
            return DisjunctiveTupleObjectMatchAll(dTuple, _filter);
        }

        private bool ExecuteOverConjunctiveTupleSystem(
            ConjunctiveTupleSystem<TEntity> cSys)
        {
            return ConjunctiveTupleObjectMatchAll(cSys, _filter);
        }

        private bool ExecuteOverDisjunctiveTupleSystem(
            DisjunctiveTupleSystem<TEntity> dSys)
        {
            return DisjunctiveTupleObjectMatchAll(dSys, _filter);
        }
    }
}
