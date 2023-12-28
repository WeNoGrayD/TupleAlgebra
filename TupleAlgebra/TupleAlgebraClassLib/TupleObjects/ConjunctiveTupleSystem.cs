using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure;

namespace TupleAlgebraClassLib.TupleObjects
{
    public class ConjunctiveTupleSystem<TEntity> : TupleObjectSystem<TEntity>
        where TEntity : new()
    {
        public bool IsDiagonal { get; set; }

        public bool IsOrthogonal { get; set; }

        public ConjunctiveTupleSystem(Action<TupleObjectBuilder<TEntity>> onTupleBuilding = null)
            : base(onTupleBuilding)
        { }

        /*
        protected override void OnAttributeAttached(object sender, AttributeChangedEventArgs eventArgs)
        {
            base.OnAttributeAttached(sender, eventArgs);
            //IsDiagonal = false;

            return;
        }
        */

        /// <summary>
        /// Обработчик события элиминации атрибута.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        protected override void OnAttributeDetached(object sender, AttributeChangedEventArgs eventArgs)
        {
            /*
             * Если C-система диагональная и не ортогональная, то она может содержать C-кортеж
             * с полными фиктивными компонентами везде, кроме элиминируемого атрибута.
             * При элиминации атрибута в этом C-кортеже получается кортеж,
             * состоящий полностью из фиктивных атрибутов, поэтому такой кортеж требуется
             * удалить.
             */
            if (IsDiagonal && !IsOrthogonal) RemoveTupleOnDetachedAttribute();
            /*
             * И лишь после удаления такого кортежа возможно элиминировать атрибут.
             */
            base.OnAttributeDetached(sender, eventArgs);

            return;

            void RemoveTupleOnDetachedAttribute()
            {
                int detachedAttrLoc = Schema.GetAttributeLoc(eventArgs.Attribute);
                SingleTupleObject<TEntity> cTuple;

                for (int i = 0; i < _tuples.Length; i++)
                {
                    if ((cTuple = _tuples[i]) is null || 
                        cTuple[detachedAttrLoc].Power.EqualsContinuum()) continue;

                    _tuples[i] = null;
                    break;
                }
            }
        }

        protected override IEnumerator<TEntity> GetEnumeratorImpl()
        {
            return null;
        }

        public override TupleObject<TEntity> Convert(TupleObject<TEntity> diagonal)
        {
            throw new NotImplementedException();
        }

        protected override TupleObject<TEntity> ComplementThe()
        {
            return null;
        }

        protected override TupleObject<TEntity> IntersectWith(TupleObject<TEntity> second)
        {
            return null;
        }

        protected override TupleObject<TEntity> UnionWith(TupleObject<TEntity> second)
        {
            return null;
        }

        protected override TupleObject<TEntity> ExceptWith(TupleObject<TEntity> second)
        {
            return null;
        }

        protected override TupleObject<TEntity> SymmetricExceptWith(TupleObject<TEntity> second)
        {
            return null;
        }

        public override TupleObject<TEntity> Diagonal()
        {
            throw new NotImplementedException();
        }
    }
}
