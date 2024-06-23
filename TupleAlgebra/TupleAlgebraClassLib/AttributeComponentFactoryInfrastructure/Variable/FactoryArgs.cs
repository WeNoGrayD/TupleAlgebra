using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.TupleBased;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure;
using TupleAlgebraClassLib.FullAttributeComponentInfrastructure;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Variable;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Variable
{
    public record VariableAttributeComponentFactoryArgs<TData>
        : NonFictionalAttributeComponentFactoryArgs<TData>,
          INonFictionalAttributeComponentFactoryArgs<VariableAttributeComponentFactoryArgs<TData>>
    {
        public string Name { get; private set; }

        public VariableAttributeComponentFactoryArgs(
            string name,
            bool isQuery = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(isQuery, queryProvider, queryExpression)
        {
            Name = name;

            return;
        }

        protected override AttributeComponentPower CreatePower()
        {
            return new VariableAttributeComponentPower();
        }
    }
}
