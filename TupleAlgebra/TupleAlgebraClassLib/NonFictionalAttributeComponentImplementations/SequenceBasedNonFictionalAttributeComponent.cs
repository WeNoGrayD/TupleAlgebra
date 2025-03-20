using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations
{
    public abstract class SequenceBasedNonFictionalAttributeComponent
        <TData, TValuesContainer>
        : NonFictionalAttributeComponent<TData>
    {
        public virtual TValuesContainer Values { get; protected set; }

        public SequenceBasedNonFictionalAttributeComponent(
            AttributeComponentPower power,
            IQueryProvider queryProvider,
            Expression queryExpression = null)
            : base(power, queryProvider, queryExpression)
        { }
    }
}
