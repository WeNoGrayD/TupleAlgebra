using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Complex;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Complex
{
    public record ComplexAttributeComponentFactoryArgs<TData>
        : NonFictionalAttributeComponentFactoryArgs<TData>,
          INonFictionalAttributeComponentFactoryArgs<ComplexAttributeComponentFactoryArgs<TData>>,
          INonFictionalAttributeComponentFactoryArgs<TData, ComplexAttributeComponentFactoryArgs<TData>>
        where TData : new()
    {
        public TupleObject<TData> Values { get; private set; }

        public ComplexAttributeComponentFactoryArgs(
            TupleObject<TData> values = null,
            bool isQuery = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(isQuery, queryProvider, queryExpression)
        {
            Values = values;

            return;
        }

        protected override AttributeComponentPower CreatePower()
        {
            return ComplexAttributeComponentPower.Instance;
        }
    }
}
