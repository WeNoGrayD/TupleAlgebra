using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectAcceptors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.FullTupleObjectInfrastructure
{
    public sealed class FullTupleObjectInclusionOrEqualityComparer<TEntity>
        : CrossTypeInstantBinaryTupleObjectAcceptor<TEntity, FullTupleObject<TEntity>, bool>
    {
        public override bool Accept(
            FullTupleObject<TEntity> first,
            EmptyTupleObject<TEntity> second)
        {
            return false;
        }

        protected override bool AcceptDefault(
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
