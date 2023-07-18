using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectAcceptors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.EmptyTupleObjectInfrastructure
{
    public sealed class EmptyTupleObjectIntersectionOperator<TEntity>
        : CrossTypeInstantBinaryTupleObjectAcceptor<TEntity, EmptyTupleObject<TEntity>, TupleObject<TEntity>>
    {
        public override TupleObject<TEntity> Accept(
            EmptyTupleObject<TEntity> first,
            EmptyTupleObject<TEntity> second)
        {
            return first;
        }

        protected override TupleObject<TEntity> AcceptDefault(
            EmptyTupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return first;
        }

        public override TupleObject<TEntity> Accept(
            EmptyTupleObject<TEntity> first,
            FullTupleObject<TEntity> second)
        {
            return first;
        }
    }
}
