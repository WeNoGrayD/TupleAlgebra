using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.EmptyTupleObjectInfrastructure
{
    public sealed class EmptyTupleObjectInclusionComparer<TEntity>
        : TupleObjectCrossTypeInstantDefaultBinaryVisitor<TEntity, EmptyTupleObject<TEntity>, bool>
        where TEntity : new()
    {
        public override bool Visit(
            EmptyTupleObject<TEntity> first,
            EmptyTupleObject<TEntity> second)
        {
            return false;
        }

        public override bool VisitDefault(
            EmptyTupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return false;
        }

        public override bool Visit(
            EmptyTupleObject<TEntity> first,
            FullTupleObject<TEntity> second)
        {
            return false;
        }
    }
}
