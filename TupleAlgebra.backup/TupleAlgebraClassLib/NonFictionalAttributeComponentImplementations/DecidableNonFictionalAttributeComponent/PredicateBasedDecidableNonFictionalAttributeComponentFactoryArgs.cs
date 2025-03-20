using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.DecidableNonFictionalAttributeComponent
{
    public class PredicateBasedDecidableNonFictionalAttributeComponentFactoryArgs<TData> : AttributeComponentFactoryArgs<TData>
    {
        public readonly IEnumerable<IEnumerable<Predicate<TData>>> Values;

        public PredicateBasedDecidableNonFictionalAttributeComponentFactoryArgs(
            AttributeDomain<TData> domain,
            params IEnumerable<Predicate<TData>>[] rules)
            : base(domain)
        {
        }
    }
}
