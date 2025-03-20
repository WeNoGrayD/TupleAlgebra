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
    public static class TupleObjectInclusionComparer
    {
        public static bool Includes<TEntity>(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            for (int attrLoc = 0; attrLoc < first.RowLength; attrLoc++)
            {
                // !(* > *), but 
                // [*] > [*]
                if (first[attrLoc].Includes(second[attrLoc]) ||
                    first[attrLoc].IsFull) continue;

                return false;
            }

            return true;
        }

        public static bool Includes<TEntity>(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            return first.Includes(!second);
        }

        public static bool Includes<TEntity>(
            DisjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            for (int attrLoc = 0; attrLoc < first.RowLength; attrLoc++)
            {
                if (first[attrLoc].IncludesOrEqualsTo(second[attrLoc]))
                    return true;
            }

            return false;
        }

        public static bool Includes<TEntity>(
            DisjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            // !(Ø > Ø), но
            // ]Ø[ > ]Ø[
            for (int attrLoc = 0; attrLoc < first.RowLength; attrLoc++)
            {
                if (first[attrLoc].Includes(second[attrLoc]) ||
                    second[attrLoc].IsEmpty) continue;

                return false;
            }

            return true;
        }

        public static bool Includes<TEntity>(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second)
            where TEntity : new()
        {
            return (second / first).IsFalse();
        }

        public static bool Includes<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            return (second / first).IsFalse();
        }

        public static bool Includes<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second)
            where TEntity : new()
        {
            return (second / first).IsFalse();
        }

        public static bool Includes<TEntity>(
            DisjunctiveTuple<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second)
            where TEntity : new()
        {
            for (int tuplePtr = 0; tuplePtr < second.ColumnLength; tuplePtr++)
            {
                if (Includes(first, second[tuplePtr])) continue;

                return false;
            }

            return true;
        }

        public static bool Includes<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            return first.Includes(!second);
        }

        public static bool Includes<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second)
            where TEntity : new()
        {
            return first.Includes(!second);
        }

        public static bool Includes<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second)
            where TEntity : new()
        {
            for (int tuplePtr = 0; tuplePtr < first.ColumnLength; tuplePtr++)
            {
                if (Includes(first[tuplePtr], second))
                    continue;

                return false;
            }

            return true;
        }

        public static bool Includes<TEntity>(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second)
            where TEntity : new()
        {
            return first.Includes(!second);
        }

        public static bool Includes<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            for (int tuplePtr = 0; tuplePtr < first.ColumnLength; tuplePtr++)
            {
                if (Includes(first[tuplePtr], second)) continue;

                return false;
            }

            return true;
        }

        public static bool Includes<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second)
            where TEntity : new()
        {
            TupleObject<TEntity> altered = !second;

            for (int tuplePtr = 0; tuplePtr < first.ColumnLength; tuplePtr++)
            {
                if (first[tuplePtr] > altered) continue;

                return false;
            }

            return true;
        }

        public static bool Includes<TEntity>(
            DisjunctiveTuple<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second)
            where TEntity : new()
        {
            return first.Includes(!second);
        }

        public static bool Includes<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            for (int tuplePtr = 0; tuplePtr < first.ColumnLength; tuplePtr++)
            {
                if (Includes(first[tuplePtr], second)) continue;

                return false;
            }

            return true;
        }
    }

    public abstract class TupleObjectInclusionComparer<TEntity, TOperand1>
        : TupleObjectCrossTypeInstantBooleanBinaryVisitor<TEntity, TOperand1>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
    {
        public override bool Visit(
            TOperand1 first,
            EmptyTupleObject<TEntity> second)
        {
            return true;
        }

        public override bool Visit(
            TOperand1 first,
            FullTupleObject<TEntity> second)
        {
            return false;
        }
    }
}
