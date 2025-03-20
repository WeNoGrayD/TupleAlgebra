using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators
{
    using static TupleObjectFactoryMethods;

    public static class TupleObjectExceptionOperator
    {
        /// <summary>
        /// cTuple || empty
        /// [a|b] / ]x|y[ = [a/x|b]/]0/y[ = [a/x|b/y] 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="factory"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IEnumerable<TupleObject<TEntity>> ExceptEnum<TEntity>(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>(first.PassSchema);

            for (int tuplePtr = 0; tuplePtr < first.RowLength; tuplePtr++)
            {
                yield return factory.CreateConjunctiveTuple<TEntity>(
                    GetFactoryArgs(tuplePtr),
                    null,
                    builder);
            }

            yield break;

            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>>
                GetFactoryArgs(int tuplePtr)
            {
                IAttributeComponent diagonalComponent =
                    first[tuplePtr].ExceptWith(second[tuplePtr]);

                if (diagonalComponent.IsEmpty) yield break;

                int attrLoc = 0;

                for (; attrLoc < tuplePtr; attrLoc++)
                {
                    yield return new IndexedComponentFactoryArgs<IAttributeComponent>(
                        tuplePtr,
                        builder,
                        first[attrLoc]);
                }

                yield return new IndexedComponentFactoryArgs<IAttributeComponent>(
                    tuplePtr,
                    builder,
                    diagonalComponent);

                for (attrLoc++; attrLoc < first.RowLength; attrLoc++)
                {
                    yield return new IndexedComponentFactoryArgs<IAttributeComponent>(
                        tuplePtr,
                        builder,
                        first[attrLoc]);
                }

                yield break;
            }
        }

        /// <summary>
        ///  cTuple || cSys || empty
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="factory"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IEnumerable<TupleObject<TEntity>> ExceptEnum<TEntity>(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>(first.PassSchema);
            TupleObject<TEntity> tupleRes, excepted;

            tupleRes = factory.CreateFull(builder);

            for (int tuplePtr = 0; tuplePtr < first.RowLength; tuplePtr++)
            {
                if (tupleRes.IsEmpty()) break;

                excepted = factory.CreateConjunctiveTupleSystem(
                    ExceptEnum(first, second[tuplePtr], factory, builder),
                    null,
                    builder);
                tupleRes = tupleRes & excepted;
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
        public static IEnumerable<TupleObject<TEntity>> ExceptEnum<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>(first.PassSchema);

            for (int tuplePtr = 0; tuplePtr < first.RowLength; tuplePtr++)
            {
                foreach (var tuple
                     in ExceptEnum(first[tuplePtr], second, factory, builder))
                {
                    yield return tuple;
                }
            }

            yield break;
        }

        /// <summary>
        /// cTuple || cSys || empty
        /// [ a b ]   [ x y ]              [ x y ]                [ x y ]
        /// [ c d ] / [ z w ] = ([ a b ] / [ z w ]) || ([ c d ] / [ z w ])
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="factory"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IEnumerable<TupleObject<TEntity>> ExceptEnum<TEntity>(
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
                     in ExceptEnum(first[tuplePtr], second, factory, builder))
                {
                    yield return tuple;
                }
            }

            yield break;
        }

        /// <summary>
        /// cTuple || empty
        /// [a|b] / ]x|y[ = [a/x|b]/]0/y[ = [a/x|b/y] 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="factory"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static TupleObject<TEntity> Except<TEntity>(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>(first.PassSchema);

            return factory.CreateConjunctiveTuple<TEntity>(
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
                        first[attrLoc].ExceptWith(second[attrLoc]));
                }

                yield break;
            }
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
        public static IEnumerable<TupleObject<TEntity>> ExceptEnum<TEntity>(
            DisjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>(first.PassSchema);

            IndexedComponentFactoryArgs<IAttributeComponent>[] diagonalFactoryArg
                = new IndexedComponentFactoryArgs<IAttributeComponent>[1];
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> factoryArgs;

            for (int attrLoc = 0; attrLoc < second.RowLength; attrLoc++)
            {
                if (first[attrLoc].IsEmpty) continue;

                foreach (var tuple in ExceptDiagonalEnum(attrLoc))
                    yield return tuple;
            }

            yield break;

            IEnumerable<TupleObject<TEntity>> ExceptDiagonalEnum(int diagonalAttrLoc)
            {
                int attrLoc = 0;

                for (int tuplePtr = 0; tuplePtr < first.RowLength; tuplePtr++)
                {
                    if (diagonalAttrLoc == tuplePtr)
                    {
                        diagonalFactoryArg[0] = new IndexedComponentFactoryArgs<IAttributeComponent>(
                            diagonalAttrLoc,
                            builder,
                            first[attrLoc].ExceptWith(second[attrLoc]));
                        factoryArgs = diagonalFactoryArg;
                    }
                    else factoryArgs = GetFactoryArgs(diagonalAttrLoc, tuplePtr);

                    yield return factory.CreateConjunctiveTuple(
                        factoryArgs,
                        null,
                        builder);
                }

                yield break;
            }

            /*
             * [a * *] / [x y z] = 
             * [a/x * *]
             * [a ~y z]
             * [a y ~z]
             */
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> 
                GetFactoryArgs(int diagonalAttrLoc, int complementedAttrLoc)
            {
                IAttributeComponent complementedComponent =
                    second[complementedAttrLoc].ComplementThe();

                if (complementedComponent.IsEmpty) yield break;

                int attrLoc = 0;

                for (; attrLoc < diagonalAttrLoc; attrLoc++)
                {
                    yield return new IndexedComponentFactoryArgs<IAttributeComponent>(
                        attrLoc,
                        builder,
                        second[attrLoc]);
                }

                yield return new IndexedComponentFactoryArgs<IAttributeComponent>(
                    attrLoc,
                    builder,
                    first[diagonalAttrLoc]);

                for (attrLoc++; attrLoc < complementedAttrLoc; attrLoc++)
                {
                    yield return new IndexedComponentFactoryArgs<IAttributeComponent>(
                        attrLoc,
                        builder,
                        second[attrLoc]);
                }

                yield return new IndexedComponentFactoryArgs<IAttributeComponent>(
                    attrLoc,
                    builder,
                    complementedComponent);

                for (attrLoc++; attrLoc < first.RowLength; attrLoc++)
                {
                    yield return new IndexedComponentFactoryArgs<IAttributeComponent>(
                        attrLoc,
                        builder,
                        second[attrLoc]);
                }

                yield break;
            }
        }

        /// <summary>
        /// cTuple || cSys || empty
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IEnumerable<TupleObject<TEntity>> ExceptEnum<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder = factory.GetBuilder<TEntity>(first.PassSchema);

            for (int tuplePtr = 0; tuplePtr < first.ColumnLength; tuplePtr++)
            {
                yield return Except(first[tuplePtr], second, factory, builder); ;
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
        public static IEnumerable<TupleObject<TEntity>> ExceptEnum<TEntity>(
            DisjunctiveTuple<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>();

            TupleObject<TEntity> excepted, tupleRes = factory.CreateFull(builder);

            for (int tuplePtr = 0; tuplePtr < second.ColumnLength; tuplePtr++)
            {
                if (tupleRes.IsEmpty()) continue;

                excepted = factory.CreateConjunctiveTupleSystem(
                    ExceptEnum(first, second[tuplePtr], factory, builder),
                    null,
                    builder);
                tupleRes /= excepted;
            }

            switch (tupleRes)
            {
                case DisjunctiveTuple<TEntity> dTuple:
                    {
                        yield return dTuple;
                        break;
                    }
                case DisjunctiveTupleSystem<TEntity> cSys:
                    {
                        for (int tuplePtr = 0; tuplePtr < cSys.ColumnLength; tuplePtr++)
                        {
                            yield return cSys[tuplePtr];
                        }

                        break;
                    }
                case FullTupleObject<TEntity> full:
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
        public static IEnumerable<TupleObject<TEntity>> ExceptEnumAsConjunctive<TEntity>(
            DisjunctiveTuple<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>();

            foreach (var tuple in ExceptEnum(first, second, factory, builder))
            {
                switch (tuple)
                {
                    case EmptyTupleObject<TEntity> empty:
                    case FullTupleObject<TEntity> full:
                        {
                            break;
                        }
                    case TupleObject<TEntity> dTupleObject:
                        {
                            TupleObject<TEntity> altered = !dTupleObject;

                            switch (tuple)
                            {
                                case EmptyTupleObject<TEntity> empty:
                                    {
                                        break;
                                    }
                                case FullTupleObject<TEntity> full:
                                    {
                                        yield return full;

                                        break;
                                    }
                                case ConjunctiveTuple<TEntity> cTuple:
                                    {
                                        yield return cTuple;

                                        break;
                                    }
                                case ConjunctiveTupleSystem<TEntity> cSys:
                                    {
                                        for (int tuplePtr = 0; 
                                             tuplePtr < cSys.ColumnLength; 
                                             tuplePtr++)
                                        {
                                            yield return cSys[tuplePtr];
                                        }

                                        break;
                                    }
                            }

                            break;
                        }
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
        public static IEnumerable<TupleObject<TEntity>> ExceptEnum<TEntity>(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>(first.PassSchema);

            for (int tuplePtr = 0; tuplePtr < second.ColumnLength; tuplePtr++)
            {
                yield return Except(first, second[tuplePtr], factory, builder);
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
        /// <returns></returns>
        public static IEnumerable<TupleObject<TEntity>> ExceptEnum<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>(first.PassSchema);

            for (int tuplePtr = 0; tuplePtr < first.ColumnLength; tuplePtr++)
            {
                foreach (TupleObject<TEntity> tuple
                     in ExceptEnum(first[tuplePtr], second, factory, builder))
                {
                    yield return tuple;
                }
            }

            yield break;
        }

        /// <summary>
        /// dSys
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="factory"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IEnumerable<TupleObject<TEntity>> ExceptEnum<TEntity>(
            DisjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>(first.PassSchema);

            yield return first;

            IndexedComponentFactoryArgs<IAttributeComponent>[] tupleFactoryArgs =
                new IndexedComponentFactoryArgs<IAttributeComponent>[1];
            IAttributeComponent ac;

            foreach (var tuple in ComplementAndConvertToAlternateEnum(
                second,
                factory,
                builder))
            {
                yield return tuple;
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
        public static IEnumerable<TupleObject<TEntity>> ExceptEnum<TEntity>(
            DisjunctiveTuple<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>();
            TupleObject<TEntity> tupleRes, excepted;

            tupleRes = factory.CreateEmpty(builder);

            for (int tuplePtr = 0; tuplePtr < first.RowLength; tuplePtr++)
            {
                if (tupleRes.IsFull()) break;

                excepted = factory.CreateDisjunctiveTupleSystem(
                    ExceptEnum(first, second[tuplePtr], factory, builder),
                    first.PassSchema,
                    builder);
                tupleRes = tupleRes | excepted;
            }

            switch (tupleRes)
            {
                case DisjunctiveTuple<TEntity> cTuple:
                    {
                        yield return cTuple;
                        break;
                    }
                case DisjunctiveTupleSystem<TEntity> dSys:
                    {
                        for (int tuplePtr = 0; tuplePtr < dSys.ColumnLength; tuplePtr++)
                        {
                            yield return dSys[tuplePtr];
                        }

                        break;
                    }
                case FullTupleObject<TEntity> full:
                    {
                        break;
                    }
            }

            yield break;
        }

        /// <summary>
        /// dSys || full
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="factory"></param>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IEnumerable<TupleObject<TEntity>> ExceptEnum<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>();

            for (int tuplePtr = 0; tuplePtr < first.RowLength; tuplePtr++)
            {
                yield return first[tuplePtr];
            }

            foreach (var tuple in ComplementAndConvertToAlternateEnum(
                second,
                factory,
                builder))
            {
                yield return tuple;
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
        public static IEnumerable<TupleObject<TEntity>> ExceptEnum<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= factory.GetBuilder<TEntity>();

            for (int tuplePtr = 0; tuplePtr < first.RowLength; tuplePtr++)
            {
                foreach (var tuple
                     in ExceptEnum(first[tuplePtr], second, factory, builder))
                {
                    yield return tuple;
                }
            }

            yield break;
        }

        public static TupleObject<TEntity> Except<TEntity>(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return first & ~second;
            /*
            TupleObjectBuilder<TEntity> builder =
                factory.GetBuilder<TEntity>(first.PassSchema);

            return factory.CreateConjunctiveTupleSystem(
                ExceptEnum(first, second, factory, builder),
                null,
                builder);
            */
        }

        public static TupleObject<TEntity> Except<TEntity>(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return first & ~second;
            //return Except<TEntity>(first, second, factory, null);
        }

        public static TupleObject<TEntity> Except<TEntity>(
            DisjunctiveTuple<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return first & ~second;
        }

        public static TupleObject<TEntity> Except<TEntity>(
            DisjunctiveTuple<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return first & !~second;
            /*
            TupleObjectBuilder<TEntity> builder =
                factory.GetBuilder<TEntity>(first.PassSchema);

            return factory.CreateDisjunctiveTupleSystem(
                ExceptEnum(first, second, factory, builder),
                null,
                builder);
            */
        }

        public static TupleObject<TEntity> Except<TEntity>(
            ConjunctiveTuple<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return first & ~second;
            /*
            TupleObjectBuilder<TEntity> builder =
                factory.GetBuilder<TEntity>(first.PassSchema);

            return factory.CreateConjunctiveTupleSystem(
                ExceptEnum(first, second, factory, builder),
                null,
                builder);
            */
        }

        public static TupleObject<TEntity> Except<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return first & !~second;
            /*
            TupleObjectBuilder<TEntity> builder =
                factory.GetBuilder<TEntity>(first.PassSchema);

            return factory.CreateConjunctiveTupleSystem(
                ExceptEnum(first, second, factory, builder),
                null,
                builder);
            */
        }

        public static TupleObject<TEntity> Except<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return first & ~second;
            /*
            TupleObjectBuilder<TEntity> builder =
                factory.GetBuilder<TEntity>(first.PassSchema);

            return factory.CreateConjunctiveTupleSystem(
                ExceptEnum(first, second, factory, builder),
                null,
                builder);
            */
        }

        public static TupleObject<TEntity> Except<TEntity>(
            DisjunctiveTuple<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return first & ~second;
        }

        public static TupleObject<TEntity> Except<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return first & ~second;
            /*
            return factory.CreateConjunctiveTupleSystem(
                ExceptEnum<TEntity>(first, second, factory),
                first.PassSchema,
                null);
            */
        }

        public static TupleObject<TEntity> Except<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return first & ~second;
            /*
            return factory.CreateConjunctiveTupleSystem(
                ExceptEnum<TEntity>(first, second, factory),
                first.PassSchema,
                null);
            */
        }

        public static TupleObject<TEntity> Except<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return first & ~second;
        }

        public static TupleObject<TEntity> Except<TEntity>(
            ConjunctiveTuple<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return first & ~second;
            /*
            return factory.CreateConjunctiveTupleSystem(
                ExceptEnum<TEntity>(first, second, factory),
                first.PassSchema,
                null);
            */
        }

        public static TupleObject<TEntity> Except<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return first & ~second;
        }

        public static TupleObject<TEntity> Except<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return first & ~second;
            /*
            TupleObjectBuilder<TEntity> builder =
                factory.GetBuilder<TEntity>(first.PassSchema);

            return factory.CreateDisjunctiveTupleSystem(
                ExceptEnum(first, second, factory, builder),
                null,
                builder);
            */
        }

        public static TupleObject<TEntity> Except<TEntity>(
            DisjunctiveTuple<TEntity> first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return first & ~second;
            /*
            TupleObjectBuilder<TEntity> builder =
                factory.GetBuilder<TEntity>(first.PassSchema);

            return factory.CreateDisjunctiveTupleSystem(
                ExceptEnum(first, second, factory, builder),
                null,
                builder);
            */
        }

        public static TupleObject<TEntity> Except<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return first & !~second;
            /*
            TupleObjectBuilder<TEntity> builder =
                factory.GetBuilder<TEntity>(first.PassSchema);

            return factory.CreateDisjunctiveTupleSystem(
                ExceptEnum(first, second, factory, builder),
                null,
                builder);
            */
        }
    }

    public abstract class TupleObjectExceptionOperator<TEntity, TOperand1>
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
            return factory.CreateEmpty<TEntity>(first.PassSchema);
        }
    }
}
