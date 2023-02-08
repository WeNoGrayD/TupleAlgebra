using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.DecidableNonFictionalAttributeComponent
{
    public class PredicateBasedDecidableNonFictionalAttributeComponentFactoryArgs<TValue> : AttributeComponentFactoryArgs<TValue>
    {
        public readonly IEnumerable<IEnumerable<Predicate<TValue>>> Values;

        public PredicateBasedDecidableNonFictionalAttributeComponentFactoryArgs(
            AttributeDomain<TValue> domain,
            params IEnumerable<Predicate<TValue>>[] rules)
            : base(domain)
        {
        }
    }
}
