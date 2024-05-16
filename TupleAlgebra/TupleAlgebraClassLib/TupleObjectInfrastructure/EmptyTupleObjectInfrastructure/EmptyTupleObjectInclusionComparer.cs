using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectAcceptors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.EmptyTupleObjectInfrastructure
{
    public sealed class EmptyTupleObjectInclusionComparer<TEntity>
        : TupleObjectCrossTypeInstantBinaryAcceptor<TEntity, EmptyTupleObject<TEntity>, bool>
        where TEntity : new()
    {
        public override bool Accept(
            EmptyTupleObject<TEntity> first,
            EmptyTupleObject<TEntity> second)
        {
            return false;
        }

        public override bool AcceptDefault(
            EmptyTupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return false;
        }

        public override bool Accept(
            EmptyTupleObject<TEntity> first,
            FullTupleObject<TEntity> second)
        {
            return false;
        }
    }
}
