using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectAcceptors
{
    public abstract class TupleObjectFactoryUnaryAcceptor<TEntity, TOperand, TOperationResult>
        : FactoryUnaryOperator<TOperand, TupleObjectFactory, TOperationResult>,
          ITupleObjectFactoryUnaryAcceptor<TEntity, TOperand, TOperationResult>
        where TEntity : new()
        where TOperand : TupleObject<TEntity>
    { }

    public abstract class TupleObjectFactoryUnarySetOperator<TEntity, TOperand>
        : TupleObjectFactoryUnaryAcceptor<TEntity, TOperand, TupleObject<TEntity>>
        where TEntity : new()
        where TOperand : TupleObject<TEntity>
    { }
}
