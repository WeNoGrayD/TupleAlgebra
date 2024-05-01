using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    /// <summary>
    /// Мощность нефиктивной компоненты.
    /// </summary>
    public abstract class NonFictionalAttributeComponentPower
        : AttributeComponentPower
    {
        #region Instance properties

        /// <summary>
        /// Константное значение типа наполнения компоненты - нефиктивный.
        /// </summary>
        public override AttributeComponentContentType ContentType
        { get => AttributeComponentContentType.NonFictional; }

        #endregion

        #region Instance methods

        public override bool EqualsZero<TData>(AttributeComponent<TData> ac)
        {
            return CompareToZero(ac) == 0;
        }

        protected abstract int CompareToZero<TData>(AttributeComponent<TData> ac);

        public override bool EqualsContinuum<TData>(AttributeComponent<TData> ac)
        {
            return ac.Domain == ac;
        }

        /// <summary>
        /// Сравнение мощностей двух нефиктивных компонент.
        /// Компоненты не обязаны иметь один тип данных: так,
        /// может проводиться сортировка компонент в схеме кортежа по мощности.
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        public abstract int CompareToNonFictional<TData>(
            AttributeComponentPower second,
            AttributeComponent<TData> ac1,
            AttributeComponent<TData> ac2);

        /*
         * Если вторая компонента является полной,
         * то необходимо сравнить мощность нефиктивной компоненты с мощностью универсума,
         * который представляет полная компонента (могущая быть компонентой другого атрибута,
         * т.е. сравнивать по типу мощности будет некорректно).
         * Если вторая компонента тоже нефиктивная, то проводится специализированное сравнение.
         * Если вторая компонента пустая, то проводится сравнение на 0.
         */
        /*
        public override sealed int CompareTo(AttributeComponentPower second)
        {
            return base.CompareTo(second) switch
            {
                < 0 => -second.CompareTo(this),
                0 => CompareToNonFictional(second),
                > 0 => CompareToZero()
            };
        }
        */

        #endregion
    }

    /// <summary>
    /// Предполагается использование только для мощностей нефиктивных компонент.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public struct AttributeComponentPowerContext<TData>
    {
        private NonFictionalAttributeComponent<TData> _component;

        private NonFictionalAttributeComponentPower _power;

        public AttributeComponentPowerContext(
            NonFictionalAttributeComponent<TData> component)
        {
            _component = component;
            _power = component.Power.As<NonFictionalAttributeComponentPower>();

            return;
        }

        public AttributeComponentPowerContext(
            IAttributeComponent<TData> component)
        {
            _component = (component as NonFictionalAttributeComponent<TData>)!;
            _power = component.Power.As<NonFictionalAttributeComponentPower>();

            return;
        }

        public int CompareTo(AttributeComponentPowerContext<TData> other)
        {
            return _power.CompareToNonFictional(
                other._power, 
                _component, 
                other._component);
        }

        #region Operators

        /// <summary>
        /// Оператор сравнения на равенство двух мощностей.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator ==(
            AttributeComponentPowerContext<TData> first,
            AttributeComponentPowerContext<TData> second)
        {
            return first.CompareTo(second) == 0;
        }

        /// <summary>
        /// Оператор сравнения на неравенство двух мощностей.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator !=(
            AttributeComponentPowerContext<TData> first,
            AttributeComponentPowerContext<TData> second)
        {
            return first.CompareTo(second) != 0;
        }

        /// <summary>
        /// Оператор сравнения "больше" двух мощностей.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator >(
            AttributeComponentPowerContext<TData> first,
            AttributeComponentPowerContext<TData> second)
        {
            return first.CompareTo(second) == 1;
        }

        /// <summary>
        /// Оператор сравнения "больше или равно" двух мощностей.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator >=(
            AttributeComponentPowerContext<TData> first,
            AttributeComponentPowerContext<TData> second)
        {
            return first.CompareTo(second) > -1;
        }

        /// <summary>
        /// Оператор сравнения "меньше" двух мощностей.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator <(
            AttributeComponentPowerContext<TData> first,
            AttributeComponentPowerContext<TData> second)
        {
            return first.CompareTo(second) == -1;
        }

        /// <summary>
        /// Оператор сравнения "меньше или равно" двух мощностей.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator <=(
            AttributeComponentPowerContext<TData> first,
            AttributeComponentPowerContext<TData> second)
        {
            return first.CompareTo(second) < 1;
        }

        #endregion
    }
}
