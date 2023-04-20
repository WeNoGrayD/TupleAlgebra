﻿using System;
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
          IFactoryBinaryAttributeComponentAcceptor<TData, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>, AttributeComponent<TData>>
    {
        public AttributeComponent<TData> Accept(
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> first,
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> second,
            AttributeComponentFactory factory)
        {
            IEnumerable<TData> remainedElements = ExceptComponentsElements();
            AttributeComponentFactoryArgs factoryArgs = first.ZipInfo(remainedElements, true);
            AttributeComponent<TData> resultComponent = factory.CreateNonFictional<TData>(factoryArgs);

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
                DisposeEnumerators();

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

                    return;
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

                    return;
                }

                void FinishReadingRemainingComponent()
                {
                    do
                    {
                        remained.Add(firstElement);
                        WithLowerBoundEnumeratorMoveNextAndReadCurrent();
                    }
                    while (isContinuesWithLowerBoundEnumerator);

                    return;
                }

                void SwapEnumeratorsAndCurrentElements()
                {
                    (withLowerBoundEnumerator, withGreaterBoundEnumerator) =
                        (withGreaterBoundEnumerator, withLowerBoundEnumerator);
                    (firstElement, secondElement) = (secondElement, firstElement);
                    enumeratorsHadSwapped = !enumeratorsHadSwapped;

                    return;
                }

                void WithLowerBoundEnumeratorMoveNextAndReadCurrent()
                {
                    if (isContinuesWithLowerBoundEnumerator = withLowerBoundEnumerator.MoveNext())
                        firstElement = withLowerBoundEnumerator.Current;

                    return;
                }

                void WithGreaterBoundEnumeratorMoveNextAndReadCurrent()
                {
                    if (isContinuesWithGreaterBoundEnumerator = withGreaterBoundEnumerator.MoveNext())
                        secondElement = withGreaterBoundEnumerator.Current;

                    return;
                }

                void DisposeEnumerators()
                {
                    firstEnumerator.Dispose();
                    secondEnumerator.Dispose();

                    return;
                }
            }
        }
    }
}
