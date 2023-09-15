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
    public class ComplementionOperator
        : FactoryUnaryAttributeComponentAcceptor<bool, BooleanNonFictionalAttributeComponent, AttributeComponent<bool>>,
          IFactoryUnaryAttributeComponentAcceptor<bool, BooleanNonFictionalAttributeComponent, AttributeComponent<bool>>
    {
        public override AttributeComponent<bool> Accept(
            BooleanNonFictionalAttributeComponent first, 
            AttributeComponentFactory factory)
        {
            if (first.Value)
                return BooleanNonFictionalAttributeComponent.False;
            else
                return BooleanNonFictionalAttributeComponent.True;
        }
    }
}
