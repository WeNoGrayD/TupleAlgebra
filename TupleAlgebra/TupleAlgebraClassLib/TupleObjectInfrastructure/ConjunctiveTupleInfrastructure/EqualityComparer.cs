using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjects;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleInfrastructure
{
    using static TupleObjectEqualityComparer;

    public sealed class ConjunctiveTupleEqualityComparer<TEntity>
        : TupleObjectEqualityComparer<TEntity, ConjunctiveTuple<TEntity>>
        where TEntity : new()
    {
        public override bool Visit(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second)
        {
            return AreEqual(first, second);
        }

        public override bool Visit(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second)
        {
            return AreEqual(first, second);
        }

        public override bool Visit(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second)
        {
            return AreEqual(second, first);
        }

        public override bool Visit(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second)
        {
            return AreEqual(second, first);
        }
    }
}
