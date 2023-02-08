using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerableNonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.SpecializedAttributeDomains
{
    public sealed class BooleanAttributeDomain : OrderedFiniteEnumerableAttributeDomain<bool>
    {
        public BooleanAttributeDomain()
            : base(new bool[2] { true, false })
        { }
    }
}
