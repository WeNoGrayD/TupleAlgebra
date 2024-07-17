using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    public abstract class NonFictionalAttributeComponentIntersectionOperator<
        TData,
        TIntermediateResult, 
        CTOperand1, 
        TFactory,
        CTFactoryArgs>
        : NonFictionalAttributeComponentSetBinaryOperator<
            TData,
            TIntermediateResult, 
            CTOperand1, 
            TFactory,
            CTFactoryArgs>,
          IAnyXVariableAttributeComponentIntersectionOperator<TData>
        where CTOperand1 : NonFictionalAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, TIntermediateResult, CTOperand1, CTFactoryArgs>
        where CTFactoryArgs : NonFictionalAttributeComponentFactoryArgs<TData>
    {
        public override IAttributeComponent<TData> Visit(
            NonFictionalAttributeComponent<TData> first,
            EmptyAttributeComponent<TData> second,
            TFactory factory)
        {
            return second;
        }

        public override IAttributeComponent<TData> Visit(
            NonFictionalAttributeComponent<TData> first,
            FullAttributeComponent<TData> second,
            TFactory factory)
        {
            return first;
        }
    }
}
