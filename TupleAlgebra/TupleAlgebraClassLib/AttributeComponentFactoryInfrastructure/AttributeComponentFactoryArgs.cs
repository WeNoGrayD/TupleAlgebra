using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure
{
    public class AttributeComponentFactoryArgs<TValue>
    {
        public readonly AttributeDomain<TValue> Domain;

        public readonly NonFictionalAttributeComponentQueryProvider QueryProvider;

        public readonly Expression QueryExpression;

        public AttributeComponentFactoryArgs(
            AttributeDomain<TValue> domain, 
            NonFictionalAttributeComponentQueryProvider queryProvider = null, 
            Expression queryExpression = null)
        {
            Domain = domain;
            QueryProvider = queryProvider;
            QueryExpression = queryExpression;
        }
    }
}
