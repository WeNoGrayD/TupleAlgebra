using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.DecidableNonFictionalAttributeComponent
{
    public abstract class DecidableAttributeDomain<TValue> : AttributeDomain<TValue>
    {
        public DecidableAttributeDomain()
            : base(null)
        { }
    }
}
