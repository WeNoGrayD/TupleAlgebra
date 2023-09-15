using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.SpecializedAttributeDomains
{
    public sealed class Integer64OrderedFiniteEnumerableAttributeDomain
        : NumericOrderedFiniteEnumerableAttributeDomain<long>
    {
        public Integer64OrderedFiniteEnumerableAttributeDomain(IEnumerable<long> universum)
            : base(universum)
        { }

        public Integer64OrderedFiniteEnumerableAttributeDomain(
            (long Start, long End, long Stride) range)
            : base(range)
        { }

        public Integer64OrderedFiniteEnumerableAttributeDomain(
            (long Start, long End) range)
            : base(range)
        { }

        public Integer64OrderedFiniteEnumerableAttributeDomain(
            params (long Start, long End, long Stride)[] ranges)
            : base(ranges)
        { }
    }
}
