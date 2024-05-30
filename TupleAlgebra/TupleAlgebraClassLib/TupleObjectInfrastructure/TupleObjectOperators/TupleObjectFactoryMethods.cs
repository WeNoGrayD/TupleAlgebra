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
    }
}
