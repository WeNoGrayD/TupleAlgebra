using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectAcceptors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    /*
    public abstract class SingleTupleObjectComplementionOperator<TEntity, TOperand>
        : TupleObjectFactoryUnarySetOperator<TEntity, TOperand>
        where TEntity : new()
        where TOperand : SingleTupleObject<TEntity>
    {
        protected TupleObject<TEntity> ComplementThe(
            TOperand first, 
            TupleObjectFactory factory)
        {
            int len = first.RowLength;
            TupleObjectBuilder<TEntity> builder = factory.GetBuilder(first.Schema);
            IndexedComponentFactoryArgs<IAttributeComponent>[] components =
                new IndexedComponentFactoryArgs<IAttributeComponent>[len];

            for (int i = 0; i < len; i++)
            {
                components[i] = new (
                    i, 
                    builder,
                    first[i].ComplementThe());
            }

            return CreateAlternateSingleTupleObject(components, factory, builder);
        }

        protected abstract TupleObject<TEntity> CreateAlternateSingleTupleObject(
            IndexedComponentFactoryArgs<IAttributeComponent>[] components,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder);
    }
    */
}
