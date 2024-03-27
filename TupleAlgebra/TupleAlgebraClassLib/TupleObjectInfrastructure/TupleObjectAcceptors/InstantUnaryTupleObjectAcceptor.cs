using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TupleAlgebraClassLib.TupleObjectAcceptors
{
    public abstract class InstantUnaryAttributeComponentAcceptor<TEntity, TOperand, TOperationResult>
        : UniversalClassLib.HierarchicallyPolymorphicOperators.InstantUnaryOperator<TOperand, TOperationResult>
        where TEntity : new()
        where TOperand : TupleObjects.TupleObject<TEntity>
    { }
}
