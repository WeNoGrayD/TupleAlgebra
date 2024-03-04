using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Streaming;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable.Streaming
{
    internal class UnionOperator<TData>
        : UnionOperator<
            TData,
            StreamingOrderedFiniteEnumerableAttributeComponent<TData>,
            IStreamingOrderedFiniteEnumerableAttributeComponentFactory<TData>,
            StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
    { }
}
