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
    public class OrderedFiniteEnumerableNonFictionalAttributeComponentIntersectionOperator<TData>
        : FactoryBinaryOrderedFiniteEnumerableNonFictionalAttributeComponentAcceptor<TData>,
          IFactoryAttributeComponentAcceptor<TData, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>, AttributeComponent<TData>>
    {
        public AttributeComponent<TData> Accept(
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> first,
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> second,
            AttributeComponentFactory<TData> factory)
        {
            IEnumerable<TData> intersectedElements = IntersectComponentsElements();
            OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData> factoryArgs =
                new OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>(first.Domain, intersectedElements);
            AttributeComponent<TData> resultComponent = factory.CreateNonFictional(factoryArgs);

            return resultComponent;

            IEnumerable<TData> IntersectComponentsElements()
            {
                int minPower = (Enumerable.Min(new[] { first.Power, second.Power })
                    as OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>
                           .OrderedFiniteEnumerableNonFictionalAttributeComponentPower)
                        .Value;
                List<TData> intersected = new List<TData>(minPower);
                IEnumerator<TData> firstEnumerator = first.GetEnumerator(),
                                    secondEnumerator = second.GetEnumerator(),
                                    withLowerBoundEnumerator = firstEnumerator,
                                    withGreaterBoundEnumerator = secondEnumerator;
                bool isContinuesWithLowerBoundEnumerator = true,
                     isContinuesWithGreaterBoundEnumerator = true;
                TData firstElement = default(TData), secondElement = default(TData);
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
