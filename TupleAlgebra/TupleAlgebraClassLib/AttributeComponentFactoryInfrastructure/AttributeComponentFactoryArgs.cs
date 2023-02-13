using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure
{
    public class AttributeComponentFactoryArgs<TData>
    {
        public readonly AttributeDomain<TData> Domain;

        public readonly AttributeComponentQueryProvider QueryProvider;

        public Expression QueryExpression { get; set; }

        public AttributeComponentFactoryArgs(
            AttributeDomain<TData> domain, 
            AttributeComponentQueryProvider queryProvider = null, 
            Expression queryExpression = null)
        {
            Domain = domain;
            QueryProvider = queryProvider;
            QueryExpression = queryExpression;
        }
    }
}
