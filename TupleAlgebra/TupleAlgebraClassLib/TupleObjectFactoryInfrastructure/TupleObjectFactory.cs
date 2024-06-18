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

        private ConjunctiveTupleFactory _cFactory;

        private ConjunctiveTupleSystemFactory _csysFactory;

        private DisjunctiveTupleFactory _dFactory;

        private DisjunctiveTupleSystemFactory _dsysFactory;

        private QueriedConjunctiveTupleFactory _qcFactory;

        private QueriedDisjunctiveTupleFactory _qdFactory;

        private QueriedConjunctiveTupleSystemFactory _qcSysFactory;

        private QueriedDisjunctiveTupleSystemFactory _qdSysFactory;

        protected override TupleObjectFactory _factory { get => this; }

        public TupleObjectFactory(TAContext context)
        {
            _context = context;
            _cFactory = new ConjunctiveTupleFactory(this);
            _dFactory = new DisjunctiveTupleFactory(this);
            _csysFactory = new ConjunctiveTupleSystemFactory(this, _cFactory, _dFactory);
            _dsysFactory = new DisjunctiveTupleSystemFactory(this, _dFactory, _cFactory);

            _qcFactory = new QueriedConjunctiveTupleFactory(this);
            _qdFactory = new QueriedDisjunctiveTupleFactory(this);
            _qcSysFactory = new QueriedConjunctiveTupleSystemFactory(this, _qcFactory, _qdFactory);
            _qdSysFactory = new QueriedDisjunctiveTupleSystemFactory(this, _qdFactory, _qcFactory);

            return;
        }

        internal QueriedTupleObject<TEntity> CreateQueried<TEntity>(
            Expression queryExpression,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
        {
            return new QueriedTupleObject<TEntity>(queryExpression, onTupleBuilding);
        }

        internal QueriedTupleObject<TEntity> CreateQueried<TEntity>(
            Expression queryExpression,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return new QueriedTupleObject<TEntity>(queryExpression, builder);
        }

        internal QueriedTupleObject<TEntity> CreateQueriedComplexTupleObject<TEntity>(
            Expression queryExpression,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
        {
            return new QueriedComplexTupleObject<TEntity>(queryExpression, onTupleBuilding);
        }

        internal QueriedTupleObject<TEntity> CreateQueriedComplexTupleObject<TEntity>(
            Expression queryExpression,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return new QueriedComplexTupleObject<TEntity>(queryExpression, builder);
        }

        internal QueriedTupleObject<TEntity> CreateQueriedSingleTupleObject<TEntity>(
            QueriedTupleType structureType,
            Expression[] componentQueryExpressions,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
        {
            return structureType switch
            {
                QueriedTupleType.C =>
                    new QueriedConjunctiveTuple<TEntity>(
                        componentQueryExpressions, onTupleBuilding),
                QueriedTupleType.D =>
                    new QueriedDisjunctiveTuple<TEntity>(
                        componentQueryExpressions, onTupleBuilding),
                 _ => throw new ArgumentException()
            };
        }

        internal QueriedTupleObject<TEntity> CreateQueriedSingleTupleObject<TEntity>(
            QueriedTupleType structureType,
            IEnumerable<NamedComponentFactoryArgs<Expression>> 
                componentQueryExpressions,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return structureType switch
            {
                QueriedTupleType.C => _qcFactory.CreateQueriedConjunctiveTuple(
                    componentQueryExpressions,
                    builder),
                QueriedTupleType.D => _qdFactory.CreateQueriedDisjunctiveTuple(
                    componentQueryExpressions,
                    builder),
                _ => throw new ArgumentException()
            };
        }

        internal QueriedTupleObject<TEntity> CreateQueriedTupleObjectSystem<TEntity>(
            QueriedTupleType structureType,
            ISquareEnumerable<NamedComponentFactoryArgs<Expression>> tupleSysFactoryArgs,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return structureType switch
            {
                QueriedTupleType.C =>
                    _qcSysFactory.CreateQueriedConjunctiveTupleSystem(
                        tupleSysFactoryArgs,
                        builder),
                QueriedTupleType.D =>
                    _qdSysFactory.CreateQueriedDisjunctiveTupleSystem(
                        tupleSysFactoryArgs,
                        builder),
                _ => throw new ArgumentException()
            };
        }

        public TupleObject<TEntity> CreateEmpty<TEntity>()
            where TEntity : new()
        {
            //TupleObject<TEntity> empty = null;
            //SubscribeOnContextDisposing(empty);

            return null;
        }

        public override TupleObject<TEntity> CreateEmpty<TEntity>(
            TupleObjectBuilder<TEntity> builder = null)
        {
            return new EmptyTupleObject<TEntity>(
                builder.Schema);
        }

        public override TupleObject<TEntity> CreateEmpty<TEntity>(
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
        {
            return new EmptyTupleObject<TEntity>(
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateConjunctiveTuple<TEntity>(
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

        public TupleObject<TEntity> CreateConjunctiveTuple<TEntity>(
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

        public TupleObject<TEntity> CreateConjunctiveTuple<TEntity>(
            ISingleTupleObjectFactoryArgs[] factoryArgs,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return _cFactory.CreateConjunctive(
                factoryArgs,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctiveTuple<TEntity>(
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

        public TupleObject<TEntity> CreateConjunctiveTupleSystem<TEntity>(
            ISingleTupleObjectFactoryArgs[][] tupleSysFactoryArgs,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return _csysFactory.CreateConjunctiveTupleSystem(
                tupleSysFactoryArgs,
                builder);
        }

        public TupleObject<TEntity> CreateConjunctiveTupleSystem<TEntity>(
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

        public TupleObject<TEntity> CreateDiagonalConjunctiveTupleSystem<TEntity>(
            DisjunctiveTuple<TEntity> tuple)
            where TEntity : new()
        {
            return _csysFactory.ToAlternateDiagonal(tuple);
        }

        public TupleObject<TEntity> CreateDiagonalConjunctiveTupleSystem<TEntity>(
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>>
            tupleSysFactoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder,
            bool makeOrthogonal)
            where TEntity : new()
        {
            return _csysFactory.CreateDiagonalConjunctiveTupleSystem(
                tupleSysFactoryArgs,
                onTupleBuilding,
                builder,
                makeOrthogonal);
        }

        public TupleObject<TEntity> CreateConjunctiveTupleSystem<TEntity>(
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

        public TupleObject<TEntity> CreateConjunctiveTupleSystem<TEntity>(
            IEnumerable<TEntity> tupleSysFactoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return _csysFactory.CreateConjunctiveTupleSystem(
                tupleSysFactoryArgs,
                onTupleBuilding,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctiveTuple<TEntity>(
            IEnumerable<NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>> attributes,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return _dFactory.CreateDisjunctive(
                attributes,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctiveTupleSystem<TEntity>(
            ISingleTupleObjectFactoryArgs[][] tupleSysFactoryArgs,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return _dsysFactory.CreateDisjunctiveTupleSystem(
                tupleSysFactoryArgs,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctiveTuple<TEntity>(
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

        public TupleObject<TEntity> CreateDisjunctiveTuple<TEntity>(
            IEnumerable<NamedComponentFactoryArgs<IAttributeComponent>> attributes,
            TupleObjectBuilder<TEntity> builder = null)
            where TEntity : new()
        {
            return _dFactory.CreateDisjunctive(
                attributes,
                builder);
        }

        public TupleObject<TEntity> CreateDisjunctiveTuple<TEntity>(
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

        public TupleObject<TEntity> CreateDisjunctiveTuple<TEntity>(
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

        public TupleObject<TEntity> CreateDisjunctiveTuple<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            params NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>[] attributes)
            where TEntity : new()
        {
            return _dFactory.CreateDisjunctive(
                builder,
                attributes);
        }

        public TupleObject<TEntity> CreateDisjunctiveTuple<TEntity>(
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

        public TupleObject<TEntity> CreateDisjunctiveTuple<TEntity>(
            TupleObjectBuilder<TEntity> builder = null,
            params NamedComponentFactoryArgs<IAttributeComponent>[] attributes)
            where TEntity : new()
        {
            return _dFactory.CreateDisjunctive(
                builder,
                attributes);
        }

        public TupleObject<TEntity> CreateDisjunctiveTuple<TEntity>(
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

        public TupleObject<TEntity> CreateDisjunctiveTupleSystem<TEntity>(
            IndexedComponentFactoryArgs<IAttributeComponent>[][] factoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            where TEntity : new()
        {
            return _dsysFactory.CreateDisjunctiveTupleSystem(
                factoryArgs,
                onTupleBuilding);
        }

        public TupleObject<TEntity> CreateDisjunctiveTupleSystem<TEntity>(
            TEntity entity)
            where TEntity : new()
        {
            return _dsysFactory.CreateDiagonalDisjunctiveTupleSystem(entity);
        }

        public TupleObject<TEntity> CreateDiagonalDisjunctiveTupleSystem<TEntity>(
            ConjunctiveTuple<TEntity> tuple)
            where TEntity : new()
        {
            return _dsysFactory.ToAlternateDiagonal(tuple);
        }

        public TupleObject<TEntity> CreateDisjunctiveTupleSystem<TEntity>(
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

        public TupleObject<TEntity> CreateDisjunctiveTupleSystem<TEntity>(
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

        public override TupleObject<TEntity> CreateFull<TEntity>(
            TupleObjectBuilder<TEntity> builder = null)
        {
            return new FullTupleObject<TEntity>(
                builder.Schema,
                _cFactory.CreateFullConjunctive(builder));
        }

        public override TupleObject<TEntity> CreateFull<TEntity>(
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
        {
            return new FullTupleObject<TEntity>(
                onTupleBuilding,
                _cFactory.CreateFullConjunctive(GetBuilder<TEntity>(onTupleBuilding)));
        }

        public TupleObject<TEntity> CreateFull<TEntity>()
            where TEntity : new()
        {
            //TupleObject<TEntity> full= null;
            //SubscribeOnContextDisposing(full);

            return CreateFull<TEntity>(onTupleBuilding: null);
        }

        protected void SubscribeOnContextDisposing(IDisposable tupleObject)
        {
            if (tupleObject is not null) _context.Disposing += () => tupleObject.Dispose();

            return;
        }
    }
}
