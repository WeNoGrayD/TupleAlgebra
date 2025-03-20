using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Iterable.Finite
{
    public class InclusionOrEqualityComparer<TData>
        : NonFictionalAttributeComponentInclusionOrEqualityComparer<
            TData,
            FiniteIterableAttributeComponent<TData>>,
          IFiniteEnumerableAttributeComponentInclusionOrEqualityComparer<
            TData>
    { }
}
