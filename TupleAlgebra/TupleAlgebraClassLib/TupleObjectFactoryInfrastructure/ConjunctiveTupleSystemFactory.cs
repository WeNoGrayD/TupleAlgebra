using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectFactoryInfrastructure
{
    using static TupleObjectHelper;

    public class ConjunctiveTupleSystemFactory
        : TupleObjectSystemFactory<ConjunctiveTupleFactory, DisjunctiveTupleFactory>
    {
        public ConjunctiveTupleSystemFactory(
            TupleObjectFactory factory,
            ConjunctiveTupleFactory cTupleFactory,
            DisjunctiveTupleFactory dTupleFactory)
            : base(factory, cTupleFactory, dTupleFactory)
        { }

        protected override TupleObject<TEntity>
            TupleObjectSystemFactoryImpl<TEntity>(
                TupleObjectSchema<TEntity> schema,
            int len)
        {
            return new ConjunctiveTupleSystem<TEntity>(schema, len);
        }

        protected override TupleObject<TEntity>
            TupleObjectSystemFactoryImpl<TEntity>(
                TupleObjectSchema<TEntity> schema,
                IList<SingleTupleObject<TEntity>> tuples)
        {
            return new ConjunctiveTupleSystem<TEntity>(schema, tuples);
        }

        protected override TupleObject<TEntity>
            SingleTupleObjectFactoryImpl<TEntity>(
                IEnumerable<NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>> tupleFactoryArgs,
                TupleObjectBuilder<TEntity> builder)
        {
            return _singleTupleObjectFactory
                .CreateConjunctive(tupleFactoryArgs, builder);
        }

        protected override TupleObject<TEntity>
            SingleTupleObjectFactoryImpl<TEntity>(
                IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> tupleFactoryArgs,
                TupleObjectBuilder<TEntity> builder)
        {
            return _singleTupleObjectFactory
                .CreateConjunctive(tupleFactoryArgs, builder);
        }

        protected override TupleObject<TEntity>
            SingleTupleObjectFactoryWithTrailingComplementImpl<TEntity>(
                IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> tupleFactoryArgs,
                TupleObjectBuilder<TEntity> builder)
        {
            return _singleTupleObjectFactory
                .CreateConjunctiveTupleWithTrailingComplement(tupleFactoryArgs, builder);
        }

        /*
        protected override TupleObject<TEntity>
            AlternateSingleTupleObjectFactoryImpl<TEntity>(
                NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>[] tupleFactoryArgs,
                TupleObjectBuilder<TEntity> builder)
        {
            return _altSingleTupleObjectFactory
                .CreateDisjunctive(tupleFactoryArgs, builder);
        }

        protected override TupleObject<TEntity>
            AlternateSingleTupleObjectFactoryImpl<TEntity>(
                IndexedComponentFactoryArgs<IAttributeComponent>[] tupleFactoryArgs,
                TupleObjectBuilder<TEntity> builder)
        {
            return _altSingleTupleObjectFactory
                .CreateDisjunctive(tupleFactoryArgs, builder);
        }
        */

        protected override bool SingleTupleObjectIsRedundant<TEntity>(
            TupleObject<TEntity> tuple)
        {
            return tuple.IsEmpty();
        }

        protected override bool SingleTupleObjectStopsBuilding<TEntity>(
            TupleObject<TEntity> tuple)
        {
            return tuple.IsFull();
        }

        protected override TupleObject<TEntity>
            ReduceTupleObjectSystemToFictional<TEntity>(
                TupleObjectBuilder<TEntity> builder)
        {
            return CreateFull<TEntity>(builder);
        }

        private TupleObject<TEntity>
            ConjunctiveTupleFactory<
                TEntity,
                TSingleTupleObjectFactoryArgs>(
                IEnumerable<IndexedComponentFactoryArgs<TSingleTupleObjectFactoryArgs>> tupleFactoryArgs,
                SetComponentHandler<TSingleTupleObjectFactoryArgs> setComponent,
                TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return _singleTupleObjectFactory.CreateConjunctive(
                tupleFactoryArgs,
                setComponent,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctiveTupleSystem<TEntity>(
            IEnumerable<TEntity> entitySet)
            where TEntity : new()
        {
            TupleObjectBuilder<TEntity> builder =
                GetDefaultBuilder<TEntity>();

            return CreateTupleObjectSystemStrategy(
                PassVerticalEntitySet(builder, entitySet),
                _singleTupleObjectFactory.CreateConjunctive,
                null,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctiveTupleSystem<TEntity>(
            IEnumerable<TupleObject<TEntity>> tupleSysFactoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return CreateTupleObjectSystemStrategy(
                tupleSysFactoryArgs,
                (tuple, b) => tuple,
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctiveTupleSystem<TEntity>(
            ISquareEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> tupleSysFactoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return CreateTupleObjectSystem(
                tupleSysFactoryArgs,
                builder,
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateConjunctiveTupleSystem<TEntity>(
            ISquareEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> tupleSysFactoryArgs)
            where TEntity : new()
        {
            TupleObjectBuilder<TEntity> builder =
                GetDefaultBuilder<TEntity>();

            return CreateConjunctiveTupleSystem(
                tupleSysFactoryArgs,
                null,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctiveTupleSystem<TEntity>(
            ISingleTupleObjectFactoryArgs[][] tupleSysFactoryArgs,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return CreateTupleObjectSystem(
                tupleSysFactoryArgs,
                builder);
        }

        public TupleObject<TEntity> ToAlternateDiagonal<TEntity>(
            DisjunctiveTuple<TEntity> tuple)
            where TEntity : new()
        {
            return base.ToAlternateDiagonal<TEntity, ConjunctiveTuple<TEntity>>(tuple);
        }

        public TupleObject<TEntity> CreateDiagonalConjunctiveTupleSystem<TEntity>(
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> 
            tupleSysFactoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder,
            bool makeOrthogonal)
            where TEntity : new()
        {
            return CreateDiagonalTupleObjectSystemStrategy<
                TEntity, ConjunctiveTuple<TEntity>>(
                tupleSysFactoryArgs,
                makeOrthogonal,
                onTupleBuilding,
                builder);
        }
    }
}
