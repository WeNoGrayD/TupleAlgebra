using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.FullTupleObjectInfrastructure
{
    public sealed class FullTupleObjectInclusionOrEqualityComparer<TEntity>
        : TupleObjectCrossTypeInstantDefaultBinaryVisitor<TEntity, FullTupleObject<TEntity>, bool>
        where TEntity : new()
    {
        public override bool Visit(
            FullTupleObject<TEntity> first,
            EmptyTupleObject<TEntity> second)
        {
            return true;
        }

        public override bool VisitDefault(
            FullTupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return true;
        }

        public override bool Visit(
            FullTupleObject<TEntity> first,
            FullTupleObject<TEntity> second)
        {
            return true;
        }
    }
}
