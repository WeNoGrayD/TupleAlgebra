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
        : CrossTypeInstantBinaryTupleObjectAcceptor<TEntity, FullTupleObject<TEntity>, bool>,
          IInstantBinaryTupleObjectAcceptor<TEntity, FullTupleObject<TEntity>, ConjunctiveTuple<TEntity>, bool>,
          IInstantBinaryTupleObjectAcceptor<TEntity, FullTupleObject<TEntity>, ConjunctiveTupleSystem<TEntity>, bool>
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

        public bool Accept(
            FullTupleObject<TEntity> first,
            ConjunctiveTuple<TEntity> second)
        {
            return false;
        }

        public bool Accept(
            FullTupleObject<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second)
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
