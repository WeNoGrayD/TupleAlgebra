using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectAcceptors;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectAcceptors
{
    public abstract class TupleObjectCrossTypeFactoryBinaryAcceptor<TEntity, TOperand1, TOperationResult>
        : TupleObjectFactoryBinaryAcceptor<TEntity, TOperand1, TOperationResult>,
          //ITupleObjectFactoryBinaryAcceptor<TEntity, TOperand1, EmptyTupleObject<TEntity>, TOperationResult>,
          //ITupleObjectFactoryBinaryAcceptor<TEntity, TOperand1, TupleObject<TEntity>, TOperationResult>,
          //ITupleObjectFactoryBinaryAcceptor<TEntity, TOperand1, FullTupleObject<TEntity>, TOperationResult>
          ITupleObjectCrossTypeFactoryBinaryAcceptor<TEntity, TOperand1, TOperationResult>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
    {

        public abstract TOperationResult Accept(
            TOperand1 first,
            EmptyTupleObject<TEntity> second,
            TupleObjectFactory factory);

        public abstract TOperationResult Accept(
            TOperand1 first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory);

        public abstract TOperationResult Accept(
            TOperand1 first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory);

        public abstract TOperationResult Accept(
            TOperand1 first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory);

        public abstract TOperationResult Accept(
            TOperand1 first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory);

        public abstract TOperationResult Accept(
            TOperand1 first,
            FullTupleObject<TEntity> second,
            TupleObjectFactory factory);
    }

    public abstract class TupleObjectCrossTypeFactorySetBinaryOperator<TEntity, TOperand1>
        : TupleObjectCrossTypeFactoryBinaryAcceptor<TEntity, TOperand1, TupleObject<TEntity>>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
    {
        protected delegate TupleObject<TEntity> OperationHandler<TOperand2>(
            TOperand1 first,
            TOperand2 second,
            TupleObjectFactory factory)
            where TOperand2 : TupleObject<TEntity>;

        protected TupleObject<TEntity> OperationStrategy<TOperand2>(
            TOperand1 first,
            TOperand2 second,
            TupleObjectFactory factory,
            OperationHandler<TOperand2> operation)
            where TOperand2 : TupleObject<TEntity>
        {
            TupleObjectSchema<TEntity> genSchema =
                first.Schema.GeneralizeWith(second.Schema);
            TupleObjectBuilder<TEntity> builder =
                factory.GetBuilder(genSchema);

            first = (first.AlignWithSchema(genSchema, factory, builder) as TOperand1)!;
            second = (second.AlignWithSchema(genSchema, factory, builder) as TOperand2)!;

            return operation(first, second, factory);
        }
    }

    /*
    public abstract class TupleObjectCrossTypeFactoryBinaryAcceptor<TEntity, TOperand1, TOperationResult>
        : TupleObjectFactoryBinaryAcceptor<TEntity, TOperand1, TOperationResult>,
          ITupleObjectFactoryBinaryAcceptor<TEntity, TOperand1, EmptyTupleObject<TEntity>, TOperationResult>,
          //ITupleObjectInstantBinaryAcceptor<TEntity, TOperand1, TupleObject<TEntity>, TOperationResult>,
          ITupleObjectFactoryBinaryAcceptor<TEntity, TOperand1, FullTupleObject<TEntity>, TOperationResult>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
    {

        public abstract TOperationResult Accept(
            TOperand1 first, 
            EmptyTupleObject<TEntity> second,
            TupleObjectFactory factory);

        TOperationResult IInstantBinaryOperator<TOperand1, TupleObject<TEntity>, TOperationResult>.Accept(TOperand1 first, TupleObject<TEntity> second)
        {
            return AcceptDefault(first, second);
        }

        protected abstract TOperationResult AcceptDefault(TOperand1 first, TupleObject<TEntity> second);
        

        public abstract TOperationResult Accept(
            TOperand1 first,
            FullTupleObject<TEntity> second,
            TupleObjectFactory factory);
    }
    */
}
