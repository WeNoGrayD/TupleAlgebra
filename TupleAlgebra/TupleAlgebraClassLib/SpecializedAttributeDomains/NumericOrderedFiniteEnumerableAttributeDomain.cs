using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.SpecializedAttributeDomains
{
    /*
    public class NumericOrderedFiniteEnumerableAttributeDomain<TNumeric> 
        : OrderedFiniteEnumerableAttributeDomain<TNumeric>
        where TNumeric : INumber<TNumeric>
    {
        public NumericOrderedFiniteEnumerableAttributeDomain(
            IEnumerable<TNumeric> universe)
            : base(universe)
        { }

        public NumericOrderedFiniteEnumerableAttributeDomain(
            (TNumeric Start, TNumeric End, TNumeric Stride) range)
            : base(BuildDomain(range))
        { }

        public NumericOrderedFiniteEnumerableAttributeDomain(
            (TNumeric Start, TNumeric End) range)
            : this((range.Start, range.End, range.Start <= range.End ? TNumeric.One : -TNumeric.One))
        { }

        public NumericOrderedFiniteEnumerableAttributeDomain(
            params (TNumeric Start, TNumeric End, TNumeric Stride)[] ranges)
            : base(BuildDomain(ranges))
        { }

        private static IEnumerable<TNumeric> BuildDomain(
            (TNumeric Start, TNumeric End, TNumeric Stride) range)
        {
            for (TNumeric i = range.Start; i < range.End; i += range.Stride)
                yield return i;
        }

        private static IEnumerable<TNumeric> BuildDomain(
            IEnumerable<(TNumeric Start, TNumeric End, TNumeric Stride)> ranges)
        {
            foreach (var range in ranges)
                foreach (var sample in BuildDomain(range))
                    yield return sample;
        }
    }
    */
}
