using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleSystemInfrastructure;

namespace TupleAlgebraClassLib.TupleObjects
{
    using static TupleObjectHelper;
    using static TupleObjectStaticDataStorage;

    public class ConjunctiveTupleSystem<TEntity> 
        : TupleObjectSystem<TEntity, ConjunctiveTuple<TEntity>>
        where TEntity : new()
    {
        static ConjunctiveTupleSystem()
        {
            Storage.RegisterType<TEntity, ConjunctiveTupleSystem<TEntity>>(
                (factory) => new ConjunctiveTupleSystemOperationExecutorsContainer(factory));

            return;
        }

        public ConjunctiveTupleSystem(
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(onTupleBuilding)
        { }

        public ConjunctiveTupleSystem(
            TupleObjectSchema<TEntity> schema,
            int len)
            : base(schema, len)
        { }

        public ConjunctiveTupleSystem(
            TupleObjectSchema<TEntity> schema,
            IList<ISingleTupleObject> tuples)
            : base(schema, tuples)
        { }

        public override IAttributeComponent<TAttribute>
            GetDefaultFictionalAttributeComponent<TAttribute>(
                IAttributeComponentFactory<TAttribute> factory)
        {
            return factory.CreateFull();
        }

        public override TupleObject<TEntity> Reproduce(
            IEnumerable<TupleObject<TEntity>> tuples,
            TupleObjectFactory factory,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
        {
            return factory.CreateConjunctiveTupleSystem(
                tuples, 
                onTupleBuilding, 
                builder);
        }

        /*
        protected override void OnAttributeAttached(object sender, AttributeChangedEventArgs eventArgs)
        {
            base.OnAttributeAttached(sender, eventArgs);
            //IsDiagonal = false;

            return;
        }
        */

        /*
        /// <summary>
        /// Обработчик события элиминации атрибута.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected override void OnAttributeDetached(object sender, AttributeChangedEventArgs eventArgs)
        {
            // 
            // * Если C-система диагональная и не ортогональная, то она может содержать C-кортеж
            // * с полными фиктивными компонентами везде, кроме элиминируемого атрибута.
            // * При элиминации атрибута в этом C-кортеже получается кортеж,
            // * состоящий полностью из фиктивных атрибутов, поэтому такой кортеж требуется
            // * удалить.
            // 
            if (IsDiagonal && !IsOrthogonal) RemoveTupleOnDetachedAttribute();
            //
            // * И лишь после удаления такого кортежа возможно элиминировать атрибут.
            // 
            base.OnAttributeDetached(sender, eventArgs);

            return;

            void RemoveTupleOnDetachedAttribute()
            {
                int detachedAttrLoc = Schema.GetAttributeLoc(eventArgs.Attribute);
                SingleTupleObject<TEntity> cTuple;

                for (int i = 0; i < _tuples.Length; i++)
                {
                    if ((cTuple = _tuples[i]) is null || 
                        cTuple[detachedAttrLoc].IsFull()) continue;

                    _tuples[i] = null;
                    break;
                }
            }
        }
        */

        protected override IEnumerator<TEntity> GetEnumeratorImpl()
        {
            for (int i = 0; i < _tuples.Length; i++)
            {
                foreach (TEntity entity in _tuples[i]) yield return entity;
            }

            yield break;
        }

        public TupleObject<TEntity> TrueUnion()
        {
            return (SetOperations as IConjunctiveTupleSystemOperationExecutorsContainer)!
                .TrueUnion(this);
        }

        #region Nested types

        public interface IConjunctiveTupleSystemOperationExecutorsContainer
        {
            public TupleObject<TEntity> TrueUnion(
                ConjunctiveTupleSystem<TEntity> first);
        }

        public class ConjunctiveTupleSystemOperationExecutorsContainer
            : NonFictionalTupleObjectOperationExecutorsContainer<ConjunctiveTupleSystem<TEntity>>,
              IConjunctiveTupleSystemOperationExecutorsContainer
        {
            private Lazy<ConjunctiveTupleSystemTrueUnionOperator<TEntity>>
                _trueUnionOperator;

            public ConjunctiveTupleSystemTrueUnionOperator<TEntity>
                TrueUnionOperator => _trueUnionOperator.Value;

            #region Constructors

            public ConjunctiveTupleSystemOperationExecutorsContainer(
                TupleObjectFactory factory)
                : base(factory,
                       () => new ConjunctiveTupleSystemComplementionOperator<TEntity>(),
                       () => new ConjunctiveTupleSystemConversionToAlternateOperator<TEntity>(),
                       () => new ConjunctiveTupleSystemIntersectionOperator<TEntity>(),
                       () => new ConjunctiveTupleSystemUnionOperator<TEntity>(),
                       () => new ConjunctiveTupleSystemExceptionOperator<TEntity>(),
                       () => new ConjunctiveTupleSystemSymmetricExceptionOperator<TEntity>(),
                       () => new ConjunctiveTupleSystemInclusionComparer<TEntity>(),
                       () => new ConjunctiveTupleSystemEqualityComparer<TEntity>(),
                       () => new ConjunctiveTupleSystemInclusionOrEqualityComparer<TEntity>())
            {
                _trueUnionOperator = 
                    new Lazy<ConjunctiveTupleSystemTrueUnionOperator<TEntity>>();

                return;
            }

            #endregion

            public TupleObject<TEntity> TrueUnion(
                ConjunctiveTupleSystem<TEntity> first)
            {
                return TrueUnionOperator
                    .Union(first, Factory);
            }
        }

        #endregion
    }
}
