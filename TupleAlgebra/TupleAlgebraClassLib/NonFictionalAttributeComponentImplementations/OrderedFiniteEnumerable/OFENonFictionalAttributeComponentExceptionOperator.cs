using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable
{
    public sealed class OrderedFiniteEnumerableNonFictionalAttributeComponentExceptionOperator<TData>
        : NonFictionalAttributeComponentExceptionOperator<TData, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>>,
          IFactoryBinaryAttributeComponentAcceptor<TData, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>, AttributeComponent<TData>>
    {
        public AttributeComponent<TData> Accept(
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> first,
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> second,
            AttributeComponentFactory factory)
        {
            IEnumerable<TData> remainedElements = ExceptComponentsElements();
            OrderedFiniteEnumerableAttributeComponentFactoryArgs factoryArgs = 
                first.ZipInfo(remainedElements, true) as OrderedFiniteEnumerableAttributeComponentFactoryArgs;
            factoryArgs.ValuesAreOrdered = true;
            AttributeComponent<TData> resultComponent = factory.CreateNonFictional<TData>(factoryArgs);

            return resultComponent;

            IEnumerable<TData> ExceptComponentsElements()
            {
                IEnumerator<TData> firstEnumerator = first.GetEnumerator(),
                                   secondEnumerator = second.GetEnumerator(),
                                   withLowerBoundEnumerator = firstEnumerator,
                                   withGreaterBoundEnumerator = secondEnumerator;
                bool isContinuesWithLowerBoundEnumerator = true,
                     isContinuesWithGreaterBoundEnumerator = true,
                     enumeratorsHadSwapped = false; 
                TData firstElement = default(TData), secondElement = default(TData);

                foreach (TData resultData in ReadComponentsUntilAtLeastOneIsOver())
                    yield return resultData;

                foreach (TData resultData in FinishReadingIfAnyComponentRemains())
                    yield return resultData;

                DisposeEnumerators();

                yield break;

                IEnumerable<TData> ReadComponentsUntilAtLeastOneIsOver()
                {
                    IComparer<TData> orderingComparer = first.OrderingComparer;
                    int elementsComparisonResult;

                    WithLowerBoundEnumeratorMoveNextAndReadCurrent();
                    WithGreaterBoundEnumeratorMoveNextAndReadCurrent();

                    while (isContinuesWithLowerBoundEnumerator && isContinuesWithGreaterBoundEnumerator)
                    {
                        elementsComparisonResult = orderingComparer.Compare(firstElement, secondElement);
                        switch (elementsComparisonResult)
                        {
                            case -1:
                                {
                                    if (!enumeratorsHadSwapped)
                                        yield return firstElement;
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
                                        yield return firstElement;
                                    break;
                                }
                        }

                        WithLowerBoundEnumeratorMoveNextAndReadCurrent();
                    }

                    yield break;
                }

                IEnumerable<TData> FinishReadingIfAnyComponentRemains()
                {
                    if ((isContinuesWithLowerBoundEnumerator && !enumeratorsHadSwapped) ||
                        (isContinuesWithGreaterBoundEnumerator && enumeratorsHadSwapped))
                    {
                        if (isContinuesWithGreaterBoundEnumerator)
                            SwapEnumeratorsAndCurrentElements();
                        return FinishReadingRemainingComponent();
                    }

                    return Enumerable.Empty<TData>();
                }

                IEnumerable<TData> FinishReadingRemainingComponent()
                {
                    do
                    {
                        yield return firstElement;
                        WithLowerBoundEnumeratorMoveNextAndReadCurrent();
                    }
                    while (isContinuesWithLowerBoundEnumerator);
                    
                    yield break;
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
