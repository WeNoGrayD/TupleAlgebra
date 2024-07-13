using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.Default;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Navigational;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Navigational
{
    public enum NavigationalPropertyMember
    {
        Key,
        Value
    }

    public record NavigationalAttributeComponentFactoryArgs<TKey, TData>
        : NonFictionalAttributeComponentFactoryArgs<TData>,
          INonFictionalAttributeComponentFactoryArgs<NavigationalAttributeComponentFactoryArgs<TKey, TData>>
        where TData : new()
    {
        public NavigationalPropertyMember Member { get; init; }

        public IAttributeComponentFactoryArgs<TKey> KeyFactoryArgs 
        { get; init; }

        public IAttributeComponentFactoryArgs<TData> ValueFactoryArgs
        { get; init; }

        public TupleObject<TData> Values { get; init; }

        public NavigationalAttributeComponentFactoryArgs(
            NonFictionalAttributeComponentFactoryArgs<TKey> keyFactoryArgs)
            : base(
                  keyFactoryArgs.IsQuery,
                  keyFactoryArgs.QueryProvider,
                  keyFactoryArgs.QueryExpression)
        {
            Member = NavigationalPropertyMember.Key;
            KeyFactoryArgs = 
                keyFactoryArgs as IAttributeComponentFactoryArgs<TKey>;

            return;
        }

        public NavigationalAttributeComponentFactoryArgs(
            NonFictionalAttributeComponentFactoryArgs<TData> valueFactoryArgs)
            : base(
                  valueFactoryArgs.IsQuery,
                  valueFactoryArgs.QueryProvider,
                  valueFactoryArgs.QueryExpression)
        {
            Member = NavigationalPropertyMember.Value;
            ValueFactoryArgs = 
                valueFactoryArgs as IAttributeComponentFactoryArgs<TData>;

            return;
        }

        public NavigationalAttributeComponentFactoryArgs(
            TupleObject<TData> values,
            bool isQuery = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(
                  isQuery,
                  queryProvider,
                  queryExpression)
        {
            Member = NavigationalPropertyMember.Value;
            Values = values;

            return;
        }

        protected override AttributeComponentPower CreatePower()
        {
            return new NavigationalAttributeComponentPower();
        }
    }

    /*
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
    */
}
