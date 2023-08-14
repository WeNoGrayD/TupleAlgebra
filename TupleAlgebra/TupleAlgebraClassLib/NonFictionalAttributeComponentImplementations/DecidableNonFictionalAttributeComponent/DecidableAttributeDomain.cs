using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.DecidableNonFictionalAttributeComponent
{
    public abstract class DecidableAttributeDomain<TData> : AttributeDomain<TData>
    {
        public DecidableAttributeDomain()
            : base((System.Linq.Expressions.Expression)null)
        { }
    }
}
