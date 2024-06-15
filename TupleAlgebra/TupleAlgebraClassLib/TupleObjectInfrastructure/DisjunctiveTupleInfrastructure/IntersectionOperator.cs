using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.DisjunctiveTupleInfrastructure
{
    using static TupleObjectIntersectionOperations;

    public sealed class DisjunctiveTupleIntersectionOperator<TEntity>
        : TupleObjectIntersectionOperator<TEntity, DisjunctiveTuple<TEntity>>
        where TEntity : new()
    {
        public override TupleObject<TEntity> Visit(
            DisjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
        {
            return OperationStrategy(first, second, factory, Intersect);
        }

        public override TupleObject<TEntity> Visit(
            DisjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
        {
            return OperationStrategy(first, second, factory, 
                new OperationHandler<DisjunctiveTuple<TEntity>>(Intersect));
        }

        public override TupleObject<TEntity> Visit(
            DisjunctiveTuple<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
        {
            return OperationStrategy(first, second, factory, Intersect);
        }

        public override TupleObject<TEntity> Visit(
            DisjunctiveTuple<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
        {
            return Intersect(second, first, factory);
        }
    }
}
