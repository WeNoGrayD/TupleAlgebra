using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable
{
    public interface IStreamingValuesProvider<TData>
        : IAttributeComponent<TData>
    {
        TData[] Values { get; }

        ReadOnlySequence<TData> GetStream()
        {
            return new ReadOnlySequence<TData>(Values);
        }
    }
}
