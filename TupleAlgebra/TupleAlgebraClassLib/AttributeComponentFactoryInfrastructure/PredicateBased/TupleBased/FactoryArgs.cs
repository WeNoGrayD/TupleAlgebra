using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure;
using TupleAlgebraClassLib.FullAttributeComponentInfrastructure;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.TupleBased;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Variable;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.TupleBased
{
    public record TupleBasedAttributeComponentFactoryArgs<TData>
        : NonFictionalAttributeComponentFactoryArgs<TData>,
          INonFictionalAttributeComponentFactoryArgs<TupleBasedAttributeComponentFactoryArgs<TData>>,
          INonFictionalAttributeComponentFactoryArgs<TData, TupleBasedAttributeComponentFactoryArgs<TData>>
    {
        public ITupleObject Sample { get; private set; }

        public ITupleObject Mask { get; private set; }

        public AttributeName AttributeNameWithinPredicate { get; }

        public TupleBasedAttributeComponentFactoryArgs(
            ITupleObject sample,
            ITupleObject mask,
            AttributeName attrNameWithinPredicate,
            bool isQuery = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(isQuery, queryProvider, queryExpression)
        {
            Sample = sample;
            Mask = mask;
            AttributeNameWithinPredicate = attrNameWithinPredicate;

            return;
        }

        protected override AttributeComponentPower CreatePower()
        {
            return new TupleBasedAttributeComponentPower();
        }
    }
}
