using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure
{
    /// <summary>
    /// Статический класс для создания нефиктивной компоненты аттрибута известного вида
    /// и произвольным параметром типа значения.
    /// </summary>
    public static class AttributeComponentGeneralFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="factoryType"></param>
        /// <param name="factoryArgs"></param>
        /// <returns></returns>
        public static AttributeComponent<TData> CreateNonFictional<TData>(
            Type factoryType, 
            AttributeComponentFactoryArgs<TData> factoryArgs)
        {
            AttributeComponentFactory<TData> componentFactory = 
                factoryType.GetConstructor(new Type[0]).Invoke(null) as AttributeComponentFactory<TData>;

            return componentFactory.CreateNonFictional(factoryArgs);
        }
    }
}
