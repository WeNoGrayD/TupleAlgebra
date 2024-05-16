using System;
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
        public static TupleObject<TEntity> Intersect<TEntity>(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            int len = first.RowLength;
            IndexedComponentFactoryArgs<IAttributeComponent>[] components = 
                new IndexedComponentFactoryArgs<IAttributeComponent>[len];

            for (int attrLoc = 0; attrLoc < len; attrLoc++)
            {
                components[attrLoc] = new(
                    attrLoc, 
                    first.Schema, 
                    first[attrLoc].IntersectWith(second[attrLoc]));
            }

            return factory.CreateConjunctive<TEntity>(
                attributes: components,
                first.Schema.PassToBuilder,
                null);
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
            return factory.CreateDisjunctive(
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
            return factory.CreateConjunctive(
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
            return factory.CreateConjunctive(
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
            return factory.CreateDisjunctive(tuples, second.PassSchema, null);
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
                first.ToAlternateDiagonal(factory) as
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
                dsys = (first[i].ToAlternateDiagonal(factory)
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
            return factory.CreateDisjunctive(tuples, second.PassSchema, null);
        }
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
