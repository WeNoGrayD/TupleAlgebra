using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.DisjunctiveTupleInfrastructure
{
    using static TupleObjectInclusionOrEqualityComparer;

    public sealed class DisjunctiveTupleInclusionOrEqualityComparer<TEntity>
        : TupleObjectInclusionOrEqualityComparer<TEntity, DisjunctiveTuple<TEntity>>
        where TEntity : new()
    {
        public override bool Visit(
            DisjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second)
        {
            return IncludesOrAreEqual(first, second);
        }

        public override bool Visit(
            DisjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second)
        {
            return IncludesOrAreEqual(first, second);
        }

        public override bool Visit(
            DisjunctiveTuple<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second)
        {
            return IncludesOrAreEqual(first, second);
        }

        public override bool Visit(
            DisjunctiveTuple<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second)
        {
            return IncludesOrAreEqual(first, second);
        }
    }
}
