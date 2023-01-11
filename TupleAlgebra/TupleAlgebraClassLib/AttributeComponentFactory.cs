using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib
{
    public class AttributeComponentFactoryArgs<TValue>
        where TValue : IComparable<TValue>
    {
        public readonly AttributeDomain<TValue> Domain;

        public AttributeComponentFactoryArgs(AttributeDomain<TValue> domain)
        {
            Domain = domain;
        }
    }

    public abstract class AttributeComponentFactory<TValue> 
        where TValue : IComparable<TValue>
    {
        public EmptyAttributeComponent<TValue> CreateEmpty()
        {
            return EmptyAttributeComponent<TValue>.Instance;
        }

        public AttributeComponent<TValue> CreateNonFictional(AttributeComponentFactoryArgs<TValue> args)
        {
            NonFictionalAttributeComponent<TValue> nonFictional = CreateSpecificNonFictional(args);
            if (nonFictional.IsEmpty())
                return CreateEmpty();
            else if (nonFictional.IsFull())
                return CreateFull(args);
            else
                return nonFictional;
        }

        protected abstract NonFictionalAttributeComponent<TValue> CreateSpecificNonFictional(
            AttributeComponentFactoryArgs<TValue> args);

        public virtual FullAttributeComponent<TValue> CreateFull(AttributeComponentFactoryArgs<TValue> args)
        {
            return new FullAttributeComponent<TValue>(args.Domain);
        }
    }
}
