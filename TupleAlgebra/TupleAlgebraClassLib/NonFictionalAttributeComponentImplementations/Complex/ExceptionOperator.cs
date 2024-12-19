using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Complex;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using UniversalClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Complex
{
    public class ExceptionOperator<TData>
        : NonFictionalAttributeComponentExceptionOperator<
            TData,
            TupleObject<TData>,
            ComplexAttributeComponent<TData>,
            IComplexAttributeComponentFactory<TData>,
            ComplexAttributeComponentFactoryArgs<TData>>,
          IComplexAttributeComponentFactoryBinarySetOperator<
            TData>,
          IFiniteEnumerableAttributeComponentExceptionOperator<
            TData,
            ComplexAttributeComponent<TData>,
            IFiniteIterableAttributeComponentFactory<TData>,
            FiniteIterableAttributeComponentFactoryArgs<TData>>
        where TData : new()
    {
        public TupleObject<TData> Operator(
            TupleObject<TData> first,
            TupleObject<TData> second)
        {
            return first / second;
        }
    }
}
