using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjects;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleInfrastructure
{
    using static TupleObjectInclusionComparer;

    public sealed class ConjunctiveTupleInclusionComparer<TEntity>
        : TupleObjectInclusionComparer<TEntity, ConjunctiveTuple<TEntity>>
        where TEntity : new()
    {
        public override bool Visit(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second)
        {
            return Includes(first, second);
        }

        public override bool Visit(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second)
        {
            return Includes(first, second);
        }

        public override bool Visit(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second)
        {
            return Includes(first, second);
        }

        public override bool Visit(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second)
        {
            return Includes(first, second);
        }
    }
}
