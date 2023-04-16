using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure;
using LINQProvider;

namespace TupleAlgebraClassLib
{
    /// <summary>
    /// Домен атрибута.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public abstract class AttributeDomain<TData> 
        : IEnumerable, IEnumerable<TData>, IQueryable<TData>, IReproducingQueryable<TData>
    {
        #region IQueryable<TData> implemented properties

        /// <summary>
        /// Выражение запроса.
        /// </summary>
        public virtual Expression Expression { get; private set; }

        /// <summary>
        /// Тип элемента запроса.
        /// </summary>
        public virtual Type ElementType { get => typeof(TData); }

        /// <summary>
        /// Провайдер запросов к домену.
        /// </summary>
        public virtual IQueryProvider Provider { get; protected set; }

        #endregion

        #region Static fields

        protected static Action<AttributeDomain<TData>> _setDomainCallback;

        #endregion

        #region Instance properties

        /// <summary>
        /// Универсум домена.
        /// </summary>
        public readonly NonFictionalAttributeComponent<TData> Universum;

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="universum"></param>
        /// <param name="queryExpression"></param>
        protected AttributeDomain(
            NonFictionalAttributeComponent<TData> universum, 
            Expression queryExpression = null)
        {
            Universum = universum;
            Provider = universum.Provider;
            this.Expression = queryExpression ?? Expression.Constant(Universum);
        }

        #endregion

        #region Instance methods

        public abstract IReproducingQueryable<TReproducedData> Reproduce<TReproducedData>(
            IEnumerable<TReproducedData> reproducedData);

        public AttributeDomain<TReproducedData> Shift<TReproducedData>(
            Func<TData, TReproducedData> itemSelector)
        {
            NonFictionalAttributeComponent<TReproducedData> shiftedUniversum =
                Universum.Reproduce(Universum.Select(itemSelector)) as NonFictionalAttributeComponent<TReproducedData>;

            return null;// new AttributeDomain<TReproducedData>(shiftedUniversum);
        }

        public AttributeDomain<TReproducedData> ShiftMany<TReproducedData>(
            Func<TData, IEnumerable<TReproducedData>> itemsSelector)
        {
            NonFictionalAttributeComponent<TReproducedData> shiftedUniversum =
                Universum.Reproduce(Universum.SelectMany(itemsSelector)) as NonFictionalAttributeComponent<TReproducedData>;

            return null;//;new AttributeDomain<TReproducedData>(shiftedUniversum);
        }

        #endregion

        #region IEnumerable implemented methods

        /// <summary>
        /// Обобщённое получение перечислителя домена.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TData> GetEnumerator()
        {
            return Universum.GetEnumerator();
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

        #region Operators

        /// <summary>
        /// Оператор пересечения домена и компоненты атрибута.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public static AttributeComponent<TData> operator &(AttributeDomain<TData> domain, NonFictionalAttributeComponent<TData> component)
        {
            return domain.Universum & component;
        }

        /// <summary>
        /// Оператор объединения домена и компоненты атрибута.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public static AttributeComponent<TData> operator |(AttributeDomain<TData> domain, NonFictionalAttributeComponent<TData> component)
        {
            return domain.Universum | component;
        }

        /// <summary>
        /// Оператор исключения компоненты атрибута из домена.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public static AttributeComponent<TData> operator /(AttributeDomain<TData> domain, NonFictionalAttributeComponent<TData> component)
        {
            return domain.Universum / component;
        }

        /// <summary>
        /// Оператор симметричного исключения домена и компоненты атрибута.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public static AttributeComponent<TData> operator ^(AttributeDomain<TData> domain, NonFictionalAttributeComponent<TData> component)
        {
            return domain.Universum ^ component;
        }

        /// <summary>
        /// Оператор сравнения на равенство домена и компоненты атрибута.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public static bool operator ==(AttributeDomain<TData> domain, NonFictionalAttributeComponent<TData> component)
        {
            return domain.Universum == component;
        }

        /// <summary>
        /// Оператор сравнения на неравенство домена и компоненты атрибута.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public static bool operator !=(AttributeDomain<TData> domain, NonFictionalAttributeComponent<TData> component)
        {
            return !(domain == component);
        }

        #endregion

        #region Nested types

        /// <summary>
        /// Провайдер запросов к домену атрибута.
        /// </summary>
        protected class AttributeDomainQueryProvider : IQueryProvider
        {
            #region Instance fields

            /// <summary>
            /// Провайдер запросов к универсуму домена атрибута.
            /// </summary>
            private QueryProvider UniversumQueryProvider;

            #endregion

            #region Instance properties

            public AttributeDomain<TData> Queryable { get; set; }

            #endregion

            #region Constructors

            /// <summary>
            /// Конструктор экземпляра.
            /// </summary>
            /// <param name="universumQueryProvider"></param>
            public AttributeDomainQueryProvider(
                QueryProvider universumQueryProvider)
            {
                UniversumQueryProvider = universumQueryProvider;
            }

            #endregion

            #region IQueryable implemented methods

            /// <summary>
            /// Создание IQueryable-компоненты.
            /// </summary>
            /// <param name="expression"></param>
            /// <returns></returns>
            public IQueryable CreateQuery(Expression expression)
            {
                return UniversumQueryProvider.CreateQuery(expression);
            }

            /// <summary>
            /// Создание IQueryable-компоненты.
            /// </summary>
            /// <typeparam name="TQueryResult"></typeparam>
            /// <param name="expression"></param>
            /// <returns></returns>
            public IQueryable<TQueryResult> CreateQuery<TQueryResult>(Expression expression)
            {
                return UniversumQueryProvider.CreateQuery<TQueryResult>(expression);
            }

            /// <summary>
            /// Выполнение запроса.
            /// </summary>
            /// <param name="expression"></param>
            /// <returns></returns>
            public object Execute(Expression expression)
            {
                return UniversumQueryProvider.Execute(expression);
            }

            /// <summary>
            /// Выполнение запроса.
            /// </summary>
            /// <typeparam name="TQueryResult"></typeparam>
            /// <param name="expression"></param>
            /// <returns></returns>
            public TQueryResult Execute<TQueryResult>(Expression expression)
            {
                return UniversumQueryProvider.Execute<TQueryResult>(expression);
            }

            #endregion
        }

        protected class AttributeDomainQueryContext
        {
            public object Execute(Expression queryExpression)
            {
                return null;
            }
        }

        #endregion
    }
}
