using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors
{
    public abstract class TupleObjectCrossTypeInstantBinaryVisitor<TEntity, TOperand1, TOperationResult>
        : TupleObjectInstantBinaryVisitor<TEntity, TOperand1, TOperationResult>,
          ITupleObjectCrossTypeInstantBinaryVisitor<TEntity, TOperand1, TOperationResult>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
    {
        public abstract TOperationResult Visit(TOperand1 first, EmptyTupleObject<TEntity> second);

        public abstract TOperationResult Visit(
            TOperand1 first,
            ConjunctiveTuple<TEntity> second);

        public abstract TOperationResult Visit(
            TOperand1 first,
            DisjunctiveTuple<TEntity> second);

        public abstract TOperationResult Visit(
            TOperand1 first,
            ConjunctiveTupleSystem<TEntity> second);

        public abstract TOperationResult Visit(
            TOperand1 first,
            DisjunctiveTupleSystem<TEntity> second);

        public abstract TOperationResult Visit(TOperand1 first, FullTupleObject<TEntity> second);
    }

    public abstract class TupleObjectCrossTypeInstantDefaultBinaryVisitor<TEntity, TOperand1, TOperationResult>
        : TupleObjectCrossTypeInstantBinaryVisitor<TEntity, TOperand1, TOperationResult>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
    {
        public abstract TOperationResult VisitDefault(
            TOperand1 first,
            TupleObject<TEntity> second);

        public override TOperationResult Visit(
            TOperand1 first,
            ConjunctiveTuple<TEntity> second)
        {
            return VisitDefault(first, second);
        }

        public override TOperationResult Visit(
            TOperand1 first,
            DisjunctiveTuple<TEntity> second)
        {
            return VisitDefault(first, second);
        }

        public override TOperationResult Visit(
            TOperand1 first,
            ConjunctiveTupleSystem<TEntity> second)
        {
            return VisitDefault(first, second);
        }

        public override TOperationResult Visit(
            TOperand1 first,
            DisjunctiveTupleSystem<TEntity> second)
        {
            return VisitDefault(first, second);
        }
    }

    public abstract class TupleObjectCrossTypeInstantBooleanBinaryVisitor<TEntity, TOperand1>
        : TupleObjectCrossTypeInstantBinaryVisitor<TEntity, TOperand1, bool>,
          ITupleObjectCrossTypeInstantBinaryVisitor<TEntity, TOperand1, bool>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
    { }
}
