using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    public abstract class TupleObjectAttributeSetupWizard<TAttribute> :
        ITupleObjectAttributeSetupWizard<TAttribute>
    {
        public ITupleObjectSchemaProvider Schema { get; protected set; }

        protected string _attributeName;

        protected TupleObjectAttributeSetupWizard(
            ITupleObjectSchemaProvider schema,
            LambdaExpression memberAccess)
        {
            AttributeGetterImpl(memberAccess);
            AttributeInfo attribute = schema[_attributeName].Value;
            attribute.SetGetter(memberAccess.Compile());
            schema[_attributeName] = attribute;
            Schema = schema;

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

        private void AttributeGetterImpl(LambdaExpression memberAccess)
        {
            MemberExpression memberAccessExpr = memberAccess.Body as MemberExpression;
            _attributeName = memberAccessExpr.Member.Name;

            return;
        }

        /// <summary>
        /// Установка домена атрибута.
        /// Побочным эффектом реализации метода является включение атрибута в схему.  
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public ITupleObjectAttributeSetupWizard<TAttribute> SetDomain(
            AttributeDomain<TAttribute> domain)
        {
            AttributeInfo attribute = Schema[_attributeName].Value;
            Schema[_attributeName] = attribute.CloneWith(
                true, attribute.HasEquivalenceRelation, domain);

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
            AttributeInfo attribute = Schema[_attributeName].Value;
            Schema[_attributeName] = attribute.CloneWith(
                attribute.IsPlugged, true);

            return this;
        }

        /// <summary>
        /// Деинсталляция отношения эквивалентности по атрибуту.
        /// </summary>
        /// <returns></returns>
        public ITupleObjectAttributeSetupWizard<TAttribute> UnsetEquivalenceRelation()
        {
            AttributeInfo attribute = Schema[_attributeName].Value;
            Schema[_attributeName] = attribute.CloneWith(
                attribute.IsPlugged, false);

            return this;
        }

        /// <summary>
        /// Игнорирование атрибута.
        /// </summary>
        /// <returns></returns>
        public ITupleObjectAttributeSetupWizard<TAttribute> Ignore()
        {
            Schema[_attributeName] = null;

            return this;
        }
    }
}
