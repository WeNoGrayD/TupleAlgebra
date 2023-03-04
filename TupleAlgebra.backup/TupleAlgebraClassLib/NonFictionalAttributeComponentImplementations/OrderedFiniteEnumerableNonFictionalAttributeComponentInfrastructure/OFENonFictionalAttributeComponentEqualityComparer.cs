using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable
{
    public sealed class OrderedFiniteEnumerableNonFictionalAttributeComponentEqualityComparer<TData>
        : InstantBinaryOrderedFiniteEnumerableNonFictionalAttributeComponentAcceptor<TData>,
          IInstantAttributeComponentAcceptor<TData, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>, bool>
    {
        public bool Accept(OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> first, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> second)
        {
            return Enumerable.SequenceEqual(first, second);
        }
    }
}
