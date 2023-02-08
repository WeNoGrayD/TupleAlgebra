using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerableNonFictionalAttributeComponentInfrastructure
{
    public abstract class InstantBinaryOrderedFiniteEnumerableNonFictionalAttributeComponentAcceptor<TValue>
        : InstantBinaryAttributeComponentAcceptor<TValue, bool>
    {
        protected static IComparer<TValue> _orderingComparer;

        internal static void InitOrderingComparer(IComparer<TValue> orderingComparer)
        {
            _orderingComparer = orderingComparer;
        }
    }
}
