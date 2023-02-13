using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.SpecializedAttributeDomains
{
    public sealed class NumericSingleOrderedFiniteEnumerableAttributeDomain : OrderedFiniteEnumerableAttributeDomain<float>
    {
        public NumericSingleOrderedFiniteEnumerableAttributeDomain(IEnumerable<float> universum)
            : base(universum)
        { }

        public NumericSingleOrderedFiniteEnumerableAttributeDomain((float Start, float End, float Stride) range)
            : base(BuildDomain(range))
        { }

        public NumericSingleOrderedFiniteEnumerableAttributeDomain(params (float Start, float End, float Stride)[] ranges)
            : base(BuildDomain(ranges))
        { }

        private static IEnumerable<float> BuildDomain((float Start, float End, float Stride) range)
        {
            for (float i = range.Start; i < range.End; i += range.Stride)
                yield return i;
        }

        private static IEnumerable<float> BuildDomain(IEnumerable<(float, float, float)> ranges)
        {
            foreach (var range in ranges)
                foreach (var sample in BuildDomain(range))
                    yield return sample;
        }
    }
}
