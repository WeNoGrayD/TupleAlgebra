using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Streaming;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Countable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable.Streaming
{
    internal class SymmetricExceptionOperator<TData>
        : SymmetricExceptionOperator<
            TData,
            StreamingOrderedFiniteEnumerableAttributeComponent<TData>,
            IStreamingOrderedFiniteEnumerableAttributeComponentFactory<TData>,
            StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>,
          IFiniteEnumerableAttributeComponentSymmetricExceptionOperator<
            TData,
            StreamingOrderedFiniteEnumerableAttributeComponent<TData>,
            IFiniteIterableAttributeComponentFactory<TData>,
            FiniteIterableAttributeComponentFactoryArgs<TData>>,
          ICountableAttributeComponentSymmetricExceptionOperator<
            TData,
            StreamingOrderedFiniteEnumerableAttributeComponent<TData>,
            IFiniteIterableAttributeComponentFactory<TData>,
            FiniteIterableAttributeComponentFactoryArgs<TData>>
    { }
}
