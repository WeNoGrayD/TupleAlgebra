using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Boolean;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Boolean
{
    public class SymmetricExceptionOperator
        : NonFictionalAttributeComponentSymmetricExceptionOperator<bool, BooleanNonFictionalAttributeComponent>,
          IFactoryBinaryAttributeComponentAcceptor<bool, BooleanNonFictionalAttributeComponent, BooleanNonFictionalAttributeComponent, AttributeComponent<bool>>
    {
        public AttributeComponent<bool> Accept(
            BooleanNonFictionalAttributeComponent first,
            BooleanNonFictionalAttributeComponent second,
            AttributeComponentFactory factory)
        {
            if (first.Value == second.Value)
            {
                return factory.CreateEmpty<bool>(first.GetDomain);
            }
            else
            {
                return factory.CreateFull<bool>(first.GetDomain);
            }
        }
    }
}
