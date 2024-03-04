using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Streaming;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable.Streaming
{
    public class IntersectionOperator<TData> 
        : IntersectionOperator<
            TData, 
            StreamingOrderedFiniteEnumerableAttributeComponent<TData>, 
            IStreamingOrderedFiniteEnumerableAttributeComponentFactory<TData>,
            StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
    { }
}
