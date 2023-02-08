using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerableNonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.SpecializedAttributeDomains
{
    /// <summary>
    /// Упорядоченный конечный перечислимый домент аттрибута, основанный на типе перечисления (enum). 
    /// </summary>
    /// <typeparam name="TValue">Тип значений домена.</typeparam>
    public class EnumBasedOrderedFiniteEnumerableAttributeDomain<TValue>
        : OrderedFiniteEnumerableAttributeDomain<TValue>
        where TValue : struct, IComparable
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public EnumBasedOrderedFiniteEnumerableAttributeDomain()
            : base(Enum.GetValues(typeof(TValue)).Cast<TValue>())
        { }
    }
}
