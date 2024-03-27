using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectAcceptors;
using TupleAlgebraClassLib.TupleObjects;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectAcceptors
{
    public abstract class CrossTypeInstantBinaryTupleObjectAcceptor<TEntity, TOperand1, TOperationResult>
        : InstantBinaryTupleObjectAcceptor<TEntity, TOperand1, TOperationResult>,
          IInstantBinaryTupleObjectAcceptor<TEntity, TOperand1, EmptyTupleObject<TEntity>, TOperationResult>,
          IInstantBinaryTupleObjectAcceptor<TEntity, TOperand1, TupleObject<TEntity>, TOperationResult>,
          IInstantBinaryTupleObjectAcceptor<TEntity, TOperand1, FullTupleObject<TEntity>, TOperationResult>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
    {

        public abstract TOperationResult Accept(TOperand1 first, EmptyTupleObject<TEntity> second);

        TOperationResult IInstantBinaryOperator<TOperand1, TupleObject<TEntity>, TOperationResult>.Accept(TOperand1 first, TupleObject<TEntity> second)
        {
            return AcceptDefault(first, second);
        }

        protected abstract TOperationResult AcceptDefault(TOperand1 first, TupleObject<TEntity> second);

        public abstract TOperationResult Accept(TOperand1 first, FullTupleObject<TEntity> second);
    }
}
