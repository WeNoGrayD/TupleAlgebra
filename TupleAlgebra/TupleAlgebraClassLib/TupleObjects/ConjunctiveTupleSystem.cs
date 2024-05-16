using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure;

namespace TupleAlgebraClassLib.TupleObjects
{
    using static TupleObjectHelper;

    public class ConjunctiveTupleSystem<TEntity> 
        : TupleObjectSystem<TEntity, ConjunctiveTuple<TEntity>>
        where TEntity : new()
    {
        public bool IsDiagonal { get; set; }

        public bool IsOrthogonal { get; set; }

        public ConjunctiveTupleSystem(TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(onTupleBuilding)
        { }

        public ConjunctiveTupleSystem(
            TupleObjectSchema<TEntity> schema,
            int len)
            : base(schema, len)
        { }

        public ConjunctiveTupleSystem(
            TupleObjectSchema<TEntity> schema,
            IList<SingleTupleObject<TEntity>> tuples)
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
            return factory.CreateConjunctive(tuples, onTupleBuilding, builder);
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

        public override TupleObject<TEntity> Convert(TupleObject<TEntity> diagonal)
        {
            throw new NotImplementedException();
        }

        public override TupleObject<TEntity> Diagonal()
        {
            throw new NotImplementedException();
        }
    }
}
