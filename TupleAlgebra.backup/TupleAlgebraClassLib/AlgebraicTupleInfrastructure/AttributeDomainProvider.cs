using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.AlgebraicTupleInfrastructure
{
    public class AttributeDomainProvider<TDomain> : AttributeDomain<TDomain>
    {
        public AttributeDomainProvider()
            : base(null)
        {

        }

        public static AttributeDomainProvider<TDomain> Construct<TAttribute>(
            AttributeDomain<TAttribute> source)
        {
            return null;
        }
    }
}
