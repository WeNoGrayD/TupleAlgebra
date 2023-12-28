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
    public abstract class AttributeComponentFactoryArgs
    {
        public AttributeComponentPower Power { get; set; }

        public Delegate DomainGetter { get; private set; }

        public bool IsQuery { get; set; }

        public readonly IQueryProvider QueryProvider;

        public Expression QueryExpression { get; set; }

        protected AttributeComponentFactoryArgs(
            bool isQuery,
            IQueryProvider queryProvider,
            Expression queryExpression)
        {
            Power = null;
            DomainGetter = null;
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

        public virtual void SetDomainGetter<TData>(Func<AttributeDomain<TData>> domainGetter)
        {
            DomainGetter = domainGetter;

            return;
        }
    }

    public class EmptyAttributeComponentFactoryArgs : AttributeComponentFactoryArgs
    {
        public EmptyAttributeComponentFactoryArgs(
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(EmptyAttributeComponentPower.Instance, false, queryProvider, queryExpression)
        {
            return;
        }
    }

    public abstract class NonFictionalAttributeComponentFactoryArgs : AttributeComponentFactoryArgs
    {
        public NonFictionalAttributeComponentFactoryArgs(
            bool isQuery,
            IQueryProvider queryProvider,
            Expression queryExpression)
            : base(isQuery, queryProvider, queryExpression)
        {
            return;
        }

        public override void SetDomainGetter<TData>(Func<AttributeDomain<TData>> domainGetter)
        {
            base.SetDomainGetter(domainGetter);
            if (Power is null) Power = CreatePower<TData>();

            return;
        }

        protected abstract AttributeComponentPower CreatePower<TData>();
    }

    public class FullAttributeComponentFactoryArgs : AttributeComponentFactoryArgs
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
