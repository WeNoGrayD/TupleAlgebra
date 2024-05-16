using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectAcceptors;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectAcceptors
{
    public interface ITupleObjectFactoryUnaryAcceptor<
        TEntity, 
        in TOperand, 
        out TOperationResult>
        : IFactoryUnaryOperator<TOperand, TupleObjectFactory, TOperationResult>
        where TEntity : new()
        where TOperand : TupleObject<TEntity>
    { }

    public interface ITupleObjectFactoryBinaryAcceptor<
        TEntity, 
        in TOperand1, 
        in TOperand2, 
        out TOperationResult>
        : IFactoryBinaryOperator<TOperand1, TOperand2, TupleObjectFactory, TOperationResult>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
        where TOperand2 : TupleObject<TEntity>
    { }

    public interface ITupleObjectCrossTypeFactoryBinaryAcceptor<
        TEntity, 
        in TOperand1, 
        out TOperationResult>
        : ITupleObjectFactoryBinaryAcceptor<TEntity, TOperand1, EmptyTupleObject<TEntity>, TOperationResult>,
          ITupleObjectFactoryBinaryAcceptor<TEntity, TOperand1, FullTupleObject<TEntity>, TOperationResult>,
          ITupleObjectFactoryBinaryAcceptor<TEntity, TOperand1, ConjunctiveTuple<TEntity>, TOperationResult>,
          ITupleObjectFactoryBinaryAcceptor<TEntity, TOperand1, DisjunctiveTuple<TEntity>, TOperationResult>,
          ITupleObjectFactoryBinaryAcceptor<TEntity, TOperand1, ConjunctiveTupleSystem<TEntity>, TOperationResult>,
          ITupleObjectFactoryBinaryAcceptor<TEntity, TOperand1, DisjunctiveTupleSystem<TEntity>, TOperationResult>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
    { }
}
