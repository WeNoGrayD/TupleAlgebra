using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Buffered;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Countable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable.Buffered;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable.Buffered
{
    public class SymmetricExceptionOperator<TData>
        : SymmetricExceptionOperator<
            TData,
            BufferedOrderedFiniteEnumerableAttributeComponent<TData>,
            IBufferedOrderedFiniteEnumerableAttributeComponentFactory<TData>,
            BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>,
          IFiniteEnumerableAttributeComponentSymmetricExceptionOperator<
            TData,
            BufferedOrderedFiniteEnumerableAttributeComponent<TData>,
            IBufferedOrderedFiniteEnumerableAttributeComponentFactory<TData>,
            BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>,
          ICountableAttributeComponentSymmetricExceptionOperator<
            TData,
            BufferedOrderedFiniteEnumerableAttributeComponent<TData>,
            IBufferedOrderedFiniteEnumerableAttributeComponentFactory<TData>,
            BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
    { }
}
