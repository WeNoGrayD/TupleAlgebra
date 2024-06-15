using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Iterable.Finite
{
    public class InclusionComparer<TData>
        : NonFictionalAttributeComponentInclusionComparer<
            TData,
            FiniteIterableAttributeComponent<TData>>,
          IFiniteEnumerableAttributeComponentInclusionComparer<TData>
    { }
}
