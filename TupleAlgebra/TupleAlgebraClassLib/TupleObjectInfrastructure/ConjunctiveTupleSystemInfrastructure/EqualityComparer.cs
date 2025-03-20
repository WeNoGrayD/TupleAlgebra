using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleSystemInfrastructure
{
    using static TupleObjectEqualityComparer;

    public sealed class ConjunctiveTupleSystemEqualityComparer<TEntity>
        : TupleObjectEqualityComparer<TEntity, ConjunctiveTupleSystem<TEntity>>
        where TEntity : new()
    {
        public override bool Visit(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second)
        {
            return AreEqual(first, second);
        }

        public override bool Visit(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second)
        {
            return AreEqual(first, second);
        }

        public override bool Visit(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second)
        {
            return AreEqual(second, first);
        }

        public override bool Visit(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second)
        {
            return AreEqual(first, second);
        }
    }
}
