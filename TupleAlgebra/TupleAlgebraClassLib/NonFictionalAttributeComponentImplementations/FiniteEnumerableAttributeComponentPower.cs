using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Boolean;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations
{
    /// <summary>
    /// Мощность упорядоченной конечной перечислимой компонентой атрибута.
    /// </summary>
    public class FiniteEnumerableAttributeComponentPower<TData, TComponent>
        : CapturingNonFictionalAttributeComponentPower<TData, TComponent>,
          IFiniteEnumerableAttributeComponentPower
        where TComponent : NonFictionalAttributeComponent<TData>,
                           IFiniteEnumerableAttributeComponent<TData>
    {
        #region Instance properties

        public virtual int NumericalRepresentation { get => Component.Values.Count(); }

        #endregion

        protected override int CompareToZero()
        {
            return (this as IFiniteEnumerableAttributeComponentPower)!.CompareToZero();
        }

        protected override int CompareToNonFictional(AttributeComponentPower second)
        {
            return second switch
            {
                IFiniteEnumerableAttributeComponentPower castedSecond => (this as IFiniteEnumerableAttributeComponentPower)!.CompareTo(castedSecond),//-castedSecond.CompareTo(this),
                _ => throw new InvalidCastException("Непустая компонента с конечным перечислимым содержимым сравнивается с непустой компонентой другого вида, в данный момент эта операция не поддерживается.")
            };
        }
    }
}
