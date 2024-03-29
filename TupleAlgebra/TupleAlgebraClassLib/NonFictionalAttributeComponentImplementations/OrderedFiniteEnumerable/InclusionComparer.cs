﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Countable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable
{
    public abstract class InclusionComparer<TData, TAttributeComponent>
        : NonFictionalAttributeComponentInclusionComparer<TData, TAttributeComponent>,
          IOrderedFiniteEnumerableAttributeComponentBooleanOperator<TData, TAttributeComponent>,
          IFiniteEnumerableAttributeComponentInclusionComparer<TData>,
          ICountableAttributeComponentInclusionComparer<TData>
        where TAttributeComponent : NonFictionalAttributeComponent<TData>, IOrderedFiniteEnumerableAttributeComponent<TData>
    {
        public bool Accept(TAttributeComponent greater,
                           IOrderedFiniteEnumerableAttributeComponent<TData> lesser)
        {
            bool isInclude = false;
            IEnumerator<TData> greaterEnumerator = greater.GetEnumerator(),
                               lesserEnumerator = lesser.GetEnumerator();
            bool isContinuesLesserEnumerator = true,
                 isContinuesGreaterEnumerator = true;
            TData firstElement = default(TData), secondElement = default(TData);
            IComparer<TData> orderingComparer = greater.OrderingComparer;
            int elementsComparisonResult;

            ReadComponentsUntilAnyIsOver();
            DisposeEnumerators();

            return isInclude;

            void ReadComponentsUntilAnyIsOver()
            {
                GreaterEnumeratorMoveNextAndReadCurrent();
                LesserEnumeratorMoveNextAndReadCurrent();

                while (isContinuesGreaterEnumerator && isContinuesLesserEnumerator &&
                       IsGreaterIncludesCurrentElementFromLesser())
                {
                    LesserEnumeratorMoveNextAndReadCurrent();
                }

                isInclude = !isContinuesLesserEnumerator && (isContinuesGreaterEnumerator || greater.Power > lesser.Power);

                return;
            }

            bool IsGreaterIncludesCurrentElementFromLesser()
            {
                while (isContinuesGreaterEnumerator)
                {
                    elementsComparisonResult = orderingComparer.Compare(firstElement, secondElement);
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

                return;
            }

            void LesserEnumeratorMoveNextAndReadCurrent()
            {
                if (isContinuesLesserEnumerator = lesserEnumerator.MoveNext())
                    secondElement = lesserEnumerator.Current;

                return;
            }

            void DisposeEnumerators()
            {
                greaterEnumerator.Dispose();
                lesserEnumerator.Dispose();

                return;
            }
        }
    }
}
