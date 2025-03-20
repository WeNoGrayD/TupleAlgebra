using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectVisitors
{
    public interface ITupleObjectInstantUnaryVisitor<TEntity, in TOperand, out TOperationResult>
        : IInstantUnaryOperator<TOperand, TOperationResult>
        where TEntity : new()
        where TOperand : TupleObject<TEntity>
    { }

    public interface ITupleObjectInstantBinaryVisitor<TEntity, in TOperand1, in TOperand2, out TOperationResult>
        : IInstantBinaryOperator<TOperand1, TOperand2, TOperationResult>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
        where TOperand2 : TupleObject<TEntity>
    { }

    public interface ITupleObjectCrossTypeInstantBinaryVisitor<TEntity, in TOperand1, out TOperationResult>
        : ITupleObjectInstantBinaryVisitor<TEntity, TOperand1, EmptyTupleObject<TEntity>, TOperationResult>,
          ITupleObjectInstantBinaryVisitor<TEntity, TOperand1, FullTupleObject<TEntity>, TOperationResult>,
          ITupleObjectInstantBinaryVisitor<TEntity, TOperand1, ConjunctiveTuple<TEntity>, TOperationResult>,
          ITupleObjectInstantBinaryVisitor<TEntity, TOperand1, DisjunctiveTuple<TEntity>, TOperationResult>,
          ITupleObjectInstantBinaryVisitor<TEntity, TOperand1, ConjunctiveTupleSystem<TEntity>, TOperationResult>,
          ITupleObjectInstantBinaryVisitor<TEntity, TOperand1, DisjunctiveTupleSystem<TEntity>, TOperationResult>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
    {
    }
}
