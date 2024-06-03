using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectFactoryInfrastructure
{
    using static TupleObjectHelper;

    public abstract class TupleObjectSystemFactory<
        TAttributeComponent,
        TSingleTupleObject,
        TSingleTupleObjectFactory,
        TAltSingleTupleObjectFactory>
        : AbstractTupleObjectFactory
        where TSingleTupleObject : ISingleTupleObject<TAttributeComponent>
        where TSingleTupleObjectFactory : AbstractTupleObjectFactory
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
                IList<TSingleTupleObject> tuples)
                where TEntity : new();

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

        protected TupleObject<TEntity> CreateDiagonalTupleObjectSystemStrategy<
            TEntity,
            CTSingleTupleObject,
            TComponentSource>(
            IEnumerable<IndexedComponentFactoryArgs<TComponentSource>> tupleSysFactoryArgs,
            SingleTupleObjectFactoryHandler<
                TEntity,
                IEnumerable<IndexedComponentFactoryArgs<TComponentSource>>>
            singleTupleObjectFactory,
            bool makeOrthogonal,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
            where CTSingleTupleObject : TupleObject<TEntity>, TSingleTupleObject
        {
            BuildTuple(ref builder, onTupleBuilding);

            int len = builder.Schema.PluggedAttributesCount, i, j = 0;
            ITupleObjectSystem<TAttributeComponent, CTSingleTupleObject> tupleSystem =
                TupleObjectSystemFactoryImpl(
                    builder.Schema, len) 
                as ITupleObjectSystem<TAttributeComponent, CTSingleTupleObject>;
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
                tupleSystem[j++] = (tuple as CTSingleTupleObject)!;
            }

            if (j == 0) return CreateEmpty<TEntity>(builder);
            if (j == 1) return tuple;

            tupleSystem.TrimRedundantRows(j);
            tupleSystem.IsOrthogonal = makeOrthogonal;

            return (tupleSystem as TupleObject<TEntity>);
        }

        protected TupleObject<TEntity> CreateTupleObjectSystemStrategy<
            TEntity,
            CTSingleTupleObject,
            TSingleTupleObjectFactoryArgs>(
            IEnumerable<TSingleTupleObjectFactoryArgs> 
            tupleSysFactoryArgs,
            SingleTupleObjectFactoryHandler<
                TEntity,
                TSingleTupleObjectFactoryArgs> singleTupleObjectFactory,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
            where CTSingleTupleObject : TupleObject<TEntity>, TSingleTupleObject
        {
            BuildTuple(ref builder, onTupleBuilding);

            TupleObject<TEntity> tuple;
            IList<TSingleTupleObject> tuples =
                new List<TSingleTupleObject>();
            foreach (TSingleTupleObjectFactoryArgs tupleFactoryArgs 
                 in tupleSysFactoryArgs)
            {
                tuple = singleTupleObjectFactory(
                    tupleFactoryArgs,
                    builder);
                if (SingleTupleObjectStopsBuilding(tuple))
                {
                    tuples.Clear();
                    return ReduceTupleObjectSystemToFictional<TEntity>(builder);
                }
                if (SingleTupleObjectIsRedundant(tuple))
                    continue;
                tuples.Add((tuple as CTSingleTupleObject)!);
            }

            TupleObject<TEntity> result;

            if (tuples.Count == 0)
            {
                result = CreateEmpty(builder);
            }
            else if (tuples.Count == 1)
            {
                result = (tuples[0] as CTSingleTupleObject)!;
            }
            else
            {
                result = TupleObjectSystemFactoryImpl(builder.Schema, tuples);
            }
            tuples.Clear();

            return result;
        }

        protected TupleObject<TEntity> CreateDiagonalTupleObjectSystem<
            TEntity,
            CTSingleTupleObject,
            TComponentFactoryArgs>(
            IndexedComponentFactoryArgs<TComponentFactoryArgs>[] factoryArgs,
            bool makeOrthogonal,
            SingleTupleObjectFactoryHandler<
                TEntity,
                IEnumerable<IndexedComponentFactoryArgs<TComponentFactoryArgs>>> singleTupleObjectFactory,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
            where CTSingleTupleObject : TupleObject<TEntity>, TSingleTupleObject
        {
            return CreateDiagonalTupleObjectSystemStrategy<
                TEntity,
                CTSingleTupleObject,
                TComponentFactoryArgs>(
                factoryArgs,
                singleTupleObjectFactory,
                makeOrthogonal,
                onTupleBuilding,
                builder);
        }
    }

    public abstract class TupleObjectSystemFactory<
        TSingleTupleObjectFactory,
        TAltSingleTupleObjectFactory>
        : TupleObjectSystemFactory<
            IAttributeComponent,
            ISingleTupleObject,
            TSingleTupleObjectFactory,
            TAltSingleTupleObjectFactory>
        where TSingleTupleObjectFactory : AbstractTupleObjectFactory
    {
        public TupleObjectSystemFactory(
            TupleObjectFactory factory,
            TSingleTupleObjectFactory singleTupleObjectFactory,
            TAltSingleTupleObjectFactory altSingleTupleObjectFactory)
            : base(
                  factory,
                  singleTupleObjectFactory,
                  altSingleTupleObjectFactory)
        {
            return;
        }

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

        protected TupleObject<TEntity> CreateDiagonalTupleObjectSystemStrategy<
            TEntity,
            CTSingleTupleObject>(
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>>
            tupleSysFactoryArgs,
            bool makeOrthogonal,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
            where CTSingleTupleObject : TupleObject<TEntity>, ISingleTupleObject
        {
            SingleTupleObjectFactoryHandler<
                TEntity,
                IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>>>
                singleTupleObjectFactory = makeOrthogonal ?
                SingleTupleObjectFactoryWithTrailingComplementImpl<TEntity> :
                SingleTupleObjectFactoryImpl<TEntity>;

            return CreateDiagonalTupleObjectSystemStrategy<
                TEntity,
                CTSingleTupleObject,
                IAttributeComponent>(
                tupleSysFactoryArgs,
                singleTupleObjectFactory,
                makeOrthogonal,
                onTupleBuilding,
                builder);
        }

        protected TupleObject<TEntity> CreateTupleObjectSystem<
            TEntity,
            CTSingleTupleObject>(
            NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>[][] factoryArgs,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            where TEntity : new()
            where CTSingleTupleObject : TupleObject<TEntity>, ISingleTupleObject
        {
            return CreateTupleObjectSystemStrategy<
                TEntity,
                CTSingleTupleObject,
                IEnumerable<NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>>>(
                new SquareEnumerable<NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>>(factoryArgs),
                SingleTupleObjectFactoryImpl,
                onTupleBuilding,
                builder);
        }

        protected TupleObject<TEntity> CreateTupleObjectSystem<
            TEntity,
            CTSingleTupleObject>(
            ISingleTupleObjectFactoryArgs[][] factoryArgs,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            where TEntity : new()
            where CTSingleTupleObject : TupleObject<TEntity>, ISingleTupleObject
        {
            builder ??= GetDefaultBuilder<TEntity>();

            return CreateTupleObjectSystemStrategy<
                TEntity,
                CTSingleTupleObject,
                IEnumerable<NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>>>(
                new SquareEnumerable<NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>>(
                    factoryArgs.Select(tupleFargs =>
                        tupleFargs.Select(farg =>
                            farg.ToNamedComponentFactoryArgs(builder)))),
                SingleTupleObjectFactoryImpl,
                onTupleBuilding,
                builder);
        }

        protected TupleObject<TEntity> CreateTupleObjectSystem<
            TEntity,
            CTSingleTupleObject>(
            ISquareEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> factoryArgs,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
            where CTSingleTupleObject : TupleObject<TEntity>, ISingleTupleObject
        {
            return CreateTupleObjectSystemStrategy<
                TEntity,
                CTSingleTupleObject,
                IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>>>(
                factoryArgs,
                SingleTupleObjectFactoryImpl,
                onTupleBuilding,
                builder);
        }

        protected TupleObject<TEntity> CreateTupleObjectSystem<
            TEntity,
            CTSingleTupleObject>(
            IndexedComponentFactoryArgs<IAttributeComponent>[][] factoryArgs,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
            where CTSingleTupleObject : TupleObject<TEntity>, ISingleTupleObject
        {
            return CreateTupleObjectSystemStrategy<
                TEntity,
                CTSingleTupleObject,
                IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>>>(
                factoryArgs,
                SingleTupleObjectFactoryImpl,
                onTupleBuilding,
                builder);
        }

        protected TupleObject<TEntity> ToAlternateDiagonal<
            TEntity,
            CTSingleTupleObject>(
            SingleTupleObject<TEntity> tuple,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
            where CTSingleTupleObject : TupleObject<TEntity>, ISingleTupleObject
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
                CTSingleTupleObject>(
                tupleSysFactoryArgs,
                true,
                tuple.PassSchema,
                builder);
        }

        protected TupleObject<TEntity> ToAlternateDiagonal<
            TEntity,
            CTSingleTupleObject>(
            SingleTupleObject<TEntity> tuple)
            where TEntity : new()
            where CTSingleTupleObject : TupleObject<TEntity>, ISingleTupleObject
        {
            TupleObjectBuilder<TEntity> builder =
                GetDefaultBuilder<TEntity>();

            return ToAlternateDiagonal<TEntity, CTSingleTupleObject>(tuple, builder);
        }
    }
}
