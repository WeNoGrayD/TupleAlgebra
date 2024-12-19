using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Complex
{
    public class InclusionOrEqualityComparer<TData>
        : NonFictionalAttributeComponentInclusionOrEqualityComparer<
            TData,
            ComplexAttributeComponent<TData>>,
          IComplexAttributeComponentInstantBinaryBooleanOperator<
            TData>,
          IFiniteEnumerableAttributeComponentInclusionOrEqualityComparer<
            TData>
        where TData : new()
    {
        public bool Operator(
            TupleObject<TData> first,
            TupleObject<TData> second)
        {
            return first >= second;
        }
    }
}
