using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors
{
    public abstract class TupleObjectCrossTypeFactoryBinaryVisitor<TEntity, TOperand1, TOperationResult>
        : TupleObjectFactoryBinaryVisitor<TEntity, TOperand1, TOperationResult>,
          ITupleObjectCrossTypeFactoryBinaryVisitor<TEntity, TOperand1, TOperationResult>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
    {
        public abstract TOperationResult Visit(
            TOperand1 first,
            EmptyTupleObject<TEntity> second,
            TupleObjectFactory factory);

        public abstract TOperationResult Visit(
            TOperand1 first,
            ConjunctiveTuple<TEntity> second,
            TupleObjectFactory factory);

        public abstract TOperationResult Visit(
            TOperand1 first,
            DisjunctiveTuple<TEntity> second,
            TupleObjectFactory factory);

        public abstract TOperationResult Visit(
            TOperand1 first,
            ConjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory);

        public abstract TOperationResult Visit(
            TOperand1 first,
            DisjunctiveTupleSystem<TEntity> second,
            TupleObjectFactory factory);

        public abstract TOperationResult Visit(
            TOperand1 first,
            FullTupleObject<TEntity> second,
            TupleObjectFactory factory);
    }

    public abstract class TupleObjectCrossTypeFactorySetBinaryOperator<TEntity, TOperand1>
        : TupleObjectCrossTypeFactoryBinaryVisitor<TEntity, TOperand1, TupleObject<TEntity>>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
    {
        protected delegate TupleObject<TEntity> OperationHandler<TOperand2>(
            TOperand1 first,
            TOperand2 second,
            TupleObjectFactory factory)
            where TOperand2 : TupleObject<TEntity>;

        protected delegate TupleObject<TEntity> ReversedOperationHandler<TOperand2>(
            TOperand2 first,
            TOperand1 second,
            TupleObjectFactory factory)
            where TOperand2 : TupleObject<TEntity>;

        private void AlignOperandsWithSchema<TOperand2>(
            ref TOperand1 first,
            ref TOperand2 second,
            TupleObjectFactory factory)
            where TOperand2 : TupleObject<TEntity>
        {
            TupleObjectSchema<TEntity> genSchema =
                first.Schema.GeneralizeWith(second.Schema);
            TupleObjectBuilder<TEntity> builder =
            factory.GetBuilder(genSchema);

            first = (first.AlignWithSchema(genSchema, factory, builder) as TOperand1)!;
            second = (second.AlignWithSchema(genSchema, factory, builder) as TOperand2)!;
        }

        protected TupleObject<TEntity> OperationStrategy<TOperand2>(
            TOperand1 first,
            TOperand2 second,
            TupleObjectFactory factory,
            OperationHandler<TOperand2> operation)
            where TOperand2 : TupleObject<TEntity>
        {
            AlignOperandsWithSchema(ref first, ref second, factory);
            return operation(first, second, factory);
        }

        protected TupleObject<TEntity> OperationStrategy<TOperand2>(
            TOperand1 first,
            TOperand2 second,
            TupleObjectFactory factory,
            ReversedOperationHandler<TOperand2> operation)
            where TOperand2 : TupleObject<TEntity>
        {
            AlignOperandsWithSchema(ref first, ref second, factory);
            return operation(second, first, factory);
        }
    }
}
