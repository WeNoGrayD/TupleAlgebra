using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerableNonFictionalAttributeComponentInfrastructure
{
    public sealed class OrderedFiniteEnumerableNonFictionalAttributeComponentEqualityComparer<TValue>
        : InstantBinaryOrderedFiniteEnumerableNonFictionalAttributeComponentAcceptor<TValue>,
          IInstantAttributeComponentAcceptor<TValue, OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue>, OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue>, bool>
    {
        public bool Accept(OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue> first, OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue> second)
        {
            return Enumerable.SequenceEqual(first, second);
        }
    }
}
