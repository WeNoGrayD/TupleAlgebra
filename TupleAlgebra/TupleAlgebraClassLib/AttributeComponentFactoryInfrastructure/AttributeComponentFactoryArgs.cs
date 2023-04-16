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
    public class AttributeComponentFactoryArgs
    {
        public readonly object Domain;

        public readonly IQueryProvider QueryProvider;

        public Expression QueryExpression { get; set; }

        public AttributeComponentFactoryArgs(
            object domain, 
            IQueryProvider queryProvider = null, 
            Expression queryExpression = null)
        {
            Domain = domain;
            QueryProvider = queryProvider;
            QueryExpression = queryExpression;
        }
    }
}
