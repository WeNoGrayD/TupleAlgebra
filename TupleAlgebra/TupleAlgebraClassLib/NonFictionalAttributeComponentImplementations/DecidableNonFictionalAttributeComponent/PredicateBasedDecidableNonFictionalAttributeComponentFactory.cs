using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.DecidableNonFictionalAttributeComponent
{
    public class PredicateBasedDecidableNonFictionalAttributeComponentFactory
        : AttributeComponentFactory,
          INonFictionalAttributeComponentFactory<PredicateBasedDecidableNonFictionalAttributeComponentFactoryArgs>
    {
        public NonFictionalAttributeComponent<TData> CreateSpecificNonFictional<TData>(PredicateBasedDecidableNonFictionalAttributeComponentFactoryArgs args)
        {
            return new PredicateBasedDecidableNonFictionalAttributeComponent<TData>(args.Domain as AttributeDomain<TData>, null);
        }
    }
}
