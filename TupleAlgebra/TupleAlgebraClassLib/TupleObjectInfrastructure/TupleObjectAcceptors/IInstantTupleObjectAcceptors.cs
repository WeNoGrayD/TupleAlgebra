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
}
