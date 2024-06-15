using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using static TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators.TupleObjectFactoryMethods;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators
{
    using static TupleObjectFactoryMethods;
    using static UniversalClassLib.CartesianProductHelper;

    public static class TupleObjectConversionToAlternateOperator
    {
        private static bool ConvertTupleToAlternateTuple<TEntity>(
            SingleTupleObject<TEntity> tuple,
            TupleObjectFactory factory,
            SingleTupleObjectFactoryHandler<TEntity> singleTupleFactory,
            out TupleObject<TEntity> tupleRes)
            where TEntity : new()
        {
            int nonDefaultIndex = -1;
            IAttributeComponent nonDefault = null;
            for (int attrLoc = 0; attrLoc < tuple.RowLength; attrLoc++)
            {
                if (!tuple.IsDefault(attrLoc))
                {
                    if (nonDefault is not null)
                    {
                        tupleRes = null;

                        return false;
                    }

                    nonDefault = tuple[nonDefaultIndex = attrLoc];
                }
            }

            TupleObjectSchema<TEntity> schema = tuple.Schema;
            tupleRes = singleTupleFactory(
                [new (nonDefaultIndex, schema, nonDefault)],
                schema.PassToBuilder,
                factory);

            return true;
        }

        private static TupleObject<TEntity> ConvertTupleToAlternateTupleSystem<TEntity>(
            SingleTupleObject<TEntity> tuple,
            TupleObjectFactory factory,
            SingleTupleObjectFactoryHandler<TEntity> singleTupleFactory,
            TupleObjectSystemFactoryHandler<TEntity> tupleSysFactory)
            where TEntity : new()
        {
            TupleObjectSchema<TEntity> schema = tuple.Schema;
            int len = tuple.RowLength;
            IndexedComponentFactoryArgs<IAttributeComponent>[] tupleFactoryArgs = 
                new IndexedComponentFactoryArgs<IAttributeComponent>[len];
            InitDefaultFactoryArgs<TEntity>(tupleFactoryArgs, schema);

            return tupleSysFactory(
                MakeTuples(),
                schema.PassToBuilder,
                factory);

            IEnumerable<TupleObject<TEntity>>
                MakeTuples()
            {
                for (int attrLoc = 0; attrLoc < tuple.RowLength; attrLoc++)
                {
                    yield return singleTupleFactory(
                        tupleFactoryArgs,
                        schema.PassToBuilder,
                        factory);
                    tupleFactoryArgs[attrLoc] = CreateDefaultFactoryArg(
                        attrLoc, tupleFactoryArgs);
                }

                yield break;
            }
        }

        private static IEnumerable<TupleObject<TEntity>>
            ConvertToAlternateTupleEnum<TEntity>(
            SingleTupleObject<TEntity> tuple,
            TupleObjectFactory factory,
            SingleTupleObjectFactoryHandler<TEntity> singleTupleObjectFactory)
            where TEntity : new()
        {
            TupleObjectSchema<TEntity> schema = tuple.Schema;
            int schemaLen = tuple.RowLength;
            IndexedComponentFactoryArgs<IAttributeComponent>[] components =
                new IndexedComponentFactoryArgs<IAttributeComponent>[1];

            for (int attrLoc = 0; attrLoc < schemaLen; attrLoc++)
            {
                if (tuple.IsDefault(attrLoc)) continue;

                components[0] = new IndexedComponentFactoryArgs<IAttributeComponent>(
                    attrLoc,
                    schema,
                    tuple[attrLoc]);

                yield return singleTupleObjectFactory(
                    components,
                    tuple.PassSchema,
                    factory);
            }

            yield break;
        }

        private static IEnumerable<TupleObject<TEntity>>
            ConvertDiagonalTupleObjectSystemToAlternate<
            TEntity,
            CTSingleTupleObject>(
            TupleObjectSystem<TEntity, CTSingleTupleObject> tupleSys,
            TupleObjectFactory factory,
            SingleTupleObjectFactoryHandler<TEntity> singleTupleFactory)
            where TEntity : new()
            where CTSingleTupleObject : SingleTupleObject<TEntity>
        {
            TupleObjectSchema<TEntity> schema = tupleSys.Schema;

            yield return singleTupleFactory(
                GetComponents(), 
                schema.PassToBuilder, 
                factory);

            yield break;

            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>>
                GetComponents()
            {
                int schemaLen = tupleSys.RowLength,
                    tuplePtr;

                for (int attrLoc = 0; attrLoc < schemaLen; attrLoc++)
                {
                    for (tuplePtr = 0; tuplePtr < tupleSys.ColumnLength; tuplePtr++)
                    {
                        if (tupleSys[tuplePtr].IsDefault(attrLoc)) continue;

                        yield return new(
                            attrLoc,
                            tupleSys.Schema,
                            tupleSys[tuplePtr][attrLoc]);

                        break;
                    }
                }

                yield break;
            }
        }

        private static IEnumerable<TupleObject<TEntity>>
            ConvertDiagonalConjunctiveTupleSystemToAlternate<TEntity>(
            ConjunctiveTupleSystem<TEntity> first,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return ConvertDiagonalTupleObjectSystemToAlternate(
                first,
                factory,
                DisjunctiveTupleFactory<TEntity>);
        }

        private static IEnumerable<TupleObject<TEntity>>
            ConvertDiagonalDisjunctiveTupleSystemToAlternate<TEntity>(
            DisjunctiveTupleSystem<TEntity> first,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return ConvertDiagonalTupleObjectSystemToAlternate(
                first,
                factory,
                ConjunctiveTupleFactory<TEntity>);
        }

        /*
        private static TupleObject<TEntity> ConvertTupleSystemToAlternateTuple<
            TEntity,
            TSingleTupleObject>(
            TupleObjectSystem<TEntity, TSingleTupleObject> tupleSys,
            TupleObjectFactory factory,
            SingleTupleObjectFactoryHandler<TEntity> singleTupleFactory)
            where TEntity : new()
            where TSingleTupleObject : SingleTupleObject<TEntity>
        {
            if (tupleSys.IsDiagonal)
            {
                TupleObjectSchema<TEntity> schema = tupleSys.Schema;
                IndexedComponentFactoryArgs<IAttributeComponent>[] components =
                    new IndexedComponentFactoryArgs<IAttributeComponent>[tupleSys.RowLength];
                InitDefaultFactoryArgs<TEntity>(components, schema);

                int schemaLen = tupleSys.RowLength,
                    tuplePtr;
                for (int attrLoc = 0; attrLoc < schemaLen; attrLoc++)
                {
                    for (tuplePtr = 0; tuplePtr < tupleSys.ColumnLength; tuplePtr++)
                    {
                        if (tupleSys[tuplePtr].IsDefault(attrLoc)) continue;

                        components[attrLoc] = new(
                            attrLoc,
                            tupleSys.Schema,
                            tupleSys[tuplePtr][attrLoc]);

                        break;
                    }
                }

                return singleTupleFactory(
                    components,
                    schema.PassToBuilder, 
                    factory);
            }

            return null;

            //throw new InvalidOperationException(
            //    "Перевод недиагональных систем кортежей в альтернативные формы отображения не поддерживается.");
        }
        */

        public static IEnumerable<TupleObject<TEntity>>
            ConvertToAlternateTupleEnum<TEntity>(
            ConjunctiveTuple<TEntity> tuple,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return ConvertToAlternateTupleEnum(
                tuple,
                factory,
                DisjunctiveTupleFactory<TEntity>);
        }

        public static TupleObject<TEntity>
            ConvertToAlternate<TEntity>(
            ConjunctiveTuple<TEntity> tuple,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            TupleObject<TEntity> tupleRes;
            if (ConvertTupleToAlternateTuple(
                tuple,
                factory,
                DisjunctiveTupleFactory<TEntity>,
                out tupleRes))
                return tupleRes;

            return factory.CreateDiagonalDisjunctiveTupleSystem(tuple);
        }

        public static IEnumerable<TupleObject<TEntity>> 
            ConvertToAlternateTupleEnum<TEntity>(
            DisjunctiveTuple<TEntity> tuple,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return ConvertToAlternateTupleEnum(
                tuple,
                factory,
                ConjunctiveTupleFactory<TEntity>);
        }

        public static TupleObject<TEntity> ConvertToAlternate<TEntity>(
            DisjunctiveTuple<TEntity> tuple,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            TupleObject<TEntity> tupleRes;
            if (ConvertTupleToAlternateTuple(
                tuple,
                factory,
                ConjunctiveTupleFactory<TEntity>,
                out tupleRes))
                return tupleRes;

            return factory.CreateDiagonalConjunctiveTupleSystem(tuple);
        }

        public static IEnumerable<TupleObject<TEntity>> ConvertToAlternateTupleEnum<TEntity>(
            ConjunctiveTupleSystem<TEntity> tupleSys,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            if (tupleSys.IsDiagonal)
                return ConvertDiagonalConjunctiveTupleSystemToAlternate(tupleSys, factory);
            else
                return TrueUnion(tupleSys.Schema, tupleSys.Tuples, factory);
        }

        public static IEnumerable<TupleObject<TEntity>> ConvertToAlternateTupleEnum<TEntity>(
            DisjunctiveTupleSystem<TEntity> tupleSys,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            if (tupleSys.IsDiagonal)
                return ConvertDiagonalDisjunctiveTupleSystemToAlternate(tupleSys, factory);
            else
                return TrueIntersect(tupleSys.Schema, tupleSys.Tuples, factory);
        }

        public static TupleObject<TEntity> ConvertToAlternate<TEntity>(
            ConjunctiveTupleSystem<TEntity> tupleSys,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return factory.CreateDisjunctiveTupleSystem(
                ConvertToAlternateTupleEnum(tupleSys, factory),
                tupleSys.PassSchema,
                null);
        }

        public static TupleObject<TEntity> ConvertToAlternate<TEntity>(
            DisjunctiveTupleSystem<TEntity> tupleSys,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return factory.CreateConjunctiveTupleSystem(
                ConvertToAlternateTupleEnum(tupleSys, factory),
                tupleSys.PassSchema,
                null);
        }

        public static IEnumerable<DisjunctiveTuple<TEntity>> TrueUnion<TEntity>(
            TupleObjectSchema<TEntity> schema,
            ConjunctiveTuple<TEntity>[] tuples,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            int schemaLen = schema.PluggedAttributesCount,
                csysLen = tuples.Length;
            if (csysLen < 2)
                throw new InvalidOperationException("У C-системы не должно быть меньше двух кортежей.");

            IndexedComponentFactoryArgs<IAttributeComponent>[]
                opFactoryArgs;
            IndexedComponentFactoryArgs<IAttributeComponent>[][] tupleOpFactoryArgs =
                new IndexedComponentFactoryArgs<IAttributeComponent>[csysLen][];
            for (int i = 0; i < csysLen; i++)
            {
                tupleOpFactoryArgs[i] = opFactoryArgs =
                    new IndexedComponentFactoryArgs<IAttributeComponent>[schemaLen];
                InitDefaultFactoryArgs<TEntity>(opFactoryArgs, schema);
            }

            return GetAccumulatedCartesianProduct(
                (RectangleInfo rect) => factory.CreateDisjunctiveTuple<TEntity>(
                    rect.Components,
                    schema.PassToBuilder,
                    null),
                tuples,
                (stackPtr, tuple) => RectangleInfo
                    .FromTuple(tuple, tupleOpFactoryArgs[stackPtr]),
                RectangleInfo.DefineBranchActionForUnion,
                RectangleInfo.BranchAccumulatorFactoryForUnion,
                tuple => !tuple.IsFull(),
                RectangleInfo.ResetFactoryArgs)
                    .OfType<DisjunctiveTuple<TEntity>>();
        }

        public static IEnumerable<ConjunctiveTuple<TEntity>> TrueIntersect<TEntity>(
            TupleObjectSchema<TEntity> schema,
            DisjunctiveTuple<TEntity>[] tuples,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            int schemaLen = schema.PluggedAttributesCount,
                dsysLen = tuples.Length;
            if (dsysLen < 2)
                throw new InvalidOperationException("У D-системы не должно быть меньше двух кортежей.");

            IndexedComponentFactoryArgs<IAttributeComponent>[] opFactoryArgs;
            IndexedComponentFactoryArgs<IAttributeComponent>[][] tupleOpFactoryArgs =
                new IndexedComponentFactoryArgs<IAttributeComponent>[dsysLen][];
            for (int i = 0; i < dsysLen; i++)
            {
                tupleOpFactoryArgs[i] = opFactoryArgs =
                    new IndexedComponentFactoryArgs<IAttributeComponent>[schemaLen];
                InitDefaultFactoryArgs<TEntity>(opFactoryArgs, schema);
            }

            return GetAccumulatedCartesianProduct(
                (RectangleInfo rect) => factory.CreateConjunctiveTuple<TEntity>(
                    rect.Components,
                    schema.PassToBuilder,
                    null),
                tuples,
                (stackPtr, tuple) => RectangleInfo
                    .FromTuple(tuple, tupleOpFactoryArgs[stackPtr]),
                RectangleInfo.DefineBranchActionForIntersection,
                RectangleInfo.BranchAccumulatorFactoryForIntersection,
                tuple => !tuple.IsEmpty(),
                RectangleInfo.ResetFactoryArgs)
                    .OfType<ConjunctiveTuple<TEntity>>();
        }

        public static TupleObject<TEntity> TrueUnion<TEntity>(
            ConjunctiveTupleSystem<TEntity> cSys,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return factory.CreateDisjunctiveTupleSystem(
                TrueUnion(cSys.Schema, cSys.Tuples, factory),
                cSys.PassSchema,
                null);
        }

        public static TupleObject<TEntity> TrueIntersect<TEntity>(
            DisjunctiveTupleSystem<TEntity> dSys,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return factory.CreateConjunctiveTupleSystem(
                TrueIntersect(dSys.Schema, dSys.Tuples, factory),
                dSys.PassSchema,
                null);
        }
    }
}
