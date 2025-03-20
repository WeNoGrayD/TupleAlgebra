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
    public class FiniteEnumerableAttributeComponentPower
        : NonFictionalAttributeComponentPower,
          IFiniteEnumerableAttributeComponentPower
    {
        #region Static properties

        public static FiniteEnumerableAttributeComponentPower Instance 
        { get; private set; }

        #endregion

        #region Constructors

        static FiniteEnumerableAttributeComponentPower()
        {
            Instance = new FiniteEnumerableAttributeComponentPower();

            return;
        }

        #endregion

        #region Instance methods

        protected override int CompareToZero<TData>(AttributeComponent<TData> ac)
        {
            return (this as IFiniteEnumerableAttributeComponentPower)!
                .CompareToZero((ac as IFiniteEnumerableAttributeComponent<TData>)!);
        }

        public override int CompareToNonFictional<TData>(
            AttributeComponentPower second,
            AttributeComponent<TData> ac1,
            AttributeComponent<TData> ac2)
        {
            return second switch
            {
                IFiniteEnumerableAttributeComponentPower castedSecond => 
                    (this as IFiniteEnumerableAttributeComponentPower)!
                        .CompareTo(
                            castedSecond,
                            (ac1 as IFiniteEnumerableAttributeComponent<TData>)!,
                            (ac2 as IFiniteEnumerableAttributeComponent<TData>)!),
                _ => throw new InvalidCastException("Непустая компонента с конечным перечислимым содержимым сравнивается с непустой компонентой другого вида, в данный момент эта операция не поддерживается.")
            };
        }
        #endregion
    }
}
