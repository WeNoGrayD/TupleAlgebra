using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Buffered;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable.Buffered
{
    public class ExceptionOperator<TData>
        : ExceptionOperator<
            TData,
            BufferedOrderedFiniteEnumerableAttributeComponent<TData>,
            IBufferedOrderedFiniteEnumerableAttributeComponentFactory<TData>,
            BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
    { }
}
