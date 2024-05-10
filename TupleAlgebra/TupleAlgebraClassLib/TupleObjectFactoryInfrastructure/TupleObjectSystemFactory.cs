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
            TSingleTupleObjectFactory singleTupleObjectFactory,
            TAltSingleTupleObjectFactory altSingleTupleObjectFactory)
        {
            _singleTupleObjectFactory = singleTupleObjectFactory;
            _altSingleTupleObjectFactory = altSingleTupleObjectFactory;

            return;
        }

        protected abstract TupleObjectSystem<TEntity>
            TupleObjectSystemFactoryImpl<TEntity>(
                TupleObjectSchema<TEntity> schema,
                int len)
                where TEntity : new();

        protected abstract TupleObjectSystem<TEntity>
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

        protected TupleObject<TEntity> ToAlternateDiagonal<TEntity>(
            SingleTupleObject<TEntity> tuple,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            int len = tuple.RowLength;
            IndexedComponentFactoryArgs<IAttributeComponent>[] tupleSysFactoryArgs =
                new IndexedComponentFactoryArgs<IAttributeComponent>[len];
            for (int i = 0; i < len; i++)
            {
                tupleSysFactoryArgs[i] = new(i, builder, tuple[i]);
            }

            return CreateDiagonalTupleObjectSystemStrategy(
                tupleSysFactoryArgs,
                SingleTupleObjectFactoryWithTrailingComplementImpl,
                //SingleTupleObjectFactoryImpl,
                tuple.PassSchema,
                builder);
        }

        protected TupleObject<TEntity> ToAlternateDiagonal<TEntity>(
            SingleTupleObject<TEntity> tuple)
            where TEntity : new()
        {
            TupleObjectBuilder<TEntity> builder =
                GetDefaultBuilder<TEntity>();

            return ToAlternateDiagonal(tuple, builder);
        }

        protected TupleObject<TEntity> CreateDiagonalTupleObjectSystemStrategy<
            TEntity,
            TComponentSource>(
            IndexedComponentFactoryArgs<TComponentSource>[] tupleSysFactoryArgs,
            SingleTupleObjectFactoryHandler<
                TEntity,
                IEnumerable<IndexedComponentFactoryArgs<TComponentSource>>> singleTupleObjectFactory,
            //SingleTupleObjectFactoryHandler<
            //    TEntity,
            //    IEnumerable<IndexedComponentFactoryArgs<TComponentSource>>> lastSingleTupleObjectFactory,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            BuildTuple(ref builder, onTupleBuilding);

            int len = tupleSysFactoryArgs.Length, i, j, k = 0;
            TupleObjectSystem<TEntity> tupleSystem =
                TupleObjectSystemFactoryImpl(builder.Schema, len);

            TupleObject<TEntity> tuple = null;
            IndexedComponentFactoryArgs<TComponentSource> farg;
            IndexedComponentFactoryArgs<TComponentSource>[] tupleFactoryArgs =
                new IndexedComponentFactoryArgs<TComponentSource>[len];

            for (i = 1; i < len; i++)
                tupleFactoryArgs[i] = new(tupleSysFactoryArgs[i].TupleManager);

            for (j = 0; j < len; j++)
            {
                farg = tupleSysFactoryArgs[j];
                i = farg.Index;
                tupleFactoryArgs[i] = farg;
                tuple = singleTupleObjectFactory(
                    tupleFactoryArgs,
                    builder);
                if (SingleTupleObjectStopsBuilding(tuple))
                    return ReduceTupleObjectSystemToFictional<TEntity>(builder);
                if (SingleTupleObjectIsRedundant(tuple))
                    continue;
                tupleSystem[k++] = (tuple as SingleTupleObject<TEntity>)!;
            }

            /*
            farg = tupleSysFactoryArgs[j];
            i = farg.Index;
            tupleFactoryArgs[i] = farg;
            tuple = lastSingleTupleObjectFactory(
                tupleFactoryArgs,
                builder);
            if (SingleTupleObjectStopsBuilding(tuple))
                return ReduceTupleObjectSystemToFictional<TEntity>(builder);
            if (!SingleTupleObjectIsRedundant(tuple))
            {
                tupleSystem[k++] = (tuple as SingleTupleObject<TEntity>)!;
            }
            */

            if (k == 1) return tuple;

            tupleSystem.TrimRedundantRows(k);

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

        protected TupleObject<TEntity> CreateTupleObjectSystem<
            TEntity,
            TComponentFactoryArgs>(
            IndexedComponentFactoryArgs<TComponentFactoryArgs>[] factoryArgs,
            SingleTupleObjectFactoryHandler<
                TEntity,
                IEnumerable<IndexedComponentFactoryArgs<TComponentFactoryArgs>>> singleTupleObjectFactory,
            //SingleTupleObjectFactoryHandler<
            //    TEntity,
            //    IEnumerable<IndexedComponentFactoryArgs<TComponentFactoryArgs>>> lastSingleTupleObjectFactory,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
        {
            return CreateDiagonalTupleObjectSystemStrategy(
                factoryArgs,
                singleTupleObjectFactory,
                //lastSingleTupleObjectFactory,
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
