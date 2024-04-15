using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using System.Reflection;
using System.Linq.Expressions;
using UniversalClassLib;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    using static TupleObjectHelper;

    public enum AttributeQuery : byte
    {
        None = 0,
        Attached = 1,
        Detached = 2
    }

    public class FlyweightAttributeInfoFactory<TEntity>
    {
        private IDictionary<string, object> _resources;

        private AttributePropertyExtractor _attributePropertyExtractor;

        public FlyweightAttributeInfoFactory()
        {
            _resources = new Dictionary<string, object>();
            _attributePropertyExtractor = new AttributePropertyExtractor();

            return;
        }

        public FlyweightAttributeInfo<TEntity, TData> Get<TData>(
            Expression<AttributeGetterHandler<TEntity, TData>> attributeGetterExpr)
        {
            PropertyInfo attributeProperty = _attributePropertyExtractor.Extract(attributeGetterExpr);
            string attrName = attributeProperty.Name;
            FlyweightAttributeInfo<TEntity, TData> attrInfo;
            object attrInfoRef;

            if (!_resources.TryGet(attrName, out attrInfoRef))
            {
                attrInfoRef = new FlyweightAttributeInfo<TEntity, TData>(
                    attributeGetterExpr.Compile(),
                    attributeProperty);
                _resources.Add(attrInfoRef);
            }

            return (attrInfoRef as FlyweightAttributeInfo<TEntity, TData>)!;
        }

        public FlyweightAttributeInfo<TEntity, TData> Get<TData>(
            string attributeName)
        {
            return (_resources[attributeName] as FlyweightAttributeInfo<TEntity, TData>)!;
        }
    }

    public class FlyweightAttributeInfo<TEntity, TData>
    {
        public AttributeGetterHandler<TEntity, TData> AttributeGetter { get; init; }

        public PropertyInfo AttributeProperty { get; init; }

        public FlyweightAttributeInfo(
            AttributeGetterHandler<TEntity, TData> attributeGetter,
            PropertyInfo attributeProperty)
        {
            AttributeGetter = attributeGetter;
            AttributeProperty = attributeProperty;

            return;
        }
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

        public ITupleObjectAttributeSetupWizard<TData> GetSetupWizard<TData>()
        {
            return SetupWizard as ITupleObjectAttributeSetupWizard<TData>;
        }

        public void SetSetupWizard<TData>(
            ITupleObjectAttributeSetupWizard<TData> setupWizard)
        {
            SetupWizard = setupWizard;

            return;
        }

        public void SetAttributeGetter(Delegate attributeGetter)
        {
            _attributeGetter = attributeGetter;

            return;
        }

        public Func<TEntity, TData> AttributeGetter<TEntity, TData>()
        {
            return _attributeGetter as Func<TEntity, TData>;
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
}
