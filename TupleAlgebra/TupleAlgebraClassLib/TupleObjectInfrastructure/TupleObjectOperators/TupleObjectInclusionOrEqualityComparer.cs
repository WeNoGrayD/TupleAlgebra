using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators
{
    public static class TupleObjectInclusionOrEqualityComparer
    {
        /*
        public static bool IncludesOrAreEqual<TEntity>(
            DisjunctiveTuple<TEntity> first,
            ISingleTupleObject second)
            where TEntity : new()
        {
            for (int attrLoc = 0; attrLoc < first.RowLength; attrLoc++)
            {
                if (first[attrLoc].IncludesOrEqualsTo(second[attrLoc]))
                    return true;
            }

            return false;
        }

        public static bool IncludesOrAreEqual<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            ITupleObjectSystem second)
            where TEntity : new()
        {
            for (int tuplePtr1 = 0; tuplePtr1 < first.ColumnLength; tuplePtr1++)
            {
                for (int tuplePtr2 = 0; tuplePtr2 < second.ColumnLength; tuplePtr2++)
                {
                    if (IncludesOrAreEqual(first[tuplePtr1], second[tuplePtr2]))
                        continue;

                    return false;
                }
            }

            return true;
        }
        */

        public static bool IncludesOrAreEqual<TEntity>(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            for (int attrLoc = 0; attrLoc < first.RowLength; attrLoc++)
            {
                if (first[attrLoc].IncludesOrEqualsTo(second[attrLoc])) continue;

                return false;
            }

            return true;
        }

        public static bool IncludesOrAreEqual<TEntity>(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            return first.IncludesOrEqualsTo(!second);
        }

        public static bool IncludesOrAreEqual<TEntity>(
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

        public static bool IncludesOrAreEqual<TEntity>(
            DisjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            for (int attrLoc = 0; attrLoc < first.RowLength; attrLoc++)
            {
                if (first[attrLoc].IncludesOrEqualsTo(second[attrLoc])) continue;

                return false;
            }

            return true;
        }

        public static bool IncludesOrAreEqual<TEntity>(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second)
            where TEntity : new()
        {
            return (second / first).IsFalse();
        }

        public static bool IncludesOrAreEqual<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            return (second / first).IsFalse();
        }

        public static bool IncludesOrAreEqual<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second)
            where TEntity : new()
        {
            return (second / first).IsFalse();
        }

        public static bool IncludesOrAreEqual<TEntity>(
            DisjunctiveTuple<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second)
            where TEntity : new()
        {
            for (int tuplePtr = 0; tuplePtr < second.ColumnLength; tuplePtr++)
            {
                if (IncludesOrAreEqual(first, second[tuplePtr])) continue;

                return false;
            }

            return true;
        }

        public static bool IncludesOrAreEqual<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            return first.IncludesOrEqualsTo(!second);
        }

        public static bool IncludesOrAreEqual<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second)
            where TEntity : new()
        {
            return first.IncludesOrEqualsTo(!second);
        }

        public static bool IncludesOrAreEqual<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second)
            where TEntity : new()
        {
            for (int tuplePtr = 0; tuplePtr < first.ColumnLength; tuplePtr++)
            {
                if (IncludesOrAreEqual(first[tuplePtr], second))
                    continue;

                return false;
            }

            return true;
        }

        public static bool IncludesOrAreEqual<TEntity>(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second)
            where TEntity : new()
        {
            return first.IncludesOrEqualsTo(!second);
        }

        public static bool IncludesOrAreEqual<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            for (int tuplePtr = 0; tuplePtr < first.ColumnLength; tuplePtr++)
            {
                if (IncludesOrAreEqual(first[tuplePtr], second)) continue;

                return false;
            }

            return true;
        }

        public static bool IncludesOrAreEqual<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second)
            where TEntity : new()
        {
            TupleObject<TEntity> altered = !second;

            for (int tuplePtr = 0; tuplePtr < first.ColumnLength; tuplePtr++)
            {
                if (first[tuplePtr] >= altered) continue;

                return false;
            }

            return true;
        }

        public static bool IncludesOrAreEqual<TEntity>(
            DisjunctiveTuple<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second)
            where TEntity : new()
        {
            return first.IncludesOrEqualsTo(!second);
        }

        public static bool IncludesOrAreEqual<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second)
            where TEntity : new()
        {
            for (int tuplePtr = 0; tuplePtr < first.ColumnLength; tuplePtr++)
            {
                if (IncludesOrAreEqual(first[tuplePtr], second)) continue;

                return false;
            }

            return true;
        }
    }

    public abstract class TupleObjectInclusionOrEqualityComparer<TEntity, TOperand1>
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
