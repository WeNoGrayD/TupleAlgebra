using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleSystemInfrastructure
{
    using static TupleObjectInclusionComparer;

    public sealed class ConjunctiveTupleSystemInclusionComparer<TEntity>
        : TupleObjectInclusionComparer<TEntity, ConjunctiveTupleSystem<TEntity>>
        where TEntity : new()
    {
        public override bool Visit(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second)
        {
            return Includes(first, second);
        }

        public override bool Visit(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second)
        {
            return Includes(first, second);
        }

        public override bool Visit(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second)
        {
            return Includes(first, second);
        }

        public override bool Visit(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second)
        {
            return Includes(first, second);
        }
    }
}
