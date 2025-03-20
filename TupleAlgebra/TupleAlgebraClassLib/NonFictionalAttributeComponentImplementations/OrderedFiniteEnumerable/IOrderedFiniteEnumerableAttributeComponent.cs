using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable
{
    public interface IOrderedFiniteEnumerableAttributeComponent<TData>
        : IFiniteEnumerableAttributeComponent<TData>
    {
        IComparer<TData> OrderingComparer { get; }
    }
}
