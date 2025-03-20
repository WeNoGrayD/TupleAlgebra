using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.DisjunctiveTupleSystemInfrastructure
{
    using static TupleObjectInclusionComparer;

    public sealed class DisjunctiveTupleSystemInclusionComparer<TEntity>
        : TupleObjectInclusionComparer<TEntity, DisjunctiveTupleSystem<TEntity>>
        where TEntity : new()
    {
        public override bool Visit(
            DisjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second)
        {
            return Includes(first, second);
        }

        public override bool Visit(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second)
        {
            return Includes(first, second);
        }

        public override bool Visit(
            DisjunctiveTupleSystem<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second)
        {
            return Includes(first, second);
        }

        public override bool Visit(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second)
        {
            return Includes(first, second);
        }
    }
}
