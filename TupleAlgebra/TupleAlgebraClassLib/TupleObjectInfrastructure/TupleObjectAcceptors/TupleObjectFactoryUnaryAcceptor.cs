using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors
{
    public abstract class TupleObjectFactoryUnaryVisitor<TEntity, TOperand, TOperationResult>
        : FactoryUnaryOperator<TOperand, TupleObjectFactory, TOperationResult>,
          ITupleObjectFactoryUnaryVisitor<TEntity, TOperand, TOperationResult>
        where TEntity : new()
        where TOperand : TupleObject<TEntity>
    { }

    public abstract class TupleObjectFactoryUnarySetOperator<TEntity, TOperand>
        : TupleObjectFactoryUnaryVisitor<TEntity, TOperand, TupleObject<TEntity>>
        where TEntity : new()
        where TOperand : TupleObject<TEntity>
    { }
}
