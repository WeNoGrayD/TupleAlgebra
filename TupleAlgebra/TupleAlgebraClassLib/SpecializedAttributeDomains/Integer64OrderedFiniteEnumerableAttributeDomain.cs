using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.SpecializedAttributeDomains
{
    public sealed class Integer64OrderedFiniteEnumerableAttributeDomain : OrderedFiniteEnumerableAttributeDomain<long>
    {
        public Integer64OrderedFiniteEnumerableAttributeDomain(IEnumerable<long> universum)
            : base(universum)
        { }

        public Integer64OrderedFiniteEnumerableAttributeDomain((long Start, long End, long Stride) range)
            : base(BuildDomain(range))
        { }

        public Integer64OrderedFiniteEnumerableAttributeDomain(params (long Start, long End, long Stride)[] ranges)
            : base(BuildDomain(ranges))
        { }

        private static IEnumerable<long> BuildDomain((long Start, long End, long Stride) range)
        {
            for (long i = range.Start; i < range.End; i += range.Stride)
                yield return i;
        }

        private static IEnumerable<long> BuildDomain(IEnumerable<(long, long, long)> ranges)
        {
            foreach (var range in ranges)
                foreach (var sample in BuildDomain(range))
                    yield return sample;
        }
    }
}
