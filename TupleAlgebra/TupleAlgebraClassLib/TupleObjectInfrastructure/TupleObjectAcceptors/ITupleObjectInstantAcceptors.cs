using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectAcceptors
{
    public interface ITupleObjectInstantUnaryAcceptor<TEntity, in TOperand, out TOperationResult>
        : IInstantUnaryOperator<TOperand, TOperationResult>
        where TEntity : new()
        where TOperand : TupleObject<TEntity>
    { }

    public interface ITupleObjectInstantBinaryAcceptor<TEntity, in TOperand1, in TOperand2, out TOperationResult>
        : IInstantBinaryOperator<TOperand1, TOperand2, TOperationResult>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
        where TOperand2 : TupleObject<TEntity>
    { }

    public interface ITupleObjectCrossTypeInstantBinaryAcceptor<TEntity, in TOperand1, out TOperationResult>
        : ITupleObjectInstantBinaryAcceptor<TEntity, TOperand1, EmptyTupleObject<TEntity>, TOperationResult>,
          ITupleObjectInstantBinaryAcceptor<TEntity, TOperand1, FullTupleObject<TEntity>, TOperationResult>,
          ITupleObjectInstantBinaryAcceptor<TEntity, TOperand1, ConjunctiveTuple<TEntity>, TOperationResult>,
          ITupleObjectInstantBinaryAcceptor<TEntity, TOperand1, DisjunctiveTuple<TEntity>, TOperationResult>,
          ITupleObjectInstantBinaryAcceptor<TEntity, TOperand1, ConjunctiveTupleSystem<TEntity>, TOperationResult>,
          ITupleObjectInstantBinaryAcceptor<TEntity, TOperand1, DisjunctiveTupleSystem<TEntity>, TOperationResult>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
    {
        TOperationResult AcceptDefault(TOperand1 first, TupleObject<TEntity> second);

        TOperationResult IInstantBinaryOperator<TOperand1, ConjunctiveTuple<TEntity>, TOperationResult>.
            Accept(TOperand1 first, ConjunctiveTuple<TEntity> second)
        {
            return AcceptDefault(first, second);
        }

        TOperationResult IInstantBinaryOperator<TOperand1, DisjunctiveTuple<TEntity>, TOperationResult>.
            Accept(TOperand1 first, DisjunctiveTuple<TEntity> second)
        {
            return AcceptDefault(first, second);
        }

        TOperationResult IInstantBinaryOperator<TOperand1, ConjunctiveTupleSystem<TEntity>, TOperationResult>.
            Accept(TOperand1 first, ConjunctiveTupleSystem<TEntity> second)
        {
            return AcceptDefault(first, second);
        }

        TOperationResult IInstantBinaryOperator<TOperand1, DisjunctiveTupleSystem<TEntity>, TOperationResult>.
            Accept(TOperand1 first, DisjunctiveTupleSystem<TEntity> second)
        {
            return AcceptDefault(first, second);
        }
    }
}
