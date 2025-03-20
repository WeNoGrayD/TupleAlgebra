using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Iterable.Finite
{
    internal class EqualityComparer<TData>
        : NonFictionalAttributeComponentEqualityComparer<
            TData,
            FiniteIterableAttributeComponent<TData>>,
          IFiniteEnumerableAttributeComponentEqualityComparer<
            TData>
    { }
}
