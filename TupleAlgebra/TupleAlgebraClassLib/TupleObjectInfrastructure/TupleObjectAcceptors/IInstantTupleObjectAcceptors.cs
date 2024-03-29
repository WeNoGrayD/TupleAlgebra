﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.TupleObjectAcceptors
{
    public interface IInstantUnaryTupleObjectAcceptor<TEntity, in TOperand, out TOperationResult>
        : UniversalClassLib.HierarchicallyPolymorphicOperators.IInstantUnaryOperator<TOperand, TOperationResult>
        where TEntity : new()
        where TOperand : TupleObjects.TupleObject<TEntity>
    { }

    public interface IInstantBinaryTupleObjectAcceptor<TEntity, in TOperand1, in TOperand2, out TOperationResult>
        : UniversalClassLib.HierarchicallyPolymorphicOperators.IInstantBinaryOperator<TOperand1, TOperand2, TOperationResult>
        where TEntity : new()
        where TOperand1 : TupleObjects.TupleObject<TEntity>
        where TOperand2 : TupleObjects.TupleObject<TEntity>
    { }
}
