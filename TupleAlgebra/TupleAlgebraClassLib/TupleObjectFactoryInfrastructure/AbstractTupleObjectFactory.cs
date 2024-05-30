using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectFactoryInfrastructure
{
    using static TupleObjectHelper;

    public abstract class AbstractTupleObjectFactory
    {
        protected virtual TupleObjectFactory _factory { get; set; }

        public delegate ITupleObjectAttributeManager
            SetComponentHandler<TComponentSource>(
            ITupleObjectAttributeManager tupleManager,
            ISingleTupleObject tuple,
            TComponentSource componentSource);

        public delegate ITupleObjectAttributeManager
            SetComponentWithComplementOpportunityHandler<TComponentSource>(
            ITupleObjectAttributeManager tupleManager,
            ISingleTupleObject tuple,
            TComponentSource componentSource,
            bool toComplement);

        protected void BuildTuple<TEntity>(
            ref TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
        {
            /*
             * При непередаче строителя предполагается, что либо
             * его конфигуратор предпринимает все необходимые изменения,
             * либо конфигуратор пуст, и строитель нужно конфигурировать стандартно.
             * В противном случае конфигуратор принимается каким он есть.
             */
            (builder, onTupleBuilding) = (builder, onTupleBuilding) switch
            {
                (null, _) => (GetBuilder<TEntity>(),
                              onTupleBuilding ??
                              DefaultTupleObjectBuildingHandler<TEntity>),
                _ => (builder, onTupleBuilding)
            };
            onTupleBuilding?.Invoke(builder);
            //builder.EndSchemaInitialization();

            return;
        }

        protected void DefaultTupleObjectBuildingHandler<TEntity>(
            TupleObjectBuilder<TEntity> builder)
        {
            builder.InitDefaultSchema();

            return;
        }

        public TupleObjectBuilder<TEntity> GetBuilder<TEntity>()
        {
            return new TupleObjectBuilder<TEntity>();
        }

        public TupleObjectBuilder<TEntity> GetBuilder<TEntity>(
            TupleObjectSchema<TEntity> schema)
        {
            return new TupleObjectBuilder<TEntity>(schema);
        }

        public TupleObjectBuilder<TEntity> GetBuilder<TEntity>(
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
        {
            TupleObjectBuilder<TEntity> builder = GetBuilder<TEntity>();
            onTupleBuilding(builder);

            return builder;
        }

        public TupleObjectBuilder<TEntity> GetDefaultBuilder<TEntity>()
        {
            TupleObjectBuilder<TEntity> builder = GetBuilder<TEntity>();
            builder.InitDefaultSchema();
            builder.EndSchemaInitialization();

            return builder;
        }

        public virtual TupleObject<TEntity> CreateEmpty<TEntity>(
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return _factory.CreateEmpty(builder);
        }

        public virtual TupleObject<TEntity> CreateEmpty<TEntity>(
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
        {
            return _factory.CreateEmpty(onTupleBuilding);
        }

        public virtual TupleObject<TEntity> CreateFull<TEntity>(
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return _factory.CreateFull(builder);
        }

        public virtual TupleObject<TEntity> CreateFull<TEntity>(
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
        {
            return _factory.CreateFull(onTupleBuilding);
        }

        protected static IEnumerable<IndexedComponentFactoryArgs<TEntity>>
            PassEntity<TEntity>(
                TupleObjectBuilder<TEntity> builder,
                TEntity entity)
        {
            int i = 0;
            foreach (var attrName in builder.Schema.PluggedAttributeNames)
                yield return new (i++, builder, entity);

            yield break;
        }

        /*
        protected static ISquareEnumerable<IndexedComponentFactoryArgs<TEntity>>
            PassEntityD<TEntity>(
                TupleObjectBuilder<TEntity> builder,
                TEntity entity)
        {
            int i = 0;
            foreach (var attrName in builder.Schema.PluggedAttributeNames)
                yield return (i++, entity);

            yield break;
        }
        */

        protected static IEnumerable<IndexedComponentFactoryArgs<IEnumerable<TEntity>>>
            PassHorizontalEntitySet<TEntity>(
                TupleObjectBuilder<TEntity> builder,
                IEnumerable<TEntity> entitySet)
        {
            int i = 0;
            foreach (var attrName in builder.Schema.PluggedAttributeNames)
                yield return new (i++, builder, entitySet);

            yield break;
        }

        /*
        protected static ISquareEnumerable<IndexedComponentFactoryArgs<TEntity>>
            PassVerticalEntitySet<TEntity>(
                TupleObjectBuilder<TEntity> builder,
                IEnumerable<TEntity> entitySet)
        {
            return new SquareEnumerable<IndexedComponentFactoryArgs<TEntity>>(
                entitySet.Select(e => PassEntity(builder, e)));
        }
        */

        protected static IEnumerable<TEntity>
            PassVerticalEntitySet<TEntity>(
                TupleObjectBuilder<TEntity> builder,
                IEnumerable<TEntity> entitySet)
        {
            return entitySet;
        }

        protected static ITupleObjectAttributeManager
            SetComponentWithTrailingComplement(
                ITupleObjectAttributeManager tupleManager,
                ISingleTupleObject tuple,
                IAttributeComponent componentSource)
        {
            return tupleManager.SetComponentWithComplementionAccumulation(
                tuple, componentSource);
        }

        protected static ITupleObjectAttributeManager
            PassMember<TEntity>(
                ITupleObjectAttributeManager tupleManager,
                ISingleTupleObject tuple,
                TEntity componentSource)
        {
            return tupleManager.SetComponentToProjectionOfOntoMember(
                tuple, componentSource, false);
        }

        protected static ITupleObjectAttributeManager
            PassMemberWithTrailingComplement<TEntity>(
                ITupleObjectAttributeManager tupleManager,
                ISingleTupleObject tuple,
                TEntity componentSource)
        {
            return tupleManager.SetComponentToProjectionOfOntoMember(
                tuple, componentSource, true);
        }

        protected static ITupleObjectAttributeManager
            PassMembers<TEntity>(
                ITupleObjectAttributeManager tupleManager,
                ISingleTupleObject tuple,
                IEnumerable<TEntity> componentSource)
        {
            return tupleManager.SetComponentToProjectionOfOntoMember(
                tuple, componentSource);
        }
    }
}
