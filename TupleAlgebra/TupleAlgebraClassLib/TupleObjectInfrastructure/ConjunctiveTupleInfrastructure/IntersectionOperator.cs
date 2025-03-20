using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleInfrastructure
{
    using static TupleObjectIntersectionOperations;

    public sealed class ConjunctiveTupleIntersectionOperator<TEntity>
        : TupleObjectIntersectionOperator<TEntity, ConjunctiveTuple<TEntity>>
        where TEntity : new()
    {
        public override TupleObject<TEntity> Visit(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
        {
            return OperationStrategy(first, second, factory, 
                new OperationHandler<ConjunctiveTuple<TEntity>>(Intersect));
        }

        public override TupleObject<TEntity> Visit(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
        {
            return OperationStrategy(first, second, factory, Intersect);
        }

        public override TupleObject<TEntity> Visit(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
        {
            return OperationStrategy(first, second, factory, Intersect);
        }

        public override TupleObject<TEntity> Visit(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
        {
            return Intersect(second, first, factory);
        }
    }
}
