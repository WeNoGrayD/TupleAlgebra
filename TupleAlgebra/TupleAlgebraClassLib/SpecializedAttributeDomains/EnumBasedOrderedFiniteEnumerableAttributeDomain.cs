using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.SpecializedAttributeComponents.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.SpecializedAttributeDomains
{
    /// <summary>
    /// Упорядоченный конечный перечислимый домент аттрибута, основанный на типе перечисления (enum). 
    /// </summary>
    /// <typeparam name="TData">Тип значений домена.</typeparam>
    public class EnumBasedOrderedFiniteEnumerableAttributeDomain<TData>
        : OrderedFiniteEnumerableAttributeDomain<EnumBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<TData>, TData>
        where TData : Enum
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public EnumBasedOrderedFiniteEnumerableAttributeDomain()
            : base(Enum.GetValues(typeof(TData)).Cast<TData>())
        { }
    }
}
