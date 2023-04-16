﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure
{
    public class AttributeComponentFactory
    {
        /// <summary>
        /// Создание пустой фиктивной компоненты атрибута.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="factoryArgs"></param>
        /// <returns></returns>
        public virtual EmptyAttributeComponent<TData> CreateEmpty<TData>(
            AttributeComponentFactoryArgs factoryArgs)
        {
            return new EmptyAttributeComponent<TData>(
                factoryArgs.Domain as AttributeDomain<TData>,
                queryExpression: factoryArgs.QueryExpression);
        }

        /// <summary>
        /// Создание нефиктивной компоненты атрибута.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="factoryArgs"></param>
        /// <returns></returns>
        public AttributeComponent<TData> CreateNonFictional<TData>(
            AttributeComponentFactoryArgs factoryArgs)
        {
            /*
             * Предполагается, что проверка компоненты на пустотность
             * является более простой, чем проверка на полноту.
             */
            NonFictionalAttributeComponent<TData> nonFictional =  CreateSpecificNonFictional();
            if (nonFictional.IsEmpty())
                return CreateEmpty<TData>(factoryArgs);
            else if (nonFictional.IsFull())
                return CreateFull<TData>(factoryArgs);
            else
                return nonFictional;

            /*
             * Построение и вызов обобщённого метода создания нефиктивной компоненты атрибута
             * конкретного типа.
             */
            NonFictionalAttributeComponent<TData> CreateSpecificNonFictional()
            {
                return this.GetType()
                    .GetMethod(nameof(CreateSpecificNonFictional), BindingFlags.NonPublic | BindingFlags.Instance)
                    .MakeGenericMethod(factoryArgs.GetType(), typeof(TData))
                    .Invoke(this, new object[] { factoryArgs })
                    as NonFictionalAttributeComponent<TData>;
            }
        }

        /// <summary>
        /// Создание нефиктивной компоненты атрибута конкретного типа.
        /// </summary>
        /// <typeparam name="TFactoryArgs"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <param name="factoryArgs"></param>
        /// <returns></returns>
        protected NonFictionalAttributeComponent<TData> CreateSpecificNonFictional<TFactoryArgs, TData>(
            TFactoryArgs factoryArgs)
            where TFactoryArgs : AttributeComponentFactoryArgs
        {
            return (this as INonFictionalAttributeComponentFactory<TFactoryArgs>)
                .CreateSpecificNonFictional<TData>(factoryArgs);
        }

        /// <summary>
        /// Создание полной фиктивной компоненты атрибута.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="factoryArgs"></param>
        /// <returns></returns>
        public virtual FullAttributeComponent<TData> CreateFull<TData>(
            AttributeComponentFactoryArgs factoryArgs)
        {
            return new FullAttributeComponent<TData>(
                factoryArgs.Domain as AttributeDomain<TData>,
                queryExpression: factoryArgs.QueryExpression);
        }
    }
}
