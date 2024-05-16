using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectAcceptors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleInfrastructure
{
    public sealed class ConjunctiveTupleUnionOperator<TEntity>
        : TupleObjectCrossTypeFactorySetBinaryOperator<TEntity, ConjunctiveTuple<TEntity>>
        where TEntity : new()
    {
        public override TupleObject<TEntity> Accept(
            ConjunctiveTuple<TEntity> first,
            EmptyTupleObject<TEntity> second,
            TupleObjectFactory factory)
        {
            return default;
        }

        public override TupleObject<TEntity> Accept(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
        {
            return default;
        }

        public override TupleObject<TEntity> Accept(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
        {
            return default;
        }

        public override TupleObject<TEntity> Accept(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
        {
            return default;
        }

        public override TupleObject<TEntity> Accept(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
        {
            return default;
        }

        public override TupleObject<TEntity> Accept(
            ConjunctiveTuple<TEntity> first,
            FullTupleObject<TEntity> second,
            TupleObjectFactory factory)
        {
            return default;
        }
    }
}
