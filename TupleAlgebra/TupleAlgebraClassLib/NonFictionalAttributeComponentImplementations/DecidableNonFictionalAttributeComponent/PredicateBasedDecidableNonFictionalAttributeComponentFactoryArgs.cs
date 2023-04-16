using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.DecidableNonFictionalAttributeComponent
{
    public class PredicateBasedDecidableNonFictionalAttributeComponentFactoryArgs
        : AttributeComponentFactoryArgs
    {
        public readonly IEnumerable<IEnumerable> Values;

        public PredicateBasedDecidableNonFictionalAttributeComponentFactoryArgs(
            object domain,
            params IEnumerable[] rules)
            : base(domain)
        {
        }

        public PredicateBasedDecidableNonFictionalAttributeComponentFactoryArgs Construct<TData>(
            AttributeDomain<TData> domain,
            params IEnumerable<Predicate<TData>>[] rules)
        {
            return new PredicateBasedDecidableNonFictionalAttributeComponentFactoryArgs(domain, rules);
        }
    }
}
