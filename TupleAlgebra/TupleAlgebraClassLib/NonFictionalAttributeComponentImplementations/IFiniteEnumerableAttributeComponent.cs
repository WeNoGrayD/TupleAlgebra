using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations
{
    public interface IFiniteEnumerableAttributeComponent<TData> 
        : IAttributeComponent<TData>
    {
        public IEnumerable<TData> Values { get; }
    }
}
