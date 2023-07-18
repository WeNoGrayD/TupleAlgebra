using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    /// <summary>
    /// Информация об атрибуте в составе схемы кортежа.
    /// </summary>
    public struct AttributeInfo
    {
        #region Instance fields

        /// <summary>
        /// Делегат получения значения атрибута из сущности.
        /// </summary>
        private Delegate _getter;

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
            bool isPlugged, 
            Type domainDataType,
            IAttributeComponentProvider domain = null,
            Delegate getter = null,
            bool hasEquivalenceRelation = false)
        {
            IsPlugged = isPlugged;
            HasEquivalenceRelation = hasEquivalenceRelation;
            Domain = domain;
            DomainDataType = domainDataType;
            _getter = getter;
            SetupWizard = null;
        }

        #endregion

        public static AttributeInfo Construct<TAttribute>(
            bool isPlugged, 
            AttributeDomain<TAttribute> domain = null,
            Delegate getter = null,
            bool hasEquivalenceRelation = false)
        {
            return new AttributeInfo(isPlugged, typeof(TAttribute), domain, getter, hasEquivalenceRelation);
        }

        public AttributeInfo CloneWith(
            bool isPlugged,
            bool hasEquivalenceRelation,
            IAttributeComponentProvider domain = null,
            Delegate getter = null)
        {
            return this with { 
                IsPlugged = isPlugged, 
                HasEquivalenceRelation = hasEquivalenceRelation,
                Domain = domain ?? this.Domain,
                _getter = getter ?? this._getter};
        }

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

        public dynamic GetSetupWizard()
        {
            return SetupWizard;
        }

        public void SetSetupWizard<TData>(
            ITupleObjectAttributeSetupWizard<TData> setupWizard)
        {
            SetupWizard = setupWizard;

            return;
        }

        public void SetGetter(Delegate getter)
        {
            _getter = getter;

            return;
        }

        public Func<TEntity, TData> Getter<TEntity, TData>()
        {
            return _getter as Func<TEntity, TData>;
        }

        public AttributeInfo PlugIn()
        {
            return this with { IsPlugged = true };
        }

        public AttributeInfo UnPlug()
        {
            return this with { IsPlugged = false };
        }

        public AttributeInfo SetEquivalenceRelation()
        {
            return this with { HasEquivalenceRelation = true };
        }

        public AttributeInfo UnsetEquivalenceRelation()
        {
            return this with { HasEquivalenceRelation = false };
        }

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
    }
}
