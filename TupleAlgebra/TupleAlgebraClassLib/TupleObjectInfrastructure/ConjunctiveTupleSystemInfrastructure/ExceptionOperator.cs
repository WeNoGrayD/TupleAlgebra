using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleSystemInfrastructure
{
    using static TupleObjectExceptionOperator;

    public sealed class ConjunctiveTupleSystemExceptionOperator<TEntity>
        : TupleObjectExceptionOperator<TEntity, ConjunctiveTupleSystem<TEntity>>
        where TEntity : new()
    {
        public override TupleObject<TEntity> Visit(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
        {
            return Except(first, second, factory);
        }

        public override TupleObject<TEntity> Visit(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
        {
            return Except(first, second, factory);
        }

        public override TupleObject<TEntity> Visit(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
        {
            return Except(first, second, factory);
        }

        public override TupleObject<TEntity> Visit(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
        {
            return Except(first, second, factory);
        }
    }
}
