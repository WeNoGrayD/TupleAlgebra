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

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure
{
    public interface IAttributeComponentFactoryArgs
    {
        AttributeComponent<TData>
            ProvideTo<TData>(IAttributeComponentFactory<TData> factory);
    }

    public interface INonFictionalAttributeComponentFactoryArgs<TFactoryArgs>
        : IAttributeComponentFactoryArgs
        where TFactoryArgs : AttributeComponentFactoryArgs, INonFictionalAttributeComponentFactoryArgs<TFactoryArgs>
    {
        AttributeComponent<TData> IAttributeComponentFactoryArgs
            .ProvideTo<TData>(IAttributeComponentFactory<TData> factory)
        {
            return factory.CreateNonFictional((this as TFactoryArgs)!);
        }
    }

    public abstract record AttributeComponentFactoryArgs
    {
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
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(EmptyAttributeComponentPower.Instance, false, queryProvider, queryExpression)
        {
            return;
        }
    }

    public abstract record NonFictionalAttributeComponentFactoryArgs<TData>
        : AttributeComponentFactoryArgs
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
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(new FullAttributeComponentPower(), false, queryProvider, queryExpression)
        {
            return;
        }
    }
}
