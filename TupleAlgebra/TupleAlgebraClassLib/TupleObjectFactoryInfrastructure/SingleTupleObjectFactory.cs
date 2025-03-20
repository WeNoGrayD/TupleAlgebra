using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectFactoryInfrastructure
{
    using static TupleObjectHelper;

    public abstract class SingleTupleObjectFactory<
        TAttributeComponent, TSingleTupleObject>
        : AbstractTupleObjectFactory
        where TSingleTupleObject : ISingleTupleObject<TAttributeComponent>
    {
        public delegate ITupleObjectAttributeManager
            SetComponentHandler<TComponentSource>(
            ITupleObjectAttributeManager tupleManager,
            TSingleTupleObject tuple,
            TComponentSource componentSource);

        public delegate ITupleObjectAttributeManager
            SetComponentWithComplementOpportunityHandler<TComponentSource>(
            ITupleObjectAttributeManager tupleManager,
            TSingleTupleObject tuple,
            TComponentSource componentSource,
            bool toComplement);

        public SingleTupleObjectFactory(
            TupleObjectFactory factory)
        {
            _factory = factory;

            return;
        }

        /*
        protected delegate TTupleObject SingleTupleObjectFactoryHandler<
            TEntity,
            TTupleObject>(
                TupleObjectSchema<TEntity> schema)
            where TEntity : new()
            where TTupleObject : TupleObject<TEntity>, ISingleTupleObject;
        */

        protected abstract TSingleTupleObject
            SingleTupleObjectFactoryImpl<TEntity>(
                TupleObjectSchema<TEntity> schema)
                where TEntity : new();

        protected abstract bool AttributeComponentStopsBuilding<TEntity>(
            ITupleObjectAttributeManager tupleManager,
            TSingleTupleObject tuple)
            where TEntity : new();

        protected abstract bool AttributeComponentIsDefault<TEntity>(
            ITupleObjectAttributeManager tupleManager,
            TSingleTupleObject tuple)
            where TEntity : new();

        protected abstract TupleObject<TEntity>
            ReduceSingleTupleObjectToFictional<TEntity>(
                TupleObjectBuilder<TEntity> builder,
                bool onStop)
            where TEntity : new();

        protected abstract void SetDefaultComponent(
            ITupleObjectAttributeManager attrManager,
            TSingleTupleObject tuple);

        protected TupleObject<TEntity> CreateSingleTupleObject<
            TEntity,
            TComponentSource>(
            IEnumerable<NamedComponentFactoryArgs<TComponentSource>> factoryArgs,
            SetComponentHandler<TComponentSource> setComponent,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            /*
             * Инициализация компонентами тех атрибутов, которые были явно названы.
             */
            TSingleTupleObject tuple =
                SingleTupleObjectFactoryImpl(builder.Schema);
            ITupleObjectAttributeManager tupleManager;
            bool isRedundant = true;
            foreach (var farg in factoryArgs)
            {
                tupleManager = farg.TupleManager;
                setComponent(tupleManager, tuple, farg.ComponentSource);

                if (AttributeComponentStopsBuilding<TEntity>(tupleManager, tuple))
                    return ReduceSingleTupleObjectToFictional(builder, true);
                isRedundant &= AttributeComponentIsDefault<TEntity>(tupleManager, tuple);
            }
            if (isRedundant)
                return ReduceSingleTupleObjectToFictional(builder, false);

            /*
             * Для тех включённых атрибутов, которые не были явно проинициализированы 
             * компонентами, происходит инициализация фиктивными компонентами 
             * по умолчанию.
             */
            ITupleObjectSchemaProvider schema = builder.Schema;
            IEnumerable<AttributeName> pluggedAttributes =
                schema.PluggedAttributeNames;
            HashSet<AttributeName> instantiatedAttributes =
                factoryArgs.Select(a => a.Name).ToHashSet();
            foreach (AttributeName attrName
                 in pluggedAttributes.Where((pa) => !instantiatedAttributes.Contains(pa)))
            {
                tupleManager = builder.Attribute(attrName).CreateManager();
                SetDefaultComponent(tupleManager, tuple);
            }
            instantiatedAttributes.Clear();

            return (tuple as TupleObject<TEntity>);
        }

        protected TupleObject<TEntity> CreateSingleTupleObject<
            TEntity,
            TComponentSource>(
            IEnumerable<IndexedComponentFactoryArgs<TComponentSource>> factoryArgs,
            SetComponentHandler<TComponentSource> setComponent,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            IndexedComponentFactoryArgs<TComponentSource> farg;
            var fargsEnumerator = factoryArgs.GetEnumerator();
            bool proceedInitPrepared = fargsEnumerator.MoveNext();
            farg = fargsEnumerator.Current;

            TSingleTupleObject tuple =
                SingleTupleObjectFactoryImpl(builder.Schema);
            ITupleObjectAttributeManager tupleManager;
            bool isRedundant = true;
            for (int i = 0; i < builder.Schema.PluggedAttributesCount; i++)
            {
                /*
                 * Инициализация компонентами тех атрибутов, которые были явно названы.
                 */
                if (proceedInitPrepared && i == farg.Index)
                {
                    tupleManager = farg.TupleManager;

                    if (farg.IsDefault)
                    {
                        SetDefaultComponent(tupleManager, tuple);
                    }
                    else
                    {
                        setComponent(tupleManager, tuple, farg.ComponentSource);
                        if (AttributeComponentStopsBuilding<TEntity>(
                            tupleManager, tuple))
                        {
                            fargsEnumerator.Dispose();
                            return ReduceSingleTupleObjectToFictional(builder, true);
                        }
                        isRedundant &= AttributeComponentIsDefault<TEntity>(
                            tupleManager, tuple);
                    }

                    proceedInitPrepared = fargsEnumerator.MoveNext();
                    if (proceedInitPrepared)
                        farg = fargsEnumerator.Current;
                }
                /*
                 * Для тех включённых атрибутов, которые не были явно проинициализированы 
                 * компонентами, происходит инициализация фиктивными компонентами 
                 * по умолчанию.
                 */
                else
                {
                    tupleManager = builder.AttributeAt(i).CreateManager();
                    SetDefaultComponent(tupleManager, tuple);
                }
            }

            fargsEnumerator.Dispose();

            if (isRedundant)
                return ReduceSingleTupleObjectToFictional(builder, false);

            return (tuple as TupleObject<TEntity>);
        }

        protected TupleObject<TEntity> CreateSingleTupleObjectWithTupleBuilding<
            TEntity,
            TComponentSource>(
            IEnumerable<NamedComponentFactoryArgs<TComponentSource>> factoryArgs,
            SetComponentHandler<TComponentSource> setComponent,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            BuildTuple(ref builder, onTupleBuilding);

            return CreateSingleTupleObject(
                factoryArgs,
                setComponent,
                builder);
        }

        protected TupleObject<TEntity> CreateSingleTupleObjectWithTupleBuilding<
            TEntity,
            TComponentSource>(
            IEnumerable<IndexedComponentFactoryArgs<TComponentSource>> factoryArgs,
            SetComponentHandler<TComponentSource> setComponent,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            BuildTuple(ref builder, onTupleBuilding);

            return CreateSingleTupleObject(
                factoryArgs,
                setComponent,
                builder);
        }



        /*
         * Попытка в асинхронные методы создания кортежей
         */

        protected TupleObject<TEntity> CreateSingleTupleObjectAsync<
            TEntity,
            TComponentSource>(
            IEnumerable<IndexedComponentFactoryArgs<TComponentSource>> factoryArgs,
            SetComponentHandler<TComponentSource> setComponent,
            TupleObjectBuilder<TEntity> builder,
            CancellationToken cancellationToken)
            where TEntity : new()
        {
            cancellationToken.ThrowIfCancellationRequested();

            IndexedComponentFactoryArgs<TComponentSource> farg;
            using var fargsEnumerator = factoryArgs.GetEnumerator();
            bool proceedInitPrepared = fargsEnumerator.MoveNext();
            farg = fargsEnumerator.Current;

            TSingleTupleObject tuple =
                SingleTupleObjectFactoryImpl(builder.Schema);
            ITupleObjectAttributeManager tupleManager;
            bool isRedundant = true;
            for (int i = 0; i < builder.Schema.PluggedAttributesCount; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                /*
                 * Инициализация компонентами тех атрибутов, которые были явно названы.
                 */
                if (proceedInitPrepared && i == farg.Index)
                {
                    tupleManager = farg.TupleManager;
                    setComponent(tupleManager, tuple, farg.ComponentSource);
                    if (AttributeComponentStopsBuilding<TEntity>(tupleManager, tuple))
                    {
                        fargsEnumerator.Dispose();
                        return ReduceSingleTupleObjectToFictional(builder, true);
                    }
                    isRedundant &= AttributeComponentIsDefault<TEntity>(
                        tupleManager, tuple);

                    proceedInitPrepared = fargsEnumerator.MoveNext();
                    if (proceedInitPrepared)
                        farg = fargsEnumerator.Current;
                }
                /*
                 * Для тех включённых атрибутов, которые не были явно проинициализированы 
                 * компонентами, происходит инициализация фиктивными компонентами 
                 * по умолчанию.
                 */
                else
                {
                    tupleManager = builder.AttributeAt(i).CreateManager();
                    SetDefaultComponent(tupleManager, tuple);
                }
            }

            fargsEnumerator.Dispose();

            if (isRedundant)
                return ReduceSingleTupleObjectToFictional(builder, false);

            return (tuple as TupleObject<TEntity>);
        }
    }
}
