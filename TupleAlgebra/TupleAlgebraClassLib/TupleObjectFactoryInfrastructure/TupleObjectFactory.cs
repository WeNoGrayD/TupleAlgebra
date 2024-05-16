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

    public class TupleObjectFactory
        : AbstractTupleObjectFactory
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
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> attributes,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return _cFactory.CreateConjunctive(
                attributes,
                onTupleBuilding,
                builder);
        }

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

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            ISquareEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> tuples,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return _csysFactory.CreateConjunctiveTupleSystem(
                tuples,
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

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            IEnumerable<TupleObject<TEntity>> tupleSysFactoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return _csysFactory.CreateConjunctiveTupleSystem(
                tupleSysFactoryArgs,
                onTupleBuilding,
                builder);
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
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> attributes,
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

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IndexedComponentFactoryArgs<IAttributeComponent>[][] factoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            where TEntity : new()
        {
            return _dsysFactory.CreateDisjunctiveTupleSystem(
                factoryArgs,
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
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

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            ISquareEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> tuples,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return _dsysFactory.CreateDisjunctiveTupleSystem(
                tuples,
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IEnumerable<TupleObject<TEntity>> tupleSysFactoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return _dsysFactory.CreateDisjunctiveTupleSystem(
                tupleSysFactoryArgs,
                onTupleBuilding,
                builder);
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
