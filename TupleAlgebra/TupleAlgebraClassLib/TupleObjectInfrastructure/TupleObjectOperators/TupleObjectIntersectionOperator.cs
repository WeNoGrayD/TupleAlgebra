using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.TupleBased;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjects;
using UniversalClassLib;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators
{
    using static TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectHelper;
    using static TupleObjectConversionToAlternateOperator;
    using static UniversalClassLib.CartesianProductHelper;

    public static class TupleObjectIntersectionOperations
    {
        private static TraverseInfo<TEntity> MakeTraverseInfo<TEntity>(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            TupleObjectBuilder<TEntity> builder = factory.GetBuilder<TEntity>();
            TupleObjectSchema<TEntity> generalSchema = first.Schema;
            TupleObjectSchema<TEntity> staticSchema = generalSchema.Clone();
            TupleObjectSchema<TEntity> maskedSchema = generalSchema.Clone();
            TupleObjectBuildingHandler<TEntity> onStaticTupleBuilding = 
                staticSchema.PassToBuilder,
                                                onMaskedTupleBuilding = 
                maskedSchema.PassToBuilder;

            for (int attrLoc = 0; attrLoc < first.RowLength; attrLoc++)
            {
                int ai = attrLoc;
                if (first[attrLoc] is IAttributeComponentWithVariables ||
                    second[attrLoc] is IAttributeComponentWithVariables)
                {
                    onStaticTupleBuilding += (b) => b.Attribute(
                        generalSchema.AttributeAt(ai)).Detach();
                }
                else
                {
                    onMaskedTupleBuilding += (b) => b.Attribute(
                        generalSchema.AttributeAt(ai)).Detach();
                }
            }
            onStaticTupleBuilding += (b) => b.EndSchemaInitialization();
            onMaskedTupleBuilding += (b) => b.EndSchemaInitialization();
            onStaticTupleBuilding(builder);
            onMaskedTupleBuilding(builder);

            return new TraverseInfo<TEntity>(
                first.Schema,
                staticSchema,
                maskedSchema);
        }

        private static TupleObject<TEntity> IntersectImpl<TEntity>(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory,
            IndexedComponentFactoryArgs<IAttributeComponent>[] components = null)
            where TEntity : new()
        {
            return factory.CreateConjunctiveTuple<TEntity>(
                attributes: IntersectComponents(),
                first.Schema.PassToBuilder,
                null);

            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>>
                IntersectComponents()
            {
                int len = first.RowLength;

                for (int attrLoc = 0; attrLoc < len; attrLoc++)
                {
                    yield return new(
                        attrLoc,
                        first.Schema,
                        first[attrLoc].IntersectWith(second[attrLoc]));
                }

                yield break;
            }
        }

        public static TupleObject<TEntity> Intersect<TEntity>(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            var traverseInfo = MakeTraverseInfo(first, second, factory);

            return traverseInfo.HasMaskedPart ?
                /*
                traverseInfo.Intersect(
                    first, 
                    second, 
                    factory, 
                    (op1, op2, f) => IntersectImpl(op1, op2, f)) :*/
                first.Unfold() & second.Unfold() :
                IntersectImpl(first, second, factory);
        }

        public static TupleObject<TEntity> Intersect<TEntity>(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return IntersectAsDisjunctiveTupleSystem(
                first,
                second,
                factory,
                op1 => ConvertToAlternateTupleEnum(op1, factory),
                op2 => [op2]);
        }

        public static TupleObject<TEntity> Intersect<TEntity>(
            DisjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return factory.CreateDisjunctiveTupleSystem(
                [first, second],
                first.Schema.PassToBuilder,
                null);
        }

        public static TupleObject<TEntity> Intersect<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            //return IntersectAsync(first, second, factory);

            return factory.CreateConjunctiveTupleSystem(
                PairwiseIntersection(),
                first.Schema.PassToBuilder,
                null);

            IEnumerable<TupleObject<TEntity>> PairwiseIntersection()
            {
                ConjunctiveTuple<TEntity>[] tuples = first.Tuples;
                int len = tuples.Length;
                for (int i = 0; i < len; i++)
                {
                    yield return Intersect(second, tuples[i], factory);
                }

                yield break;
            }
        }

        public static TupleObject<TEntity> Intersect<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            //return IntersectAsync(first, second, factory);
            
            return factory.CreateConjunctiveTupleSystem(
                PairwiseIntersection(),
                first.Schema.PassToBuilder,
                null);

            IEnumerable<ConjunctiveTuple<TEntity>> PairwiseIntersection()
            {
                ConjunctiveTuple<TEntity>[] tuples = first.Tuples;
                ConjunctiveTuple<TEntity> tuple;
                int len = tuples.Length;
                for (int i = 0; i < len; i++)
                {
                    tuple = tuples[i];
                    switch (Intersect(second, tuple, factory))
                    {
                        case ConjunctiveTuple<TEntity> ct:
                            {
                                yield return ct;

                                break;
                            }
                        case ConjunctiveTupleSystem<TEntity> cts:
                            {
                                for (int j = 0; j < cts.ColumnLength; j++)
                                    yield return cts[j];

                                break;
                            }
                        default: continue;
                    };
                }

                yield break;
            }
            
        }

        public static TupleObject<TEntity> Intersect<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            /*
            return IntersectConjunctiveSystemWithDisjunctiveTupleObject(
                first,
                second,
                factory,
                _ => 1,
                (op2, tuples, j) => tuples[j] = op2);
            */
            return IntersectAsDisjunctiveTupleSystem(
                first,
                second,
                factory,
                op1 => ConvertToAlternateTupleEnum(op1, factory),
                op2 => [op2]);
        }

        public static TupleObject<TEntity> Intersect<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            /*
            return IntersectConjunctiveSystemWithDisjunctiveTupleObject(
                first,
                second,
                factory,
                (op2) => op2.ColumnLength,
                (op2, tuples, j) => op2.Tuples.CopyTo(tuples, j));
            */
            return IntersectAsDisjunctiveTupleSystem(
                first,
                second,
                factory,
                op1 => ConvertToAlternateTupleEnum(op1, factory),
                op2 => op2.Tuples);
        }

        public static TupleObject<TEntity> Intersect<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return IntersectAsDisjunctiveTupleSystem(
                first,
                second,
                factory,
                op1 => op1.Tuples,
                op2 => ConvertToAlternateTupleEnum(op2, factory));
        }

        public static TupleObject<TEntity> Intersect<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return IntersectAsDisjunctiveTupleSystem(
                first,
                second,
                factory,
                op1 => op1.Tuples,
                op2 => op2.Tuples);
        }

        public static TupleObject<TEntity> Intersect<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return IntersectAsDisjunctiveTupleSystem(
                first,
                second,
                factory,
                op1 => op1.Tuples,
                op2 => [op2]);
        }

        /*
        private static TupleObject<TEntity>
            IntersectDisjunctiveSystemWithDisjunctiveTupleObject<
            TEntity,
            TOperand2>(
            DisjunctiveTupleSystem<TEntity> first,
            TOperand2 second,
            TupleObjectFactory factory,
            Func<TOperand2, int> getSecondLen,
            Action<TOperand2, DisjunctiveTuple<TEntity>[], int> copyToResultAt)
            where TEntity : new()
            where TOperand2 : TupleObject<TEntity>
        {
            int len1 = first.ColumnLength,
                len2 = getSecondLen(second),
                len = len1 + len2;
            DisjunctiveTuple<TEntity>[] tuples =
                new DisjunctiveTuple<TEntity>[len];

            first.Tuples.CopyTo(tuples, 0);
            copyToResultAt(second, tuples, len1);

            return factory.CreateDisjunctiveTupleSystem(
                tuples, 
                second.PassSchema,
                null);
        }

        private static TupleObject<TEntity> IntersectConjunctiveTupleWithDisjunctiveTupleObject<
            TEntity,
            TOperand2>(
            ConjunctiveTuple<TEntity> first,
            TOperand2 second,
            TupleObjectFactory factory,
            Func<TOperand2, int> getSecondLen,
            Action<TOperand2, DisjunctiveTuple<TEntity>[], int> copyToResultAt)
            where TEntity : new()
            where TOperand2 : TupleObject<TEntity>
        {
            DisjunctiveTupleSystem<TEntity> firstDisjunctive =
                first.ConvertToAlternate() as
                DisjunctiveTupleSystem<TEntity>;

            return IntersectDisjunctiveSystemWithDisjunctiveTupleObject(
                firstDisjunctive, second, factory, getSecondLen, copyToResultAt);
        }
        */

        /*
        private static TupleObject<TEntity> IntersectConjunctiveSystemWithDisjunctiveTupleObject<
            TEntity,
            TOperand2>(
            ConjunctiveTupleSystem<TEntity> first,
            TOperand2 second,
            TupleObjectFactory factory,
            Func<TOperand2, int> getSecondLen,
            Action<TOperand2, DisjunctiveTuple<TEntity>[], int> copyToResultAt)
            where TEntity : new()
            where TOperand2 : TupleObject<TEntity>
        {
            int len1 = first.ColumnLength,
                len2 = getSecondLen(second),
                len = (len1 * first.RowLength) + len2;
            DisjunctiveTuple<TEntity>[] tuples =
                new DisjunctiveTuple<TEntity>[len];
            DisjunctiveTupleSystem<TEntity> dsys;

            int j = 0;
            for (int i = 0; i < len1; i++)
            {
                dsys = (first[i].ConvertToAlternate()
                    as DisjunctiveTupleSystem<TEntity>)!;
                dsys.Tuples.CopyTo(tuples, j);

                j += dsys.ColumnLength;
            }

            copyToResultAt(second, tuples, j);
            j += len2;
            if (j < len) tuples = tuples[..j];

            return factory.CreateDisjunctiveTupleSystem(
                tuples,
                second.PassSchema, 
                null);
        }
        */

        /*
        private static TupleObject<TEntity> IntersectConjunctiveSystemWithDisjunctiveTupleObject<
            TEntity,
            TOperand2>(
            ConjunctiveTupleSystem<TEntity> first,
            TOperand2 second,
            TupleObjectFactory factory,
            Func<TOperand2, IEnumerable<DisjunctiveTuple<TEntity>>> getSecondTuples)
            where TEntity : new()
            where TOperand2 : TupleObject<TEntity>
        {
            return factory.CreateDisjunctiveTupleSystem(
                GetTuples(),
                second.PassSchema,
                null);

            IEnumerable<TupleObject<TEntity>> GetTuples()
            {
                return ConvertToAlternateTupleEnum(first, factory)
                    .Concat(getSecondTuples(second));
            }
        }
        */

        private static TupleObject<TEntity> 
            IntersectAsDisjunctiveTupleSystem<
            TEntity,
            TOperand1,
            TOperand2>(
            TOperand1 first,
            TOperand2 second,
            TupleObjectFactory factory,
            Func<TOperand1, IEnumerable<TupleObject<TEntity>>> getFirstTuples,
            Func<TOperand2, IEnumerable<TupleObject<TEntity>>> getSecondTuples)
            where TEntity : new()
            where TOperand1 : TupleObject<TEntity>
            where TOperand2 : TupleObject<TEntity>
        {
            return factory.CreateDisjunctiveTupleSystem(
                GetTuples(),
                second.PassSchema,
                null);

            IEnumerable<TupleObject<TEntity>> GetTuples()
            {
                return getFirstTuples(first).Concat(getSecondTuples(second));
            }
        }






        private async static Task<TupleObject<TEntity>> IntersectImplAsync<TEntity>(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory,
            IndexedComponentFactoryArgs<IAttributeComponent>[] components = null)
            where TEntity : new()
        {
            int len = first.RowLength;
            components ??= new IndexedComponentFactoryArgs<IAttributeComponent>[len];

            for (int attrLoc = 0; attrLoc < len; attrLoc++)
            {
                components[attrLoc] = new(
                    attrLoc,
                    first.Schema,
                    first[attrLoc].IntersectWith(second[attrLoc]));
            }

            return factory.CreateConjunctiveTuple<TEntity>(
                attributes: components,
                first.Schema.PassToBuilder,
                null);
        }





        private static IndexedComponentFactoryArgs<IAttributeComponent>[][]
            MakeTupleFactoryArgs(int rowLen)
        {
            IndexedComponentFactoryArgs<IAttributeComponent>[][] tupleFactoryArgs =
                new IndexedComponentFactoryArgs<IAttributeComponent>
                [Environment.ProcessorCount][];
            for (int i = 0; i < 0; i++)
            {
                tupleFactoryArgs[i] =
                    new IndexedComponentFactoryArgs<IAttributeComponent>[rowLen];
            }

            return tupleFactoryArgs;
        }

        private static IEnumerable<TupleObject<TEntity>> PairwiseIntersection<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory,
            IndexedComponentFactoryArgs<IAttributeComponent>[][] tupleFactoryArgs = null)
            where TEntity : new()
        {
            ConjunctiveTuple<TEntity>[] tuples = first.Tuples;
            int len = tuples.Length,
                processorCount = Environment.ProcessorCount;

            tupleFactoryArgs ??= MakeTupleFactoryArgs(first.RowLength);

            return tuples.AsParallel()
                .WithMergeOptions(ParallelMergeOptions.NotBuffered)
                //.Zip(GetExecutingProcessors())
                .Select(tupleFarg =>
                    IntersectImpl(
                        second,
                        tupleFarg,//.First,
                        factory));//,
                        //tupleFactoryArgs[tupleFarg.Second]));

            /*
            IEnumerable<int> GetExecutingProcessors()
            {
                int core;
                while (true)
                {
                    for (core = 0; core < processorCount; core++)
                        yield return core;
                }

                yield break;
            }
            */
        }

        public static TupleObject<TEntity> IntersectAsync<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return factory.CreateConjunctiveTupleSystem(
                PairwiseIntersection(first, second, factory),
                first.Schema.PassToBuilder,
                null);
        }

        public static TupleObject<TEntity> IntersectAsync<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return factory.CreateConjunctiveTupleSystem(
                PairwiseIntersectionImpl(),
                first.Schema.PassToBuilder,
                null);

            IEnumerable<TupleObject<TEntity>> PairwiseIntersectionImpl()
            {
                ConjunctiveTuple<TEntity>[] tuples = first.Tuples;
                int len = tuples.Length;

                var tupleFactoryArgs = MakeTupleFactoryArgs(first.RowLength);

                return tuples
                    .SelectMany(tuple => 
                        PairwiseIntersection(
                            second, 
                            tuple, 
                            factory, 
                            tupleFactoryArgs));
            }
        }

        /*

            async IAsyncEnumerable<TupleObject<TEntity>> PairwiseIntersection2()
            {
                BlockingCollection<Task<TupleObject<TEntity>>>
                    tasks = new BlockingCollection<Task<TupleObject<TEntity>>>(
                        Environment.ProcessorCount);
                ConjunctiveTuple<TEntity>[] tuples = first.Tuples;
                int len = tuples.Length;

                IndexedComponentFactoryArgs<IAttributeComponent>[] components =
                    new IndexedComponentFactoryArgs<IAttributeComponent>[len];

                var loop = Parallel.For(
                    0,
                    len,
                    CC);

                Task<TupleObject<TEntity>> task;
                for (int left = len; left >= 0; left--)
                {
                    while (!tasks.TryTake(out task)) ;
                    yield return await tasks.Take();
                }

                yield break;

                void CC(int i)
                {
                    var tuple = 
                        IntersectImpl(second, tuples[i], factory, components);
                    tasks.Add(Task.FromResult(tuple));

                    return;
                }
            }*/

        public static IEnumerable<TupleObject<TEntity>> IntersectEnum<
            TEntity>(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null,
            IAttributeComponentWithVariables[] tupleBasedComponents = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>(first.PassSchema);
            tupleBasedComponents ??=
                new IAttributeComponentWithVariables[first.RowLength];
            // тут возникнет ошибка, поскольку такие переменные могут встречаться у обоих операндов и не во всех атрибутах в каждом
            for (int attrLoc = 0; attrLoc < first.RowLength; attrLoc++)
            {
                tupleBasedComponents[attrLoc] =
                    first[attrLoc] as IAttributeComponentWithVariables;
            }

            return GetCartesianProduct(
                IntersectImpl,
                tupleBasedComponents,
                (cmp) => (cmp as IAttributeComponentWithVariables).UnfoldAsEnum());

            TupleObject<TEntity> IntersectImpl(IEnumerator[] components)
            {
                return factory.CreateConjunctiveTuple(
                    GetFactoryArgs(),
                    null,
                    builder);

                IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>>
                    GetFactoryArgs()
                {
                    IAttributeComponent intersected;
                    for (int attrLoc = 0; attrLoc < first.RowLength; attrLoc++)
                    {
                        intersected = (components[attrLoc].Current as IAttributeComponent)
                            .IntersectWith(second[attrLoc]);
                        if (first.VariableContainer
                            .AnyIsEmpty(tupleBasedComponents[attrLoc]))
                        {
                            // возвращение пустой компоненты и обрыв дальнейшего вычисления
                            yield return new IndexedComponentFactoryArgs<IAttributeComponent>(
                                attrLoc,
                                builder);
                        }
                        yield return new IndexedComponentFactoryArgs<IAttributeComponent>(
                            attrLoc,
                            builder,
                            intersected);
                    }

                    yield break;
                }
            }
        }

        public static IEnumerable<TupleObject<TEntity>> IntersectEnum<
            TEntity>(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null,
            IAttributeComponentWithVariables[] tupleBasedComponents = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>(first.PassSchema);
            tupleBasedComponents ??=
                new IAttributeComponentWithVariables[first.RowLength];

            for (int tuplePtr = 0; tuplePtr < first.RowLength; tuplePtr++)
            {
                foreach (TupleObject<TEntity> res
                     in IntersectEnum(
                         first,
                         second[tuplePtr],
                         factory,
                         builder,
                         tupleBasedComponents))
                    yield return res;
            }

            yield break;
        }

        public static IEnumerable<TupleObject<TEntity>> IntersectEnum<
            TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null,
            IAttributeComponentWithVariables[] tupleBasedComponents = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>(first.PassSchema);
            tupleBasedComponents ??=
                new IAttributeComponentWithVariables[first.RowLength];

            for (int tuplePtr = 0; tuplePtr < first.RowLength; tuplePtr++)
            {
                foreach (TupleObject<TEntity> res
                     in IntersectEnum(
                         first[tuplePtr],
                         second,
                         factory,
                         builder,
                         tupleBasedComponents))
                    yield return res;
            }

            yield break;
        }
    }

    public abstract class TupleObjectIntersectionOperator<TEntity, TOperand1>
        : TupleObjectCrossTypeFactorySetBinaryOperator<TEntity, TOperand1>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
    {
        public override TupleObject<TEntity> Visit(
            TOperand1 first,
            EmptyTupleObject<TEntity> second,
            TupleObjectFactory factory)
        {
            return second;
        }

        public override TupleObject<TEntity> Visit(
            TOperand1 first,
            FullTupleObject<TEntity> second,
            TupleObjectFactory factory)
        {
            return first;
        }
    }

    public readonly record struct TraverseInfo<TEntity>(
        TupleObjectSchema<TEntity> GeneralSchema,
        TupleObjectSchema<TEntity> StaticPartSchema,
        TupleObjectSchema<TEntity> MaskedPartSchema)
        where TEntity : new()
    {
        public delegate TupleObject<TEntity> IntersectionHandler<TOperand1, TOperand2>(
            TOperand1 first,
            TOperand2 second,
            TupleObjectFactory factory);

        public bool HasStaticPart 
        { get => StaticPartSchema.PluggedAttributesCount > 0; }

        public bool HasMaskedPart
        { get => MaskedPartSchema.PluggedAttributesCount > 0; }

        public TupleObject<TEntity> Enumerate(
            ConjunctiveTuple<TEntity> first,
            TupleObjectFactory factory)
        {
            TupleObjectBuilder<TEntity> builder;
            TupleObjectSchema<TEntity> maskedSchema = MaskedPartSchema;
            TupleObject<TEntity> maskedFirst =
                first.AlignWithSchema(MaskedPartSchema, factory, null);
            IAttributeComponentWithVariables[] tupleBasedComponents =
                new IAttributeComponentWithVariables[first.RowLength];
            // тут возникнет ошибка, поскольку такие переменные могут встречаться у обоих операндов и не во всех атрибутах в каждом
            for (int attrLoc = 0; attrLoc < first.RowLength; attrLoc++)
            {
                tupleBasedComponents[attrLoc] =
                    first[attrLoc] as IAttributeComponentWithVariables;
            }
            builder = factory.GetBuilder<TEntity>(maskedSchema.PassToBuilder);

            return (first.AlignWithSchema(StaticPartSchema, factory, null)
                         .AlignWithSchema(GeneralSchema, factory, null))
                & factory.CreateConjunctiveTupleSystem(
                    GetCartesianProduct(
                        EnumerateImpl,
                        tupleBasedComponents,
                        (cmp) => (cmp as IAttributeComponentWithVariables).UnfoldAsEnum()),
                    GeneralSchema.PassToBuilder,
                    null);

            TupleObject<TEntity> EnumerateImpl(IEnumerator[] components)
            {
                return factory.CreateConjunctiveTuple(
                    GetFactoryArgs(),
                    null,
                    builder);

                IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>>
                    GetFactoryArgs()
                {
                    for (int attrLoc = 0; attrLoc < first.RowLength; attrLoc++)
                    {
                        yield return new IndexedComponentFactoryArgs<IAttributeComponent>(
                            attrLoc,
                            builder,
                            (components[attrLoc].Current as IAttributeComponent)!);
                    }

                    yield break;
                }
            }
        }

        public TupleObject<TEntity> Intersect(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory,
            IntersectionHandler<ConjunctiveTuple<TEntity>, ConjunctiveTuple<TEntity>> operation/*,
            Func<TupleObject<TEntity>, 
                ConjunctiveTuple<TEntity>, 
                TupleObject<TEntity>> factoryHandler*/)
        {
            TupleObjectBuilder<TEntity> builder;
            TupleObject<TEntity> resStaticPart;
            ConjunctiveTuple<TEntity> maskedFirst;
            ConjunctiveTuple<TEntity> maskedSecond;

            if (HasStaticPart)
            {
                builder = factory.GetBuilder<TEntity>();
                ConjunctiveTuple<TEntity> staticFirst =
                    first.AlignWithSchema(StaticPartSchema, factory, builder)
                         .AlignWithSchema(GeneralSchema, factory, builder)
                    as ConjunctiveTuple<TEntity>;
                ConjunctiveTuple<TEntity> staticSecond =
                    second.AlignWithSchema(StaticPartSchema, factory, builder)
                          .AlignWithSchema(GeneralSchema, factory, builder)
                    as ConjunctiveTuple<TEntity>;
                resStaticPart = operation(staticFirst, staticSecond, factory);
                if (resStaticPart.IsEmpty()) return resStaticPart;

                maskedFirst =
                    first.AlignWithSchema(MaskedPartSchema, factory, builder)
                    as ConjunctiveTuple<TEntity>;
                maskedSecond =
                    second.AlignWithSchema(MaskedPartSchema, factory, builder)
                    as ConjunctiveTuple<TEntity>;
            }
            else
            {
                builder = factory.GetBuilder<TEntity>(MaskedPartSchema.PassToBuilder);
                resStaticPart = factory.CreateFull<TEntity>(
                    GeneralSchema.PassToBuilder);
                maskedFirst = first;
                maskedSecond = second;
            }

            return factory.CreateConjunctiveTupleSystem(
                    MakeTuples(), 
                    null,
                    builder);

            IEnumerable<TupleObject<TEntity>> MakeTuples()
            {
                foreach (var tuple in TupleObjectIntersectionOperations
                    .IntersectEnum(maskedFirst, maskedSecond, factory, builder))
                    yield return resStaticPart & tuple;

                yield break;
            }
        }
    }
}
