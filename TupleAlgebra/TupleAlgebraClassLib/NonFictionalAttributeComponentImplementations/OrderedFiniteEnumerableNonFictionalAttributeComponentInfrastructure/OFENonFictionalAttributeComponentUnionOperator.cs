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
    public class OrderedFiniteEnumerableNonFictionalAttributeComponentUnionOperator<TData>
        : FactoryBinaryOrderedFiniteEnumerableNonFictionalAttributeComponentAcceptor<TData>,
          IFactoryBinaryAttributeComponentAcceptor<TData, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>, AttributeComponent<TData>>
    {
        public AttributeComponent<TData> Accept(
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> first,
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> second,
            AttributeComponentFactory factory)
        {
            IEnumerable<TData> unitedElements = UnionComponentsElements();
            AttributeComponentFactoryArgs factoryArgs = first.ZipInfo(unitedElements, true);
            AttributeComponent<TData> resultComponent = factory.CreateNonFictional<TData>(factoryArgs);

            return resultComponent;

            IEnumerable<TData> UnionComponentsElements()
            {
                int summaryPower = (first.Power as OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>.OrderedFiniteEnumerableNonFictionalAttributeComponentPower).Value +
                    (second.Power as OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>.OrderedFiniteEnumerableNonFictionalAttributeComponentPower).Value;
                List<TData> united = new List<TData>(summaryPower);
                IEnumerator<TData> firstEnumerator = first.GetEnumerator(),
                                   secondEnumerator = second.GetEnumerator(),
                                   withLowerBoundEnumerator = firstEnumerator,
                                   withGreaterBoundEnumerator = secondEnumerator;
                bool isContinuesWithLowerBoundEnumerator = true,
                     isContinuesWithGreaterBoundEnumerator = true;
                TData firstElement = default(TData), secondElement = default(TData);
                int elementsComparisonResult;

                ReadComponentsUntilAtLeastOneIsOver();
                FinishReadingIfAnyComponentRemains();
                DisposeEnumerators();

                return united;

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

                    return;
                }

                void FinishReadingIfAnyComponentRemains()
                {
                    if (isContinuesWithLowerBoundEnumerator || isContinuesWithGreaterBoundEnumerator)
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
                        united.Add(firstElement);
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
