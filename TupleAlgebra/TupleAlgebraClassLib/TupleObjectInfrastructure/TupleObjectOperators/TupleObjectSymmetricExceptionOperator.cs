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
    using static TupleObjectExceptionOperator;

    public static class TupleObjectSymmetricExceptionOperator
    {
        /// <summary>
        /// cTuple || cSys || empty
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="factory"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IEnumerable<TupleObject<TEntity>> SymmetricExceptEnum<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>(first.PassSchema);

            TupleObject<TEntity> tupleRes = factory.CreateFull(builder);
            ConjunctiveTuple<TEntity> symExcepted;

            for (int tuplePtr = 0; tuplePtr < first.RowLength; tuplePtr++)
            {
                symExcepted = first[tuplePtr];
                foreach (var tuple
                     in ExceptEnum(symExcepted, second, factory, builder))
                {
                    yield return tuple;
                }

                tupleRes &= first / symExcepted;
            }

            switch (tupleRes)
            {
                case ConjunctiveTuple<TEntity> cTuple:
                    {
                        yield return cTuple;
                        break;
                    }
                case ConjunctiveTupleSystem<TEntity> cSys:
                    {
                        for (int tuplePtr = 0; tuplePtr < cSys.ColumnLength; tuplePtr++)
                        {
                            yield return cSys[tuplePtr];
                        }

                        break;
                    }
                case EmptyTupleObject<TEntity> empty:
                    {
                        break;
                    }
            }

            yield break;
        }

        /// <summary>
        /// cTuple || cSys || empty
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="factory"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IEnumerable<TupleObject<TEntity>> SymmetricExceptEnum<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>(first.PassSchema);

            for (int tuplePtr = 0; tuplePtr < first.RowLength; tuplePtr++)
            {
                foreach (var tuple
                     in SymmetricExceptEnum(first, second[tuplePtr], factory, builder))
                {
                    yield return tuple;
                }
            }

            yield break;
        }

        /// <summary>
        /// cTuple || cSys || empty
        /// [a b c] ^ ]x y z[ =
        /// [x/a b c]
        /// [x ~b c]
        /// [x b ~c]
        /// [a y/b c]
        /// [~a y c]
        /// [a y ~c]
        /// [a b z/c]
        /// [~a b z]
        /// [a ~b z]
        /// [a/x b/y c/z]
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="factory"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IEnumerable<TupleObject<TEntity>> SymmetricExceptEnum<TEntity>(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>(first.PassSchema);

            foreach (var tuple in ExceptEnum(second, first, factory, builder))
                yield return tuple;

            yield return Except(first, second, factory, builder);

            yield break;
        }

        /// <summary>
        /// cTuple || cSys || empty
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="factory"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IEnumerable<TupleObject<TEntity>> SymmetricExceptEnum<TEntity>(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>(first.PassSchema);

            foreach (var tuple in ExceptEnum(first, second, factory, builder))
                yield return tuple;

            TupleObject<TEntity> excepted, tupleRes = factory.CreateFull(builder);

            for (int tuplePtr = 0; tuplePtr < second.ColumnLength; tuplePtr++)
            {
                if (tupleRes.IsEmpty()) break;

                excepted = factory.CreateConjunctiveTupleSystem(
                    ExceptEnum(second[tuplePtr], first, factory, builder),
                    null,
                    builder);
                tupleRes &= excepted;
            }

            switch (tupleRes)
            {
                case ConjunctiveTuple<TEntity> cTuple:
                    {
                        yield return cTuple;
                        break;
                    }
                case ConjunctiveTupleSystem<TEntity> cSys:
                    {
                        for (int tuplePtr = 0; tuplePtr < cSys.ColumnLength; tuplePtr++)
                        {
                            yield return cSys[tuplePtr];
                        }

                        break;
                    }
                case EmptyTupleObject<TEntity> empty:
                    {
                        break;
                    }
            }

            yield break;
        }

        /// <summary>
        /// cTuple || cSys || empty
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="factory"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IEnumerable<TupleObject<TEntity>> SymmetricExceptEnum<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>(first.PassSchema);

            for (int tuplePtr = 0; tuplePtr < first.ColumnLength; tuplePtr++)
            {
                yield return Except(first[tuplePtr], second, factory, builder);
            }

            foreach (var tuple
                 in ExceptEnumAsConjunctive(second, first, factory, builder))
            {
                yield return tuple;
            }

            yield break;
        }

        /// <summary>
        /// cTuple || cSys || empty
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="factory"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IEnumerable<TupleObject<TEntity>> SymmetricExceptEnum<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>(first.PassSchema);

            for (int tuplePtr = 0; tuplePtr < first.ColumnLength; tuplePtr++)
            {
                foreach (var tuple
                     in ExceptEnum(first[tuplePtr], second, factory, builder))
                {
                    yield return tuple;
                }

                foreach (var tuple
                     in ExceptEnumAsConjunctive(second[tuplePtr], first, factory, builder))
                {
                    yield return tuple;
                }
            }

            yield break;
        }

        /// <summary>
        /// dTuple || dSys || full
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="factory"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IEnumerable<TupleObject<TEntity>> SymmetricExceptEnum<TEntity>(
            DisjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>(first.PassSchema);

            foreach (var tuple in ExceptEnum(first, second, factory, builder))
                yield return tuple;

            foreach (var tuple in ExceptEnum(second, first, factory, builder))
                yield return tuple;

            yield break;
        }

        /// <summary>
        /// dTuple || dSys || full
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="factory"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IEnumerable<TupleObject<TEntity>> SymmetricExceptEnum<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>(first.PassSchema);

            foreach (var tuple in ExceptEnum(first, second, factory, builder))
                yield return tuple;

            foreach (var tuple in ExceptEnum(second, first, factory, builder))
                yield return tuple;

            yield break;
        }

        /// <summary>
        /// dTuple || dSys || full
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="factory"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IEnumerable<TupleObject<TEntity>> SymmetricExceptEnum<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>(first.PassSchema);

            foreach (var tuple in ExceptEnum(first, second, factory, builder))
                yield return tuple;

            foreach (var tuple in ExceptEnum(second, first, factory, builder))
                yield return tuple;

            yield break;
        }

        public static TupleObject<TEntity> SymmetricExcept<TEntity>(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return (first / second) | (second / first);
            /*
            return (first / second).UnionWith(second / first);
            */
        }

        public static TupleObject<TEntity> SymmetricExcept<TEntity>(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return (first / second) | (second / first);
            /*
            TupleObjectBuilder<TEntity> builder =
                factory.GetBuilder<TEntity>(first.PassSchema);

            return factory.CreateConjunctiveTupleSystem(
                SymmetricExceptEnum(first, second, factory, builder),
                null,
                builder);
            */
        }

        public static TupleObject<TEntity> SymmetricExcept<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return (first / second) | (second / first);
            /*
            TupleObjectBuilder<TEntity> builder =
                factory.GetBuilder<TEntity>(first.PassSchema);

            return factory.CreateConjunctiveTupleSystem(
                SymmetricExceptEnum(first, second, factory, builder),
                null,
                builder);
            */
        }

        public static TupleObject<TEntity> SymmetricExcept<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return (first / second) | (second / first);
            /*
            TupleObjectBuilder<TEntity> builder =
                factory.GetBuilder<TEntity>(first.PassSchema);

            return factory.CreateConjunctiveTupleSystem(
                SymmetricExceptEnum(first, second, factory, builder),
                null,
                builder);
            */
        }

        public static TupleObject<TEntity> SymmetricExcept<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return (first / second) | (second / first);
            /*
            TupleObjectBuilder<TEntity> builder =
                factory.GetBuilder<TEntity>(first.PassSchema);

            return factory.CreateConjunctiveTupleSystem(
                SymmetricExceptEnum(first, second, factory, builder),
                null,
                builder);
            */
        }

        public static TupleObject<TEntity> SymmetricExcept<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return (first / second) | (second / first);
            /*
            TupleObjectBuilder<TEntity> builder =
                factory.GetBuilder<TEntity>(first.PassSchema);

            return factory.CreateConjunctiveTupleSystem(
                SymmetricExceptEnum(first, second, factory, builder),
                null,
                builder);
            */
        }

        public static TupleObject<TEntity> SymmetricExcept<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return (first / second) | (second / first);
            /*
            TupleObjectBuilder<TEntity> builder =
                factory.GetBuilder<TEntity>(first.PassSchema);

            return factory.CreateConjunctiveTupleSystem(
                SymmetricExceptEnum(second, first, factory, builder),
                null,
                builder);
            */
        }

        public static TupleObject<TEntity> SymmetricExcept<TEntity>(
            DisjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return (first / second) | (second / first);
            /*
            TupleObjectBuilder<TEntity> builder =
                factory.GetBuilder<TEntity>(first.PassSchema);

            return factory.CreateDisjunctiveTupleSystem(
                SymmetricExceptEnum(first, second, factory, builder),
                null,
                builder);
            */
        }

        public static TupleObject<TEntity> SymmetricExcept<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return (first / second) | (second / first);
            /*
            TupleObjectBuilder<TEntity> builder =
                factory.GetBuilder<TEntity>(first.PassSchema);

            return factory.CreateDisjunctiveTupleSystem(
                SymmetricExceptEnum(first, second, factory, builder),
                null,
                builder);
            */
        }

        public static TupleObject<TEntity> SymmetricExcept<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return (first / second) | (second / first);
            /*
            TupleObjectBuilder<TEntity> builder =
                factory.GetBuilder<TEntity>(first.PassSchema);

            return factory.CreateDisjunctiveTupleSystem(
                SymmetricExceptEnum(first, second, factory, builder),
                null,
                builder);
            */
        }
    }

    public abstract class TupleObjectSymmetricExceptionOperator<TEntity, TOperand1>
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
            return ~first;
        }
    }
}
