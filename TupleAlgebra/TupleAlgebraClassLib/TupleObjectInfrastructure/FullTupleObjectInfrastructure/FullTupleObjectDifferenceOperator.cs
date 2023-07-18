using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectAcceptors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.FullTupleObjectInfrastructure
{
    public sealed class FullTupleObjectDifferenceOperator<TEntity>
        : CrossTypeInstantBinaryTupleObjectAcceptor<TEntity, FullTupleObject<TEntity>, TupleObject<TEntity>>
    {
        public override TupleObject<TEntity> Accept(
            FullTupleObject<TEntity> first,
            EmptyTupleObject<TEntity> second)
        {
            return second;
        }

        protected override TupleObject<TEntity> AcceptDefault(
            FullTupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return !second;
        }

        public override TupleObject<TEntity> Accept(
            FullTupleObject<TEntity> first,
            FullTupleObject<TEntity> second)
        {
            return null;
        }
    }
}
