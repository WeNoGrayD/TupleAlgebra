﻿using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable.Streaming;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Streaming
{
    public class StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>
        : OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>
    {
        public StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs(
            IEnumerable<TData> values,
            IComparer<TData> orderingComparer = null,
            bool valuesAreOrdered = false,
            bool isQuery = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(
                  values,
                  orderingComparer,
                  valuesAreOrdered, 
                  isQuery, 
                  queryProvider,
                  queryExpression)
        {
            return;
        }

        protected override AttributeComponentPower CreatePower()
        {
            return new StreamingOrderedFiniteEnumerableAttributeComponentPower<TData>();
        }
    }
}