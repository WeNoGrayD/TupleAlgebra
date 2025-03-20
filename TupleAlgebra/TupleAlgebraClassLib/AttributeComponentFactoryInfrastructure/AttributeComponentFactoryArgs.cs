using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure;
using TupleAlgebraClassLib.FullAttributeComponentInfrastructure;
using LINQProvider;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.Default;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure
{
    public interface IAttributeComponentFactoryArgs
    {
        AttributeComponent<TData>
            ProvideTo<TData>(IAttributeComponentFactory<TData> factory)
        { return default; }
    }

    public interface IAttributeComponentFactoryArgs<TData>
    {
        AttributeComponent<TData>
            ProvideTo(IAttributeComponentFactory<TData> factory)
        { return default; }
    }

    public interface INonFictionalAttributeComponentFactoryArgs<TFactoryArgs>
        : IAttributeComponentFactoryArgs
        where TFactoryArgs : AttributeComponentFactoryArgs, INonFictionalAttributeComponentFactoryArgs<TFactoryArgs>
    {
        AttributeComponent<TData> IAttributeComponentFactoryArgs
            .ProvideTo<TData>(IAttributeComponentFactory<TData> factory)
        {
            return factory.CreateNonFictional((this as NonFictionalAttributeComponentFactoryArgs<TData>)!);
        }
    }

    public interface INonFictionalAttributeComponentFactoryArgs<TData, TFactoryArgs>
        : IAttributeComponentFactoryArgs<TData>
        where TFactoryArgs : NonFictionalAttributeComponentFactoryArgs<TData>, INonFictionalAttributeComponentFactoryArgs<TData, TFactoryArgs>
    {
        AttributeComponent<TData> IAttributeComponentFactoryArgs<TData>
            .ProvideTo(IAttributeComponentFactory<TData> factory)
        {
            return factory.CreateNonFictional(this as TFactoryArgs);
        }
    }

    public abstract record AttributeComponentFactoryArgs
    {
        protected static IQueryProvider _queryProvider =
            new DefaultAttributeComponentQueryProvider();

        public AttributeComponentPower Power { get; set; }

        public bool IsQuery { get; set; }

        public IQueryProvider QueryProvider { get; init; }

        public Expression QueryExpression { get; set; }

        protected AttributeComponentFactoryArgs(
            bool isQuery,
            IQueryProvider queryProvider,
            Expression queryExpression)
        {
            Power = null;
            IsQuery = isQuery;
            QueryProvider = queryProvider;
            QueryExpression = queryExpression;

            return;
        }

        protected AttributeComponentFactoryArgs(
            AttributeComponentPower power,
            bool isQuery,
            IQueryProvider queryProvider = null, 
            Expression queryExpression = null)
            : this(isQuery, queryProvider, queryExpression)
        {
            Power = power;

            return;
        }
    }

    public record EmptyAttributeComponentFactoryArgs 
        : AttributeComponentFactoryArgs
    {
        public EmptyAttributeComponentFactoryArgs(
            Expression queryExpression = null)
            : base(EmptyAttributeComponentPower.Instance, false, _queryProvider, queryExpression)
        {
            return;
        }
    }

    public abstract record NonFictionalAttributeComponentFactoryArgs<TData>
        : AttributeComponentFactoryArgs, IAttributeComponentFactoryArgs
    {
        public NonFictionalAttributeComponentFactoryArgs(
            bool isQuery,
            IQueryProvider queryProvider,
            Expression queryExpression)
            : base(isQuery, queryProvider, queryExpression)
        {
            Power = CreatePower();

            return;
        }

        protected abstract AttributeComponentPower CreatePower();
    }

    public record FullAttributeComponentFactoryArgs 
        : AttributeComponentFactoryArgs
    {
        public FullAttributeComponentFactoryArgs(
            Expression queryExpression = null)
            : base(new FullAttributeComponentPower(), false, _queryProvider, queryExpression)
        {
            return;
        }
    }
}
