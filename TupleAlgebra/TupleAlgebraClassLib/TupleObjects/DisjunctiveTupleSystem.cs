using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleSystemInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.DisjunctiveTupleSystemInfrastructure;
using UniversalClassLib;

namespace TupleAlgebraClassLib.TupleObjects
{
    using static TupleObjectStaticDataStorage;
    using static TupleObjectHelper;
    using static CartesianProductHelper;
    using static TupleObjectInfrastructure.TupleObjectOperators.TupleObjectFactoryMethods;

    public class DisjunctiveTupleSystem<TEntity> 
        : TupleObjectSystem<TEntity, DisjunctiveTuple<TEntity>>
        where TEntity : new()
    {
        static DisjunctiveTupleSystem()
        {
            Storage.RegisterType<TEntity, DisjunctiveTupleSystem<TEntity>>(
                (factory) => new DisjunctiveTupleSystemOperationExecutorsContainer(factory));

            return;
        }

        public DisjunctiveTupleSystem(TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(onTupleBuilding)
        { }

        public DisjunctiveTupleSystem(
            TupleObjectSchema<TEntity> schema,
            int len)
            : base(schema, len)
        { }

        public DisjunctiveTupleSystem(
            TupleObjectSchema<TEntity> schema,
            IList<ISingleTupleObject> tuples)
            : base(schema, tuples)
        { }

        /// <summary>
        /// Кортеж непустой, если имеется хотя бы одна неленивая компонента
        /// либо ленивая непустая компонента.
        /// </summary>
        /// <returns></returns>
        public override bool IsFalse()
        {
            (int AttrLoc, int NonEmptyComponentsCount)[] nonConflictingAttributes = null;
            if (!TryFindMonotonicColumn()) return false;

            return IsReducedSystemFalse();

            bool TryFindMonotonicColumn()
            {
                bool isFalse = true;
                object resLocker = new object(), ctsLocker = new object();
                ConcurrentBag<(int AttrLoc, int NonEmptyComponentsCount)>
                    nonConflictingColumns
                    = new ConcurrentBag<(int AttrLoc, int NonEmptyComponentsCount)>();
                CancellationTokenSource cts = new CancellationTokenSource();
                try
                {
                    Parallel.For(0, RowLength,
                        new ParallelOptions() { CancellationToken = cts.Token },
                        IntersectColumn);
                }
                catch (AggregateException) { }
                catch (OperationCanceledException)
                {
                    // it's okay
                    ;
                }
                if (isFalse)
                    nonConflictingAttributes = nonConflictingColumns.ToArray();

                return isFalse;

                void IntersectColumn(int attrLoc)
                {
                    bool isConflicting = false, isMonotonic = true;
                    int nonConflictingComponentsCount = 1;
                    IAttributeComponent ac, acRes = null;

                    for (int tuplePtr = 0; tuplePtr < ColumnLength; tuplePtr++)
                    {
                        if ((ac = this[tuplePtr][attrLoc]).IsEmpty)
                        {
                            isMonotonic = false;
                            continue;
                        }

                        if (acRes is null) acRes = ac;
                        else
                        {
                            lock (ctsLocker)
                            {
                                if (cts.Token.IsCancellationRequested) return;
                            }
                            acRes = acRes.IntersectWith(ac);
                            if (acRes.IsFalse())
                            {
                                isConflicting = true;
                                isMonotonic = false;
                                break;
                            }
                            nonConflictingComponentsCount++;
                        }
                    }

                    if (isMonotonic && acRes is not null)
                    {
                        lock (ctsLocker)
                            lock (resLocker)
                            {
                                isFalse = false;
                                cts.Cancel();
                                return;
                            }
                    }
                    if (!isConflicting && acRes is not null)
                    {
                        lock (resLocker)
                        {
                            nonConflictingColumns
                                .Add((attrLoc, nonConflictingComponentsCount));
                        }
                    }

                    return;
                }
            }

            bool IsReducedSystemFalse()
            {
                int nonConflictingAttributesLen = nonConflictingAttributes.Length;
                if (nonConflictingAttributesLen == 0) return true;
                if (nonConflictingAttributesLen == RowLength) return false;

                DisjunctiveTuple<TEntity>[] rearrangedTuples = null;
                DisjunctiveTuple<TEntity> dTuple;
                int nonEmptyRowPtr = 0, 
                    emptyRowPtr = ColumnLength,
                    attrLoc, 
                    i;
                bool rowIsNonEmpty;

                for (int tuplePtr = 0; tuplePtr < ColumnLength; tuplePtr++)
                {
                    rowIsNonEmpty = false;
                    dTuple = this[tuplePtr];
                    for (i = 0; i < nonConflictingAttributesLen; i++)
                    {
                        attrLoc = nonConflictingAttributes[i].AttrLoc;
                        if (dTuple[attrLoc].IsEmpty) continue;
                        rowIsNonEmpty = true;
                        break;
                    }
                    if (rowIsNonEmpty)
                    {
                        if (rearrangedTuples is not null)
                            rearrangedTuples[nonEmptyRowPtr] = dTuple;
                        nonEmptyRowPtr++;
                    }
                    else
                    {
                        if (rearrangedTuples is null)
                        {
                            rearrangedTuples = 
                                new DisjunctiveTuple<TEntity>[ColumnLength];
                            Array.Copy(
                                _tuples,
                                0,
                                rearrangedTuples,
                                0,
                                nonEmptyRowPtr);
                        }
                        rearrangedTuples[--emptyRowPtr] = dTuple;
                    }
                }

                // если этот блок монотонный, то система непустая
                if (emptyRowPtr == ColumnLength) return false;

                return Factory.CreateDisjunctiveTupleSystem(
                    rearrangedTuples[emptyRowPtr..],
                    ReduceSchema,
                    null)
                    .IsFalse();

                void ReduceSchema(TupleObjectBuilder<TEntity> builder)
                {
                    builder.Schema = Schema.Clone();

                    for (int j = 0; j < nonConflictingAttributesLen; j++)
                    {
                        builder.Attribute(
                            Schema.AttributeAt(nonConflictingAttributes[j].AttrLoc))
                            .Detach();
                    }

                    builder.EndSchemaInitialization();

                    return;
                }
            }
        }

        public override IAttributeComponent<TAttribute>
            GetDefaultFictionalAttributeComponent<TAttribute>(
                IAttributeComponentFactory<TAttribute> factory)
        {
            return factory.CreateEmpty();
        }

        public override TupleObject<TEntity> Reproduce(
            IEnumerable<TupleObject<TEntity>> tuples,
            TupleObjectFactory factory,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
        {
            return factory.CreateDisjunctiveTupleSystem(
                tuples, 
                onTupleBuilding, 
                builder);
        }

        protected override IEnumerator<TEntity> GetEnumeratorImpl()
        {
            return TrueIntersect().GetEnumerator();
        }

        public TupleObject<TEntity> TrueIntersect()
        {
            return (SetOperations as IDisjunctiveTupleSystemOperationExecutorsContainer)!
                .TrueIntersect(this);
        }

        #region Nested types

        public interface IDisjunctiveTupleSystemOperationExecutorsContainer
        {
            public TupleObject<TEntity> TrueIntersect(
                DisjunctiveTupleSystem<TEntity> first);
        }

        public class DisjunctiveTupleSystemOperationExecutorsContainer
            : NonFictionalTupleObjectOperationExecutorsContainer<DisjunctiveTupleSystem<TEntity>>,
              IDisjunctiveTupleSystemOperationExecutorsContainer
        {
            private Lazy<DisjunctiveTupleSystemTrueIntersectionOperator<TEntity>>
                _trueIntersectionOperator;

            public DisjunctiveTupleSystemTrueIntersectionOperator<TEntity>
                TrueIntersectionOperator => _trueIntersectionOperator.Value;

            #region Constructors

            public DisjunctiveTupleSystemOperationExecutorsContainer(
                TupleObjectFactory factory)
                : base(factory,
                       () => new DisjunctiveTupleSystemComplementionOperator<TEntity>(),
                       () => new DisjunctiveTupleSystemConversionToAlternateOperator<TEntity>(),
                       () => new DisjunctiveTupleSystemIntersectionOperator<TEntity>(),
                       () => new DisjunctiveTupleSystemUnionOperator<TEntity>(),
                       () => new DisjunctiveTupleSystemExceptionOperator<TEntity>(),
                       () => new DisjunctiveTupleSystemSymmetricExceptionOperator<TEntity>(),
                       () => new DisjunctiveTupleSystemInclusionComparer<TEntity>(),
                       () => new DisjunctiveTupleSystemEqualityComparer<TEntity>(),
                       () => new DisjunctiveTupleSystemInclusionOrEqualityComparer<TEntity>())
            {
                _trueIntersectionOperator = 
                    new Lazy<DisjunctiveTupleSystemTrueIntersectionOperator<TEntity>>();

                return;
            }

            #endregion

            public TupleObject<TEntity> TrueIntersect(
                DisjunctiveTupleSystem<TEntity> first)
            {
                return TrueIntersectionOperator.Intersect(first, Factory);
            }
        }

        #endregion
    }
}
