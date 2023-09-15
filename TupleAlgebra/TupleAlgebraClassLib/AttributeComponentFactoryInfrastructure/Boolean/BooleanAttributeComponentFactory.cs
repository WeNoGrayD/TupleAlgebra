using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Boolean;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Boolean
{
    public class BooleanAttributeComponentFactory
        : AttributeComponentFactory,
          INonFictionalAttributeComponentFactory<BooleanAttributeComponentFactoryArgs>
    {
        public NonFictionalAttributeComponent<TData> CreateSpecificNonFictional<TData>(
            BooleanAttributeComponentFactoryArgs args)
        {
            return new BooleanNonFictionalAttributeComponent(
                args.Power,
                args.Value,
                args.QueryProvider, 
                args.QueryExpression) as NonFictionalAttributeComponent<TData>;
        }
    }
}
