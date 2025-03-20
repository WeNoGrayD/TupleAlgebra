using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.DisjunctiveTupleInfrastructure
{
    using static TupleObjectSymmetricExceptionOperator;

    public sealed class DisjunctiveTupleSymmetricExceptionOperator<TEntity>
        : TupleObjectSymmetricExceptionOperator<TEntity, DisjunctiveTuple<TEntity>>
        where TEntity : new()
    {
        public override TupleObject<TEntity> Visit(
            DisjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
        {
            return SymmetricExcept(second, first, factory);
        }

        public override TupleObject<TEntity> Visit(
            DisjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
        {
            return SymmetricExcept(first, second, factory);
        }

        public override TupleObject<TEntity> Visit(
            DisjunctiveTuple<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
        {
            return SymmetricExcept(second, first, factory);
        }

        public override TupleObject<TEntity> Visit(
            DisjunctiveTuple<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
        {
            return SymmetricExcept(second, first, factory);
        }
    }
}
