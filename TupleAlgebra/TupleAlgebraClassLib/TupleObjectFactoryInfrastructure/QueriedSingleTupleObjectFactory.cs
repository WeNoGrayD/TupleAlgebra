using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using static TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectHelper;

namespace TupleAlgebraClassLib.TupleObjectFactoryInfrastructure
{
    internal abstract class QueriedSingleTupleObjectFactory
        : SingleTupleObjectFactory<
            System.Linq.Expressions.Expression,
            IQueriedSingleTupleObject>
    {
        public QueriedSingleTupleObjectFactory(
            TupleObjectFactory factory)
            : base(factory)
        { }

        protected override void SetDefaultComponent(
            ITupleObjectAttributeManager attrManager,
            IQueriedSingleTupleObject tuple)
        {
            attrManager.SetDefaultFictionalAttributeComponent(tuple);

            return;
        }

        protected override bool AttributeComponentStopsBuilding<TEntity>(
            ITupleObjectAttributeManager tupleManager,
            IQueriedSingleTupleObject tuple)
        {
            return false;
        }

        protected override bool AttributeComponentIsDefault<TEntity>(
            ITupleObjectAttributeManager tupleManager,
            IQueriedSingleTupleObject tuple)
        {
            return false;
        }

        protected override TupleObject<TEntity>
            ReduceSingleTupleObjectToFictional<TEntity>(
                TupleObjectBuilder<TEntity> builder,
                bool onStop)
        {
            throw new InvalidOperationException("Кортеж-запрос невозможно привести к фиктивному АК-объекту, и эта операция не должна быть вызвана.");
        }

        protected ITupleObjectAttributeManager SetComponent(
            ITupleObjectAttributeManager attrManager,
            IQueriedSingleTupleObject tuple,
            Expression factoryArgs)
        {
            attrManager.SetComponent(tuple, factoryArgs);

            return attrManager;
        }
    }

    internal class QueriedConjunctiveTupleFactory
        : QueriedSingleTupleObjectFactory
    {
        public QueriedConjunctiveTupleFactory(
            TupleObjectFactory factory)
            : base(factory)
        { }


        protected override IQueriedSingleTupleObject
            SingleTupleObjectFactoryImpl<TEntity>(TupleObjectSchema<TEntity> schema)
        {
            return new QueriedConjunctiveTuple<TEntity>(schema);
        }

        public QueriedTupleObject<TEntity> CreateQueriedConjunctiveTuple<TEntity>(
            IEnumerable<NamedComponentFactoryArgs<Expression>> tupleFactoryArgs,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return CreateSingleTupleObject(
                tupleFactoryArgs,
                SetComponent,
                builder) 
                as QueriedTupleObject<TEntity>;
        }

        public QueriedTupleObject<TEntity> CreateQueriedConjunctiveTuple<TEntity>(
            IEnumerable<NamedComponentFactoryArgs<Expression>> tupleFactoryArgs,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
        {
            return CreateSingleTupleObjectWithTupleBuilding(
                tupleFactoryArgs,
                SetComponent,
                onTupleBuilding,
                builder) 
                as QueriedTupleObject<TEntity>;
        }
    }

    internal class QueriedDisjunctiveTupleFactory
        : QueriedSingleTupleObjectFactory
    {
        public QueriedDisjunctiveTupleFactory(
            TupleObjectFactory factory)
            : base(factory)
        { }

        protected override IQueriedSingleTupleObject
            SingleTupleObjectFactoryImpl<TEntity>(TupleObjectSchema<TEntity> schema)
        {
            return new QueriedDisjunctiveTuple<TEntity>(schema);
        }

        public QueriedTupleObject<TEntity> CreateQueriedDisjunctiveTuple<TEntity>(
            IEnumerable<NamedComponentFactoryArgs<Expression>> tupleFactoryArgs,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return CreateSingleTupleObject(
                tupleFactoryArgs,
                SetComponent,
                builder)
                as QueriedTupleObject<TEntity>;
        }

        public QueriedTupleObject<TEntity> CreateQueriedDisjunctiveTuple<TEntity>(
            IEnumerable<NamedComponentFactoryArgs<Expression>> tupleFactoryArgs,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            where TEntity : new()
        {
            return CreateSingleTupleObjectWithTupleBuilding(
                tupleFactoryArgs,
                SetComponent,
                onTupleBuilding,
                builder)
                as QueriedTupleObject<TEntity>;
        }
    }
}
