using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations
{
    public interface ICountableAttributeComponent<TData>
        : IFiniteEnumerableAttributeComponent<TData>
    {
        int Count { get; }
    }
}
