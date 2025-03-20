using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.DecidableNonFictionalAttributeComponent
{
    public class PredicateBasedDecidableNonFictionalAttributeComponentFactoryArgs
        : AttributeComponentFactoryArgs
    {
        public readonly IEnumerable<IEnumerable> Values;

        public PredicateBasedDecidableNonFictionalAttributeComponentFactoryArgs(params IEnumerable[] rules)
            : base(false, null, null)
        {
            return;
        }

        public PredicateBasedDecidableNonFictionalAttributeComponentFactoryArgs Construct<TData>(
            AttributeDomain<TData> domain,
            params IEnumerable<Predicate<TData>>[] rules)
        {
            return new PredicateBasedDecidableNonFictionalAttributeComponentFactoryArgs(domain, rules);
        }
    }
}
