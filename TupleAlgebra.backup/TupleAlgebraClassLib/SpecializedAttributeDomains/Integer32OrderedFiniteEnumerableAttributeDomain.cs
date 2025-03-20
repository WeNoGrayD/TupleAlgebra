using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.SpecializedAttributeDomains
{
    public sealed class Integer32OrderedFiniteEnumerableAttributeDomain : OrderedFiniteEnumerableAttributeDomain<int>
    {
        public Integer32OrderedFiniteEnumerableAttributeDomain(IEnumerable<int> universum)
            : base(universum)
        { }

        public Integer32OrderedFiniteEnumerableAttributeDomain((int Start, int End, int Stride) range)
            : base(BuildDomain(range))
        { }

        public Integer32OrderedFiniteEnumerableAttributeDomain(params (int Start, int End, int Stride)[] ranges)
            : base(BuildDomain(ranges))
        { }

        private static IEnumerable<int> BuildDomain((int Start, int End, int Stride) range)
        {
            for (int i = range.Start; i < range.End; i += range.Stride)
                yield return i;
        }

        private static IEnumerable<int> BuildDomain(IEnumerable<(int, int, int)> ranges)
        {
            foreach (var range in ranges)
                foreach (var sample in BuildDomain(range))
                    yield return sample;
        }

        /*
        private static IEnumerable<int> BuildDomain(IEnumerable<(int, int, int)> ranges)
        {
            List<(int Start, int End, int Stride)> normalizedRanges = NormaliseRanges();
            var rangesSortedByRangeStart = normalizedRanges.ToLookup(range => range.Start);
            var rangesSortedByRangeEnd = normalizedRanges.ToLookup(range => range.End);
            bool isContinuesLowerBoundGroupingEnumerator,
                 isContinuesGreaterBoundGroupingEnumerator;

            List<IGrouping<int, (int, int, int)>> lowerBoundGroupings =
                rangesSortedByRangeStart.ToList(), 
                                                  greaterBoundGroupings =
                rangesSortedByRangeEnd.ToList();
            lowerBoundGroupings.Sort((group1, group2) => group1.Key.CompareTo(group2.Key));
            greaterBoundGroupings.Sort((group1, group2) => group1.Key.CompareTo(group2.Key));

            var lowerBoundGroupingEnumerator = lowerBoundGroupings.GetEnumerator();
            var greaterBoundGroupingEnumerator = greaterBoundGroupings.GetEnumerator();

            List<(int Start, int End, int Stride)> lowerBoundRanges, greaterBoundRanges, 
                                                   resultDomain = new List<(int Start, int End, int Stride)>();
            IEnumerator<(int Start, int End, int Stride)> lowerBoundRangeEnumerator,
                                                          greaterBoundRangeEnumerator;
            bool isContinuesLowerBoundRangeEnumerator,
                 isContinuesGreaterBoundRangeEnumerator;
            (int Start, int End, int Stride) lowerBoundRange, greaterBoundRange;
            int elementsComparisonResult;

            LowerBoundGroupingEnumeratorMoveNextAndReadCurrent();
            GreaterBoundGroupingEnumeratorMoveNextAndReadCurrent();
            LowerBoundRangeEnumeratorMoveNextAndReadCurrent();
            GreaterBoundRangeEnumeratorMoveNextAndReadCurrent();

            while (isContinuesLowerBoundGroupingEnumerator && isContinuesGreaterBoundGroupingEnumerator)
            {
                while (isContinuesLowerBoundGroupingEnumerator)
                {
                    int j = 0;
                    greaterBoundRange = greaterBoundRanges[j];
                    while (isContinuesGreaterBoundGroupingEnumerator)
                    {
                        while (!lowerBoundRange.Equals(greaterBoundRange))
                        {
                            for (int i = 0; i < lowerBoundRanges.Count; i++)
                            {
                                lowerBoundRange = lowerBoundRanges[i];
                                if (!lowerBoundRange.Equals(greaterBoundRange))
                                {
                                    resultDomain.Add((lowerBoundRange.Start, greaterBoundRange.Start, lowerBoundRange.Stride));
                                }
                                else
                                    break;
                            }
                            greaterBoundRange = greaterBoundRanges[++j];
                        }
                        GreaterBoundRangeEnumeratorMoveNextAndReadCurrent();
                    }
                    resultDomain.Add((greaterBoundRange.End, lowerBoundRange.End, lowerBoundRange.Stride));
                    LowerBoundRangeEnumeratorMoveNextAndReadCurrent();
                }
            }

            return null;

            List<(int, int, int)> NormaliseRanges()
            {
                List<(int, int, int)> normalized = new List<(int, int, int)>(ranges);
                (int Start, int End, int Stride) range;
                for (int i = 0; i < normalized.Count; i++)
                {
                    range = normalized[i];
                    if (range.Start > range.End)
                        normalized[i] = (range.End + 1, range.Start + 1, -range.Stride);
                }

                return normalized;
            }

            int RangeComparisonByRangeStart(
                (int Start, int End, int Stride) range1,
                (int Start, int End, int Stride) range2)
            {
                int cmpRes = range1.Start.CompareTo(range2.Start);
                if (cmpRes == 0)
                    cmpRes = range1.Stride.CompareTo(range2.Stride);

                return cmpRes;
            }

            int RangeComparisonByRangeEnd(
                (int Start, int End, int Stride) range1,
                (int Start, int End, int Stride) range2)
            {
                int cmpRes = range1.End.CompareTo(range2.End);
                if (cmpRes == 0)
                    cmpRes = range1.Stride.CompareTo(range2.Stride);

                return cmpRes;
            }

            void LowerBoundGroupingEnumeratorMoveNextAndReadCurrent()
            {
                isContinuesLowerBoundGroupingEnumerator = lowerBoundGroupingEnumerator.MoveNext();
                lowerBoundRanges = lowerBoundGroupingEnumerator.Current.ToList();
                lowerBoundRanges.Sort(RangeComparisonByRangeEnd);
                lowerBoundRangeEnumerator = lowerBoundRanges.GetEnumerator();
            }

            void LowerBoundRangeEnumeratorStart()
            {
                //lowerBoundGroupingEnumerator
                //LowerBoundGroupingEnumeratorMoveNextAndReadCurrent();
            }
            
            void GreaterBoundGroupingEnumeratorMoveNextAndReadCurrent()
            {
                isContinuesGreaterBoundGroupingEnumerator = greaterBoundGroupingEnumerator.MoveNext();
                greaterBoundRanges = greaterBoundGroupingEnumerator.Current.ToList();
                greaterBoundRanges.Sort(RangeComparisonByRangeStart);
                greaterBoundRangeEnumerator = greaterBoundRanges.GetEnumerator();
            }

            void LowerBoundRangeEnumeratorMoveNextAndReadCurrent()
            {
                isContinuesLowerBoundRangeEnumerator = lowerBoundRangeEnumerator.MoveNext();
                lowerBoundRange = lowerBoundRangeEnumerator.Current;
            }

            void GreaterBoundRangeEnumeratorMoveNextAndReadCurrent()
            {
                isContinuesGreaterBoundRangeEnumerator = greaterBoundRangeEnumerator.MoveNext();
                greaterBoundRange = greaterBoundRangeEnumerator.Current;
            }
        }
        */
    }
}
