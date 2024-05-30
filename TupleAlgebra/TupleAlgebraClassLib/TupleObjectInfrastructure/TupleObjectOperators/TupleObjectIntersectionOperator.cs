using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectAcceptors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators
{
    public static class TupleObjectIntersectionOperations
    {
        private static TupleObject<TEntity> IntersectImpl<TEntity>(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory,
            IndexedComponentFactoryArgs<IAttributeComponent>[] components = null)
            where TEntity : new()
        {
            /*
            int len = first.RowLength;
            components ??= new IndexedComponentFactoryArgs<IAttributeComponent>[len];

            for (int attrLoc = 0; attrLoc < len; attrLoc++)
            {
                components[attrLoc] = new(
                    attrLoc,
                    first.Schema,
                    first[attrLoc].IntersectWith(second[attrLoc]));
            }
            */

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
            return IntersectImpl(first, second, factory);
        }

        public static TupleObject<TEntity> Intersect<TEntity>(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return IntersectConjunctiveTupleWithDisjunctiveTupleObject(
                first,
                second,
                factory,
                _ => 1,
                (op2, tuples, j) => tuples[j] = op2);

            /*
            return factory.CreateConjunctive<TEntity>(
                new SquareEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>>(
                    MakeTuples()),
                first.Schema.PassToBuilder,
                null);

            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>[]>
                MakeTuples()
            {
                ITupleObjectSchemaProvider schema = first.Schema;
                int len = first.RowLength;
                IndexedComponentFactoryArgs<IAttributeComponent>[] tupleFactoryArgs =
                    new IndexedComponentFactoryArgs<IAttributeComponent>[len];
                for (int attrLoc = 0; attrLoc < len; attrLoc++)
                {
                    tupleFactoryArgs[attrLoc] = new(attrLoc, schema, first[attrLoc]);
                }

                IndexedComponentFactoryArgs<IAttributeComponent> bufFarg;
                for (int i = 0; i < len; i++)
                {
                    bufFarg = tupleFactoryArgs[i];
                    tupleFactoryArgs[i] = new(i, schema, (first[i].IntersectWith(second[i])));
                    yield return tupleFactoryArgs;
                    tupleFactoryArgs[i] = bufFarg;
                }

                yield break;
            }
            */
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
            return IntersectAsync(first, second, factory);
            
            return factory.CreateConjunctiveTupleSystem(
                PairwiseIntersection(),
                first.Schema.PassToBuilder,
                null);

            IEnumerable<TupleObject<TEntity>> PairwiseIntersection()
            {
                ConjunctiveTuple<TEntity>[] tuples = first.Tuples;
                int len = tuples.Length;
                IndexedComponentFactoryArgs<IAttributeComponent>[] components =
                    new IndexedComponentFactoryArgs<IAttributeComponent>[len];
                for (int i = 0; i < len; i++)
                {
                    yield return IntersectImpl(second, tuples[i], factory, components);
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
            return IntersectAsync(first, second, factory);
            
            return factory.CreateConjunctiveTupleSystem(
                PairwiseIntersection(),
                first.Schema.PassToBuilder,
                null);

            IEnumerable<TupleObject<TEntity>> PairwiseIntersection()
            {
                SingleTupleObject<TEntity>[] tuples = first.Tuples;
                ConjunctiveTuple<TEntity> tuple;
                int len = tuples.Length;
                for (int i = 0; i < len; i++)
                {
                    tuple = (tuples[i] as ConjunctiveTuple<TEntity>)!;
                    yield return Intersect(second, tuple, factory);
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
            return IntersectConjunctiveSystemWithDisjunctiveTupleObject(
                first,
                second,
                factory,
                _ => 1,
                (op2, tuples, j) => tuples[j] = op2);
        }

        public static TupleObject<TEntity> Intersect<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return IntersectConjunctiveSystemWithDisjunctiveTupleObject(
                first,
                second,
                factory,
                (op2) => op2.ColumnLength,
                (op2, tuples, j) => op2.Tuples.CopyTo(tuples, j));
        }

        public static TupleObject<TEntity> Intersect<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return IntersectConjunctiveTupleWithDisjunctiveTupleObject(
                second,
                first,
                factory,
                op2 => op2.ColumnLength,
                (op2, tuples, j) => op2.Tuples.CopyTo(tuples, j));
        }

        public static TupleObject<TEntity> Intersect<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return IntersectDisjunctiveSystemWithDisjunctiveTupleObject(
                first,
                second,
                factory,
                _ => 1,
                (op2, tuples, j) => op2.Tuples.CopyTo(tuples, j));
        }

        public static TupleObject<TEntity> Intersect<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return IntersectDisjunctiveSystemWithDisjunctiveTupleObject(
                first,
                second,
                factory,
                _ => 1,
                (op2, tuples, j) => tuples[j] = op2);
        }

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

            /*
             * !!!!!!!!!!!!!!!
             * Здесь нужно попытаться переиспользовать хранилище.
             * В методе IEnumerable превращается в IList.
             * Нам без нужды.
             */
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

            /*
             * !!!!!!!!!!!!!!!
             * Здесь нужно попытаться переиспользовать хранилище.
             * В методе IEnumerable превращается в IList.
             * Нам без нужды.
             */
            return factory.CreateDisjunctiveTupleSystem(
                tuples,
                second.PassSchema, 
                null);
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
                .Zip(GetExecutingProcessors())
                .Select(tupleFarg =>
                    IntersectImpl(
                        second,
                        tupleFarg.First,
                        factory,
                        tupleFactoryArgs[tupleFarg.Second]));

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
    }

    public abstract class TupleObjectIntersectionOperator<TEntity, TOperand1>
        : TupleObjectCrossTypeFactorySetBinaryOperator<TEntity, TOperand1>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
    {
        public override TupleObject<TEntity> Accept(
            TOperand1 first,
            EmptyTupleObject<TEntity> second,
            TupleObjectFactory factory)
        {
            return second;
        }

        public override TupleObject<TEntity> Accept(
            TOperand1 first,
            FullTupleObject<TEntity> second,
            TupleObjectFactory factory)
        {
            return first;
        }
    }
}
