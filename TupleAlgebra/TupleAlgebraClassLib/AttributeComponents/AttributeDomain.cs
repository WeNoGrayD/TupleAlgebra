using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using LINQProvider;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.AttributeComponents
{
    using static AttributeComponentHelper;

    /// <summary>
    /// Домен атрибута.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class AttributeDomain<TData>
        : IEnumerable, 
          IEnumerable<TData>, 
          IQueryable<TData> //, IReproducingQueryable<TData>
    {
        #region IQueryable<TData> implemented properties

        /// <summary>
        /// Выражение запроса.
        /// </summary>
        public virtual Expression Expression { get => Universe.Expression; }//; private set; }

        /// <summary>
        /// Тип элемента запроса.
        /// </summary>
        public virtual Type ElementType { get => typeof(TData); }

        /// <summary>
        /// Провайдер запросов к домену.
        /// </summary>
        public virtual IQueryProvider Provider { get => Universe.Provider; }

        #endregion

        #region Instance properties

        /// <summary>
        /// Универсум домена.
        /// </summary>
        public NonFictionalAttributeComponent<TData> Universe { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="universe"></param>
        protected AttributeDomain()
        {
            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="universe"></param>
        public AttributeDomain(
            NonFictionalAttributeComponent<TData> universe)
        {
            Universe = universe;
            universe.Domain = this;

            return;
        }

        #endregion

        #region Instance methods

        /// <summary>
        /// Преобразование домена с отношением 1-к-1.
        /// </summary>
        /// <typeparam name="TShiftedData"></typeparam>
        /// <param name="itemSelector"></param>
        /// <returns></returns>
        public AttributeDomain<TShiftedData> Shift<TShiftedData>(
            Expression<Func<TData, TShiftedData>> itemSelector)
        {
            NonFictionalAttributeComponent<TShiftedData> shiftedUniverse =
                Universe.Select(itemSelector) as NonFictionalAttributeComponent<TShiftedData>;

            return new AttributeDomain<TShiftedData>(shiftedUniverse);
        }

        public AttributeDomain<TShiftedData> Shift<TShiftedData>(
            Func<TData, TShiftedData> itemSelector)
        {
            NonFictionalAttributeComponent<TShiftedData> shiftedUniverse =
                Universe.Select(itemSelector) as NonFictionalAttributeComponent<TShiftedData>;

            return new AttributeDomain<TShiftedData>(shiftedUniverse);
        }

        /// <summary>
        /// Преобразование домена с отношением 1-к-N.
        /// </summary>
        /// <typeparam name="TShiftedData"></typeparam>
        /// <param name="itemsSelector"></param>
        /// <returns></returns>
        public AttributeDomain<TShiftedData> ShiftMany<TShiftedData>(
            Expression<Func<TData, IEnumerable<TShiftedData>>> itemsSelector)
        {
            NonFictionalAttributeComponent<TShiftedData> shiftedUniverse =
                Universe.Reproduce(Universe.SelectMany(itemsSelector)) as NonFictionalAttributeComponent<TShiftedData>;

            return new AttributeDomain<TShiftedData>(shiftedUniverse);
        }

        public AttributeDomain<TShiftedData> ShiftMany<TShiftedData>(
            Func<TData, IEnumerable<TShiftedData>> itemsSelector)
        {
            NonFictionalAttributeComponent<TShiftedData> shiftedUniverse =
                Universe.Reproduce(Universe.SelectMany(itemsSelector)) as NonFictionalAttributeComponent<TShiftedData>;

            return new AttributeDomain<TShiftedData>(shiftedUniverse);
        }

        #endregion

        #region IEnumerable implemented methods

        /// <summary>
        /// Обобщённое получение перечислителя домена.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TData> GetEnumerator()
        {
            return Universe.GetEnumerator();
        }

        /// <summary>
        /// Необобщённое получение перечислителя домена.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Static methods

        #endregion

        #region Operators

        /// <summary>
        /// Оператор пересечения домена и компоненты атрибута.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public static IAttributeComponent<TData> operator &(
            AttributeDomain<TData> domain, 
            AttributeComponent<TData> component)
        {
            return domain.Universe & component;
        }

        /// <summary>
        /// Оператор объединения домена и компоненты атрибута.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public static IAttributeComponent<TData> operator |(
            AttributeDomain<TData> domain, 
            AttributeComponent<TData> component)
        {
            return domain.Universe | component;
        }

        /// <summary>
        /// Оператор исключения компоненты атрибута из домена.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public static IAttributeComponent<TData> operator /(
            AttributeDomain<TData> domain,
            AttributeComponent<TData> component)
        {
            return domain.Universe / component;
        }

        /// <summary>
        /// Оператор симметричного исключения домена и компоненты атрибута.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public static IAttributeComponent<TData> operator ^(
            AttributeDomain<TData> domain, 
            AttributeComponent<TData> component)
        {
            return domain.Universe ^ component;
        }

        /// <summary>
        /// Оператор сравнения на равенство домена и компоненты атрибута.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public static bool operator ==(
            AttributeDomain<TData> domain,
            AttributeComponent<TData> component)
        {
            return domain.Universe == component;
        }

        /// <summary>
        /// Оператор сравнения на неравенство домена и компоненты атрибута.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public static bool operator !=(
            AttributeDomain<TData> domain, 
            AttributeComponent<TData> component)
        {
            return !(domain == component);
        }

        #endregion
    }

    /// <summary>
    /// Мощность полной фиктивной компоненты атрибута.
    /// </summary>
    public class AttributeUniversePower : AttributeComponentPower
    {
        #region Instance fields

        public override AttributeComponentContentType ContentType
        { get => AttributeComponentContentType.Full; }

        #endregion

        #region Instance properties

        protected AttributeComponentPower NonFictionalPower { get; private set; }

        #endregion

        #region Constructors

        public AttributeUniversePower(AttributeComponentPower nonFictionalPower)
        {
            NonFictionalPower = nonFictionalPower;

            return;
        }

        #endregion

        #region IAttributeComponentPower implementation

        public override bool EqualsZero<TData>(AttributeComponent<TData> ac)
            => false;

        public override bool EqualsContinuum<TData>(AttributeComponent<TData> ac)
            => false;

        /*
         * Переопределять метод сравнения нет необходимости.
         * Поскольку сравнение производится при помощи ContentType, то всегда this > second.
         * Для доступа к необходимым полям нефиктивной мощности требуется приводить эту к нужному типу.
         */
        //public override int CompareTo(AttributeComponent second);

        /// <summary>
        /// Подменяет в необходимом контексте себя сохранённой мощнойстью нефиктивной компоненты.
        /// </summary>
        /// <typeparam name="TConverting"></typeparam>
        /// <returns></returns>
        public override TConverting As<TConverting>() => (NonFictionalPower as TConverting)!;

        #endregion
    }
}
