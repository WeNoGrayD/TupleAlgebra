﻿using System;
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

namespace TupleAlgebraClassLib.AttributeComponents
{
    /// <summary>
    /// Домен атрибута.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class AttributeDomain<TData>
        : IEnumerable, 
          IEnumerable<TData>, 
          IQueryable<TData>, 
          IAttributeComponentProvider//, IReproducingQueryable<TData>
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
            universum.GetDomain += UniversumDomainGetter;
            Provider = universum.Provider;
            Expression = queryExpression ?? Expression.Constant(Universum);
        }

        #endregion

        #region Instance methods

        public AttributeDomain<TData> UniversumDomainGetter() => this;

        //public abstract IReproducingQueryable<TReproducedData> Reproduce<TReproducedData>(
        //    IEnumerable<TReproducedData> reproducedData);

        /// <summary>
        /// Преобразование домена с отношением 1-к-1.
        /// </summary>
        /// <typeparam name="TShiftedData"></typeparam>
        /// <param name="itemSelector"></param>
        /// <returns></returns>
        public AttributeDomain<TShiftedData> Shift<TShiftedData>(
            Expression<Func<TData, TShiftedData>> itemSelector)
        {
            NonFictionalAttributeComponent<TShiftedData> shiftedUniversum =
                Universum.Select(itemSelector) as NonFictionalAttributeComponent<TShiftedData>;

            return new AttributeDomain<TShiftedData>(shiftedUniversum);
        }

        public AttributeDomain<TShiftedData> Shift<TShiftedData>(
            Func<TData, TShiftedData> itemSelector)
        {
            NonFictionalAttributeComponent<TShiftedData> shiftedUniversum =
                Universum.Select(itemSelector) as NonFictionalAttributeComponent<TShiftedData>;

            return new AttributeDomain<TShiftedData>(shiftedUniversum);
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
            NonFictionalAttributeComponent<TShiftedData> shiftedUniversum =
                Universum.Reproduce(Universum.SelectMany(itemsSelector)) as NonFictionalAttributeComponent<TShiftedData>;

            return new AttributeDomain<TShiftedData>(shiftedUniversum);
        }

        public AttributeDomain<TShiftedData> ShiftMany<TShiftedData>(
            Func<TData, IEnumerable<TShiftedData>> itemsSelector)
        {
            NonFictionalAttributeComponent<TShiftedData> shiftedUniversum =
                Universum.Reproduce(Universum.SelectMany(itemsSelector)) as NonFictionalAttributeComponent<TShiftedData>;

            return new AttributeDomain<TShiftedData>(shiftedUniversum);
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

        #region IAttributeComponentProvider

        public IAlgebraicSetObject CreateAttributeComponent<TEntity>(AttributeInfo attribute, IEnumerable<TEntity> entitySource)
        {
            Func<TEntity, TData> dataSelector = attribute.Getter<TEntity, TData>();

            return Universum.Reproduce(entitySource.Select(entity => dataSelector(entity))) as IAlgebraicSetObject;
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