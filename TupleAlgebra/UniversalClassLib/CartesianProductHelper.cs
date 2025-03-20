using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace UniversalClassLib
{
    public interface IBufferizedEnumerable : IEnumerable
    {
        public IEnumerator GetBufferizedEnumerator();
    }

    public delegate TEntity EntityFactoryHandler<TEntity>(IEnumerator[] properties);

    public delegate TEntity EntityFactoryHandler<TPartialData, TEntity>(
        IEnumerator<TPartialData>[] properties);

    public static class CartesianProductHelper
    {
        public static IEnumerator<TResult> GetPrimitiveEnumerator<
            TPartial,
            TResult>(
            TPartial[] parts)
            where TPartial : IEnumerable
        {
            return IterateOverPrimitive();

            IEnumerator<TResult> IterateOverPrimitive()
            {
                return (parts[0].GetEnumerator() as IEnumerator<TResult>)!;
            }
        }

        public static IEnumerator<TResult> GetCartesianProductEnumeratorWithBuffering<
            TPartial,
            TPartialPower,
            TResult>(
            EntityFactoryHandler<TResult> entityFactory,
            TPartial[] parts,
            Func<TPartial, TPartialPower> partialPowerGetter)
            where TPartial : IBufferizedEnumerable
            where TPartialPower : IComparable<TPartialPower>
        {
            int partsCount = parts.Length;

            /*
             * Если число компонент равно 1, то производится упрощённый перебор значений
             * одного атрибута.
             * Если больше 1, то производится поочерёдный перебор значений каждой
             * компоненты в соответствии с прямым произведением компонент.
             */
            return partsCount > 1 ?
                IterateOverManyAttachedAttributes() :
                IterateOverOneAttachedAttribute(entityFactory, parts);

            /*
             * TODO: сделать возможность обхода компонентов по желанию пользователя.
             * Буферизация перечислителей должна происходить иначе, без допкопирования.
             */
            IEnumerator<TResult> IterateOverManyAttachedAttributes()
            {
                // Оптимизация меняет порядок перечислителей,
                // передающихся entityFactory.
                // Последнюю надо менять в соответствии с этим порядком.

                /*
                 * Попытка оптимизировать потребление памяти при переборе значений компонент
                 * атрибутов, если пользователь не задавал специального порядка обхода.
                 * Компонента с самой большой мощностью ставится на нулевой индекс
                 * в массиве перечислителей и не буферизируется.
                 */

                /*
                int largestPartLoc = -1;
                TPartial largestPart = parts.MaxBy(partialPowerGetter)!,
                         part;
                IEnumerator largestPartEnumerator =
                    largestPart.GetEnumerator();
                IEnumerator[] partsEnumerators = new IEnumerator[partsCount];

                for (int i = 0; i < partsCount; i++)
                {
                    if ((part = parts[i]) is null)
                        continue;

                    if (part.Equals(largestPart))
                    {
                        largestPartLoc = i;
                        partsEnumerators[i] = largestPartEnumerator;
                    }
                    else
                    {
                        // Производится буферизирование компоненты.
                        partsEnumerators[i] =
                            part.GetBufferizedEnumerator();
                    }

                }
                */

                /*
                 * Вставка крупнейшей компоненты на нулевой индекс массива перечислителей. 
                 */

                /*
                if (largestPartLoc > 0)
                {
                    (partsEnumerators[largestPartLoc], partsEnumerators[0]) =
                        (partsEnumerators[0], partsEnumerators[largestPartLoc]);
                }

                while (largestPartEnumerator.MoveNext())
                {
                    foreach (TResult entity
                         in EnumerateCartesianProduct(
                            entityFactory,
                            partsEnumerators,
                            1))
                        yield return entity;
                }

                yield break;
                */

                TPartial part;
                IEnumerator[] partsEnumerators = new IEnumerator[partsCount];

                for (int i = 0; i < partsCount; i++)
                {
                    if ((part = parts[i]) is null)
                        continue;

                    // Производится буферизирование компоненты.
                    partsEnumerators[i] =
                        part.GetBufferizedEnumerator();
                }

                foreach (TResult entity
                         in EnumerateCartesianProduct(
                            entityFactory,
                            partsEnumerators))
                    yield return entity;

                yield break;
            }
        }

        public static IEnumerator<TResult> GetCartesianProductEnumerator<
            TPartial,
            TResult>(
            EntityFactoryHandler<TResult> entityFactory,
            TPartial[] parts)
            where TPartial : IEnumerable
        {
            int partsCount = parts.Length;

            /*
             * Если число компонент равно 1, то производится упрощённый перебор значений
             * одного атрибута.
             * Если больше 1, то производится поочерёдный перебор значений каждой
             * компоненты в соответствии с прямым произведением компонент.
             */
            return partsCount > 1 ?
                IterateOverManyAttachedAttributes() :
                IterateOverOneAttachedAttribute(entityFactory, parts);

            /*
             * TODO: сделать возможность обхода компонентов по желанию пользователя.
             * Буферизация перечислителей должна происходить иначе, без допкопирования.
             */
            IEnumerator<TResult> IterateOverManyAttachedAttributes()
            {
                IEnumerator[] partsEnumerators = new IEnumerator[partsCount];
                for (int i = 0; i < partsCount; i++)
                    partsEnumerators[i] = parts[i].GetEnumerator();

                return EnumerateCartesianProduct(
                            entityFactory,
                            partsEnumerators)
                    .GetEnumerator();
            }
        }

        public static IEnumerable<TResult> GetCartesianProduct<
            TPartial,
            TPartialData,
            TResult>(
            EntityFactoryHandler<TResult> entityFactory,
            TPartial[] parts,
            Func<TPartial, IEnumerable<TPartialData>> getEnumerable)
        {
            int partsCount = parts.Length;

            /*
             * Если число компонент равно 1, то производится упрощённый перебор значений
             * одного атрибута.
             * Если больше 1, то производится поочерёдный перебор значений каждой
             * компоненты в соответствии с прямым произведением компонент.
             */
            return partsCount > 1 ?
                IterateOverManyAttachedAttributes() :
                IterateOverOneAttachedAttribute(
                    entityFactory, 
                    parts,
                    part => getEnumerable(part).GetEnumerator());

            /*
             * TODO: сделать возможность обхода компонентов по желанию пользователя.
             * Буферизация перечислителей должна происходить иначе, без допкопирования.
             */
            IEnumerable<TResult> IterateOverManyAttachedAttributes()
            {
                IEnumerator[] partsEnumerators = new IEnumerator[partsCount];
                for (int i = 0; i < partsCount; i++)
                    partsEnumerators[i] = new ResettableEnumerator<TPartialData>(
                        getEnumerable(parts[i]));
                return EnumerateCartesianProduct(
                            entityFactory,
                            partsEnumerators);
            }
        }

        private static IEnumerator<TResult> IterateOverOneAttachedAttribute<
            TPartial,
            TResult>(
            EntityFactoryHandler<TResult> entityFactory,
            TPartial[] parts)
            where TPartial : IEnumerable
        {
            return IterateOverOneAttachedAttribute(
                entityFactory,
                parts,
                (p) => p.GetEnumerator())
                .GetEnumerator();
        }

        private static IEnumerable<TResult> IterateOverOneAttachedAttribute<
            TPartial,
            TPartialEnumerator,
            TResult>(
            EntityFactoryHandler<TResult> entityFactory,
            TPartial[] parts,
            Func<TPartial, TPartialEnumerator> getPartialEnumerator)
            where TPartialEnumerator : class, IEnumerator
        {
            IEnumerator onePartEnumerator = getPartialEnumerator(parts[0]);
            IEnumerator[] partsEnumerators = [onePartEnumerator];

            while (onePartEnumerator.MoveNext())
                yield return entityFactory(partsEnumerators);

            yield break;
        }

        /// <summary>
        /// Перечисление прямого (декартова) произведения нескольких атрибутов.
        /// </summary>
        /// <param name="entityFactory">Сконструированная инструкция по генерации сущностей.</param>
        /// <param name="partsEnumerators">Массив перечислителей компонент, в соответствии
        /// с которым была построена инструкция по генерации сущностей.</param>
        /// <param name="startPartLoc">Номер атрибута, с которого начинает производиться
        /// перечисление значений компоненты.</param>
        /// <param name="partsStack">Стек для обхода компонент.</param>
        /// <returns></returns>
        private static IEnumerable<TResult> EnumerateCartesianProduct<TResult>(
            EntityFactoryHandler<TResult> entityFactory,
            IEnumerator[] partsEnumerators,
            int startPartLoc = 0)
        {
            int stackPtr = startPartLoc;
            //int stackPtr = startPartLoc,
            //    partsCount = partsEnumerators.Length - stackPtr;
            IEnumerator partEnumerator;

            //Stack<IEnumerator> partsStack = new Stack<IEnumerator>(partsCount);
            Stack<IEnumerator> partsStack = new Stack<IEnumerator>(
                partsEnumerators.Length - stackPtr);
            partsStack.Push(partEnumerator = partsEnumerators[stackPtr]);

            do
            {
                if (partEnumerator.MoveNext())
                {
                    if (stackPtr < partsEnumerators.Length - 1)
                    {
                        partsStack.Push(partsEnumerators[++stackPtr]);
                    }
                    else
                    {
                        yield return entityFactory(partsEnumerators);
                    }
                }
                else
                {
                    stackPtr--;
                    /*
                     * Указатель стека переходит на перечислитель предыдущего атрибута,
                     * а перечислитель текущего атрибута перезапускается, чтобы
                     * в следующий раз перебор начался сначала.
                     */
                    partsStack.Pop().Reset();
                }
            }
            while (partsStack.TryPeek(out partEnumerator));

            yield break;
        }

        public static IEnumerable<TResult> GetCartesianProduct<
            TPartial,
            TPartialEnumerator,
            TResult>(
            EntityFactoryHandler<TResult> entityFactory,
            TPartial[] parts,
            Func<TPartial, TPartialEnumerator> getPartialEnumerator)
            where TPartialEnumerator : class, IEnumerator
        {
            int partsCount = parts.Length;

            /*
             * Если число компонент равно 1, то производится упрощённый перебор значений
             * одного атрибута.
             * Если больше 1, то производится поочерёдный перебор значений каждой
             * компоненты в соответствии с прямым произведением компонент.
             */
            return partsCount > 1 ?
                IterateOverManyAttachedAttributes() :
                IterateOverOneAttachedAttribute(entityFactory, parts, getPartialEnumerator);

            /*
             * TODO: сделать возможность обхода компонентов по желанию пользователя.
             * Буферизация перечислителей должна происходить иначе, без допкопирования.
             */
            IEnumerable<TResult> IterateOverManyAttachedAttributes()
            {
                IEnumerator[] partsEnumerators = new TPartialEnumerator[partsCount];
                for (int i = 0; i < partsCount; i++)
                    partsEnumerators[i] = getPartialEnumerator(parts[i]);

                return EnumerateCartesianProduct(
                            entityFactory,
                            partsEnumerators);
            }
        }

        public static IEnumerable<TResult> GetAccumulatedCartesianProduct<
            TPartial,
            TPartialData,
            TResult>(
            Func<TPartialData, TResult> entityFactory,
            TPartial[] parts,
            Func<int, TPartial, IEnumerable<TPartialData>> getPartialEnumerable,
            Func<TPartialData, TPartialData, BranchAction> defineBranchAction,
            Func<BranchAction, TPartialData, TPartialData, TPartialData> accFactory,
            Predicate<TResult> resultValidation,
            Action<TPartialData> accumulatorResetHandler = null!)
        {
            int partsCount = parts.Length;

            return IterateOverManyAttachedAttributes();

            /*
             * Если число компонент равно 1, то производится упрощённый перебор значений
             * одного атрибута.
             * Если больше 1, то производится поочерёдный перебор значений каждой
             * компоненты в соответствии с прямым произведением компонент.
             */
            //return partsCount > 1 ?
            //    IterateOverManyAttachedAttributes() :
            //    IterateOverOneAttachedAttribute(entityFactory, parts, getPartialEnumerator);

            /*
             * TODO: сделать возможность обхода компонентов по желанию пользователя.
             * Буферизация перечислителей должна происходить иначе, без допкопирования.
             */
            IEnumerable<TResult> IterateOverManyAttachedAttributes()
            {
                IEnumerator<TPartialData>[] partsEnumerators = 
                    new IEnumerator<TPartialData>[partsCount];
                for (int i = 0; i < partsCount; i++)
                    partsEnumerators[i] = new ResettableEnumerator<TPartialData>(
                        getPartialEnumerable(i, parts[i]));

                return EnumerateAccumulatedCartesianProduct(
                            entityFactory,
                            partsEnumerators,
                            defineBranchAction,
                            accFactory,
                            resultValidation,
                            accumulatorResetHandler);
            }
        }

        public static IEnumerable<TResult> GetAccumulatedCartesianProduct<
            TPartialData,
            TResult>(
            Func<TPartialData, TResult> entityFactory,
            IEnumerable<TPartialData>[] parts,
            Func<TPartialData, TPartialData, BranchAction> defineBranchAction,
            Func<BranchAction, TPartialData, TPartialData, TPartialData> accFactory,
            Predicate<TResult> resultValidation,
            Action<TPartialData> accumulatorResetHandler = null!)
        {
            int partsCount = parts.Length;

            return IterateOverManyAttachedAttributes();

            /*
             * Если число компонент равно 1, то производится упрощённый перебор значений
             * одного атрибута.
             * Если больше 1, то производится поочерёдный перебор значений каждой
             * компоненты в соответствии с прямым произведением компонент.
             */
            //return partsCount > 1 ?
            //    IterateOverManyAttachedAttributes() :
            //    IterateOverOneAttachedAttribute(entityFactory, parts, getPartialEnumerator);

            IEnumerable<TResult> IterateOverManyAttachedAttributes()
            {
                IEnumerator<TPartialData>[] partsEnumerators =
                    new IEnumerator<TPartialData>[partsCount];
                for (int i = 0; i < partsCount; i++)
                    partsEnumerators[i] = new ResettableEnumerator<TPartialData>(
                        parts[i]);

                return EnumerateAccumulatedCartesianProduct(
                            entityFactory,
                            partsEnumerators,
                            defineBranchAction,
                            accFactory,
                            resultValidation,
                            accumulatorResetHandler);
            }
        }

        /// <summary>
        /// Перечисление прямого (декартова) произведения нескольких атрибутов.
        /// </summary>
        /// <param name="entityFactory">Сконструированная инструкция по генерации сущностей.</param>
        /// <param name="partsEnumerators">Массив перечислителей компонент, в соответствии
        /// с которым была построена инструкция по генерации сущностей.</param>
        /// <param name="startPartLoc">Номер атрибута, с которого начинает производиться
        /// перечисление значений компоненты.</param>
        /// <param name="partsStack">Стек для обхода компонент.</param>
        /// <returns></returns>
        private static IEnumerable<TResult>
            EnumerateAccumulatedCartesianProduct<TPartialData, TResult>(
            Func<TPartialData, TResult> entityFactory,
            IEnumerator<TPartialData>[] branchEnumerators,
            Func<TPartialData, TPartialData, BranchAction> defineBranchAction,
            Func<BranchAction, TPartialData, TPartialData, TPartialData> accFactory,
            Predicate<TResult> resultValidation,
            Action<TPartialData> accumulatorResetHandler = null)
        {
            int stackPtr = 0,
                branchCount = branchEnumerators.Length - 1;
            Stack<CartesianProductBranchInfo<TPartialData>> branchStack =
                new Stack<CartesianProductBranchInfo<TPartialData>>(branchCount);
            CartesianProductBranchInfo<TPartialData> currentBranch;
            IEnumerator<TPartialData> currentBranchEnumerator;

            TPartialData acc, prevAcc = acc = default!;
            TResult result;
            BranchAction branchAction;
            var firstBranchEnumerator = branchEnumerators[0];

            while (firstBranchEnumerator.MoveNext())
            {
                currentBranch = new(
                    branchAction = BranchAction.NonDefined,
                    currentBranchEnumerator = branchEnumerators[++stackPtr],
                    prevAcc = firstBranchEnumerator.Current,
                    default!);
                branchStack.Push(currentBranch);

                do
                {
                    if (MoveCurrentBranchEnumeratorNext())
                    {
                        switch (branchAction)
                        {
                            //return BranchAction.ContinueBranchTillEnd;
                            case BranchAction.CutOffBranchSelectSecond:
                                {
                                    //RenewCurrentBranchInfo();
                                    break;
                                }
                            // нужно посмотреть, как именно алгоритм должен вести себя
                            // при выборе первого операнда, нужно ли отсекать ветвь
                            // целиком или же нет
                            case BranchAction.CutOffBranchSelectFirst:
                            case BranchAction.ConsiderSubBranchLater:
                            case BranchAction.NonDefined:
                                {
                                    switch (branchAction = defineBranchAction(
                                        prevAcc, acc))
                                    {
                                        case BranchAction.ConsiderSubBranchLater:
                                            {
                                                RenewCurrentBranchInfo();

                                                continue;
                                            }
                                        case BranchAction.CutOffBranchSelectFirst:
                                        case BranchAction.CutOffBranchSelectSecond:
                                            {
                                                break;
                                            }
                                        case BranchAction.ContinueBranchTillEnd:
                                            {
                                                currentBranchEnumerator.Reset();
                                                ResetAccumulator();
                                                currentBranchEnumerator.MoveNext();
                                                acc = currentBranchEnumerator.Current;

                                                break;
                                            }
                                    }

                                    goto default;
                                }
                            default:
                                {
                                    acc = accFactory(
                                        branchAction,
                                        prevAcc,
                                        acc);
                                    RenewCurrentBranchInfo();

                                    if (stackPtr == branchCount)
                                    {
                                        result = entityFactory(acc);
                                        if (resultValidation(result))
                                            yield return result;
                                    }
                                    else
                                    {
                                        branchStack.Push(new(
                                            BranchAction.NonDefined,
                                            branchEnumerators[++stackPtr],
                                            acc,
                                            default!));
                                    }

                                    continue;
                                }
                        }
                    }
                    else if (branchAction == BranchAction.ConsiderSubBranchLater)
                    {
                        branchAction = BranchAction.ContinueBranchTillEnd;
                        RenewCurrentBranchInfo();
                        currentBranchEnumerator.Reset();
                        ResetAccumulator();
                        continue;
                    }

                    stackPtr--;
                    /*
                     * Указатель стека переходит на перечислитель предыдущего атрибута,
                     * а перечислитель текущего атрибута перезапускается, чтобы
                     * в следующий раз перебор начался сначала.
                     */
                    currentBranch = branchStack.Pop();
                    ResetAccumulator();
                    currentBranch.Enumerator.Reset();
                }
                while (TryPeekNextBranch());

                accumulatorResetHandler(firstBranchEnumerator.Current);
            }

            yield break;

            bool TryPeekNextBranch()
            {
                bool peekTry = branchStack.TryPeek(out currentBranch);
                currentBranchEnumerator = currentBranch.Enumerator;

                return peekTry;
            }

            void RenewCurrentBranchInfo()
            {
                branchStack.Pop();
                currentBranch = new(
                    branchAction,
                    currentBranchEnumerator,
                    prevAcc,
                    acc,
                    true);
                branchStack.Push(currentBranch);

                return;
            }

            void ResetAccumulator()
            {
                if (currentBranch.HasMovedBefore)
                {
                    accumulatorResetHandler(currentBranch.Accumulator);
                }

                return;
            }

            bool MoveCurrentBranchEnumeratorNext()
            {
                ResetAccumulator();
                bool moveNext = currentBranchEnumerator.MoveNext();
                acc = currentBranchEnumerator.Current;
                prevAcc = currentBranch.PreviousAccumulator;
                branchAction = currentBranch.Action;

                return moveNext;
            }
        }
    }

    public struct CartesianProductBranchInfo<TPartialData>
    {
        public BranchAction Action { get; init; }

        public IEnumerator<TPartialData> Enumerator { get; init; }

        public TPartialData PreviousAccumulator { get; init; }

        public TPartialData Accumulator { get; init; }

        public bool HasMovedBefore { get; init; }

        public CartesianProductBranchInfo(
            BranchAction action,
            IEnumerator<TPartialData> enumerator,
            TPartialData prevAccumulator,
            TPartialData accumulator,
            bool hasMovedBefore = false)
        {
            Action = action;
            Enumerator = enumerator;
            PreviousAccumulator = prevAccumulator;
            Accumulator = accumulator;
            HasMovedBefore = hasMovedBefore;

            return;
        }
    }

    public enum BranchAction : sbyte
    {
        NonDefined = -2,
        ConsiderSubBranchLater = -1,
        ContinueBranchTillEnd = 0,
        CutOffBranchSelectFirst = 1,
        CutOffBranchSelectSecond = 2
    }

    public class ResettableEnumerator<T> : IEnumerator<T>
    {
        private IEnumerable<T> _source;

        private IEnumerator<T> _sourceEnumerator;

        public T Current { get => _sourceEnumerator.Current; }

        object IEnumerator.Current { get => Current; }

        public ResettableEnumerator(IEnumerable<T> source)
        {
            _source = source;
            Reset();

            return;
        }

        public bool MoveNext()
        {
            return _sourceEnumerator.MoveNext();
        }

        public void Dispose()
        {
            _sourceEnumerator.Dispose();

            return;
        }

        public void Reset()
        {
            _sourceEnumerator?.Dispose();
            _sourceEnumerator = _source.GetEnumerator();

            return;
        }
    }
}
