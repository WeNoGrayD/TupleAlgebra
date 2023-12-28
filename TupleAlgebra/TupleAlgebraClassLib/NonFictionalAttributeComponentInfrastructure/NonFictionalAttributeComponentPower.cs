using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    /// <summary>
    /// Мощность нефиктивной компоненты.
    /// </summary>
    public abstract class NonFictionalAttributeComponentPower<TData> : AttributeComponentPower
    {
        #region Instance properties

        /// <summary>
        /// Константное значение типа наполнения компоненты - нефиктивный.
        /// </summary>
        public override AttributeComponentContentType ContentType
        { get => AttributeComponentContentType.NonFictional; }

        protected abstract NonFictionalAttributeComponent<TData> Component { get; }

        #endregion

        #region Instance methods

        public abstract void InitAttributeComponent(NonFictionalAttributeComponent<TData> component);

        public override bool EqualsContinuum()
        {
            return Component.Domain == Component;
        }

        protected abstract int CompareToZero();

        protected abstract int CompareToSame(dynamic second);

        public override sealed int CompareTo(AttributeComponentPower second)
        {
            /*
             * Если вторая компонента является полной,
             * то необходимо сравнить мощность нефиктивной компоненты с мощностью универсума,
             * который представляет полная компонента (могущая быть компонентой другого атрибута,
             * т.е. сравнивать по типу мощности будет некорректно).
             * Если вторая компонента тоже нефиктивная, то проводится специализированное сравнение.
             * Если вторая компонента пустая, то проводится сравнение на 0.
             */
            return base.CompareTo(second) switch
            {
                -1 => -second.CompareTo(this),
                0 => CompareToSame(second),
                _ => CompareToZero()
            };
        }

        #endregion
    }
}
