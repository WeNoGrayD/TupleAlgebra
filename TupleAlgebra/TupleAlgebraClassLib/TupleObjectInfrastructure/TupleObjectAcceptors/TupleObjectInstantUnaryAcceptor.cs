using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TupleAlgebraClassLib.TupleObjects;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.TupleObjectAcceptors
{
    public abstract class TupleObjectInstantUnaryAcceptor<TEntity, TOperand, TOperationResult>
        : InstantUnaryOperator<TOperand, TOperationResult>
        where TEntity : new()
        where TOperand : TupleObject<TEntity>
    { }
}
