using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleInfrastructure;
using UniversalClassLib;

namespace TupleAlgebraClassLib.TupleObjects
{
    using static TupleObjectHelper;
    using static TupleObjectStaticDataStorage;
    using static CartesianProductHelper;

    public class ConjunctiveTuple<TEntity> : SingleTupleObject<TEntity>
        where TEntity : new()
    {
        #region Constructors

        static ConjunctiveTuple()
        {
            Storage.RegisterType<TEntity, ConjunctiveTuple<TEntity>>(
                (factory) => new ConjunctiveTupleOperationExecutorsContainer(factory));

            return;
        }

        public ConjunctiveTuple(TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(onTupleBuilding)
        { }

        public ConjunctiveTuple(TupleObjectSchema<TEntity> schema)
            : base(schema)
        { }

        #endregion

        #region Instance methods

        public override bool IsDefault(IAttributeComponent component)
        {
            return component.IsFull;
        }

        public override IAttributeComponent<TAttribute>
            GetDefaultFictionalAttributeComponent<TAttribute>(
                IAttributeComponentFactory<TAttribute> factory)
        {
            return factory.CreateFull();
        }

        public override TupleObject<TEntity> Reproduce(
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> components,
            TupleObjectFactory factory,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
        {
            return factory.CreateConjunctiveTuple(
                components, 
                onTupleBuilding, 
                builder);
        }

        #endregion

        protected override IEnumerator<TEntity> GetEnumeratorImpl()
        {
            /*
             * В предположении, что в схему включен хотя бы один атрибут.
             * Если тип сущности является примитивным, то единственная компонента
             * содержит перечисление сущностей.
             * Иначе производится генерация сущностей с прикреплёнными свойствами-атрибутами.
             */
            return TupleObjectSchema<TEntity>.IsEntityPrimitive ?
                GetPrimitiveEnumerator<IAttributeComponent, TEntity>(_components) :
                GetCartesianProductEnumeratorWithBuffering(
                    Schema.EntityFactory, 
                    _components, 
                    (c) => c.Power);
        }

        #region Nested types

        public class ConjunctiveTupleOperationExecutorsContainer
            : NonFictionalTupleObjectOperationExecutorsContainer<ConjunctiveTuple<TEntity>>
        {
            #region Constructors

            public ConjunctiveTupleOperationExecutorsContainer(
                TupleObjectFactory factory)
                : base(factory,
                       () => new ConjunctiveTupleComplementionOperator<TEntity>(),
                       () => new ConjunctiveTupleConversionToAlternateOperator<TEntity>(),
                       () => new ConjunctiveTupleIntersectionOperator<TEntity>(),
                       () => new ConjunctiveTupleUnionOperator<TEntity>(),
                       () => new ConjunctiveTupleExceptionOperator<TEntity>(),
                       () => new ConjunctiveTupleSymmetricExceptionOperator<TEntity>(),
                       () => new ConjunctiveTupleInclusionComparer<TEntity>(),
                       () => new ConjunctiveTupleEqualityComparer<TEntity>(),
                       () => new ConjunctiveTupleInclusionOrEqualityComparer<TEntity>())
            {
                return;
            }

            #endregion
        }

        #endregion
    }
}
