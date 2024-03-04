using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Boolean;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Boolean
{
    public interface IBooleanAttributeComponentFactory
        : INonFictionalAttributeComponentFactory<
              bool,
              BooleanNonFictionalAttributeComponent,
              BooleanAttributeComponentFactoryArgs>
    {
        NonFictionalAttributeComponent<bool>
            INonFictionalAttributeComponentFactory<
                bool,
                BooleanAttributeComponentFactoryArgs>
            .CreateSpecificNonFictional(
                BooleanAttributeComponentFactoryArgs args)
        {
            bool bValue = args.Value;

            return new BooleanNonFictionalAttributeComponent(
                args.Power, bValue);
        }
    }

    public class BooleanAttributeComponentFactory
        : OrderedFiniteEnumerableAttributeComponentFactory<bool>,
          IBooleanAttributeComponentFactory
    {

        #region Static properties

        public static BooleanAttributeComponentFactory Instance 
        { get; private set; }

        #endregion

        #region Constructors

        static BooleanAttributeComponentFactory()
        {
            Instance = new BooleanAttributeComponentFactory();

            return;
        }

        private BooleanAttributeComponentFactory()
            : base(new bool[2] { false, true })
        { }

        #endregion
    }
}
