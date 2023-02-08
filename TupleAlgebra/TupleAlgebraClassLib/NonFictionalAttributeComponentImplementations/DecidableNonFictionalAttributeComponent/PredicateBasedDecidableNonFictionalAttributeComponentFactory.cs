using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.DecidableNonFictionalAttributeComponent
{
    public class PredicateBasedDecidableNonFictionalAttributeComponentFactory<TValue>
        : AttributeComponentFactory<TValue>,
          INonFictionalAttributeComponentFactory<TValue, PredicateBasedDecidableNonFictionalAttributeComponentFactoryArgs<TValue>>
    {
        public NonFictionalAttributeComponent<TValue> CreateSpecificNonFictional(PredicateBasedDecidableNonFictionalAttributeComponentFactoryArgs<TValue> args)
        {
            return new PredicateBasedDecidableNonFictionalAttributeComponent<TValue>(args.Domain, null);
        }
    }
}
