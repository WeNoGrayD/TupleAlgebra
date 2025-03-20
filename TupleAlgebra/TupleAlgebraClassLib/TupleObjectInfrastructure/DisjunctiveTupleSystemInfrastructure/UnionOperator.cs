using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.DisjunctiveTupleSystemInfrastructure
{
    using static TupleObjectUnionOperator;

    public sealed class DisjunctiveTupleSystemUnionOperator<TEntity>
        : TupleObjectUnionOperator<TEntity, DisjunctiveTupleSystem<TEntity>>
        where TEntity : new()
    {
        public override TupleObject<TEntity> Visit(
            DisjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
        {
            return Union(first, second, factory);
        }

        public override TupleObject<TEntity> Visit(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
        {
            return Union(first, second, factory);
        }

        public override TupleObject<TEntity> Visit(
            DisjunctiveTupleSystem<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
        {
            return Union(second, first, factory);
        }

        public override TupleObject<TEntity> Visit(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
        {
            return Union(first, second, factory);
        }
    }
}
