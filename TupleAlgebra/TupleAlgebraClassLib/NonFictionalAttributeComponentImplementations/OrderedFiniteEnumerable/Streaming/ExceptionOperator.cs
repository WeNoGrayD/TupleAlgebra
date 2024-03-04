using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Streaming;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable.Streaming
{
    public class ExceptionOperator<TData>
        : ExceptionOperator<
            TData,
            StreamingOrderedFiniteEnumerableAttributeComponent<TData>,
            IStreamingOrderedFiniteEnumerableAttributeComponentFactory<TData>,
            StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
    { }
}
