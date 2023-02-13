using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable
{
    public class OrderedFiniteEnumerableNonFictionalAttributeComponentInclusionOrEqualityComparer<TData>
        : InstantBinaryOrderedFiniteEnumerableNonFictionalAttributeComponentAcceptor<TData>,
          IInstantAttributeComponentAcceptor<TData, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>, bool>
    {
        public bool Accept(
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> greater,
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> lesser)
        {
            bool isIncludes = false;
            IEnumerator<TData> greaterEnumerator = greater.GetEnumerator(),
                                lesserEnumerator = lesser.GetEnumerator();
            bool isContinuesLesserEnumerator = true,
                 isContinuesGreaterEnumerator = true;
            TData firstElement = default(TData), secondElement = default(TData);
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
                    elementsComparisonResult = _orderingComparer.Compare(firstElement, secondElement);
                    switch (elementsComparisonResult)
                    {
                        case -1:
                            {
                                GreaterEnumeratorMoveNextAndReadCurrent();
                                break;
                            }
                        case 0:
                            {
                                GreaterEnumeratorMoveNextAndReadCurrent();
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
                if (isContinuesGreaterEnumerator = greaterEnumerator.MoveNext())
                    firstElement = greaterEnumerator.Current;
            }

            void LesserEnumeratorMoveNextAndReadCurrent()
            {
                if (isContinuesLesserEnumerator = lesserEnumerator.MoveNext())
                    secondElement = lesserEnumerator.Current;
            }
        }
    }
}
