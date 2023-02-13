using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.SpecializedAttributeDomains
{
    public sealed class NumericDoubleOrderedFiniteEnumerableAttributeDomain : OrderedFiniteEnumerableAttributeDomain<double>
    {
        public NumericDoubleOrderedFiniteEnumerableAttributeDomain(IEnumerable<double> universum)
            : base(universum)
        { }

        public NumericDoubleOrderedFiniteEnumerableAttributeDomain((double Start, double End, double Stride) range)
            : base(BuildDomain(range))
        { }

        public NumericDoubleOrderedFiniteEnumerableAttributeDomain(params (double Start, double End, double Stride)[] ranges)
            : base(BuildDomain(ranges))
        { }

        private static IEnumerable<double> BuildDomain((double Start, double End, double Stride) range)
        {
            for (double i = range.Start; i < range.End; i += range.Stride)
                yield return i;
        }

        private static IEnumerable<double> BuildDomain(IEnumerable<(double, double, double)> ranges)
        {
            foreach (var range in ranges)
                foreach (var sample in BuildDomain(range))
                    yield return sample;
        }
    }
}
