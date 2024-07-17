using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.Filtering;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.TupleBased;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Variable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Variable;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    using static TupleObjectHelper;

    public abstract class TupleObjectAttributeSetupWizard<TAttribute> :
        ITupleObjectAttributeSetupWizard<TAttribute>
    {
        public ITupleObjectSchemaProvider Schema { get; protected set; }

        public AttributeName AttributeName { get => _attributeName; }

        protected AttributeName _attributeName;

        protected virtual ITupleObjectAttributeInfo<TAttribute> AttributeInfo
        {
            get => (Schema[_attributeName] as ITupleObjectAttributeInfo<TAttribute>)!;
            set => Schema[_attributeName] = value;
        }

        protected TupleObjectAttributeSetupWizard(
            ITupleObjectSchemaProvider schema,
            LambdaExpression memberAccess)
        {
            Schema = schema;
            _attributeName = MemberExtractor.ExtractFrom(memberAccess).Name;

            return;
        }

        protected TupleObjectAttributeSetupWizard(
            ITupleObjectSchemaProvider schema,
            string attributeName)
        {
            Schema = schema;
            _attributeName = attributeName;

            return;
        }

        public IAttributeComponentFactory<TAttribute> GetFactory()
        {
            return AttributeInfo.ComponentFactory;
        }

        public virtual ITupleObjectAttributeSetupWizard<TAttribute> SetFactory(
            IAttributeComponentFactory<TAttribute> factory)
        {
            Schema[_attributeName] = AttributeInfo.SetFactory(factory);

            return this;
        }

        /*
        /// <summary>
        /// Установка домена атрибута с проекцией.
        /// Побочным эффектом реализации метода является включение атрибута в схему.  
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public ITupleObjectAttributeSetupWizard<TAttribute> SetDomain<TDomain>(
            AttributeDomain<TDomain> domain,
            Func<TDomain, TAttribute> selector)
        {
            AttributeDomain<TAttribute> domain2 = domain.Select(selector);
            Schema[_attributeName] = AttributeInfo.Construct(true, domain2);
            return this;
        }

        /// <summary>
        /// Установка домена атрибута со множественной проекцией.
        /// Побочным эффектом реализации метода является включение атрибута в схему.  
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public ITupleObjectAttributeSetupWizard<TAttribute> SetDomain<TDomain>(
            AttributeDomain<TDomain> domain,
            Func<TDomain, IEnumerable<TAttribute>> selector)
        {
            AttributeDomain<TAttribute> domain2 = domain.Select(selector);
            Schema[_attributeName] = AttributeInfo.Construct(true, domain2);
            return this;
        }
        */

        /// <summary>
        /// Установка отношения эквивалентности по атрибуту. 
		/// (сжатие объектов по этому атрибуту).
		/// (можно не сжимать, тогда кортеж будет раздутый).
        /// </summary>
        /// <returns></returns>
        public ITupleObjectAttributeSetupWizard<TAttribute> SetEquivalenceRelation()
        {
            /*
            AttributeInfo attribute = Schema[_attributeName].Value;
            Schema[_attributeName] = attribute.CloneWith(
                isPlugged: attribute.IsPlugged,
                hasEquivalenceRelation: true);

            return this;
            */

            return this;
        }

        public ITupleObjectAttributeSetupWizard<TAttribute> SetEquivalenceRelation(
            IEqualityComparer<TAttribute> equivalenceRelationComparer)
        {
            /*
            AttributeInfo attribute = Schema[_attributeName].Value;
            Schema[_attributeName] = attribute.CloneWith(
                isPlugged: attribute.IsPlugged,
                equivalenceRelationComparer: equivalenceRelationComparer);

            return this;
            */

            return this;
        }

        public ITupleObjectAttributeSetupWizard<TAttribute> SetEquivalenceRelation(
            Func<TAttribute, TAttribute, bool> equivalenceRelation)
        {
            /*
            AttributeInfo attribute = Schema[_attributeName].Value;
            Schema[_attributeName] = attribute.CloneWith(
                isPlugged: attribute.IsPlugged,
                equivalenceRelation: equivalenceRelation);

            return this;
            */

            return this;
        }

        /// <summary>
        /// Деинсталляция отношения эквивалентности по атрибуту.
        /// </summary>
        /// <returns></returns>
        public ITupleObjectAttributeSetupWizard<TAttribute> UnsetEquivalenceRelation()
        {
            /*
            AttributeInfo attribute = Schema[_attributeName].Value;
            Schema[_attributeName] = attribute.CloneWith(
                isPlugged: attribute.IsPlugged,
                hasEquivalenceRelation: false);

            return this;
            */

            return this;
        }

        /// <summary>
        /// Игнорирование атрибута.
        /// </summary>
        /// <returns></returns>
        public ITupleObjectAttributeSetupWizard<TAttribute> Ignore()
        {
            Schema.RemoveAttribute(_attributeName);

            return this;
        }

        public ITupleObjectAttributeSetupWizard<TAttribute> Attach()
        {
            Schema.AttachAttribute(_attributeName);

            return this;
        }

        public ITupleObjectAttributeSetupWizard<TAttribute> Detach()
        {
            Schema.DetachAttribute(_attributeName);

            return this;
        }

        public virtual ITupleObjectAttributeManager CreateManager()
        {
            return new TupleObjectAttributeManager<TAttribute>(this);
        }
    }

    /*
    public abstract class TupleObjectNavigationalAttributeSetupWizard<TKey, TAttribute> 
        : TupleObjectAttributeSetupWizard<KeyValuePair<TKey, TAttribute>>
    {
        protected TupleObjectNavigationalAttributeSetupWizard(
            ITupleObjectSchemaProvider schema,
            LambdaExpression memberAccess)
            : base(schema, memberAccess)
        {
            return;
        }

        protected TupleObjectNavigationalAttributeSetupWizard(
            ITupleObjectSchemaProvider schema,
            string attributeName)
            : base(schema, attributeName)
        {
            return;
        }

        public override ITupleObjectAttributeSetupWizard<KeyValuePair<TKey, TAttribute>> 
            SetFactory(
            IAttributeComponentFactory<KeyValuePair<TKey, TAttribute>> factory)
        {
            // Этот метод ничего не делает, поскольку
            // фабрика навигационного атрибута статичная.
            return this;
        }

        public override ITupleObjectAttributeManager CreateManager()
        {
            return new TupleObjectAttributeManager<KeyValuePair<TKey, TAttribute>>(this);
        }
    }
    */
}
