using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectAcceptors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.DisjunctiveTupleInfrastructure
{
    public sealed class DisjunctiveTupleComplementionOperator<TEntity>
        : SingleTupleObjectComplementionOperator<TEntity, DisjunctiveTuple<TEntity>>
        where TEntity : new()
    {
        public override TupleObject<TEntity> Accept(
            DisjunctiveTuple<TEntity> first,
            TupleObjectFactory factory)
        {
            return ComplementThe(first, factory);
        }

        protected override TupleObject<TEntity> CreateAlternateSingleTupleObject(
            IndexedComponentFactoryArgs<IAttributeComponent>[] components,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder)
        {
            return factory.CreateConjunctive<TEntity>(components);
        }
    }
}
