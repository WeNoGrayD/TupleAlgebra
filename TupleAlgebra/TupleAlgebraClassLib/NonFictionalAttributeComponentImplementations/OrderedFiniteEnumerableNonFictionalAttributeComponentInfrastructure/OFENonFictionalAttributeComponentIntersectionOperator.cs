using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerableNonFictionalAttributeComponentInfrastructure
{
    public class OrderedFiniteEnumerableNonFictionalAttributeComponentIntersectionOperator<TValue>
        : FactoryBinaryOrderedFiniteEnumerableNonFictionalAttributeComponentAcceptor<TValue>,
          IFactoryAttributeComponentAcceptor<TValue, OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue>, OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue>, AttributeComponent<TValue>>
    {
        public AttributeComponent<TValue> Accept(
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue> first,
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory)
        {
            IEnumerable<TValue> intersectedElements = IntersectComponentsElements();
            OrderedFiniteEnumerableNonFictionalAttributeComponentFactoryArgs<TValue> factoryArgs =
                new OrderedFiniteEnumerableNonFictionalAttributeComponentFactoryArgs<TValue>(first.Domain, intersectedElements);
            AttributeComponent<TValue> resultComponent = factory.CreateNonFictional(factoryArgs);

            return resultComponent;

            IEnumerable<TValue> IntersectComponentsElements()
            {
                int minPower = (Enumerable.Min(new[] { first.Power, second.Power })
                    as OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue>
                           .OrderedFiniteEnumerableNonFictionalAttributeComponentPower)
                        .Value;
                List<TValue> intersected = new List<TValue>(minPower);
                IEnumerator<TValue> firstEnumerator = first.GetEnumerator(),
                                    secondEnumerator = second.GetEnumerator(),
                                    withLowerBoundEnumerator = firstEnumerator,
                                    withGreaterBoundEnumerator = secondEnumerator;
                bool isContinuesWithLowerBoundEnumerator = true,
                     isContinuesWithGreaterBoundEnumerator = true;
                TValue firstElement = default(TValue), secondElement = default(TValue);
                int elementsComparisonResult;

                ReadComponentsUntilAtLeastOneIsOver();

                return intersected;

                void ReadComponentsUntilAtLeastOneIsOver()
                {
                    WithLowerBoundEnumeratorMoveNextAndReadCurrent();
                    WithGreaterBoundEnumeratorMoveNextAndReadCurrent();

                    while (isContinuesWithLowerBoundEnumerator && isContinuesWithGreaterBoundEnumerator)
                    {
                        elementsComparisonResult = _orderingComparer.Compare(firstElement, secondElement);
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
                    if (isContinuesWithLowerBoundEnumerator = withLowerBoundEnumerator.MoveNext())
                        firstElement = withLowerBoundEnumerator.Current;
                }

                void WithGreaterBoundEnumeratorMoveNextAndReadCurrent()
                {
                    if (isContinuesWithGreaterBoundEnumerator = withGreaterBoundEnumerator.MoveNext())
                        secondElement = withGreaterBoundEnumerator.Current;
                }
            }
        }
    }
}
