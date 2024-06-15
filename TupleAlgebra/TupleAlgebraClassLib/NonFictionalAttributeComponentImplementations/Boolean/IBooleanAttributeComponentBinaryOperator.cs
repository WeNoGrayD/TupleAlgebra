using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Boolean;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Boolean
{
    public interface IBooleanAttributeComponentBinaryOperator
        : IFactoryBinaryAttributeComponentVisitor<bool, bool, BooleanNonFictionalAttributeComponent, BooleanNonFictionalAttributeComponent, IBooleanAttributeComponentFactory, BooleanAttributeComponentFactoryArgs, IAttributeComponent<bool>>
    { }

    public interface IBooleanAttributeComponentBooleanOperator
        : IAttributeComponentBooleanOperator<bool, BooleanNonFictionalAttributeComponent, BooleanNonFictionalAttributeComponent>
    { }
}
