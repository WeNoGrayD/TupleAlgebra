using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Boolean
{
    /// <summary>
    /// Мощность компоненты, которая может включать 
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public sealed class AtomicNonFictionalAttributeComponentPower
        : NonFictionalAttributeComponentPower,
          IFiniteEnumerableAttributeComponentPower
    {
        #region Static properties

        public static AtomicNonFictionalAttributeComponentPower Instance { get; private set; }

        #endregion

        #region Instance properties

        public int NumericalRepresentation { get => 1; }

        #endregion

        #region Constructors

        static AtomicNonFictionalAttributeComponentPower()
        {
            Instance = new AtomicNonFictionalAttributeComponentPower();

            return;
        }

        private AtomicNonFictionalAttributeComponentPower()
        {
            return;
        }

        #endregion

        #region Instance methods

        public override bool EqualsZero<TData>(AttributeComponent<TData> ac)
        {
            return false;
        }

        public override bool EqualsContinuum<TData>(AttributeComponent<TData> ac)
        {
            return false;
        }

        protected override int CompareToZero<TData>(AttributeComponent<TData> ac)
        {
            return 1;
        }

        public override int CompareToNonFictional<TData>(
            AttributeComponentPower second,
            AttributeComponent<TData> ac1,
            AttributeComponent<TData> ac2)
        {
            if (second is AtomicNonFictionalAttributeComponentPower second2)
                return 0;
            else
                throw new InvalidCastException("Непустая булевая компонента сравнивается с непустой компонентой другого вида, в данный момент эта операция не поддерживается.");
        }

        #endregion
    }
}
