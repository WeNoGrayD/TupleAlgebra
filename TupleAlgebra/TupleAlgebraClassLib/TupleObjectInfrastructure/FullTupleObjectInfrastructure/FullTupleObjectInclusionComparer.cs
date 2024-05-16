using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectAcceptors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.FullTupleObjectInfrastructure
{
    public sealed class FullTupleObjectInclusionComparer<TEntity>
        : TupleObjectCrossTypeInstantBinaryAcceptor<TEntity, FullTupleObject<TEntity>, bool>
        where TEntity : new()
    {
        public override bool Accept(
            FullTupleObject<TEntity> first,
            EmptyTupleObject<TEntity> second)
        {
            return true;
        }

        public override bool AcceptDefault(
            FullTupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return true;
        }

        public override bool Accept(
            FullTupleObject<TEntity> first,
            FullTupleObject<TEntity> second)
        {
            return false;
        }
    }
}
