using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Navigational;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Navigational
{
    /*
    public record NavigationalAttributeComponentFactoryArgs<TKey, TData>
        : NonFictionalAttributeComponentFactoryArgs<TData>,
          INonFictionalAttributeComponentFactoryArgs<NavigationalAttributeComponentFactoryArgs<TKey, TData>>
    {
        public NavigationalAttributeComponentFactoryArgs(
            bool isQuery = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(isQuery, queryProvider, queryExpression)
        {
            return;
        }

        protected override AttributeComponentPower CreatePower()
        {
            return new NavigationalAttributeComponentPower();
        }
    }*/

    public record NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>
        : NonFictionalAttributeComponentFactoryArgs<TData>,
          INonFictionalAttributeComponentFactoryArgs<NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>>
        where TData : new()
    {
        public NavigationalAttributeComponentWithSimpleKeyFactoryArgs(
            IAttributeComponent<TKey> foreignKeyComponent,
            bool isQuery = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(isQuery, queryProvider, queryExpression)
        {
            return;
        }

        public NavigationalAttributeComponentWithSimpleKeyFactoryArgs(
            IAttributeComponent<TData> navigationalComponent,
            bool isQuery = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(isQuery, queryProvider, queryExpression)
        {
            return;
        }

        protected override AttributeComponentPower CreatePower()
        {
            return new NavigationalAttributeComponentPower();
        }
    }

    public record NavigationalAttributeComponentWithComplexKeyFactoryArgs<TKey, TData>
        : NonFictionalAttributeComponentFactoryArgs<TData>,
          INonFictionalAttributeComponentFactoryArgs<NavigationalAttributeComponentWithComplexKeyFactoryArgs<TKey, TData>>
        where TKey : new()
        where TData : new()
    {
        public NavigationalAttributeComponentWithComplexKeyFactoryArgs(
            bool isQuery = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(isQuery, queryProvider, queryExpression)
        {
            return;
        }

        protected override AttributeComponentPower CreatePower()
        {
            return new NavigationalAttributeComponentPower();
        }
    }
}
