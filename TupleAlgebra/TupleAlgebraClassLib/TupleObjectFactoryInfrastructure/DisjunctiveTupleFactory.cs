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

    public class DisjunctiveTupleFactory : SingleTupleObjectFactory
    {
        public DisjunctiveTupleFactory(
            TupleObjectFactory factory)
            : base(factory)
        { }

        protected override SingleTupleObject<TEntity>
            SingleTupleObjectFactoryImpl<TEntity>(TupleObjectSchema<TEntity> schema)
        {
            return new DisjunctiveTuple<TEntity>(schema);
        }

        protected override bool AttributeComponentStopsBuilding<TEntity>(
            ITupleObjectAttributeManager tupleManager,
            SingleTupleObject<TEntity> tuple)
        {
            return tupleManager.IsFull(tuple);
        }

        protected override bool AttributeComponentIsDefault<TEntity>(
            ITupleObjectAttributeManager tupleManager,
            SingleTupleObject<TEntity> tuple)
        {
            return tupleManager.IsEmpty(tuple);
        }

        protected override TupleObject<TEntity>
            ReduceSingleTupleObjectToFictional<TEntity>(
                TupleObjectBuilder<TEntity> builder,
                bool onStop)
        {
            return onStop ? CreateFull(builder) : CreateEmpty(builder);
        }

        protected TupleObject<TEntity> CreateDisjunctive<TEntity, TComponentSource>(
            IEnumerable<NamedComponentFactoryArgs<TComponentSource>> factoryArgs,
            SetComponentHandler<TComponentSource> setComponent,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return CreateSingleTupleObject(
                factoryArgs,
                setComponent,
                builder);
        }

        protected TupleObject<TEntity> CreateDisjunctive<TEntity, TComponentSource>(
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

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IEnumerable<NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>> factoryArgs,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return CreateDisjunctive(
                factoryArgs,
                (setupWizard, tuple, args) => setupWizard.SetComponent(tuple, args),
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IEnumerable<NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>> factoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return CreateDisjunctive(
                factoryArgs,
                (setupWizard, tuple, args) => setupWizard.SetComponent(tuple, args),
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IEnumerable<NamedComponentFactoryArgs<IAttributeComponent>> factoryArgs,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return CreateDisjunctive(
                factoryArgs,
                (setupWizard, tuple, ac) => setupWizard.SetComponent(tuple, ac),
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IEnumerable<NamedComponentFactoryArgs<IAttributeComponent>> factoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return CreateDisjunctive(
                factoryArgs,
                (setupWizard, tuple, ac) => setupWizard.SetComponent(tuple, ac),
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IEnumerable<ISingleTupleObjectFactoryArgs> factoryArgs,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= GetDefaultBuilder<TEntity>();

            return CreateDisjunctive(
                factoryArgs.Select(fa => fa.ToNamedComponentFactoryArgs(builder)),
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IEnumerable<ISingleTupleObjectFactoryArgs> factoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            builder ??= GetDefaultBuilder<TEntity>();

            return CreateDisjunctive(
                factoryArgs.Select(fa => fa.ToNamedComponentFactoryArgs(builder)),
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> factoryArgs,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return CreateSingleTupleObject(
                factoryArgs,
                (setupWizard, tuple, args) => setupWizard.SetComponent(tuple, args),
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> factoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return CreateSingleTupleObjectWithTupleBuilding(
                factoryArgs,
                (setupWizard, tuple, args) => setupWizard.SetComponent(tuple, args),
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            params NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>[] factoryArgs)
            where TEntity : new()
        {
            return CreateDisjunctive(
                factoryArgs,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder = null,
            params NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>[] factoryArgs)
            where TEntity : new()
        {
            return CreateDisjunctive(
                factoryArgs,
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            params NamedComponentFactoryArgs<IAttributeComponent>[] factoryArgs)
            where TEntity : new()
        {
            return CreateDisjunctive(
                factoryArgs,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder = null,
            params NamedComponentFactoryArgs<IAttributeComponent>[] factoryArgs)
            where TEntity : new()
        {
            return CreateDisjunctive(
                factoryArgs,
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            params IndexedComponentFactoryArgs<IAttributeComponent>[] factoryArgs)
            where TEntity : new()
        {
            return CreateDisjunctive(
                factoryArgs,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder = null,
            params IndexedComponentFactoryArgs<IAttributeComponent>[] factoryArgs)
            where TEntity : new()
        {
            return CreateDisjunctive(
                factoryArgs,
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            params ISingleTupleObjectFactoryArgs[] factoryArgs)
            where TEntity : new()
        {
            return CreateDisjunctive(
                factoryArgs,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder = null,
            params ISingleTupleObjectFactoryArgs[] factoryArgs)
            where TEntity : new()
        {
            return CreateDisjunctive(
                factoryArgs,
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctiveTupleWithTrailingComplement<
            TEntity>(
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> factoryArgs,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return CreateSingleTupleObjectWithTrailingComplement(
                factoryArgs,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity, TComponentSource>(
            IEnumerable<IndexedComponentFactoryArgs<TComponentSource>> factoryArgs,
            SetComponentHandler<TComponentSource> setComponent,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return CreateSingleTupleObject(
                factoryArgs,
                setComponent,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity, TComponentSource>(
            IEnumerable<IndexedComponentFactoryArgs<TComponentSource>> factoryArgs,
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
    }
}
