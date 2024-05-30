using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectFactoryInfrastructure
{
    using static TupleObjectHelper;

    public abstract class TupleObjectSystemFactory<
        TSingleTupleObjectFactory,
        TAltSingleTupleObjectFactory>
        : AbstractTupleObjectFactory
        where TSingleTupleObjectFactory : SingleTupleObjectFactory
    {
        protected TSingleTupleObjectFactory _singleTupleObjectFactory;

        protected TAltSingleTupleObjectFactory _altSingleTupleObjectFactory;

        protected delegate TupleObject<TEntity>
            SingleTupleObjectFactoryHandler<
                TEntity,
                TSingleTupleObjectFactoryArgs>(
                TSingleTupleObjectFactoryArgs tupleFactoryArgs,
                TupleObjectBuilder<TEntity> builder)
            where TEntity : new();

        public TupleObjectSystemFactory(
            TupleObjectFactory factory,
            TSingleTupleObjectFactory singleTupleObjectFactory,
            TAltSingleTupleObjectFactory altSingleTupleObjectFactory)
        {
            _factory = factory;
            _singleTupleObjectFactory = singleTupleObjectFactory;
            _altSingleTupleObjectFactory = altSingleTupleObjectFactory;

            return;
        }

        protected abstract TupleObject<TEntity>
            TupleObjectSystemFactoryImpl<TEntity>(
                TupleObjectSchema<TEntity> schema,
                int len)
                where TEntity : new();

        protected abstract TupleObject<TEntity>
            TupleObjectSystemFactoryImpl<TEntity>(
                TupleObjectSchema<TEntity> schema,
                IList<SingleTupleObject<TEntity>> tuples)
                where TEntity : new();

        protected abstract TupleObject<TEntity>
            SingleTupleObjectFactoryImpl<TEntity>(
                IEnumerable<NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>> tupleFactoryArgs,
                TupleObjectBuilder<TEntity> builder)
            where TEntity : new();

        protected abstract TupleObject<TEntity>
            SingleTupleObjectFactoryImpl<TEntity>(
                IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> tupleFactoryArgs,
                TupleObjectBuilder<TEntity> builder)
            where TEntity : new();

        protected abstract TupleObject<TEntity>
            SingleTupleObjectFactoryWithTrailingComplementImpl<TEntity>(
                IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> tupleFactoryArgs,
                TupleObjectBuilder<TEntity> builder)
            where TEntity : new();

        /*
        protected abstract TupleObject<TEntity>
            AlternateSingleTupleObjectFactoryImpl<TEntity>(
                NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>[] tupleFactoryArgs,
                TupleObjectBuilder<TEntity> builder)
            where TEntity : new();

        protected abstract TupleObject<TEntity>
            AlternateSingleTupleObjectFactoryImpl<TEntity>(
                IndexedComponentFactoryArgs<IAttributeComponent>[] tupleFactoryArgs,
                TupleObjectBuilder<TEntity> builder)
            where TEntity : new();
        */

        protected abstract bool SingleTupleObjectIsRedundant<TEntity>(
            TupleObject<TEntity> tuple)
            where TEntity : new();

        protected abstract bool SingleTupleObjectStopsBuilding<TEntity>(
            TupleObject<TEntity> tuple)
            where TEntity : new();

        protected abstract TupleObject<TEntity>
            ReduceTupleObjectSystemToFictional<TEntity>(
                TupleObjectBuilder<TEntity> builder)
            where TEntity : new();

        /*
        private (int Ptr, IAttributeComponent ComponentSource)[]
            EnumerateComponents(IAttributeComponent[] components)
        {
            int len = components.Length;
            (int Ptr, IAttributeComponent ComponentSource)[] res =
                new (int Ptr, IAttributeComponent ComponentSource)[len];
            for (int i = 0; i < len; i++)
                res[i] = (i, components[i]);

            return res;
        }
        */

        protected TupleObject<TEntity> ToAlternateDiagonal<
            TEntity,
            TSingleTupleObject>(
            SingleTupleObject<TEntity> tuple,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
            where TSingleTupleObject : SingleTupleObject<TEntity>
        {
            int len = tuple.RowLength;
            IndexedComponentFactoryArgs<IAttributeComponent>[] tupleSysFactoryArgs =
                new IndexedComponentFactoryArgs<IAttributeComponent>[len];
            for (int i = 0; i < len; i++)
            {
                tupleSysFactoryArgs[i] = new(i, builder, tuple[i]);
            }

            return CreateDiagonalTupleObjectSystemStrategy<
                TEntity,
                TSingleTupleObject>(
                tupleSysFactoryArgs,
                true,
                tuple.PassSchema,
                builder);
        }

        protected TupleObject<TEntity> ToAlternateDiagonal<
            TEntity,
            TSingleTupleObject>(
            SingleTupleObject<TEntity> tuple)
            where TEntity : new()
            where TSingleTupleObject : SingleTupleObject<TEntity>
        {
            TupleObjectBuilder<TEntity> builder =
                GetDefaultBuilder<TEntity>();

            return ToAlternateDiagonal<TEntity, TSingleTupleObject>(tuple, builder);
        }

        protected TupleObject<TEntity> CreateDiagonalTupleObjectSystemStrategy<
            TEntity,
            TSingleTupleObject>(
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> 
            tupleSysFactoryArgs,
            bool makeOrthogonal,
            //SingleTupleObjectFactoryHandler<
            //    TEntity,
            //    IEnumerable<IndexedComponentFactoryArgs<TComponentSource>>> lastSingleTupleObjectFactory,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
            where TSingleTupleObject : SingleTupleObject<TEntity>
        {
            SingleTupleObjectFactoryHandler<
                TEntity,
                IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>>>
                singleTupleObjectFactory = makeOrthogonal ?
                SingleTupleObjectFactoryWithTrailingComplementImpl<TEntity> :
                SingleTupleObjectFactoryImpl<TEntity>;

            return CreateDiagonalTupleObjectSystemStrategy<
                TEntity,
                TSingleTupleObject,
                IAttributeComponent>(
                tupleSysFactoryArgs,
                singleTupleObjectFactory,
                makeOrthogonal,
                onTupleBuilding,
                builder);
        }

        protected TupleObject<TEntity> CreateDiagonalTupleObjectSystemStrategy<
            TEntity,
            TSingleTupleObject,
            TComponentSource>(
            IEnumerable<IndexedComponentFactoryArgs<TComponentSource>> tupleSysFactoryArgs,
            SingleTupleObjectFactoryHandler<
                TEntity,
                IEnumerable<IndexedComponentFactoryArgs<TComponentSource>>>
            singleTupleObjectFactory,
            bool makeOrthogonal,
            //SingleTupleObjectFactoryHandler<
            //    TEntity,
            //    IEnumerable<IndexedComponentFactoryArgs<TComponentSource>>> lastSingleTupleObjectFactory,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
            where TSingleTupleObject : SingleTupleObject<TEntity>
        {
            BuildTuple(ref builder, onTupleBuilding);

            int len = builder.Schema.PluggedAttributesCount, i, j = 0;
            TupleObjectSystem<TEntity, TSingleTupleObject> tupleSystem =
                TupleObjectSystemFactoryImpl(
                    builder.Schema, len)
                as TupleObjectSystem<TEntity, TSingleTupleObject>;
            tupleSystem.IsDiagonal = true;

            TupleObject<TEntity> tuple = null;
            IndexedComponentFactoryArgs<TComponentSource> farg;
            IndexedComponentFactoryArgs<TComponentSource>[] tupleFactoryArgs =
                new IndexedComponentFactoryArgs<TComponentSource>[len];

            for (i = 0; i < len; i++)
                tupleFactoryArgs[i] = new(i, builder);

            foreach (var tupleFactoryArg in tupleSysFactoryArgs)
            {
                i = tupleFactoryArg.Index;
                farg = tupleFactoryArgs[i];
                tupleFactoryArgs[i] = tupleFactoryArg;
                tuple = singleTupleObjectFactory(
                    tupleFactoryArgs,
                    builder);
                tupleFactoryArgs[i] = farg;
                if (SingleTupleObjectStopsBuilding(tuple))
                    return ReduceTupleObjectSystemToFictional<TEntity>(builder);
                if (SingleTupleObjectIsRedundant(tuple))
                    continue;
                tupleSystem[j++] = (tuple as TSingleTupleObject)!;
            }

            if (j == 0) return CreateEmpty<TEntity>(builder);
            if (j == 1) return tuple;

            tupleSystem.TrimRedundantRows(j);
            tupleSystem.IsOrthogonal = makeOrthogonal;

            return tupleSystem;
        }

        protected TupleObject<TEntity> CreateTupleObjectSystemStrategy<
            TEntity,
            TSingleTupleObjectFactoryArgs>(
            IEnumerable<TSingleTupleObjectFactoryArgs> 
            tupleSysFactoryArgs,
            SingleTupleObjectFactoryHandler<
                TEntity,
                TSingleTupleObjectFactoryArgs> singleTupleObjectFactory,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            BuildTuple(ref builder, onTupleBuilding);

            TupleObject<TEntity> tuple;
            IList<SingleTupleObject<TEntity>> tuples =
                new List<SingleTupleObject<TEntity>>();
            foreach (TSingleTupleObjectFactoryArgs tupleFactoryArgs 
                 in tupleSysFactoryArgs)
            {
                tuple = singleTupleObjectFactory(
                    tupleFactoryArgs,
                    builder);
                if (SingleTupleObjectStopsBuilding(tuple))
                    return ReduceTupleObjectSystemToFictional<TEntity>(builder);
                if (SingleTupleObjectIsRedundant(tuple))
                    continue;
                tuples.Add((tuple as SingleTupleObject<TEntity>)!);
            }

            if (tuples.Count == 0) return CreateEmpty(builder);
            if (tuples.Count == 1) return tuples[0];

            return TupleObjectSystemFactoryImpl(builder.Schema, tuples);
        }

        protected TupleObject<TEntity> CreateTupleObjectSystem<
            TEntity>(
            NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>[][] factoryArgs,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            where TEntity : new()
        {
            return CreateTupleObjectSystemStrategy(
                new SquareEnumerable<NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>>(factoryArgs),
                SingleTupleObjectFactoryImpl,
                onTupleBuilding,
                builder);
        }

        protected TupleObject<TEntity> CreateTupleObjectSystem<
            TEntity>(
            ISingleTupleObjectFactoryArgs[][] factoryArgs,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            where TEntity : new()
        {
            builder ??= GetDefaultBuilder<TEntity>();

            return CreateTupleObjectSystemStrategy(
                new SquareEnumerable<NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>>(
                    factoryArgs.Select(tupleFargs => 
                        tupleFargs.Select(farg => 
                            farg.ToNamedComponentFactoryArgs(builder)))),
                SingleTupleObjectFactoryImpl,
                onTupleBuilding,
                builder);
        }

        /*
        protected TupleObject<TEntity> CreateTupleObjectSystem<
            TEntity>(
            (AttributeName Name, IAttributeComponent ComponentSource)[][] factoryArgs,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
        {
            return CreateTupleObjectSystemStrategy(
                factoryArgs,
                SingleTupleObjectFactoryImpl,
                builder,
                onTupleBuilding);
        }
        */

        protected TupleObject<TEntity> CreateTupleObjectSystem<
            TEntity>(
            ISquareEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> factoryArgs,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
        {
            return CreateTupleObjectSystemStrategy(
                factoryArgs,
                SingleTupleObjectFactoryImpl,
                onTupleBuilding,
                builder);
        }

        protected TupleObject<TEntity> CreateTupleObjectSystem<
            TEntity>(
            IndexedComponentFactoryArgs<IAttributeComponent>[][] factoryArgs,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
        {
            return CreateTupleObjectSystemStrategy(
                factoryArgs,
                SingleTupleObjectFactoryImpl,
                onTupleBuilding,
                builder);
        }

        protected TupleObject<TEntity> CreateDiagonalTupleObjectSystem<
            TEntity,
            TSingleTupleObject,
            TComponentFactoryArgs>(
            IndexedComponentFactoryArgs<TComponentFactoryArgs>[] factoryArgs,
            bool makeOrthogonal,
            SingleTupleObjectFactoryHandler<
                TEntity,
                IEnumerable<IndexedComponentFactoryArgs<TComponentFactoryArgs>>> singleTupleObjectFactory,
            //SingleTupleObjectFactoryHandler<
            //    TEntity,
            //    IEnumerable<IndexedComponentFactoryArgs<TComponentFactoryArgs>>> lastSingleTupleObjectFactory,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
            where TSingleTupleObject : SingleTupleObject<TEntity>
        {
            return CreateDiagonalTupleObjectSystemStrategy<
                TEntity,
                TSingleTupleObject,
                TComponentFactoryArgs>(
                factoryArgs,
                singleTupleObjectFactory,
                makeOrthogonal,
                onTupleBuilding,
                builder);
        }

        /*
        protected TupleObject<TEntity> CreateDiagonalTupleObjectSystem<
            TEntity>(
            IndexedComponentFactoryArgs<IAttributeComponent>[][] factoryArgs,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
        {
            return CreateDiagonalTupleObjectSystemStrategy(
                factoryArgs,
                SingleTupleObjectFactoryImpl,
                onTupleBuilding,
                builder);
        }
        */
    }
}
