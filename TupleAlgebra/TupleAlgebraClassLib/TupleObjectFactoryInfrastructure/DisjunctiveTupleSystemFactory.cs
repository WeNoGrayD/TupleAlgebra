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

    public class DisjunctiveTupleSystemFactory
        : TupleObjectSystemFactory<DisjunctiveTupleFactory, ConjunctiveTupleFactory>
    {
        public DisjunctiveTupleSystemFactory(
            DisjunctiveTupleFactory dTupleFactory,
            ConjunctiveTupleFactory cTupleFactory)
            : base(dTupleFactory, cTupleFactory)
        { }

        protected override TupleObjectSystem<TEntity>
            TupleObjectSystemFactoryImpl<TEntity>(
                TupleObjectSchema<TEntity> schema,
                int len)
        {
            return new DisjunctiveTupleSystem<TEntity>(schema, len);
        }

        protected override TupleObjectSystem<TEntity>
            TupleObjectSystemFactoryImpl<TEntity>(
                TupleObjectSchema<TEntity> schema,
                IList<SingleTupleObject<TEntity>> tuples)
        {
            return new DisjunctiveTupleSystem<TEntity>(schema, tuples);
        }

        protected override TupleObject<TEntity>
            SingleTupleObjectFactoryImpl<TEntity>(
                IEnumerable<NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>> tupleFactoryArgs,
                TupleObjectBuilder<TEntity> builder)
        {
            return _singleTupleObjectFactory
                .CreateDisjunctive(tupleFactoryArgs, builder);
        }

        protected override TupleObject<TEntity>
            SingleTupleObjectFactoryImpl<TEntity>(
                IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> tupleFactoryArgs,
                TupleObjectBuilder<TEntity> builder)
        {
            return _singleTupleObjectFactory
                .CreateDisjunctive(tupleFactoryArgs, builder);
        }

        protected override TupleObject<TEntity>
            SingleTupleObjectFactoryWithTrailingComplementImpl<TEntity>(
                IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> tupleFactoryArgs,
                TupleObjectBuilder<TEntity> builder)
        {
            return _singleTupleObjectFactory
                .CreateDisjunctiveTupleWithTrailingComplement(tupleFactoryArgs, builder);
        }

        /*
        protected override TupleObject<TEntity>
            AlternateSingleTupleObjectFactoryImpl<TEntity>(
                NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>[] tupleFactoryArgs,
                TupleObjectBuilder<TEntity> builder)
        {
            return _altSingleTupleObjectFactory
                .CreateConjunctive(tupleFactoryArgs, builder);
        }

        protected override TupleObject<TEntity>
            AlternateSingleTupleObjectFactoryImpl<TEntity>(
                IndexedComponentFactoryArgs<IAttributeComponent>[] tupleFactoryArgs,
                TupleObjectBuilder<TEntity> builder)
        {
            return _altSingleTupleObjectFactory
                .CreateConjunctive(tupleFactoryArgs, builder);
        }
        */

        protected override bool SingleTupleObjectIsRedundant<TEntity>(
            TupleObject<TEntity> tuple)
        {
            return tuple.IsFull();
        }

        protected override bool SingleTupleObjectStopsBuilding<TEntity>(
            TupleObject<TEntity> tuple)
        {
            return tuple.IsEmpty();
        }

        protected override TupleObject<TEntity>
            ReduceTupleObjectSystemToFictional<TEntity>(
                TupleObjectBuilder<TEntity> builder)
        {
            return CreateEmpty<TEntity>(builder);
        }

        public TupleObject<TEntity> ToAlternateDiagonal<TEntity>(
            ConjunctiveTuple<TEntity> tuple)
            where TEntity : new()
        {
            return base.ToAlternateDiagonal(tuple);
        }

        /*
        public TupleObject<TEntity> CreateDisjunctiveTupleSystem<TEntity>(
            TEntity entity,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return CreateDisjunctiveTupleSystem(
                PassHorizontalEntitySet(builder, entitySet),
                PassMembers,
                builder,
                null);
        }
        */

        public TupleObject<TEntity> CreateDisjunctiveTupleSystem<TEntity>(
            TEntity entity,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            int len = builder.Schema.PluggedAttributesCount;
            IndexedComponentFactoryArgs<TEntity>[] tupleSysFactoryArgs =
                new IndexedComponentFactoryArgs<TEntity>[len];
            for (int i = 0; i < len; i++)
            {
                tupleSysFactoryArgs[i] = new (i, builder, entity);
            }

            return CreateTupleObjectSystem(
                tupleSysFactoryArgs,
                CreateDisjunctiveWithTrailingComplement,
                //CreateDisjunctive,
                builder,
                null);
        }

        public TupleObject<TEntity> CreateDisjunctiveTupleSystem<TEntity>(
            TEntity entity)
            where TEntity : new()
        {
            TupleObjectBuilder<TEntity> builder =
                GetDefaultBuilder<TEntity>();

            return CreateDisjunctiveTupleSystem(entity, builder);
        }

        private TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IEnumerable<IndexedComponentFactoryArgs<TEntity>> tupleFactoryArgs,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return _singleTupleObjectFactory.CreateDisjunctive(
                tupleFactoryArgs,
                PassMember,
                builder);
        }

        private TupleObject<TEntity> CreateDisjunctiveWithTrailingComplement<TEntity>(
            IEnumerable<IndexedComponentFactoryArgs<TEntity>> tupleFactoryArgs,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return _singleTupleObjectFactory.CreateDisjunctive(
                tupleFactoryArgs,
                PassMemberWithTrailingComplement,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctiveTupleSystem<TEntity>(
            IndexedComponentFactoryArgs<IAttributeComponent>[][] tupleSysFactoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
        {
            TupleObjectBuilder<TEntity> builder =
                GetDefaultBuilder<TEntity>();

            return CreateTupleObjectSystem(
                tupleSysFactoryArgs,
                builder,
                onTupleBuilding);
        }

        /*
        public TupleObject<TEntity> CreateDisjunctiveTupleSystem<TEntity>(
            ISquareEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> tupleSysFactoryArgs)
            where TEntity : new()
        {
            TupleObjectBuilder<TEntity> builder = 
                GetDefaultBuilder<TEntity>();

            return CreateTupleObjectSystem(
                tupleSysFactoryArgs,
                CreateDisjunctiveWithTrailingComplement,
                CreateDisjunctive,
                builder,
                null);
        }
        */

        /*
        private TupleObject<TEntity> CreateDisjunctive<TEntity>(
            TEntity entity,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return _singleTupleObjectFactory.CreateDisjunctive(
                PassEntity(builder, entity),
                PassMember,
                builder);
        }

        private TupleObject<TEntity> CreateDisjunctiveWithTrailingComplement<TEntity>(
            TEntity entity,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return _singleTupleObjectFactory.CreateDisjunctive(
                PassEntity(builder, entity),
                PassMemberWithTrailingComplement,
                builder);
        }
        */
    }
}
