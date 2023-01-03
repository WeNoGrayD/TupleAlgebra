using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib
{
    internal static class AttributeComponentIntersectionRules
    {
        public static EmptyAttributeComponent<TValue> Intersect<TValue>(
            EmptyAttributeComponent<TValue> first,
            EmptyAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return first;
        }

        public static EmptyAttributeComponent<TValue> Intersect<TValue>(
            EmptyAttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return first;
        }

        public static EmptyAttributeComponent<TValue> Intersect<TValue>(
            EmptyAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return first;
        }

        public static AttributeComponent<TValue> Intersect<TValue>(
            NonFictionalAttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            IEnumerable<TValue> intersectedElements = IntersectComponentsElements();
            NonFictionalAttributeComponent<TValue> resultComponent =
                AttributeComponentFactory<TValue>.CreateNonFictional(intersectedElements);

            return resultComponent;

            IEnumerable<TValue> IntersectComponentsElements()
            {
                List<TValue> intersected = new List<TValue>();
                IEnumerator<TValue> firstEnumerator = first.Values.GetEnumerator(),
                                    secondEnumerator = second.Values.GetEnumerator(),
                                    withLowerBoundEnumerator = firstEnumerator,
                                    withGreaterBoundEnumerator = secondEnumerator;
                bool isContinuesWithLowerBoundEnumerator,
                     isContinuesWithGreaterBoundEnumerator;
                TValue firstElement, secondElement;
                int comparisonResult;

                ReadComponentsUntilAtLeastOneIsOver();

                return intersected;

                void ReadComponentsUntilAtLeastOneIsOver()
                {
                    WithLowerBoundEnumeratorMoveNextAndReadCurrent();
                    WithGreaterBoundEnumeratorMoveNextAndReadCurrent();

                    while (isContinuesWithLowerBoundEnumerator && isContinuesWithGreaterBoundEnumerator)
                    {
                        comparisonResult = firstElement.CompareTo(secondElement);
                        switch (comparisonResult)
                        {
                            case -1: break;
                            case 0:
                                {
                                    WithGreaterBoundEnumeratorMoveNextAndReadCurrent();
                                    intersected.Add(firstElement);
                                    break;
                                }
                            case 1:
                                {
                                    SwapEnumeratorsAndCurrentElements();
                                    break;
                                }
                        }

                        WithLowerBoundEnumeratorMoveNextAndReadCurrent();
                    }
                }

                void SwapEnumeratorsAndCurrentElements()
                {
                    (withLowerBoundEnumerator, withGreaterBoundEnumerator) =
                        (withGreaterBoundEnumerator, withLowerBoundEnumerator);
                    (firstElement, secondElement) = (secondElement, firstElement);
                }

                void WithLowerBoundEnumeratorMoveNextAndReadCurrent()
                {
                    isContinuesWithLowerBoundEnumerator = withLowerBoundEnumerator.MoveNext();
                    firstElement = withLowerBoundEnumerator.Current;
                }

                void WithGreaterBoundEnumeratorMoveNextAndReadCurrent()
                {
                    isContinuesWithGreaterBoundEnumerator = withGreaterBoundEnumerator.MoveNext();
                    secondElement = withGreaterBoundEnumerator.Current;
                }
            }
            /*
            List<TValue> united = new List<TValue>();
            IEnumerator<TValue> firstEnumerator = first.Values.GetEnumerator(),
                                secondEnumerator = second.Values.GetEnumerator();

            TValue firstElement, secondElement;
            while (firstEnumerator.MoveNext() & secondEnumerator.MoveNext())
            {
                (firstElement, secondElement) = (firstEnumerator.Current, secondEnumerator.Current);
                if (firstElement.CompareTo(secondElement) == 0)
                    continue;
                united.Add(firstElement);
            }
            (firstElement, secondElement) = (firstEnumerator.Current, secondEnumerator.Current);
            if (firstEnumerator.MoveNext())
            {

            }
            else
            {

            }

            return null;

            IEnumerable<TValue> AddElementsOfComponentToUnion()
            {
                return null;
            }
            */
        }

        public static NonFictionalAttributeComponent<TValue> Intersect<TValue>(
            NonFictionalAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return first;
        }

        public static FullAttributeComponent<TValue> Intersect<TValue>(
            FullAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return first;
        }
    }
}
