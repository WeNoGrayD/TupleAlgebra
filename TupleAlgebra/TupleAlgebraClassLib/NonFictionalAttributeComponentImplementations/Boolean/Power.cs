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
    public sealed class AtomicNonFictionalAttributeComponentPower<TData>
        : NonFictionalAttributeComponentPower<TData>,
          IFiniteEnumerableAttributeComponentPower
    {
        #region Static properties

        public static AtomicNonFictionalAttributeComponentPower<TData> Instance { get; private set; }

        #endregion

        #region Instance properties

        public int NumericalRepresentation { get => 1; }

        #endregion

        #region Constructors

        static AtomicNonFictionalAttributeComponentPower()
        {
            Instance = new AtomicNonFictionalAttributeComponentPower<TData>();

            return;
        }

        private AtomicNonFictionalAttributeComponentPower()
        {
            return;
        }

        #endregion

        #region Instance methods

        public override bool EqualsZero()
        {
            return false;
        }

        public override bool EqualsContinuum()
        {
            return false;
        }

        public override void InitWith(NonFictionalAttributeComponent<TData> component)
        {
            return;
        }

        protected override int CompareToZero()
        {
            return 1;
        }

        protected override int CompareToNonFictional(AttributeComponentPower second)
        {
            if (second is AtomicNonFictionalAttributeComponentPower<TData> second2)
                return 0;
            else
                throw new InvalidCastException("Непустая булевая компонента сравнивается с непустой компонентой другого вида, в данный момент эта операция не поддерживается.");
        }

        #endregion
    }
}
