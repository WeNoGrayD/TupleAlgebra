﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.DisjunctiveTupleSystemInfrastructure
{
    using static TupleObjectIntersectionOperations;

    public sealed class DisjunctiveTupleSystemIntersectionOperator<TEntity>
        : TupleObjectIntersectionOperator<TEntity, DisjunctiveTupleSystem<TEntity>>
        where TEntity : new()
    {
        public override TupleObject<TEntity> Visit(
            DisjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
        {
            return OperationStrategy(first, second, factory, Intersect);
        }

        public override TupleObject<TEntity> Visit(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
        {
            return OperationStrategy(first, second, factory, Intersect);
        }

        public override TupleObject<TEntity> Visit(
            DisjunctiveTupleSystem<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
        {
            return OperationStrategy(first, second, factory, Intersect);
        }

        public override TupleObject<TEntity> Visit(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
        {
            return OperationStrategy(first, second, factory,
                new OperationHandler<DisjunctiveTupleSystem<TEntity>>(Intersect));
        }
    }
}
