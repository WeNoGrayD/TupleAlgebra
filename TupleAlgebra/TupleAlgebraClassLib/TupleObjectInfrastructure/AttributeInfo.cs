using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using System.Reflection;
using System.Linq.Expressions;
using UniversalClassLib;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    using static TupleObjectHelper;

    public enum AttributeQuery : sbyte
    {
        Removed = -2,
        Detached = -1,
        None = 0,
        Attached = 1
    }

    public record AttributeInfo<TEntity, TAttribute>
        : ITupleObjectAttributeInfo<TEntity, TAttribute>
    {
        public AttributeName Name { get => AttributeMember.Name; }

        /// <summary>
        /// Запрос к атрибуту: 
        /// </summary>
        public AttributeQuery Query { get; private set; }

        public AttributeGetterHandler<TEntity, TAttribute> AttributeGetter 
        { get; init; }

        public MemberInfo AttributeMember { get; init; }

        public IAttributeComponentFactory<TAttribute> ComponentFactory { get; init; }

        //public ITupleObjectAttributeSetupWizard<TAttribute> SetupWizard { get; init; }

        public AttributeSetupWizardFactoryHandler<TAttribute>
            SetupWizardFactory { get; init; }

        /// <summary>
        /// Отношение эквивалентности.
        /// </summary>
        public bool HasEquivalenceRelation { get; private set; }

        public AttributeDomain<TAttribute> Domain { get => ComponentFactory.Domain; }

        /*
        public AttributeInfo(
            Expression<AttributeGetterHandler<TEntity, TAttribute>> 
                attributeGetterExpr,
            IAttributeComponentFactory<TAttribute> componentFactory = null,
            ITupleObjectAttributeSetupWizard<TAttribute> setupWizard = null,
            string attributeName = null,
            bool hasEquivalenceRelation = false)
        {
            Query = AttributeQuery.None;
            AttributeGetter = attributeGetterExpr.Compile();
            AttributeMember = MemberExtractor.ExtractFrom(attributeGetterExpr);
            ComponentFactory = componentFactory;
            SetupWizard = setupWizard;
            HasEquivalenceRelation = hasEquivalenceRelation;

            return;
        }
        */

        public AttributeInfo(
            Expression<AttributeGetterHandler<TEntity, TAttribute>>
                attributeGetterExpr,
            IAttributeComponentFactory<TAttribute> componentFactory = null,
            AttributeSetupWizardFactoryHandler<TAttribute> setupWizardFactory = null,
            bool hasEquivalenceRelation = false)
        {
            Query = AttributeQuery.None;
            AttributeGetter = attributeGetterExpr.Compile();
            AttributeMember = MemberExtractor.ExtractFrom(attributeGetterExpr);
            ComponentFactory = componentFactory;
            //SetupWizard = setupWizard;
            SetupWizardFactory = setupWizardFactory ??
                TupleObjectOneToOneAttributeSetupWizard<TAttribute>.Construct;
            HasEquivalenceRelation = hasEquivalenceRelation;

            return;
        }

        /*
        public ITupleObjectAttributeInfo GeneralizeWith(
            ITupleObjectAttributeInfo second,
            out bool gotFirst,
            out bool gotSecond)
        {
            if (this.AttributeMember != second.AttributeMember)
                throw new Exception($"Попытка генерализации двух разных атрибутов: {this.AttributeMember.Name} и {second.AttributeMember.Name}!");

            (ITupleObjectAttributeInfo res, gotFirst, gotSecond) = 
                (this.IsPlugged, second.IsPlugged) switch
            {
                (false, false) or (true, true) => (this, true, true),
                (true, false) => (this, true, false),
                _ => (second, false, true)
            };

            return res;
        }
        */

        public ITupleObjectAttributeInfo<TAttribute> SetFactory(
            IAttributeComponentFactory<TAttribute> factory)
        {
            return this with { ComponentFactory = factory };
        }

        public ITupleObjectAttributeInfo PlugIn()
        {
            return null;
            //return this with
            //{
            //    IsPlugged = true
            //};
        }

        public ITupleObjectAttributeInfo Unplug()
        {
            return null;
            //return this with
            //{
            //    IsPlugged = false
            //};
        }

        public ITupleObjectAttributeInfo SetEquivalenceRelation()
        {
            return this with
            {
                HasEquivalenceRelation = true
            };
        }

        public ITupleObjectAttributeInfo UnsetEquivalenceRelation()
        {
            return this with
            {
                HasEquivalenceRelation = false
            };
        }

        public override int GetHashCode() => Name.GetHashCode();

        public void UndoQuery()
        {
            Query = AttributeQuery.None;

            return;
        }

        /// <summary>
        /// Создание запроса к атрибуту на его прикрепление.
        /// </summary>
        /// <returns></returns>
        public void AttachQuery()
        {
            Query = AttributeQuery.Attached;

            return;
        }

        /// <summary>
        /// Создание запроса к атрибуту на его открепление.
        /// </summary>
        /// <returns></returns>
        public void DetachQuery()
        {
            Query = AttributeQuery.Detached;

            return;
        }

        /// <summary>
        /// Создание запроса к атрибуту на его открепление.
        /// </summary>
        /// <returns></returns>
        public void RemoveQuery()
        {
            Query = AttributeQuery.Removed;

            return;
        }
    }

    /*

    public enum AttributeQuery : byte
    {
        None = 0,
        Attached = 1,
        Detached = 2
    }

    /// <summary>
    /// Информация об атрибуте в составе схемы кортежа.
    /// </summary>
    public struct AttributeInfo
    {
        #region Instance fields

        /// <summary>
        /// Делегат получения значения атрибута из сущности.
        /// </summary>
        private Delegate _attributeGetter;

        /// <summary>
        /// Запрос к атрибуту: 
        /// </summary>
        private AttributeQuery _query;

        #endregion

        #region Instance properties

        /// <summary>
        /// Домен атрибута.
        /// </summary>
        public IAttributeComponentProvider Domain { get; private set; }

        /// <summary>
        /// Флаг подключения атрибута к схеме.
        /// </summary>
        public bool IsPlugged { get; private set; }

        /// <summary>
        /// Отношение эквивалентности.
        /// </summary>
        public bool HasEquivalenceRelation { get; private set; }

        /// <summary>
        /// Тип данных домена.
        /// </summary>
        public Type DomainDataType { get; private set; }

        /// <summary>
        /// Информация о свойстве сущности.
        /// </summary>
        public PropertyInfo AttributeProperty { get; init; }

        /// <summary>
        /// Настройщик.
        /// </summary>
        public object SetupWizard { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="isPlugged"></param>
        /// <param name="domain"></param>
        private AttributeInfo(
            Type domainDataType,
            bool hasEquivalenceRelation,
            bool isPlugged,
            IAttributeComponentProvider domain = null,
            Delegate attributeGetter = null)
        {
            IsPlugged = isPlugged;
            Domain = domain;
            DomainDataType = domainDataType;
            _attributeGetter = attributeGetter;
            SetupWizard = null;
        }

        #endregion

        #region Construction methods

        public static AttributeInfo Construct<TEntity, TAttribute>(
            bool hasEquivalenceRelation = false,
            bool isPlugged = false,
            AttributeDomain<TAttribute> domain = null,
            Func<TEntity, TAttribute> attributeGetter = null)
        {
            return new AttributeInfo(
                domainDataType: typeof(TAttribute),
                hasEquivalenceRelation: hasEquivalenceRelation,
                isPlugged: isPlugged,
                domain: domain,
                attributeGetter: attributeGetter);
        }

        #endregion

        #region Cloning methods

        public AttributeInfo CloneWith<TEntity, TAttribute>(
            IAttributeComponentProvider domain = null,
            Func<TEntity, TAttribute> attributeGetter = null,
            bool hasEquivalenceRelation = false)
        {
            bool isPlugged = (_query == AttributeQuery.Attached) || 
                (_query != AttributeQuery.Detached && IsPlugged);

            return this with
            {
                IsPlugged = isPlugged,
                _query = AttributeQuery.None,
                Domain = domain ?? this.Domain,
                _attributeGetter = attributeGetter ?? this._attributeGetter
            };
        }

        #endregion

        public AttributeDomain<TAttribute> GetDomain<TAttribute>()
        {
            return Domain as AttributeDomain<TAttribute>;
        }

        public IAttributeComponentProvider GetDomain()
        {
            return Domain;
        }

        public void SetDomain(IAttributeComponentProvider domain)
        {
            Domain = domain;

            return;
        }

        public ITupleObjectAttributeSetupWizard<TAttribute> GetSetupWizard<TAttribute>()
        {
            return SetupWizard as ITupleObjectAttributeSetupWizard<TAttribute>;
        }

        public void SetSetupWizard<TAttribute>(
            ITupleObjectAttributeSetupWizard<TAttribute> setupWizard)
        {
            SetupWizard = setupWizard;

            return;
        }

        public void SetAttributeGetter(Delegate attributeGetter)
        {
            _attributeGetter = attributeGetter;

            return;
        }

        public Func<TEntity, TAttribute> AttributeGetter<TEntity, TAttribute>()
        {
            return _attributeGetter as Func<TEntity, TAttribute>;
        }

        private AttributeInfo UndoQuery()
        {
            return this with { _query = AttributeQuery.None };
        }

        private AttributeInfo AttachQuery()
        {
            return this with { _query = AttributeQuery.Attached };
        }

        private AttributeInfo DetachQuery()
        {
            return this with { _query = AttributeQuery.Detached };
        }

        /// <summary>
        /// Создание запроса к атрибуту на его прикрепление.
        /// </summary>
        /// <returns></returns>
        public AttributeInfo Attach()
        {
            return (IsPlugged, _query) switch
            {
                (_, AttributeQuery.Attached) => this,
                (true, AttributeQuery.None) => this,
                (true, _) => UndoQuery(),
                (_, _) => AttachQuery(),
            };
        }

        /// <summary>
        /// Создание запроса к атрибуту на его открепление.
        /// </summary>
        /// <returns></returns>
        public AttributeInfo Detach()
        {
            return (IsPlugged, _query) switch
            {
                (_, AttributeQuery.Detached) => this,
                (false, AttributeQuery.None) => this,
                (false, _) => UndoQuery(),
                (_, _) => DetachQuery()
            };
        }

        public AttributeInfo ExecuteQuery()
        {
            if (_query == AttributeQuery.None)
                return this;


            bool isPlugged = (_query == AttributeQuery.Attached) ||
                (_query != AttributeQuery.Detached && IsPlugged);

            return this with { IsPlugged = IsPlugged, _query = AttributeQuery.None };
        }

        #region Equivalence relation set/unset methods

        private static IEqualityComparer<TAttribute> CreateEquivalenceRelationComparer<TAttribute>(
            Func<TAttribute, TAttribute, bool> equivalenceRelation) =>
            new EquivalenceRelationComparer<TAttribute>(equivalenceRelation);

        public AttributeInfo SetEquivalenceRelation<T>()
        {
            return this with
            {
                HasEquivalenceRelation = true
            };
        }

        public AttributeInfo UnsetEquivalenceRelation()
        {
            return this with
            {
                HasEquivalenceRelation = false
            };
        }

        #endregion

        #region Operators

        public static bool operator ==(AttributeInfo first, AttributeInfo second)
        {
            return first.IsPlugged == second.IsPlugged;
        }

        public static bool operator !=(AttributeInfo first, AttributeInfo second)
        {
            return !(first == second);
        }

        #endregion

        #region Nested types

        private class EquivalenceRelationComparer<T>
            : IEqualityComparer<T>
        {
            private Func<T, T, bool> _equalityComparison;

            public EquivalenceRelationComparer(Func<T, T, bool> equalityComparison)
            {
                _equalityComparison = equalityComparison;
                //_equalityComparison = Comparer<T>.Create(((Comparison<T>)null));

                return;
            }

            public bool Equals(T first, T second)
            {
                return _equalityComparison(first, second);
            }

            public int GetHashCode(T obj) => obj.GetHashCode();
        }

        #endregion
    }
    */
}
