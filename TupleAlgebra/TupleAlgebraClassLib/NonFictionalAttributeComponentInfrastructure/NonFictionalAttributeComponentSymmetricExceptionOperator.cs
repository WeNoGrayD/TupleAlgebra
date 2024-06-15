using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    public abstract class NonFictionalAttributeComponentSymmetricExceptionOperator<
        TData,
        TIntermediateResult, 
        CTOperand1, 
        TFactory,
        CTFactoryArgs>
        : NonFictionalAttributeComponentSetBinaryOperator<TData, TIntermediateResult, CTOperand1, TFactory, CTFactoryArgs>
        where CTOperand1 : NonFictionalAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, TIntermediateResult, CTOperand1, CTFactoryArgs>
        where CTFactoryArgs : AttributeComponentFactoryArgs
    {
        public override IAttributeComponent<TData> Visit(
            NonFictionalAttributeComponent<TData> first,
            EmptyAttributeComponent<TData> second,
            TFactory factory)
        {
            return first;
        }

        public override IAttributeComponent<TData> Visit(
            NonFictionalAttributeComponent<TData> first,
            FullAttributeComponent<TData> second,
            TFactory factory)
        {
            return ~first;
        }
    }
}
