using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators
{
    public static class TupleObjectEqualityComparer
    {
        public static bool AreEqual<TEntity>(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            return (first ^ second).IsFalse();
        }

        public static bool AreEqual<TEntity>(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            return (first ^ second).IsFalse();
        }

        public static bool AreEqual<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            return (first ^ second).IsFalse();
        }

        public static bool AreEqual<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second)
            where TEntity : new()
        {
            return (first ^ second).IsFalse();
        }

        public static bool AreEqual<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            return (first ^ second).IsFalse();
        }

        public static bool AreEqual<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second)
            where TEntity : new()
        {
            return (first ^ second).IsFalse();
        }

        public static bool AreEqual<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            return (first ^ second).IsFalse(); // first ^ second - это C-объект, его пустота будет очевидна
        }

        public static bool AreEqual<TEntity>(
            DisjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            TupleObject<TEntity> alteredFirst = !first,
                                 alteredSecond = !second;

            return (alteredFirst ^ alteredSecond).IsFalse(); // first ^ second - это D-объект, его пустоту необходимо доказывать
        }

        public static bool AreEqual<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second)
            where TEntity : new()
        {
            TupleObject<TEntity> alteredFirst = !first,
                                 alteredSecond = !second;

            return (alteredFirst ^ alteredSecond).IsFalse(); // first ^ second - это D-объект, его пустоту необходимо доказывать
        }

        public static bool AreEqual<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            TupleObject<TEntity> alteredFirst = !first,
                                 alteredSecond = !second;

            return (alteredFirst ^ alteredSecond).IsFalse(); // first ^ second - это D-объект, его пустоту необходимо доказывать
        }
    }

    public abstract class TupleObjectEqualityComparer<TEntity, TOperand1>
        : TupleObjectCrossTypeInstantBooleanBinaryVisitor<TEntity, TOperand1>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
    {
        public override bool Visit(
            TOperand1 first,
            EmptyTupleObject<TEntity> second)
        {
            return false;
        }

        public override bool Visit(
            TOperand1 first,
            FullTupleObject<TEntity> second)
        {
            return false;
        }
    }
}
