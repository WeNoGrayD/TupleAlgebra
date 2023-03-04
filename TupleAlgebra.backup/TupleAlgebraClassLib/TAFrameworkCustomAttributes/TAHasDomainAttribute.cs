using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TupleAlgebraClassLib.TAFrameworkCustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public abstract class TAHasDomainAttribute : Attribute
    {
        private object _domain;

        protected TAHasDomainAttribute(Type domainType, string domainNature, object[] domainInitializationParameters)
        {

        }
    }

    public class TAHasOrderedFiniteEnumerableDomainAttribute : TAHasDomainAttribute
    {
        protected TAHasOrderedFiniteEnumerableDomainAttribute(
            Type domainType, object[] domainInitializationParameters)
            : base(domainType, "OrderedFiniteEnumerable", domainInitializationParameters)
        {

        }
    }

    public class TAHasBooleanDomainAttribute : TAHasOrderedFiniteEnumerableDomainAttribute
    {
        static TAHasBooleanDomainAttribute()
        {

        }

        protected TAHasBooleanDomainAttribute()
            : base(typeof(bool), new object[0])
        {

        }
    }

    /// <summary>
    /// Универсальный тип для упорядоченных конечных перечислимых доменов аттрибутов числового типа. 
    /// </summary>
    public class TAHasOrderedFiniteEnumerableNumericDomainAttribute : TAHasOrderedFiniteEnumerableDomainAttribute
    {
        static TAHasOrderedFiniteEnumerableNumericDomainAttribute()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numericType"></param>
        /// <param name="domainInitializationParameters"></param>
        public TAHasOrderedFiniteEnumerableNumericDomainAttribute(
            Type numericType, params object[] domainInitializationParameters)
            : base(numericType, BuildDomainInitializationParameters(numericType, domainInitializationParameters))
        { }

        private static object[] BuildDomainInitializationParameters(
            Type numericType, object[] domainInitializationParameters)
        {
            if (domainInitializationParameters.Length == 0)
                return domainInitializationParameters;
            //Type tupleType = TypeInfo.
            return Array.ConvertAll(domainInitializationParameters, val => Convert.ChangeType(val, numericType));
        }
    }
}
