using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using static TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectHelper;

namespace TupleAlgebraClassLib.TupleObjectFactoryInfrastructure
{
    internal abstract class QueriedTupleObjectSystemFactory<
        TSingleTupleObjectFactory,
        TAltSingleTupleObjectFactory>
        : TupleObjectSystemFactory<
            Expression,
            IQueriedSingleTupleObject,
            TSingleTupleObjectFactory,
            TAltSingleTupleObjectFactory>
        where TSingleTupleObjectFactory : AbstractTupleObjectFactory
    {
        public QueriedTupleObjectSystemFactory(
            TupleObjectFactory factory,
            TSingleTupleObjectFactory singleTupleObjectFactory,
            TAltSingleTupleObjectFactory altSingleTupleObjectFactory)
            : base(
                  factory,
                  singleTupleObjectFactory,
                  altSingleTupleObjectFactory)
        { }

        protected abstract TupleObject<TEntity>
            SingleTupleObjectFactoryImpl<TEntity>(
                IEnumerable<NamedComponentFactoryArgs<Expression>> tupleFactoryArgs,
                TupleObjectBuilder<TEntity> builder)
            where TEntity : new();

        protected override bool SingleTupleObjectIsRedundant<TEntity>(
            TupleObject<TEntity> tuple)
        {
            return false;
        }

        protected override bool SingleTupleObjectStopsBuilding<TEntity>(
            TupleObject<TEntity> tuple)
        {
            return false;
        }
    }

    internal class QueriedConjunctiveTupleSystemFactory
        : QueriedTupleObjectSystemFactory<
            QueriedConjunctiveTupleFactory,
            QueriedDisjunctiveTupleFactory>
    {
        public QueriedConjunctiveTupleSystemFactory(
            TupleObjectFactory factory,
            QueriedConjunctiveTupleFactory singleTupleObjectFactory,
            QueriedDisjunctiveTupleFactory altSingleTupleObjectFactory)
            : base(
                  factory,
                  singleTupleObjectFactory,
                  altSingleTupleObjectFactory)
        { }

        protected override TupleObject<TEntity>
            TupleObjectSystemFactoryImpl<TEntity>(
                TupleObjectSchema<TEntity> schema,
                int len)
        {
            return new QueriedConjunctiveTupleSystem<TEntity>(schema, len);
        }

        protected override TupleObject<TEntity>
            TupleObjectSystemFactoryImpl<TEntity>(
                TupleObjectSchema<TEntity> schema,
                IList<IQueriedSingleTupleObject> tuples)
        {
            return new QueriedConjunctiveTupleSystem<TEntity>(schema, tuples);
        }

        protected override TupleObject<TEntity>
            SingleTupleObjectFactoryImpl<TEntity>(
                IEnumerable<NamedComponentFactoryArgs<Expression>> tupleFactoryArgs,
                TupleObjectBuilder<TEntity> builder)
        {
            return _singleTupleObjectFactory
                .CreateQueriedConjunctiveTuple(tupleFactoryArgs, builder);
        }

        protected override TupleObject<TEntity>
            ReduceTupleObjectSystemToFictional<TEntity>(
                TupleObjectBuilder<TEntity> builder)
        {
            return CreateFull<TEntity>();
        }

        public QueriedTupleObject<TEntity> CreateQueriedConjunctiveTupleSystem<TEntity>(
            ISquareEnumerable<NamedComponentFactoryArgs<Expression>> tupleFactoryArgs,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return CreateTupleObjectSystemStrategy<
                TEntity,
                QueriedConjunctiveTuple<TEntity>,
                IEnumerable<NamedComponentFactoryArgs<Expression>>>(
                tupleFactoryArgs,
                SingleTupleObjectFactoryImpl<TEntity>,
                null,
                builder)
                as QueriedTupleObject<TEntity>;
        }

        public QueriedTupleObject<TEntity> CreateQueriedConjunctiveTupleSystem<TEntity>(
            ISquareEnumerable<NamedComponentFactoryArgs<Expression>> tupleFactoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return CreateTupleObjectSystemStrategy<
                TEntity,
                QueriedConjunctiveTuple<TEntity>,
                IEnumerable<NamedComponentFactoryArgs<Expression>>>(
                tupleFactoryArgs,
                SingleTupleObjectFactoryImpl<TEntity>,
                onTupleBuilding,
                builder)
                as QueriedTupleObject<TEntity>;
        }
    }

    internal class QueriedDisjunctiveTupleSystemFactory
        : QueriedTupleObjectSystemFactory<
            QueriedDisjunctiveTupleFactory,
            QueriedConjunctiveTupleFactory>
    {
        public QueriedDisjunctiveTupleSystemFactory(
            TupleObjectFactory factory,
            QueriedDisjunctiveTupleFactory singleTupleObjectFactory,
            QueriedConjunctiveTupleFactory altSingleTupleObjectFactory)
            : base(
                  factory,
                  singleTupleObjectFactory,
                  altSingleTupleObjectFactory)
        { }

        protected override TupleObject<TEntity>
            TupleObjectSystemFactoryImpl<TEntity>(
                TupleObjectSchema<TEntity> schema,
                int len)
        {
            return new QueriedConjunctiveTupleSystem<TEntity>(schema, len);
        }

        protected override TupleObject<TEntity>
            TupleObjectSystemFactoryImpl<TEntity>(
                TupleObjectSchema<TEntity> schema,
                IList<IQueriedSingleTupleObject> tuples)
        {
            return new QueriedConjunctiveTupleSystem<TEntity>(schema, tuples);
        }

        protected override TupleObject<TEntity>
            SingleTupleObjectFactoryImpl<TEntity>(
                IEnumerable<NamedComponentFactoryArgs<Expression>> tupleFactoryArgs,
                TupleObjectBuilder<TEntity> builder)
        {
            return _singleTupleObjectFactory
                .CreateQueriedDisjunctiveTuple(tupleFactoryArgs, builder);
        }

        protected override TupleObject<TEntity>
            ReduceTupleObjectSystemToFictional<TEntity>(
                TupleObjectBuilder<TEntity> builder)
        {
            return CreateEmpty<TEntity>();
        }

        public QueriedTupleObject<TEntity> CreateQueriedDisjunctiveTupleSystem<TEntity>(
            ISquareEnumerable<NamedComponentFactoryArgs<Expression>> tupleFactoryArgs,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return CreateTupleObjectSystemStrategy<
                TEntity,
                QueriedDisjunctiveTuple<TEntity>,
                IEnumerable<NamedComponentFactoryArgs<Expression>>>(
                tupleFactoryArgs,
                SingleTupleObjectFactoryImpl<TEntity>,
                null,
                builder)
                as QueriedTupleObject<TEntity>;
        }

        public QueriedTupleObject<TEntity> CreateQueriedDisjunctiveTupleSystem<TEntity>(
            ISquareEnumerable<NamedComponentFactoryArgs<Expression>> tupleFactoryArgs,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
            where TEntity : new()
        {
            return CreateTupleObjectSystemStrategy<
                TEntity,
                QueriedDisjunctiveTuple<TEntity>,
                IEnumerable<NamedComponentFactoryArgs<Expression>>>(
                tupleFactoryArgs,
                SingleTupleObjectFactoryImpl<TEntity>,
                onTupleBuilding,
                builder)
                as QueriedTupleObject<TEntity>;
        }
    }
}
