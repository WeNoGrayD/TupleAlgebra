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

    public class ConjunctiveTupleFactory
        : SingleTupleObjectFactory
    {
        public ConjunctiveTupleFactory(
            TupleObjectFactory factory)
            : base(factory)
        { }

        protected override SingleTupleObject<TEntity>
            SingleTupleObjectFactoryImpl<TEntity>(TupleObjectSchema<TEntity> schema)
        {
            return new ConjunctiveTuple<TEntity>(schema);
        }

        protected override bool AttributeComponentStopsBuilding<TEntity>(
            ITupleObjectAttributeManager tupleManager,
            SingleTupleObject<TEntity> tuple)
        {
            return tupleManager.IsEmpty(tuple);
        }

        protected override bool AttributeComponentIsDefault<TEntity>(
            ITupleObjectAttributeManager tupleManager,
            SingleTupleObject<TEntity> tuple)
        {
            return tupleManager.IsFull(tuple);
        }

        protected override TupleObject<TEntity>
            ReduceSingleTupleObjectToFictional<TEntity>(
                TupleObjectBuilder<TEntity> builder,
                bool onStop)
        {
            return onStop ? CreateEmpty(builder) : CreateFull(builder);
        }

        protected TupleObject<TEntity> CreateConjunctive<TEntity, TComponentSource>(
            IEnumerable<NamedComponentFactoryArgs<TComponentSource>> factoryArgs,
            SetComponentHandler<TComponentSource> setComponent,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            if (builder is null) BuildTuple(ref builder, null);

            return CreateSingleTupleObject(
                    factoryArgs,
                    setComponent,
                    builder);
        }

        protected TupleObject<TEntity> CreateConjunctive<TEntity, TComponentSource>(
            IEnumerable<NamedComponentFactoryArgs<TComponentSource>> factoryArgs,
            SetComponentHandler<TComponentSource> setComponent,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return CreateSingleTupleObjectWithTupleBuilding(
                factoryArgs,
                setComponent,
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            IEnumerable<NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>> factoryArgs,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= GetDefaultBuilder<TEntity>();

            return CreateConjunctive(
                factoryArgs,
                (setupWizard, tuple, args) => setupWizard.SetComponent(tuple, args),
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            IEnumerable<NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>> factoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= GetDefaultBuilder<TEntity>();

            return CreateConjunctive(
                factoryArgs,
                (setupWizard, tuple, args) => setupWizard.SetComponent(tuple, args),
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            IEnumerable<NamedComponentFactoryArgs<IAttributeComponent>> factoryArgs,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= GetDefaultBuilder<TEntity>();

            return CreateConjunctive(
                factoryArgs,
                (setupWizard, tuple, ac) => setupWizard.SetComponent(tuple, ac),
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            IEnumerable<NamedComponentFactoryArgs<IAttributeComponent>> factoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= GetDefaultBuilder<TEntity>();

            return CreateConjunctive(
                factoryArgs,
                (setupWizard, tuple, ac) => setupWizard.SetComponent(tuple, ac),
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            IEnumerable<ISingleTupleObjectFactoryArgs> factoryArgs,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= GetDefaultBuilder<TEntity>();

            return CreateConjunctive(
                factoryArgs.Select(a => a.ToNamedComponentFactoryArgs(builder)),
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            IEnumerable<ISingleTupleObjectFactoryArgs> factoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= GetDefaultBuilder<TEntity>();

            return CreateConjunctive(
                factoryArgs.Select(a => a.ToNamedComponentFactoryArgs(builder)),
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity, TComponentSource>(
            IEnumerable<IndexedComponentFactoryArgs<TComponentSource>> factoryArgs,
            SetComponentHandler<TComponentSource> setComponent,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return CreateSingleTupleObject(
                factoryArgs,
                setComponent,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity, TComponentSource>(
            IEnumerable<IndexedComponentFactoryArgs<TComponentSource>> factoryArgs,
            SetComponentHandler<TComponentSource> setComponent,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return CreateSingleTupleObjectWithTupleBuilding(
                factoryArgs,
                setComponent,
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> factoryArgs,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return CreateConjunctive(
                factoryArgs,
                (setupWizard, tuple, args) => setupWizard.SetComponent(tuple, args),
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> factoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return CreateConjunctive(
                factoryArgs,
                (setupWizard, tuple, args) => setupWizard.SetComponent(tuple, args),
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            params NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>[] factoryArgs)
            where TEntity : new()
        {
            return CreateConjunctive(
                factoryArgs,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder = null,
            params NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>[] factoryArgs)
            where TEntity : new()
        {
            return CreateConjunctive(
                factoryArgs,
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            params NamedComponentFactoryArgs<IAttributeComponent>[] factoryArgs)
            where TEntity : new()
        {
            return CreateConjunctive(
                factoryArgs,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder = null,
            params NamedComponentFactoryArgs<IAttributeComponent>[] factoryArgs)
            where TEntity : new()
        {
            return CreateConjunctive(
                factoryArgs,
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            params IndexedComponentFactoryArgs<IAttributeComponent>[] factoryArgs)
            where TEntity : new()
        {
            return CreateConjunctive(
                factoryArgs,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder = null,
            params IndexedComponentFactoryArgs<IAttributeComponent>[] factoryArgs)
            where TEntity : new()
        {
            return CreateConjunctive(
                factoryArgs,
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            params ISingleTupleObjectFactoryArgs[] factoryArgs)
            where TEntity : new()
        {
            return CreateConjunctive(
                factoryArgs,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder = null,
            params ISingleTupleObjectFactoryArgs[] factoryArgs)
            where TEntity : new()
        {
            return CreateConjunctive(
                factoryArgs,
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctiveTupleWithTrailingComplement<
            TEntity>(
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> factoryArgs,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return CreateSingleTupleObjectWithTrailingComplement(
                factoryArgs,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            TEntity entity,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return CreateConjunctive(
                PassEntity(builder, entity), 
                PassMember, 
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            TEntity entity)
            where TEntity : new()
        {
            TupleObjectBuilder<TEntity> builder =
                GetDefaultBuilder<TEntity>();

            return CreateConjunctive(entity, builder);
        }

        public ConjunctiveTuple<TEntity> CreateFullConjunctive<TEntity>(
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            ConjunctiveTuple<TEntity> tuple =
                new ConjunctiveTuple<TEntity>(builder.Schema);
            ITupleObjectAttributeManager tupleManager;
            for (int i = 0; i < builder.Schema.PluggedAttributesCount; i++)
            {
                tupleManager = builder.AttributeAt(i).CreateManager();
                tupleManager.SetDefaultFictionalAttributeComponent(tuple);
            }

            return tuple;
        }
    }
}
