using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectAcceptors;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectAcceptors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.FullTupleObjectInfrastructure
{
    public sealed class FullTupleObjectEqualityComparer<TEntity>
        : TupleObjectCrossTypeInstantBinaryAcceptor<TEntity, FullTupleObject<TEntity>, bool>,
          ITupleObjectInstantBinaryAcceptor<TEntity, FullTupleObject<TEntity>, ConjunctiveTuple<TEntity>, bool>,
          ITupleObjectInstantBinaryAcceptor<TEntity, FullTupleObject<TEntity>, ConjunctiveTupleSystem<TEntity>, bool>
        where TEntity : new()
    {
        public override bool Accept(
            FullTupleObject<TEntity> first,
            EmptyTupleObject<TEntity> second)
        {
            return false;
        }

        public override bool AcceptDefault(
            FullTupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return false;
        }

        public override bool Accept(
            FullTupleObject<TEntity> first,
            FullTupleObject<TEntity> second)
        {
            return true;
        }
    }
}
