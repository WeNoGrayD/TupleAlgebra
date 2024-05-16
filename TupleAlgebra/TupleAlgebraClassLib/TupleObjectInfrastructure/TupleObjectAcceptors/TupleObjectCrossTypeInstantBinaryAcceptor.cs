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
    public abstract class TupleObjectCrossTypeInstantBinaryAcceptor<TEntity, TOperand1, TOperationResult>
        : TupleObjectInstantBinaryAcceptor<TEntity, TOperand1, TOperationResult>,
          //ITupleObjectInstantBinaryAcceptor<TEntity, TOperand1, EmptyTupleObject<TEntity>, TOperationResult>,
          //ITupleObjectInstantBinaryAcceptor<TEntity, TOperand1, TupleObject<TEntity>, TOperationResult>,
          //ITupleObjectInstantBinaryAcceptor<TEntity, TOperand1, FullTupleObject<TEntity>, TOperationResult>
          ITupleObjectCrossTypeInstantBinaryAcceptor<TEntity, TOperand1, TOperationResult>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
    {
        public abstract TOperationResult Accept(TOperand1 first, EmptyTupleObject<TEntity> second);

        public abstract TOperationResult AcceptDefault(TOperand1 first, TupleObject<TEntity> second);

        public abstract TOperationResult Accept(TOperand1 first, FullTupleObject<TEntity> second);
    }
}
