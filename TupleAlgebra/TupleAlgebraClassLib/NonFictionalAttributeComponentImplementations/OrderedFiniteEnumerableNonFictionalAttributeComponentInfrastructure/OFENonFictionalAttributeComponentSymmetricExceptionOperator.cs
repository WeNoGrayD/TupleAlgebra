using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerableNonFictionalAttributeComponentInfrastructure
{
    public sealed class OrderedFiniteEnumerableNonFictionalAttributeComponentSymmetricExceptionOperator<TValue>
        : FactoryBinaryOrderedFiniteEnumerableNonFictionalAttributeComponentAcceptor<TValue>,
          IFactoryAttributeComponentAcceptor<TValue, OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue>, OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue>, AttributeComponent<TValue>>
    {
        public AttributeComponent<TValue> Accept(
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue> first,
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory)
        {
            IEnumerable<TValue> remainedElements = ExceptComponentsElements();
            OrderedFiniteEnumerableNonFictionalAttributeComponentFactoryArgs<TValue> factoryArgs =
                new OrderedFiniteEnumerableNonFictionalAttributeComponentFactoryArgs<TValue>(first.Domain, remainedElements);
            AttributeComponent<TValue> resultComponent = factory.CreateNonFictional(factoryArgs);

            return resultComponent;

            IEnumerable<TValue> ExceptComponentsElements()
            {
                int summaryPower = (first.Power as OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue>.OrderedFiniteEnumerableNonFictionalAttributeComponentPower).Value +
                    (second.Power as OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue>.OrderedFiniteEnumerableNonFictionalAttributeComponentPower).Value;
                List<TValue> remained = new List<TValue>(summaryPower);
                IEnumerator<TValue> firstEnumerator = first.GetEnumerator(),
                                    secondEnumerator = second.GetEnumerator(),
                                    withLowerBoundEnumerator = firstEnumerator,
                                    withGreaterBoundEnumerator = secondEnumerator;
                bool isContinuesWithLowerBoundEnumerator = true,
                     isContinuesWithGreaterBoundEnumerator = true;
                TValue firstElement = default(TValue), secondElement = default(TValue);
                int elementsComparisonResult;

                ReadComponentsUntilAtLeastOneIsOver();
                FinishReadingIfAnyComponentRemains();

                return remained;

                void ReadComponentsUntilAtLeastOneIsOver()
                {
                    WithLowerBoundEnumeratorMoveNextAndReadCurrent();
                    WithGreaterBoundEnumeratorMoveNextAndReadCurrent();

                    while (isContinuesWithLowerBoundEnumerator && isContinuesWithGreaterBoundEnumerator)
                    {
                        elementsComparisonResult = _orderingComparer.Compare(firstElement, secondElement);
                        switch (elementsComparisonResult)
                        {
                            case -1:
                                {
                                    remained.Add(firstElement);
                                    break;
                                }
                            case 0:
                                {
                                    WithGreaterBoundEnumeratorMoveNextAndReadCurrent();
                                    break;
                                }
                            case 1:
                                {
                                    SwapEnumeratorsAndCurrentElements();
                                    remained.Add(firstElement);
                                    break;
                                }
                        }

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
                        remained.Add(firstElement);
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
