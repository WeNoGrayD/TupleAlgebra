using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    /// <summary>
    /// Дополнение нефиктивной компоненты, стандартная реализация.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="CTOperand"></typeparam>
    /// <typeparam name="CTFactoryArgs"></typeparam>
    public class NonFictionalAttributeComponentComplementationOperator<TData, CTOperand, TFactory, CTFactoryArgs>
        : FactoryUnaryAttributeComponentAcceptor<TData, CTOperand, TFactory, CTFactoryArgs, AttributeComponent<TData>>,
          IFactoryUnaryAttributeComponentAcceptor<TData, CTOperand, TFactory, CTFactoryArgs, AttributeComponent<TData>>
        where CTOperand: NonFictionalAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, CTOperand, CTFactoryArgs>
        where CTFactoryArgs : AttributeComponentFactoryArgs
    {
        public override AttributeComponent<TData> Accept(
            CTOperand first, 
            TFactory factory)
        {
            return first.Domain ^ first;
        }
    }
}
