﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Linq.Expressions;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable
{
    public class OrderedFiniteEnumerableAttributeComponentFactoryArgs
        : AttributeComponentFactoryArgs
    {
        public IEnumerable Values { get; private set; }

        public object OrderingComparer { get; private set; }

        private OrderedFiniteEnumerableAttributeComponentFactoryArgs(
            object orderingComparer,
            IEnumerable values = null,
            OrderedFiniteEnumerableAttributeComponentQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(queryProvider, queryExpression)
        {
            Values = values;
            OrderingComparer = orderingComparer;
        }

        public static OrderedFiniteEnumerableAttributeComponentFactoryArgs Construct<TData>(
            IComparer<TData> orderingComparer,
            IEnumerable<TData> values = null,
            OrderedFiniteEnumerableAttributeComponentQueryProvider queryProvider = null,
            Expression queryExpression = null)
        {
            return new OrderedFiniteEnumerableAttributeComponentFactoryArgs(
                orderingComparer,
                values, 
                queryProvider,
                queryExpression);
        }
    }
}
