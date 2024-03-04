using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Boolean;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Boolean
{
    public interface IBooleanAttributeComponentBinaryOperator
        : IFactoryBinaryAttributeComponentAcceptor<bool, BooleanNonFictionalAttributeComponent, BooleanNonFictionalAttributeComponent, IBooleanAttributeComponentFactory, BooleanAttributeComponentFactoryArgs, AttributeComponent<bool>>
    { }

    public interface IBooleanAttributeComponentBooleanOperator
        : IAttributeComponentBooleanOperator<bool, BooleanNonFictionalAttributeComponent, BooleanNonFictionalAttributeComponent>
    { }
}
