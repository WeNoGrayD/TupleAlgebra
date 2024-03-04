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
    public class ComplementationOperator
        : FactoryUnaryAttributeComponentAcceptor<bool, BooleanNonFictionalAttributeComponent, IBooleanAttributeComponentFactory, BooleanAttributeComponentFactoryArgs, AttributeComponent<bool>>,
          IFactoryUnaryAttributeComponentAcceptor<bool, BooleanNonFictionalAttributeComponent, IBooleanAttributeComponentFactory, BooleanAttributeComponentFactoryArgs, AttributeComponent<bool>>
    {
        public override AttributeComponent<bool> Accept(
            BooleanNonFictionalAttributeComponent first,
            IBooleanAttributeComponentFactory factory)
        {
            if (first.Value)
                return BooleanNonFictionalAttributeComponent.False;
            else
                return BooleanNonFictionalAttributeComponent.True;
        }
    }
}
