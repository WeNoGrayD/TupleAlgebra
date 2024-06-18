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
        : TupleObjectSystemFactory<
            ConjunctiveTupleFactory,
            DisjunctiveTupleFactory>
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
                IList<ISingleTupleObject> tuples)
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

        public TupleObject<TEntity> CreateConjunctiveTupleSystem<TEntity>(
            IEnumerable<TEntity> entitySet,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return CreateTupleObjectSystemStrategy<
                TEntity,
                ConjunctiveTuple<TEntity>,
                TEntity>(
                PassVerticalEntitySet(builder, entitySet),
                _singleTupleObjectFactory.CreateConjunctive,
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctiveTupleSystem<TEntity>(
            IEnumerable<TupleObject<TEntity>> tupleSysFactoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return CreateTupleObjectSystemStrategy<
                TEntity,
                ConjunctiveTuple<TEntity>,
                TupleObject<TEntity>>(
                tupleSysFactoryArgs,
                (tuple, b) => tuple.AlignWithSchema(b.Schema, _factory, b),
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctiveTupleSystem<TEntity>(
            ISquareEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> tupleSysFactoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return CreateTupleObjectSystem<
                TEntity,
                ConjunctiveTuple<TEntity>>(
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
            return CreateTupleObjectSystem<
                TEntity,
                ConjunctiveTuple<TEntity>>(
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
