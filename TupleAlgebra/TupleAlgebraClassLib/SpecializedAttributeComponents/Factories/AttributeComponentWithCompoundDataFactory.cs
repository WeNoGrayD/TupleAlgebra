using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;
using System.Reflection;

namespace TupleAlgebraClassLib.SpecializedAttributeComponents.Factories
{
    public abstract class AttributeComponentWithCompoundDataFactory<TCompoundData, TFactoryArgs>
        : AttributeComponentFactory<TCompoundData>,
          INonFictionalAttributeComponentFactory<TCompoundData, TFactoryArgs>
        where TFactoryArgs : AttributeComponentFactoryArgs
    {
        private MethodInfo _createSpecificNonFictionalImplMIPattern;

        public AttributeComponentWithCompoundDataFactory(
            string createSpecificNonFictionalImplMethodName,
            BindingFlags methodFlags)
            : base(null)
        {
            _createSpecificNonFictionalImplMIPattern = this.GetType().
                GetMethod(createSpecificNonFictionalImplMethodName,
                          methodFlags);

            return;
        }

        public AttributeComponentWithCompoundDataFactory(
            string createSpecificNonFictionalImplMethodName)
            : this(createSpecificNonFictionalImplMethodName,
                   BindingFlags.Instance | BindingFlags.NonPublic)
        {
            return;
        }

        public NonFictionalAttributeComponent<TCompoundData> CreateSpecificNonFictional(
            TFactoryArgs args)
        {
            return _createSpecificNonFictionalImplMIPattern
                .MakeGenericMethod(typeof(TCompoundData).GetGenericArguments())
                .Invoke(this, new object[] { args })
                as NonFictionalAttributeComponent<TCompoundData>;
        }
    }
}
