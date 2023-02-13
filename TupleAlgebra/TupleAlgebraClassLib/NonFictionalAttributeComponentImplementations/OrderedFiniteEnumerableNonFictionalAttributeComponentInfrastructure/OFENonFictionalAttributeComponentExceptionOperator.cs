using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable
{
    public sealed class OrderedFiniteEnumerableNonFictionalAttributeComponentExceptionOperator<TData>
        : FactoryBinaryOrderedFiniteEnumerableNonFictionalAttributeComponentAcceptor<TData>,
          IFactoryAttributeComponentAcceptor<TData, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>, AttributeComponent<TData>>
    {
        public AttributeComponent<TData> Accept(
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> first,
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> second,
            AttributeComponentFactory<TData> factory)
        {
            IEnumerable<TData> remainedElements = ExceptComponentsElements();
            OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData> factoryArgs =
                new OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>(first.Domain, remainedElements);
            AttributeComponent<TData> resultComponent = factory.CreateNonFictional(factoryArgs);

            return resultComponent;

            IEnumerable<TData> ExceptComponentsElements()
            {
                int firstPower = (first.Power as OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>.OrderedFiniteEnumerableNonFictionalAttributeComponentPower).Value;
                List<TData> remained = new List<TData>(firstPower);
                IEnumerator<TData> firstEnumerator = first.GetEnumerator(),
                                    secondEnumerator = second.GetEnumerator(),
                                    withLowerBoundEnumerator = firstEnumerator,
                                    withGreaterBoundEnumerator = secondEnumerator;
                bool isContinuesWithLowerBoundEnumerator = true,
                     isContinuesWithGreaterBoundEnumerator = true,
                     enumeratorsHadSwapped = false; ;
                TData firstElement = default(TData), secondElement = default(TData);
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
                                    if (!enumeratorsHadSwapped)
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
                                    if (!enumeratorsHadSwapped)
                                        remained.Add(firstElement);
                                    break;
                                }
                        }

                        WithLowerBoundEnumeratorMoveNextAndReadCurrent();
                    }
                }

                void FinishReadingIfAnyComponentRemains()
                {
                    if ((isContinuesWithLowerBoundEnumerator && firstEnumerator.Equals(withLowerBoundEnumerator)) ||
                        (isContinuesWithGreaterBoundEnumerator && firstEnumerator.Equals(withGreaterBoundEnumerator)))
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
                    enumeratorsHadSwapped = !enumeratorsHadSwapped;
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
