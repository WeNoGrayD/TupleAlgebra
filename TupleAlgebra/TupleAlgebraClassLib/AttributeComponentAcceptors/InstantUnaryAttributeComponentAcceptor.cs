using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using UniversalClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectVisitors;

namespace TupleAlgebraClassLib.AttributeComponentVisitors
{
    public abstract class InstantUnaryAttributeComponentVisitor<TData, TOperand, TOperationResult>
        : InstantUnaryOperator<TOperand, TOperationResult>//,
          //IInstantUnaryAttributeComponentVisitor<TData, TOperand, TOperationResult>
        where TOperand : IAttributeComponent<TData>
    {
    }

    public abstract class AttributeComponentInstantUnarySetOperator<TData, TOperand>
        : InstantUnaryAttributeComponentVisitor<TData, TOperand, IAttributeComponent<TData>>
        where TOperand : IAttributeComponent<TData>
    {
    }

    public abstract class AttributeComponentInstantUnaryBooleanOperator<TData, TOperand>
        : InstantUnaryAttributeComponentVisitor<TData, TOperand, bool>
        where TOperand : IAttributeComponent<TData>
    {
    }
}
