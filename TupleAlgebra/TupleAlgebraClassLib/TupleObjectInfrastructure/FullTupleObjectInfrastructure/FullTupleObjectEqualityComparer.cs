using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.FullTupleObjectInfrastructure
{
    public sealed class FullTupleObjectEqualityComparer<TEntity>
        : TupleObjectCrossTypeInstantDefaultBinaryVisitor<TEntity, FullTupleObject<TEntity>, bool>
        where TEntity : new()
    {
        public override bool Visit(
            FullTupleObject<TEntity> first,
            EmptyTupleObject<TEntity> second)
        {
            return false;
        }

        public override bool VisitDefault(
            FullTupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return false;
        }

        public override bool Visit(
            FullTupleObject<TEntity> first,
            FullTupleObject<TEntity> second)
        {
            return true;
        }
    }
}
