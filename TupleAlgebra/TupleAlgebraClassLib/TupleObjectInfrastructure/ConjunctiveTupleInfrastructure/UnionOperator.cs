using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleInfrastructure
{
    using static TupleObjectUnionOperator;

    public sealed class ConjunctiveTupleUnionOperator<TEntity>
        : TupleObjectUnionOperator<TEntity, ConjunctiveTuple<TEntity>>
        where TEntity : new()
    {
        public override TupleObject<TEntity> Visit(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
        {
            return Union(first, second, factory);
        }

        public override TupleObject<TEntity> Visit(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
        {
            return Union(first, second, factory);
        }

        public override TupleObject<TEntity> Visit(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
        {
            return Union(second, first, factory);
        }

        public override TupleObject<TEntity> Visit(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
        {
            return Union(second, first, factory);
        }
    }
}
