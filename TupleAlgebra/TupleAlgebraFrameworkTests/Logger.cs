using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraFrameworkTests
{
    public class Logger
    {
        public static void PrintSchema(ITupleObject tupleObject)
        {
            Console.WriteLine();
            Console.Write("[");
            foreach (var attrName in tupleObject.Schema.PluggedAttributeNames)
            {
                Console.Write($"{attrName,14}|");
            }
            Console.WriteLine("]");

            return;
        }

        public static void PrintSingleTupleObject(
            ISingleTupleObject tuple,
            string defaultAcSymbol,
            string openingBracket,
            string closingBracket,
            bool printSchema = true)
        {
            if (printSchema)
            {
                PrintSchema(tuple);
            }

            IAttributeComponent ac;

            Console.Write(openingBracket);
            for (int attrLoc = 0; attrLoc < tuple.RowLength; attrLoc++)
            {
                if (attrLoc > 0) Console.Write(", ");
                ac = tuple[attrLoc];
                if (ac.IsDefault)
                {
                    Console.Write($"{defaultAcSymbol + "      ",15}");
                    continue;
                }
                Console.Write($"[{string.Join(',', ExplodeAC()),13}]");
            }
            Console.WriteLine(closingBracket);

            return;

            IEnumerable<string> ExplodeAC()
            {
                foreach (object d in (IEnumerable)ac)
                {
                    yield return d.ToString();
                }

                yield break;
            }
        }

        public static void PrintTupleObjectSystem(
            ITupleObjectSystem tupleSys,
            string defaultAcSymbol,
            string openingBracket,
            string closingBracket)
        {
            PrintSchema(tupleSys);

            for (int i = 0; i < tupleSys.ColumnLength; i++)
            {
                Console.Write($"{i,-3} ");
                PrintSingleTupleObject(
                    tupleSys[i],
                    defaultAcSymbol,
                    openingBracket,
                    closingBracket,
                    false);
            }

            return;
        }

        public static void PrintConjunctiveTuple(
            ISingleTupleObject tuple)
        {
            PrintSingleTupleObject(tuple, "*", "[", "]");

            return;
        }

        public static void PrintDisjunctiveTuple(
            ISingleTupleObject tuple)
        {
            PrintSingleTupleObject(tuple, "Ø", "]", "[");

            return;
        }

        public static void PrintConjunctiveTupleSystem(
            ITupleObjectSystem tupleSys)
        {
            PrintTupleObjectSystem(tupleSys, "*", "[", "]");

            return;
        }

        public static void PrintDisjunctiveTupleSystem(
            ITupleObjectSystem tupleSys)
        {
            PrintTupleObjectSystem(tupleSys, "Ø", "]", "[");

            return;
        }

        public static void PrintTupleObject<TEntity>(
            TupleObject<TEntity> to)
            where TEntity : new()
        {
            switch (to)
            {
                case EmptyTupleObject<TEntity>: break;
                case FullTupleObject<TEntity> full:
                    {
                        PrintTupleObject(full.Inner);
                        break;
                    }
                case ConjunctiveTuple<TEntity> cTuple:
                    {
                        PrintConjunctiveTuple(cTuple);

                        break;
                    }
                case DisjunctiveTuple<TEntity> dTuple:
                    {
                        PrintDisjunctiveTuple(dTuple);

                        break;
                    }
                case ConjunctiveTupleSystem<TEntity> cSys:
                    {
                        PrintConjunctiveTupleSystem(cSys);

                        break;
                    }
                case DisjunctiveTupleSystem<TEntity> dSys:
                    {
                        PrintDisjunctiveTupleSystem(dSys);

                        break;
                    }
            }

            return;
        }
    }
}
