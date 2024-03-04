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
    public abstract class NonFictionalAttributeComponentPower<TData> : AttributeComponentPower
    {
        #region Instance properties

        /// <summary>
        /// Константное значение типа наполнения компоненты - нефиктивный.
        /// </summary>
        public override AttributeComponentContentType ContentType
        { get => AttributeComponentContentType.NonFictional; }

        #endregion

        #region Instance methods

        public abstract void InitWith(NonFictionalAttributeComponent<TData> component);

        public override bool EqualsZero()
        {
            return CompareToZero() == 0;
        }

        protected abstract int CompareToZero();

        /// <summary>
        /// Сравнение мощностей двух нефиктивных компонент.
        /// Компоненты не обязаны иметь один тип данных: так,
        /// может проводиться сортировка компонент в схеме кортежа по мощности.
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        protected abstract int CompareToNonFictional(AttributeComponentPower second);

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
                <0 => -second.CompareTo(this),
                0 => CompareToNonFictional(second),
                >0 => CompareToZero()
            };
        }

        #endregion
    }

    /// <summary>
    /// Мощной захватываемой в контекст нефиктивной компоненты.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TComponent"></typeparam>
    public abstract class CapturingNonFictionalAttributeComponentPower<TData, TComponent> 
        : NonFictionalAttributeComponentPower<TData>
        where TComponent : NonFictionalAttributeComponent<TData>,
                           IFiniteEnumerableAttributeComponent<TData>
    {
        #region Instance fields

        private TComponent _component;

        #endregion

        #region Instance properties

        protected TComponent Component { get => _component; }

        #endregion

        #region Instance methods

        public override bool EqualsContinuum()
        {
            return _component.Domain == _component;
        }

        public override void InitWith(NonFictionalAttributeComponent<TData> component)
        {
            _component = (component as TComponent)!;

            return;
        }

        #endregion
    }
}
