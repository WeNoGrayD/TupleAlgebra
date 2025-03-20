using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace TupleAlgebraClassLib.AlgebraicTupleInfrastructure
{
    public abstract class AlgebraicTupleAttributeSetupWizard<TAttribute> :
        IAlgebraicTupleAttributeSetupWizard<TAttribute>
    {
        public IAlgebraicTupleSchemaProvider Schema { get; protected set; }

        protected string _attributeName;

        protected AlgebraicTupleAttributeSetupWizard(
            IAlgebraicTupleSchemaProvider schema,
            LambdaExpression memberAccess)
        {
            Schema = schema;
            AttributeGetterImpl(memberAccess);
        }

        protected AlgebraicTupleAttributeSetupWizard(
            IAlgebraicTupleSchemaProvider schema,
            string attributeName)
        {
            Schema = schema;
            _attributeName = attributeName;
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
        public IAlgebraicTupleAttributeSetupWizard<TAttribute> SetDomain(
            AttributeDomain<TAttribute> domain)
        {
            Schema[_attributeName] = AttributeInfo.Construct(true, domain);
            return this;
        }

        /*
        /// <summary>
        /// Установка домена атрибута с проекцией.
        /// Побочным эффектом реализации метода является включение атрибута в схему.  
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public IAlgebraicTupleAttributeSetupWizard<TAttribute> SetDomain<TDomain>(
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
        public IAlgebraicTupleAttributeSetupWizard<TAttribute> SetDomain<TDomain>(
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
        /// </summary>
        /// <returns></returns>
        public IAlgebraicTupleAttributeSetupWizard<TAttribute> SetEquivalenceRelation()
        {
            return this;
        }

        /// <summary>
        /// Деинсталляция отношения эквивалентности по атрибуту.
        /// </summary>
        /// <returns></returns>
        public IAlgebraicTupleAttributeSetupWizard<TAttribute> UnsetEquivalenceRelation()
        {
            return this;
        }

        /// <summary>
        /// Игнорирование атрибута.
        /// </summary>
        /// <returns></returns>
        public IAlgebraicTupleAttributeSetupWizard<TAttribute> Ignore()
        {
            Schema[_attributeName] = null;
            return this;
        }
    }
}
