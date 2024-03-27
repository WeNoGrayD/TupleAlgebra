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
    public class ExceptionOperator<TData>
        : NonFictionalAttributeComponentExceptionOperator<
            TData,
            IEnumerable<TData>,
            FiniteIterableAttributeComponent<TData>,
            IFiniteIterableAttributeComponentFactory<TData>,
            FiniteIterableAttributeComponentFactoryArgs<TData>>,
          IFiniteEnumerableAttributeComponentExceptionOperator<
            TData,
            FiniteIterableAttributeComponent<TData>,
            IFiniteIterableAttributeComponentFactory<TData>,
            FiniteIterableAttributeComponentFactoryArgs<TData>>
    { }
}
