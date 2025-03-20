using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.EmptyTupleObjectInfrastructure
{
    public sealed class EmptyTupleObjectSymmetricExceptionOperator<TEntity>
        : TupleObjectCrossTypeInstantDefaultBinaryVisitor<TEntity, EmptyTupleObject<TEntity>, TupleObject<TEntity>>
        where TEntity : new()
    {
        public override TupleObject<TEntity> Visit(
            EmptyTupleObject<TEntity> first,
            EmptyTupleObject<TEntity> second)
        {
            return first;
        }

        public override TupleObject<TEntity> VisitDefault(
            EmptyTupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return second;
        }

        public override TupleObject<TEntity> Visit(
            EmptyTupleObject<TEntity> first,
            FullTupleObject<TEntity> second)
        {
            return second;
        }
    }
}
