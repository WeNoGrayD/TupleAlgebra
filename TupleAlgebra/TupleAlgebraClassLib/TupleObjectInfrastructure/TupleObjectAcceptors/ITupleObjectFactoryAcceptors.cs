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
    public interface ITupleObjectFactoryUnaryVisitor<
        TEntity, 
        in TOperand, 
        out TOperationResult>
        : IFactoryUnaryOperator<TOperand, TupleObjectFactory, TOperationResult>
        where TEntity : new()
        where TOperand : TupleObject<TEntity>
    { }

    public interface ITupleObjectFactoryBinaryVisitor<
        TEntity, 
        in TOperand1, 
        in TOperand2, 
        out TOperationResult>
        : IFactoryBinaryOperator<TOperand1, TOperand2, TupleObjectFactory, TOperationResult>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
        where TOperand2 : TupleObject<TEntity>
    { }

    public interface ITupleObjectCrossTypeFactoryBinaryVisitor<
        TEntity, 
        in TOperand1, 
        out TOperationResult>
        : ITupleObjectFactoryBinaryVisitor<TEntity, TOperand1, EmptyTupleObject<TEntity>, TOperationResult>,
          ITupleObjectFactoryBinaryVisitor<TEntity, TOperand1, FullTupleObject<TEntity>, TOperationResult>,
          ITupleObjectFactoryBinaryVisitor<TEntity, TOperand1, ConjunctiveTuple<TEntity>, TOperationResult>,
          ITupleObjectFactoryBinaryVisitor<TEntity, TOperand1, DisjunctiveTuple<TEntity>, TOperationResult>,
          ITupleObjectFactoryBinaryVisitor<TEntity, TOperand1, ConjunctiveTupleSystem<TEntity>, TOperationResult>,
          ITupleObjectFactoryBinaryVisitor<TEntity, TOperand1, DisjunctiveTupleSystem<TEntity>, TOperationResult>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
    { }
}
