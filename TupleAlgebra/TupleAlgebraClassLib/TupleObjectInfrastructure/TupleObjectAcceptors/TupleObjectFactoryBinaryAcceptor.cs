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
    public abstract class TupleObjectFactoryBinaryVisitor<
        TEntity,
        TOperand1,
        TOperationResult>
        : FactoryBinaryOperator<
            TOperand1, 
            TupleObject<TEntity>, 
            TupleObjectFactory, 
            TOperationResult>
        where TEntity : new()
        where TOperand1 : TupleObject<TEntity>
    { }
}
