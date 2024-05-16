using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectAcceptors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleInfrastructure
{
    public sealed class ConjunctiveTupleEqualityComparer<TEntity>
        : TupleObjectCrossTypeInstantBinaryAcceptor<TEntity, ConjunctiveTuple<TEntity>, bool>
        where TEntity : new()
    {
        public override bool Accept(
            ConjunctiveTuple<TEntity> first,
            EmptyTupleObject<TEntity> second)
        {
            return false;
        }

        public override bool AcceptDefault(
            ConjunctiveTuple<TEntity> first,
            TupleObject<TEntity> second)
        {
            return false;
        }

        public override bool Accept(
            ConjunctiveTuple<TEntity> first,
            FullTupleObject<TEntity> second)
        {
            return false;
        }
    }
}
