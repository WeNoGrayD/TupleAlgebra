using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleSystemInfrastructure
{
    using static TupleObjectInclusionOrEqualityComparer;

    public sealed class ConjunctiveTupleSystemInclusionOrEqualityComparer<TEntity>
        : TupleObjectInclusionOrEqualityComparer<TEntity, ConjunctiveTupleSystem<TEntity>>
        where TEntity : new()
    {
        public override bool Visit(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second)
        {
            return IncludesOrAreEqual(first, second);
        }

        public override bool Visit(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second)
        {
            return IncludesOrAreEqual(first, second);
        }

        public override bool Visit(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second)
        {
            return IncludesOrAreEqual(first, second);
        }

        public override bool Visit(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second)
        {
            return IncludesOrAreEqual(first, second);
        }
    }
}
