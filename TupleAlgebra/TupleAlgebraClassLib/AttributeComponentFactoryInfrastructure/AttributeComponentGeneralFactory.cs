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
        /// <typeparam name="TValue"></typeparam>
        /// <param name="factoryType"></param>
        /// <param name="factoryArgs"></param>
        /// <returns></returns>
        public static AttributeComponent<TValue> CreateNonFictional<TValue>(
            Type factoryType, 
            AttributeComponentFactoryArgs<TValue> factoryArgs)
        {
            AttributeComponentFactory<TValue> componentFactory = 
                factoryType.GetConstructor(new Type[0]).Invoke(null) as AttributeComponentFactory<TValue>;

            return componentFactory.CreateNonFictional(factoryArgs);
        }
    }
}
