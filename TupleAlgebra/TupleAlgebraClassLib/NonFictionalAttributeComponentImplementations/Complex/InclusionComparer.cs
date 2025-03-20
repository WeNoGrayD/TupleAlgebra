using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Complex
{
    public class InclusionComparer<TData>
        : NonFictionalAttributeComponentInclusionComparer<
            TData,
            ComplexAttributeComponent<TData>>,
          IComplexAttributeComponentInstantBinaryBooleanOperator<
            TData>,
          IFiniteEnumerableAttributeComponentInclusionComparer<TData>
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
