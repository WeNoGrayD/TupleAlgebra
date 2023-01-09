using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TupleAlgebraClassLib
{
    /*
    static class helper
    {
        public static TOperationResult Accept<TOperationResult, TValue>(this AttributeComponentAcceptor<TOperationResult> acceptor, AttributeComponent<TValue> first, FiniteEnumerableNonFictionalAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var data = acceptor.Accept<NonFictionalAttributeComponent<TValue>, TValue>(first, second);
            sw.Stop();
            var (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            return data;
        }
    }
    */

    public class FiniteEnumerableNonFictionalAttributeComponent<TValue> : NonFictionalAttributeComponent<TValue>
        where TValue : IComparable<TValue>
    {
        private const string NATURE_TYPE = "FiniteEnumerable";

        protected override string NatureType { get => NATURE_TYPE; }

        private IEnumerable<TValue> _values;

        public int Value { get => _values.Count(); }

        static FiniteEnumerableNonFictionalAttributeComponent()
        {
            NonFictionalAttributeComponent<TValue>.InitSetOperations(
                NATURE_TYPE,
                new FiniteEnumerableNonFictionalAttributeComponentOperationExecutersContainer());
        }

        public FiniteEnumerableNonFictionalAttributeComponent(
            AttributeDomain<TValue> domain,
            IEnumerable<TValue> values)
            : base(domain, new FiniteEnumerableNonFictionalAttributeComponentPower(values))
        {
            List<TValue> sortedValues = new List<TValue>(values);
            sortedValues.Sort();
            _values = sortedValues;
        }

        public override bool IsEmpty()
        {
            return _values.Count() == 0;
        }

        public override IEnumerator<TValue> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        private class FiniteEnumerableNonFictionalAttributeComponentOperationExecutersContainer : SetOperationExecutersContainer
        {
            public FiniteEnumerableNonFictionalAttributeComponentOperationExecutersContainer() : base(
                new FiniteEnumerableNonFictionalAttributeComponentIntersectionOperator(),
                new FiniteEnumerableNonFictionalAttributeComponentUnionOperator(),
                null,
                null,
                new FiniteEnumerableNonFictionalAttributeComponentInclusionOrEqualityComparer())
            { }
        }

        public class FiniteEnumerableNonFictionalAttributeComponentPower : NonFictionalAttributeComponentPower
        {
            private IEnumerable<TValue> _componentValues;

            public int Value { get => _componentValues.Count(); }

            public FiniteEnumerableNonFictionalAttributeComponentPower(IEnumerable<TValue> componentValues)
            {
                _componentValues = componentValues;
            }

            protected override int CompareToSame(dynamic second)
            {
                if (second is FiniteEnumerableNonFictionalAttributeComponentPower second2)
                    return this.CompareToSame(second);
                else
                    throw new InvalidCastException("Непустая компонента с конечным перечислимым содержимым сравнивается с непустой компонентой другого вида, в данный момент эта операция не поддерживается.");
            }

            protected int CompareToSame(FiniteEnumerableNonFictionalAttributeComponentPower second)
            {
                return this.Value.CompareTo(second.Value);
            }
        }
    }

    public class FiniteEnumerableNonFictionalAttributeComponentIntersectionOperator
        : AttributeComponentAcceptor<AttributeComponent>, IFiniteEnumerableNonFictionalAttributeComponentAcceptor<AttributeComponent>
    {
        public AttributeComponent Accept<TValue>(
            NonFictionalAttributeComponent<TValue> first, 
            FiniteEnumerableNonFictionalAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            IEnumerable<TValue> intersectedElements = IntersectComponentsElements();
            AttributeComponent<TValue> resultComponent =
                new FiniteEnumerableNonFictionalAttributeComponent<TValue>(first.Domain, intersectedElements);

            return resultComponent;

            IEnumerable<TValue> IntersectComponentsElements()
            {
                List<TValue> intersected = new List<TValue>();
                IEnumerator<TValue> firstEnumerator = first.GetEnumerator(),
                                    secondEnumerator = second.GetEnumerator(),
                                    withLowerBoundEnumerator = firstEnumerator,
                                    withGreaterBoundEnumerator = secondEnumerator;
                bool isContinuesWithLowerBoundEnumerator,
                     isContinuesWithGreaterBoundEnumerator;
                TValue firstElement, secondElement;
                int elementsComparisonResult;

                ReadComponentsUntilAtLeastOneIsOver();

                return intersected;

                void ReadComponentsUntilAtLeastOneIsOver()
                {
                    WithLowerBoundEnumeratorMoveNextAndReadCurrent();
                    WithGreaterBoundEnumeratorMoveNextAndReadCurrent();

                    while (isContinuesWithLowerBoundEnumerator && isContinuesWithGreaterBoundEnumerator)
                    {
                        elementsComparisonResult = firstElement.CompareTo(secondElement);
                        switch (elementsComparisonResult)
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
        }
    }

    public class FiniteEnumerableNonFictionalAttributeComponentUnionOperator
        : AttributeComponentAcceptor<AttributeComponent>, IFiniteEnumerableNonFictionalAttributeComponentAcceptor<AttributeComponent>
    {
        public AttributeComponent Accept<TValue>(
            NonFictionalAttributeComponent<TValue> first,
            FiniteEnumerableNonFictionalAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            IEnumerable<TValue> unitedElements = UnionComponentsElements();
            AttributeComponent<TValue> resultComponent =
                new FiniteEnumerableNonFictionalAttributeComponent<TValue>(first.Domain, unitedElements);

            return resultComponent;

            IEnumerable<TValue> UnionComponentsElements()
            {
                List<TValue> united = new List<TValue>();
                IEnumerator<TValue> firstEnumerator = first.GetEnumerator(),
                                    secondEnumerator = second.GetEnumerator(),
                                    withLowerBoundEnumerator = firstEnumerator,
                                    withGreaterBoundEnumerator = secondEnumerator;
                bool isContinuesWithLowerBoundEnumerator,
                     isContinuesWithGreaterBoundEnumerator;
                TValue firstElement, secondElement;
                int elementsComparisonResult;

                ReadComponentsUntilAtLeastOneIsOver();
                FinishReadingIfAnyComponentRemains();

                return united;

                void ReadComponentsUntilAtLeastOneIsOver()
                {
                    WithLowerBoundEnumeratorMoveNextAndReadCurrent();
                    WithGreaterBoundEnumeratorMoveNextAndReadCurrent();

                    while (isContinuesWithLowerBoundEnumerator && isContinuesWithGreaterBoundEnumerator)
                    {
                        elementsComparisonResult = firstElement.CompareTo(secondElement);
                        switch (elementsComparisonResult)
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
    }

    public class FiniteEnumerableNonFictionalAttributeComponentInclusionOrEqualityComparer
        : AttributeComponentAcceptor<bool>, IFiniteEnumerableNonFictionalAttributeComponentAcceptor<bool>
    {
        public bool Accept<TValue>(
            NonFictionalAttributeComponent<TValue> greater,
            FiniteEnumerableNonFictionalAttributeComponent<TValue> lesser)
            where TValue : IComparable<TValue>
        {
            bool isIncludes = false;
            IEnumerator<TValue> greaterEnumerator = greater.GetEnumerator(),
                                lesserEnumerator = lesser.GetEnumerator();
            bool isContinuesGreaterEnumerator,
                 isContinuesLesserEnumerator;
            TValue firstElement, secondElement;
            int elementsComparisonResult;

            ReadComponentsUntilAnyIsOver();

            return isIncludes;

            void ReadComponentsUntilAnyIsOver()
            {
                GreaterEnumeratorMoveNextAndReadCurrent();
                LesserEnumeratorMoveNextAndReadCurrent();

                while (isContinuesGreaterEnumerator && isContinuesLesserEnumerator &&
                       IsGreaterIncludesCurrentElementFromLesser())
                {
                    LesserEnumeratorMoveNextAndReadCurrent();
                }

                isIncludes = !isContinuesLesserEnumerator;

                return;
            }

            bool IsGreaterIncludesCurrentElementFromLesser()
            {
                while (isContinuesGreaterEnumerator)
                {
                    elementsComparisonResult = firstElement.CompareTo(secondElement);
                    switch (elementsComparisonResult)
                    {
                        case -1:
                            {
                                GreaterEnumeratorMoveNextAndReadCurrent();
                                break;
                            }
                        case 0:
                            {
                                return true;
                            }
                        case 1:
                            {
                                return false;
                            }
                    }
                }

                return false;
            }

            void GreaterEnumeratorMoveNextAndReadCurrent()
            {
                isContinuesGreaterEnumerator = greaterEnumerator.MoveNext();
                firstElement = greaterEnumerator.Current;
            }

            void LesserEnumeratorMoveNextAndReadCurrent()
            {
                isContinuesLesserEnumerator = lesserEnumerator.MoveNext();
                secondElement = lesserEnumerator.Current;
            }
        }
    }
}
