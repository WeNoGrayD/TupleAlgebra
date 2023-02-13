using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.DecidableNonFictionalAttributeComponent
{
    public class PredicateBasedDecidableNonFictionalAttributeComponentFactory<TData>
        : AttributeComponentFactory<TData>,
          INonFictionalAttributeComponentFactory<TData, PredicateBasedDecidableNonFictionalAttributeComponentFactoryArgs<TData>>
    {
        public NonFictionalAttributeComponent<TData> CreateSpecificNonFictional(PredicateBasedDecidableNonFictionalAttributeComponentFactoryArgs<TData> args)
        {
            return new PredicateBasedDecidableNonFictionalAttributeComponent<TData>(args.Domain, null);
        }
    }
}
