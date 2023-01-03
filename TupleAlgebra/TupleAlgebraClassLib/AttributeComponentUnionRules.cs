using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib
{
    internal static class AttributeComponentUnionRules
    {
        public static EmptyAttributeComponent<TValue> Union<TValue>(
            EmptyAttributeComponent<TValue> first,
            EmptyAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return first;
        }

        public static NonFictionalAttributeComponent<TValue> Union<TValue>(
            EmptyAttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return second;
        }

        public static FullAttributeComponent<TValue> Union<TValue>(
            EmptyAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return second;
        }

        public static AttributeComponent<TValue> Union<TValue>(
            NonFictionalAttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            IEnumerable<TValue> unitedElements = UnionComponentsElements();
            NonFictionalAttributeComponent<TValue> resultComponent =
                AttributeComponentFactory<TValue>.CreateNonFictional(unitedElements);

            return resultComponent;

            IEnumerable<TValue> UnionComponentsElements()
            {
                List<TValue> united = new List<TValue>();
                IEnumerator<TValue> firstEnumerator = first.Values.GetEnumerator(),
                                    secondEnumerator = second.Values.GetEnumerator(),
                                    withLowerBoundEnumerator = firstEnumerator,
                                    withGreaterBoundEnumerator = secondEnumerator;
                bool isContinuesWithLowerBoundEnumerator,
                     isContinuesWithGreaterBoundEnumerator;
                TValue firstElement, secondElement;
                int comparisonResult;

                ReadComponentsUntilAtLeastOneIsOver();
                FinishReadingIfAnyComponentRemains();

                return united;

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
                                    break;
                                }
                            case 1:
                                {
                                    SwapEnumeratorsAndCurrentElements();
                                    break;
                                }
                        }
                        united.Add(firstElement);

                        WithLowerBoundEnumeratorMoveNextAndReadCurrent();
                    }
                }

                void FinishReadingIfAnyComponentRemains()
                {
                    if (isContinuesWithLowerBoundEnumerator || isContinuesWithGreaterBoundEnumerator)
                    {
                        if (isContinuesWithGreaterBoundEnumerator)
                            SwapEnumeratorsAndCurrentElements();
                        FinishReadingRemainingComponent();
                    }
                }

                void FinishReadingRemainingComponent()
                {
                    do
                    {
                        united.Add(firstElement);
                        WithLowerBoundEnumeratorMoveNextAndReadCurrent();
                    }
                    while (isContinuesWithLowerBoundEnumerator);
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
        }

        public static FullAttributeComponent<TValue> Union<TValue>(
            NonFictionalAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return second;
        }

        public static FullAttributeComponent<TValue> Union<TValue>(
            FullAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return first;
        }
    }
}
