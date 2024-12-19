using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Complex
{
    internal class EqualityComparer<TData>
        : NonFictionalAttributeComponentEqualityComparer<
            TData,
            ComplexAttributeComponent<TData>>,
          IComplexAttributeComponentInstantBinaryBooleanOperator<
            TData>,
          IFiniteEnumerableAttributeComponentEqualityComparer<
            TData>
        where TData : new()
    {
        public bool Operator(
            TupleObject<TData> first,
            TupleObject<TData> second)
        {
            return first == second;
        }
    }
}
