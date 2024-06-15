using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators
{
    using static TupleObjectConversionToAlternateOperator;

    public static class TupleObjectUnionOperator
    {
        private static TupleObject<TEntity> UnionImpl<TEntity>(
            DisjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return factory.CreateDisjunctiveTuple<TEntity>(
                attributes: UnionComponents(),
                first.Schema.PassToBuilder,
                null);

            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>>
                UnionComponents()
            {
                int len = first.RowLength;

                for (int attrLoc = 0; attrLoc < len; attrLoc++)
                {
                    yield return new(
                        attrLoc,
                        first.Schema,
                        first[attrLoc].UnionWith(second[attrLoc]));
                }

                yield break;
            }
        }

        public static TupleObject<TEntity> Union<TEntity>(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return factory.CreateConjunctiveTupleSystem(
                [first, second],
                first.Schema.PassToBuilder,
                null);
        }

        public static TupleObject<TEntity> Union<TEntity>(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return UnionAsConjunctiveTupleSystem(
                first,
                second,
                factory,
                op1 => [op1],
                op2 => ConvertToAlternateTupleEnum(op2, factory));
        }

        public static TupleObject<TEntity> Union<TEntity>(
            DisjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return UnionImpl(first, second, factory);
        }

        public static TupleObject<TEntity> Union<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return UnionAsConjunctiveTupleSystem(
                first,
                second,
                factory,
                op1 => op1.Tuples,
                op2 => [op2]);
        }

        public static TupleObject<TEntity> Union<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return UnionAsConjunctiveTupleSystem(
                first,
                second,
                factory,
                op1 => op1.Tuples,
                op2 => op2.Tuples);
        }

        public static TupleObject<TEntity> Union<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return UnionAsConjunctiveTupleSystem(
                first,
                second,
                factory,
                op1 => op1.Tuples,
                op2 => ConvertToAlternateTupleEnum(op2, factory));
        }

        public static TupleObject<TEntity> Union<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return UnionAsConjunctiveTupleSystem(
                first,
                second,
                factory,
                op1 => op1.Tuples,
                op2 => ConvertToAlternateTupleEnum(op2, factory));
        }

        public static TupleObject<TEntity> Union<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return UnionAsConjunctiveTupleSystem(
                first,
                second,
                factory,
                op1 => ConvertToAlternateTupleEnum(op1, factory),
                op2 => [op2]);
        }

        public static TupleObject<TEntity> Union<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return factory.CreateDisjunctiveTupleSystem(
                PairwiseUnion(),
                first.Schema.PassToBuilder,
                null);

            IEnumerable<DisjunctiveTuple<TEntity>> PairwiseUnion()
            {
                DisjunctiveTuple<TEntity>[] tuples = first.Tuples;
                DisjunctiveTuple<TEntity> tuple;
                int len = tuples.Length;
                for (int i = 0; i < len; i++)
                {
                    tuple = tuples[i];
                    switch (Union(second, tuple, factory))
                    {
                        case DisjunctiveTuple<TEntity> ct:
                            {
                                yield return ct;

                                break;
                            }
                        case DisjunctiveTupleSystem<TEntity> cts:
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

        public static TupleObject<TEntity> Union<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return factory.CreateConjunctiveTupleSystem(
                PairwiseUnion(),
                first.Schema.PassToBuilder,
                null);

            IEnumerable<TupleObject<TEntity>> PairwiseUnion()
            {
                DisjunctiveTuple<TEntity>[] tuples = first.Tuples;
                int len = tuples.Length;
                for (int i = 0; i < len; i++)
                {
                    yield return UnionImpl(second, tuples[i], factory);
                }

                yield break;
            }
        }

        private static TupleObject<TEntity>
            UnionAsConjunctiveTupleSystem<
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
            return factory.CreateConjunctiveTupleSystem(
                GetTuples(),
                second.PassSchema,
                null);

            IEnumerable<TupleObject<TEntity>> GetTuples()
            {
                return getFirstTuples(first).Concat(getSecondTuples(second));
            }
        }
    }

    public abstract class TupleObjectUnionOperator<TEntity, TOperand1>
        : TupleObjectCrossTypeFactorySetBinaryOperator<TEntity, TOperand1>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
    {
        public override TupleObject<TEntity> Visit(
            TOperand1 first,
            EmptyTupleObject<TEntity> second,
            TupleObjectFactory factory)
        {
            return first;
        }

        public override TupleObject<TEntity> Visit(
            TOperand1 first,
            FullTupleObject<TEntity> second,
            TupleObjectFactory factory)
        {
            return second;
        }
    }
}
