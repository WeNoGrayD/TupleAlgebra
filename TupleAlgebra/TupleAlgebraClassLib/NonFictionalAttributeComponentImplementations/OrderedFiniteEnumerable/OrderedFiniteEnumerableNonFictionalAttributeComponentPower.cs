using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable
{
    /// <summary>
    /// Мощность упорядоченной конечной перечислимой компонентой аттрибута.
    /// </summary>
    public class OrderedFiniteEnumerableNonFictionalAttributeComponentPower<TData> 
        : NonFictionalAttributeComponentPower<TData>
    {
        #region Instance fields

        private OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> _component;

        #endregion

        #region Instance properties

        protected override NonFictionalAttributeComponent<TData> Component { get => _component; }

        public int Value { get => _component.Values.Count(); }

        #endregion

        public override void InitAttributeComponent(NonFictionalAttributeComponent<TData> component)
        {
            _component = (component as OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>)!;

            return;
        }

        public override bool EqualsZero()
        {
            return Value == 0;
        }

        protected override int CompareToSame(dynamic second)
        {
            if (second is OrderedFiniteEnumerableNonFictionalAttributeComponentPower<TData> castedSecond)
                return this.CompareToSame(castedSecond);
            else
                throw new InvalidCastException("Непустая компонента с конечным перечислимым содержимым сравнивается с непустой компонентой другого вида, в данный момент эта операция не поддерживается.");
        }

        protected int CompareToSame(OrderedFiniteEnumerableNonFictionalAttributeComponentPower<TData> second)
        {
            return this.Value.CompareTo(second.Value);
        }
    }
}
