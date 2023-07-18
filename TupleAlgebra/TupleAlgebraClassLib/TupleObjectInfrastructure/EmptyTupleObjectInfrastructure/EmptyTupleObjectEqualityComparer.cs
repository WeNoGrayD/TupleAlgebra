using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectAcceptors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.EmptyTupleObjectInfrastructure
{
    public sealed class EmptyTupleObjectEqualityComparer<TEntity>
        : CrossTypeInstantBinaryTupleObjectAcceptor<TEntity, EmptyTupleObject<TEntity>, bool>
    {
        public override bool Accept(
            EmptyTupleObject<TEntity> first,
            EmptyTupleObject<TEntity> second)
        {
            return true;
        }

        protected override bool AcceptDefault(
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
