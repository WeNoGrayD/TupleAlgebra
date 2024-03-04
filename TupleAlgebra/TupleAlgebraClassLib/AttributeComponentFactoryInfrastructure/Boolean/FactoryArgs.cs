using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Linq.Expressions;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Boolean;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Boolean
{
    public class BooleanAttributeComponentFactoryArgs
        : NonFictionalAttributeComponentFactoryArgs<bool>
    {
        public bool Value { get; private set; }

        public BooleanAttributeComponentFactoryArgs(
            bool value,
            bool isQuery = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(isQuery, queryProvider, queryExpression)
        {
            Value = value;

            return;
        }

        protected override AttributeComponentPower CreatePower()
        {
            return AtomicNonFictionalAttributeComponentPower<bool>.Instance;
        }
    }
}
