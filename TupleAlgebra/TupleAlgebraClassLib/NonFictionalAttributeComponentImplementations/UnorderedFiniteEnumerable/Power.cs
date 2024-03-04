using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable
{
    /// <summary>
    /// Мощность упорядоченной конечной перечислимой компонентой атрибута.
    /// </summary>
    public class UnorderedFiniteEnumerableNonFictionalAttributeComponentPower<TData>
        : FiniteEnumerableAttributeComponentPower<TData, UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>>
    {
    }
}
