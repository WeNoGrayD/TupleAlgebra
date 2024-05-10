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

    public readonly struct IndexedComponentFactoryArgs<TComponentSource>
    {
        public int Index { get; init; } = -1;

        public ITupleObjectAttributeManager TupleManager { get; init; }

        public TComponentSource ComponentSource { get; init; } = default;

        public IndexedComponentFactoryArgs(
            ITupleObjectAttributeManager tupleManager) 
        {
            TupleManager = tupleManager;

            return;
        }

        private IndexedComponentFactoryArgs(
            int index,
            TComponentSource componentSource)
        {
            Index = index;
            ComponentSource = componentSource;

            return;
        }

        public IndexedComponentFactoryArgs(
            int index,
            TupleObjectBuilder builder,
            TComponentSource componentSource)
            : this(index, componentSource)
        {
            TupleManager = builder.AttributeAt(index).CreateManager();

            return;
        }

        public IndexedComponentFactoryArgs(
            int index,
            ITupleObjectAttributeManager tupleManager,
            TComponentSource componentSource)
            : this(index, componentSource)
        {
            TupleManager = tupleManager;

            return;
        }
    }

    public readonly struct NamedComponentFactoryArgs<TComponentSource>
    {
        public AttributeName Name { get; init; }

        public ITupleObjectAttributeManager TupleManager { get; init; }

        public TComponentSource ComponentSource { get; init; } = default;

        public NamedComponentFactoryArgs() { }

        private NamedComponentFactoryArgs(
            AttributeName name,
            TComponentSource componentSource)
        {
            Name = name;
            ComponentSource = componentSource;

            return;
        }

        public NamedComponentFactoryArgs(
            AttributeName name,
            TupleObjectBuilder builder,
            TComponentSource componentSource)
            : this (name, componentSource)
        {
            TupleManager = builder.Attribute(name).CreateManager();

            return;
        }

        public NamedComponentFactoryArgs(
            AttributeName name,
            ITupleObjectAttributeManager tupleManager,
            TComponentSource componentSource)
            : this(name, componentSource)
        {
            TupleManager = tupleManager;

            return;
        }

        /*
        public static implicit operator (AttributeName Name, TComponentSource ComponentSource)(
             NamedComponentFactoryArgs<TComponentSource> factoryArgs)
        {
            return (factoryArgs.Name, factoryArgs.ComponentSource);
        }
        */
    }

    public interface ISingleTupleObjectFactoryArgs
    {
        LambdaExpression Getter { get; }

        AttributeName AttributeName { get => Getter; }

        IAttributeComponentFactoryArgs ComponentFactoryArgs { get; }

        /*
        public (AttributeName Name, IAttributeComponentFactoryArgs ComponentFactoryArgs)
            ToTuple() => (Getter, ComponentFactoryArgs);
        
        */

        public NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>
            ToNamedComponentFactoryArgs<TEntity>(
                TupleObjectBuilder<TEntity> builder)
        {
            return new (AttributeName, builder, ComponentFactoryArgs);
        }
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

        private ConjunctiveTupleSystemFactory _csysFactory;

        private DisjunctiveTupleFactory _dFactory = new DisjunctiveTupleFactory();

        private DisjunctiveTupleSystemFactory _dsysFactory;

        public TupleObjectFactory(TAContext context)
        {
            _context = context;
            _csysFactory = new ConjunctiveTupleSystemFactory(_cFactory, _dFactory);
            _dsysFactory = new DisjunctiveTupleSystemFactory(_dFactory, _cFactory);

            return;
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

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            TEntity entity)
            where TEntity : new()
        {
            return _cFactory.CreateConjunctive(entity);
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
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return _cFactory.CreateConjunctive(
                factoryArgs,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            ISingleTupleObjectFactoryArgs[] factoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return _cFactory.CreateConjunctive(
                factoryArgs,
                onTupleBuilding,
                builder);
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

        public TupleObject<TEntity> CreateConjunctiveSystem<TEntity>(
            IndexedComponentFactoryArgs<IAttributeComponent>[][] factoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return null;
        }
        */

        public TupleObject<TEntity> CreateDiagonalConjunctiveSystem<TEntity>(
            DisjunctiveTuple<TEntity> tuple)
            where TEntity : new()
        {
            return _csysFactory.ToAlternateDiagonal(tuple);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IEnumerable<NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>> attributes,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return _dFactory.CreateDisjunctive(
                attributes,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IEnumerable<NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>> attributes,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return _dFactory.CreateDisjunctive(
                attributes,
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IEnumerable<NamedComponentFactoryArgs<IAttributeComponent>> attributes,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return _dFactory.CreateDisjunctive(
                attributes,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IEnumerable<NamedComponentFactoryArgs<IAttributeComponent>> attributes,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return _dFactory.CreateDisjunctive(
                attributes,
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            params NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>[] attributes)
            where TEntity : new()
        {
            return _dFactory.CreateDisjunctive(
                builder,
                attributes);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null,
            TupleObjectBuilder<TEntity> builder = null,
            params NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>[] attributes)
            where TEntity : new()
        {
            return _dFactory.CreateDisjunctive(
                onTupleBuilding,
                builder,
                attributes);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            params NamedComponentFactoryArgs<IAttributeComponent>[] attributes)
            where TEntity : new()
        {
            return _dFactory.CreateDisjunctive(
                builder,
                attributes);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null,
            TupleObjectBuilder<TEntity> builder = null,
            params NamedComponentFactoryArgs<IAttributeComponent>[] attributes)
            where TEntity : new()
        {
            return _dFactory.CreateDisjunctive(
                onTupleBuilding,
                builder,
                attributes);
        }

        public TupleObject<TEntity> CreateDisjunctiveSystem<TEntity>(
            IndexedComponentFactoryArgs<IAttributeComponent>[][] factoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            where TEntity : new()
        {
            return _dsysFactory.CreateDisjunctiveTupleSystem(
                factoryArgs,
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateDisjunctiveSystem<TEntity>(
            TEntity entity)
            where TEntity : new()
        {
            return _dsysFactory.CreateDisjunctiveTupleSystem(entity);
        }

        public TupleObject<TEntity> CreateDiagonalDisjunctiveSystem<TEntity>(
            ConjunctiveTuple<TEntity> tuple)
            where TEntity : new()
        {
            return _dsysFactory.ToAlternateDiagonal(tuple);
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
}
