﻿using System;
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

    public interface NonFictionalAttributeComponentFactory<TValue, TFactoryArgs>
        where TValue : IComparable<TValue>
        where TFactoryArgs : AttributeComponentFactoryArgs<TValue>
    {
        NonFictionalAttributeComponent<TValue> CreateSpecificNonFictional(TFactoryArgs args);
    }

    public class AttributeComponentFactory<TValue> 
        where TValue : IComparable<TValue>
    {
        public EmptyAttributeComponent<TValue> CreateEmpty()
        {
            return EmptyAttributeComponent<TValue>.Instance;
        }

        /// <summary>
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public AttributeComponent<TValue> CreateNonFictional(AttributeComponentFactoryArgs<TValue> args)
        {
            /*
             * Предполагается, что проверка компоненты на пустотность
             * является более простой, чем проверка на полноту.
             */
            NonFictionalAttributeComponent<TValue> nonFictional = CreateSpecificNonFictional((dynamic)args);
            if (nonFictional.IsEmpty())
                return CreateEmpty();
            else if (nonFictional.IsFull())
                return CreateFull(args);
            else
                return nonFictional;
        }

        protected NonFictionalAttributeComponent<TValue> CreateSpecificNonFictional<TFactoryArgs>(TFactoryArgs args)
            where TFactoryArgs : AttributeComponentFactoryArgs<TValue>
        {
            return (this as NonFictionalAttributeComponentFactory<TValue, TFactoryArgs>).CreateSpecificNonFictional(args);
        }

        public virtual FullAttributeComponent<TValue> CreateFull(AttributeComponentFactoryArgs<TValue> args)
        {
            return new FullAttributeComponent<TValue>(args.Domain);
        }
    }
}
