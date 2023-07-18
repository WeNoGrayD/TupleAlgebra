using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure
{
    public class AttributeComponentFactoryArgs
    {
        public Delegate DomainGetter { get; private set; } = null;

        public readonly IQueryProvider QueryProvider;

        public Expression QueryExpression { get; set; }

        public AttributeComponentFactoryArgs(
            IQueryProvider queryProvider = null, 
            Expression queryExpression = null)
        {
            QueryProvider = queryProvider;
            QueryExpression = queryExpression;

            return;
        }

        public void SetAttributeDomainGetter<TData>(Func<AttributeDomain<TData>> domainGetter)
        {
            DomainGetter = domainGetter;

            return;
        }
    }
}
