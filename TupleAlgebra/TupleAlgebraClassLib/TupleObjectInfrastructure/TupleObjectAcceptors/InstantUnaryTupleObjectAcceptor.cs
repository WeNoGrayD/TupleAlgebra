using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TupleAlgebraClassLib.TupleObjectAcceptors
{
    public abstract class InstantUnaryAttributeComponentAcceptor<TEntity, TOperand, TOperationResult>
        : HierarchicallyPolymorphicOperators.InstantUnaryOperator<TOperand, TOperationResult>
        where TOperand : TupleObjects.TupleObject<TEntity>
    { }
}
