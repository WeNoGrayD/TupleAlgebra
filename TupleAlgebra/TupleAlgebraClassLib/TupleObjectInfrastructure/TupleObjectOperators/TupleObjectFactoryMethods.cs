using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using static TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectHelper;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators
{
    internal static class TupleObjectFactoryMethods
    {
        public static void AlignOperandsWithSchema<TEntity, TOperand1, TOperand2>(
            ref TOperand1 first,
            ref TOperand2 second,
            TupleObjectFactory factory,
            TupleObjectSchema<TEntity> generalSchema = null)
            where TEntity : new()
            where TOperand1 : TupleObject<TEntity>
            where TOperand2 : TupleObject<TEntity>
        {
            generalSchema ??= first.Schema.GeneralizeWith(second.Schema);
            TupleObjectBuilder<TEntity> builder =
            factory.GetBuilder(generalSchema);

            first = (first.AlignWithSchema(generalSchema, factory, builder) as TOperand1)!;
            second = (second.AlignWithSchema(generalSchema, factory, builder) as TOperand2)!;
        }

        public static void InitDefaultFactoryArgs<TEntity>(
            IndexedComponentFactoryArgs<IAttributeComponent>[] factoryArgs,
            ITupleObjectSchemaProvider schema)
            where TEntity : new()
        {
            for (int attrLoc = 0; attrLoc < factoryArgs.Length; attrLoc++)
            {
                factoryArgs[attrLoc] = new(attrLoc, schema);
            }

            return;
        }

        public static IndexedComponentFactoryArgs<IAttributeComponent>
            CreateDefaultFactoryArg(
            int attrLoc,
            IndexedComponentFactoryArgs<IAttributeComponent>[] source)
        {
            return new(attrLoc, source[attrLoc].TupleManager);
        }

        public delegate TupleObject<TEntity>
            SingleTupleObjectFactoryHandler<TEntity>(
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>>
            tupleFactoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectFactory factory)
            where TEntity : new();

        public delegate TupleObject<TEntity>
            TupleObjectSystemFactoryHandler<TEntity>(
            IEnumerable<TupleObject<TEntity>> tupleSysFactoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectFactory factory)
            where TEntity : new();

        public static TupleObject<TEntity> ConjunctiveTupleFactory<TEntity>(
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>>
            tupleFactoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return factory.CreateConjunctiveTuple<TEntity>(
                tupleFactoryArgs,
                onTupleBuilding);
        }

        public static TupleObject<TEntity> DisjunctiveTupleFactory<TEntity>(
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>>
            tupleFactoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return factory.CreateDisjunctiveTuple<TEntity>(
                tupleFactoryArgs,
                onTupleBuilding);
        }

        public static TupleObject<TEntity> ConjunctiveTupleSystemFactory<TEntity>(
            IEnumerable<TupleObject<TEntity>>
            tupleSysFactoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return factory.CreateConjunctiveTupleSystem<TEntity>(
                tupleSysFactoryArgs, onTupleBuilding, null);
        }

        public static TupleObject<TEntity> DisjunctiveTupleSystemFactory<TEntity>(
            IEnumerable<TupleObject<TEntity>>
            tupleSysFactoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return factory.CreateDisjunctiveTupleSystem<TEntity>(
                tupleSysFactoryArgs, onTupleBuilding, null);
        }

        public static TupleObject<TEntity> DiagonalConjunctiveTupleSystemFactory<TEntity>(
            DisjunctiveTuple<TEntity> tuple,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return factory.CreateDiagonalConjunctiveTupleSystem(tuple);
        }

        public static TupleObject<TEntity> DiagonalDisjunctiveTupleSystemFactory<TEntity>(
            ConjunctiveTuple<TEntity> tuple,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return factory.CreateDiagonalDisjunctiveTupleSystem(tuple);
        }

        public static IEnumerable<CTSingleTupleObject> 
            TupleObjectSystemToAlternateTupleEnumerable<
            TEntity,
            CTSingleTupleObject>(
            TupleObjectSystem<TEntity, CTSingleTupleObject> tupleSys)
            where TEntity : new()
            where CTSingleTupleObject : SingleTupleObject<TEntity>
        {
            for (int tuplePtr = 0; tuplePtr < tupleSys.ColumnLength; tuplePtr++)
            {
                
            }

            yield break;
        }
        private static IEnumerable<TupleObject<TEntity>> ComplementAndConvertToAlternateEnum<
            TEntity>(
            ISingleTupleObject tuple,
            SingleTupleObjectFactoryHandler<TEntity> singleTupleFactory,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            IndexedComponentFactoryArgs<IAttributeComponent>[] tupleFactoryArgs =
                new IndexedComponentFactoryArgs<IAttributeComponent>[1];
            IAttributeComponent ac;
            TupleObjectBuildingHandler<TEntity> onTupleBuilding =
                tuple.PassSchema<TEntity>;

            for (int attrLoc = 0; attrLoc < tuple.RowLength; attrLoc++)
            {
                ac = tuple[attrLoc];
                if (ac.IsDefault) continue;

                tupleFactoryArgs[0] = new IndexedComponentFactoryArgs<IAttributeComponent>(
                    attrLoc,
                    builder,
                    ac.ComplementThe());

                yield return singleTupleFactory(
                    tupleFactoryArgs,
                    onTupleBuilding,
                    factory);
            }

            yield break;
        }

        public static IEnumerable<TupleObject<TEntity>> ComplementAndConvertToAlternateEnum<TEntity>(
            ConjunctiveTuple<TEntity> cTuple,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return ComplementAndConvertToAlternateEnum(
                cTuple,
                DisjunctiveTupleFactory<TEntity>,
                factory,
                builder);
        }

        public static IEnumerable<TupleObject<TEntity>> ComplementAndConvertToAlternateEnum<TEntity>(
            DisjunctiveTuple<TEntity> dTuple,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return ComplementAndConvertToAlternateEnum(
                dTuple,
                ConjunctiveTupleFactory<TEntity>,
                factory,
                builder);
        }
    }
}
