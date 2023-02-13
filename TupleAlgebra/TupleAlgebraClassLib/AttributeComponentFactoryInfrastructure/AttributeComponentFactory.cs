using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure
{
    public class AttributeComponentFactory<TData>
    {
        public virtual EmptyAttributeComponent<TData> CreateEmpty(
            AttributeComponentFactoryArgs<TData> factoryArgs)
        {
            return new EmptyAttributeComponent<TData>(
                factoryArgs.Domain,
                factoryArgs.QueryProvider,
                factoryArgs.QueryExpression);
        }

        /// <summary>
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public AttributeComponent<TData> CreateNonFictional(
            AttributeComponentFactoryArgs<TData> factoryArgs)
        {
            /*
             * Предполагается, что проверка компоненты на пустотность
             * является более простой, чем проверка на полноту.
             */
            NonFictionalAttributeComponent<TData> nonFictional = 
                CreateSpecificNonFictional((dynamic)factoryArgs);
            if (nonFictional.IsEmpty())
                return CreateEmpty(factoryArgs);
            else if (nonFictional.IsFull())
                return CreateFull(factoryArgs);
            else
                return nonFictional;
        }

        protected NonFictionalAttributeComponent<TData> CreateSpecificNonFictional<TFactoryArgs>(
            TFactoryArgs factoryArgs)
            where TFactoryArgs : AttributeComponentFactoryArgs<TData>
        {
            return (this as INonFictionalAttributeComponentFactory<TData, TFactoryArgs>)
                .CreateSpecificNonFictional(factoryArgs);
        }

        public virtual FullAttributeComponent<TData> CreateFull(
            AttributeComponentFactoryArgs<TData> factoryArgs)
        {
            return new FullAttributeComponent<TData>(
                factoryArgs.Domain,
                factoryArgs.QueryProvider,
                factoryArgs.QueryExpression);
        }
    }
}
