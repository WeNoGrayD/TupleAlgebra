using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.TupleObjectFactoryInfrastructure
{
    using static TupleObjectHelper;

    public interface ISingleTupleObjectFactoryArgs
    {
        LambdaExpression Getter { get; }

        AttributeName AttributeName { get => Getter; }

        IAttributeComponentFactoryArgs ComponentFactoryArgs { get; }

        public (AttributeName Name, IAttributeComponentFactoryArgs ComponentFactoryArgs)
            ToTuple() => (Getter, ComponentFactoryArgs);
    }

    public record SingleTupleObjectFactoryArgs<TEntity, TAttribute>
        : ISingleTupleObjectFactoryArgs
    {
        private Expression<AttributeGetterHandler<TEntity, TAttribute>> _getter;

        private NonFictionalAttributeComponentFactoryArgs<TAttribute> _componentFactoryArgs;

        LambdaExpression ISingleTupleObjectFactoryArgs
            .Getter { get => _getter; }

        IAttributeComponentFactoryArgs ISingleTupleObjectFactoryArgs.
            ComponentFactoryArgs { get => _componentFactoryArgs; }

        public SingleTupleObjectFactoryArgs(
            Expression<AttributeGetterHandler<TEntity, TAttribute>> getter,
            NonFictionalAttributeComponentFactoryArgs<TAttribute> componentFactoryArgs)
        {
            _getter = getter;
            _componentFactoryArgs = componentFactoryArgs;

            return;
        }

        public static implicit operator SingleTupleObjectFactoryArgs<TEntity, TAttribute>(
            (Expression<AttributeGetterHandler<TEntity, TAttribute>> Getter,
             NonFictionalAttributeComponentFactoryArgs<TAttribute> ComponentFactoryArgs)
            factoryArgs)
        {
            return new SingleTupleObjectFactoryArgs<TEntity, TAttribute>(
                factoryArgs.Getter, factoryArgs.ComponentFactoryArgs);
        }
    }

    public class TupleObjectFactory
    {
        private TAContext _context;

        private ConjunctiveTupleFactory _cFactory = new ConjunctiveTupleFactory();

        private DisjunctiveTupleFactory _dFactory = new DisjunctiveTupleFactory();

        public TupleObjectFactory(TAContext context)
        {
            _context = context;
        }

        private TupleObjectBuilder<TEntity> GetBuilder<TEntity>()
        { return new TupleObjectBuilder<TEntity>(); }

        public TupleObject<TEntity> CreateEmpty<TEntity>()
            where TEntity : new()
        {
            TupleObject<TEntity> empty = null;
            SubscribeOnContextDisposing(empty);

            return null;
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>()
            where TEntity : new()
        {
            return null;
        }

        /*
        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            IEnumerable<(AttributeName Name, IAttributeComponentFactoryArgs FactoryArgs)> attributes,
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            where TEntity : new()
        {
            return _cFactory.CreateConjunctive(
                attributes,
                builder,
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            IEnumerable<(AttributeName Name, IAttributeComponent Component)> attributes,
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            where TEntity : new()
        {
            return _cFactory.CreateConjunctive(
                attributes,
                builder,
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            (AttributeName Name, IAttributeComponentFactoryArgs FactoryArgs)[] attributes,
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            where TEntity : new()
        {
            return _cFactory.CreateConjunctive(
                builder,
                onTupleBuilding,
                attributes);
        }
        */

        /*
        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            (LambdaExpression Getter, IAttributeComponentFactoryArgs FactoryArgs)[] attributes,
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            where TEntity : new()
        {
            return _cFactory.CreateConjunctive(
                attributes.Select<
                    (LambdaExpression Getter, IAttributeComponentFactoryArgs FactoryArgs),
                    (AttributeName Name, IAttributeComponentFactoryArgs FactoryArgs)> 
                    (a => a),
                builder,
                onTupleBuilding);
        }
        */

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            ISingleTupleObjectFactoryArgs[] factoryArgs,
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            where TEntity : new()
        {
            return _cFactory.CreateConjunctive(
                factoryArgs,
                builder,
                onTupleBuilding);
        }

        /*
        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null,
            params (AttributeName Name, IAttributeComponent Component)[] factoryArgs)
            where TEntity : new()
        {
            return _cFactory.CreateConjunctive(
                builder,
                onTupleBuilding,
                factoryArgs);
        }
        */

        public TupleObject<TEntity> CreateDisjunctive<TEntity>()
            where TEntity : new()
        {
            return null;
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IEnumerable<(AttributeName Name, IAttributeComponentFactoryArgs FactoryArgs)> attributes,
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            where TEntity : new()
        {
            return _dFactory.CreateDisjunctive(
                attributes,
                builder,
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IEnumerable<(AttributeName Name, IAttributeComponent Component)> attributes,
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            where TEntity : new()
        {
            return _dFactory.CreateDisjunctive(
                attributes,
                builder,
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null,
            params (AttributeName Name, IAttributeComponentFactoryArgs FactoryArgs)[] attributes)
            where TEntity : new()
        {
            return _dFactory.CreateDisjunctive(
                builder,
                onTupleBuilding,
                attributes);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null,
            params (AttributeName Name, IAttributeComponent Component)[] attributes)
            where TEntity : new()
        {
            return _dFactory.CreateDisjunctive(
                builder,
                onTupleBuilding,
                attributes);
        }

        public TupleObject<TEntity> CreateFull<TEntity>()
            where TEntity : new()
        {
            TupleObject<TEntity> full= null;
            SubscribeOnContextDisposing(full);

            return null;
        }

        protected void SubscribeOnContextDisposing(IDisposable tupleObject)
        {
            if (tupleObject is not null) _context.Disposing += () => tupleObject.Dispose();

            return;
        }
    }

    public abstract class AbstractTupleObjectFactory
    {
        protected void DefaultTupleObjectBuildingHandler<TEntity>(
            TupleObjectBuilder<TEntity> builder)
        {
            builder.InitDefaultSchema();

            return;
        }

        protected TupleObjectBuilder<TEntity> GetBuilder<TEntity>()
        {
            return new TupleObjectBuilder<TEntity>();
        }

        public TupleObject<TEntity> CreateEmpty<TEntity>(
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return null;
        }

        public TupleObject<TEntity> CreateFull<TEntity>(
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return null;
        }
    }

    public class SingleTupleObjectFactory
        : AbstractTupleObjectFactory
    {
        protected delegate ITupleObjectAttributeSetupWizard SetComponentHandler<TComponentSource>(
            ITupleObjectAttributeSetupWizard setupWizard,
            ISingleTupleObject tuple,
            TComponentSource componentSource);

        protected delegate TTupleObject SingleTupleObjectFactoryHandler<
            TEntity,
            TTupleObject>(
                TupleObjectSchema<TEntity> schema)
            where TEntity : new()
            where TTupleObject : TupleObject<TEntity>, ISingleTupleObject;

        protected TupleObject<TEntity> CreateSingleTupleObject<
            TEntity,
            TTupleObject,
            TComponentSource>(
            SingleTupleObjectFactoryHandler<TEntity, TTupleObject> factory,
            IEnumerable<(AttributeName Name, TComponentSource ComponentSource)> factoryArgs,
            SetComponentHandler<TComponentSource> setComponent,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
            where TTupleObject : TupleObject<TEntity>, ISingleTupleObject
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
                (_, _) => (builder, onTupleBuilding)
            };
            onTupleBuilding += TupleObject<TEntity>.EndSchemaInitialization;
            onTupleBuilding(builder);

            /*
             * Инициализация компонентами тех атрибутов, которые были явно названы.
             */
            TTupleObject tuple = factory(builder.Schema);
            ITupleObjectAttributeSetupWizard setupWizard;
            foreach (var farg in factoryArgs)
            {
                setupWizard = builder.Attribute(farg.Name);
                setComponent(setupWizard, tuple, farg.ComponentSource);
            }

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
                setupWizard = builder.Attribute(attrName);
                setupWizard.SetDefaultFictionalAttributeComponent(tuple);
            }
            instantiatedAttributes.Clear();

            return tuple;
        }
    }

    public class ConjunctiveTupleFactory : SingleTupleObjectFactory
    {
        protected TupleObject<TEntity> CreateConjunctive<TEntity, TComponentSource>(
            IEnumerable<(AttributeName Name, TComponentSource ComponentSource)> factoryArgs,
            SetComponentHandler<TComponentSource> setComponent,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
        {
            return CreateSingleTupleObject(
                (schema) => new ConjunctiveTuple<TEntity>(schema),
                factoryArgs,
                setComponent, 
                builder,
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            IEnumerable<(AttributeName Name, IAttributeComponentFactoryArgs FactoryArgs)> factoryArgs,
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            where TEntity : new()
        {
            return CreateConjunctive(
                factoryArgs,
                (setupWizard, tuple, args) => setupWizard.SetComponent(tuple, args),
                builder,
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            IEnumerable<(AttributeName Name, IAttributeComponent Component)> factoryArgs,
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            where TEntity : new()
        {
            return CreateConjunctive(
                factoryArgs,
                (setupWizard, tuple, ac) => setupWizard.SetComponent(tuple, ac),
                builder,
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            IEnumerable<ISingleTupleObjectFactoryArgs> factoryArgs,
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            where TEntity : new()
        {
            return CreateConjunctive(
                factoryArgs.Select(a => a.ToTuple()),
                builder,
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null,
            params (AttributeName Name, IAttributeComponentFactoryArgs FactoryArgs)[] factoryArgs)
            where TEntity : new()
        {
            return CreateConjunctive(
                factoryArgs,
                builder,
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null,
            params (AttributeName Name, IAttributeComponent Component)[] factoryArgs)
            where TEntity : new()
        {
            return CreateConjunctive(
                factoryArgs,
                builder,
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null,
            params ISingleTupleObjectFactoryArgs[] factoryArgs)
            where TEntity : new()
        {
            return CreateConjunctive(
                factoryArgs, 
                builder,
                onTupleBuilding);
        }
    }

    public class DisjunctiveTupleFactory : SingleTupleObjectFactory
    {
        protected TupleObject<TEntity> CreateDisjunctive<TEntity, TComponentSource>(
            IEnumerable<(AttributeName Name, TComponentSource ComponentSource)> factoryArgs,
            SetComponentHandler<TComponentSource> setComponent,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
        {
            return CreateSingleTupleObject(
                (schema) => new DisjunctiveTuple<TEntity>(schema),
                factoryArgs,
                setComponent,
                builder,
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IEnumerable<(AttributeName Name, IAttributeComponentFactoryArgs FactoryArgs)> factoryArgs,
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            where TEntity : new()
        {
            return CreateDisjunctive(
                factoryArgs,
                (setupWizard, tuple, args) => setupWizard.SetComponent(tuple, args),
                builder,
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IEnumerable<(AttributeName Name, IAttributeComponent Component)> factoryArgs,
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            where TEntity : new()
        {
            return CreateDisjunctive(
                factoryArgs,
                (setupWizard, tuple, ac) => setupWizard.SetComponent(tuple, ac),
                builder,
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IEnumerable<ISingleTupleObjectFactoryArgs> factoryArgs,
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            where TEntity : new()
        {
            return CreateDisjunctive(
                factoryArgs.Select(fa => fa.ToTuple()),
                builder,
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null,
            params (AttributeName Name, IAttributeComponentFactoryArgs FactoryArgs)[] factoryArgs)
            where TEntity : new()
        {
            return CreateDisjunctive(
                factoryArgs,
                builder,
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null,
            params (AttributeName Name, IAttributeComponent Component)[] factoryArgs)
            where TEntity : new()
        {
            return CreateDisjunctive(
                factoryArgs,
                builder,
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null,
            params ISingleTupleObjectFactoryArgs[] factoryArgs)
            where TEntity : new()
        {
            return CreateDisjunctive(
                factoryArgs,
                builder,
                onTupleBuilding);
        }
    }
}
