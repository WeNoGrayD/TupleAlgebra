using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Streaming;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.Default;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Boolean;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Iterable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using static TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Boolean.BooleanAttributeComponentFactory;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Boolean
{
    public interface IBooleanAttributeComponentFactory
        : INonFictionalAttributeComponentFactory<
              bool,
              bool,
              BooleanNonFictionalAttributeComponent,
              BooleanAttributeComponentFactoryArgs>,
          INonFictionalAttributeComponentFactory<
              bool,
              IEnumerable<bool>,
              BooleanNonFictionalAttributeComponent,
              BooleanAttributeComponentFactoryArgs>
    {
        BooleanAttributeComponentFactoryArgs
            CreateFactoryArgs_Default(
                bool resultElements)
        {
            return new BooleanAttributeComponentFactoryArgs(resultElements);
        }

        BooleanAttributeComponentFactoryArgs
            ISetOperationResultFactory<
                BooleanNonFictionalAttributeComponent,
                bool,
                BooleanAttributeComponentFactoryArgs,
                AttributeComponent<bool>>
            .CreateFactoryArgs(
                bool resultElements)
        {
            return CreateFactoryArgs_Default(resultElements);
        }

        BooleanAttributeComponentFactoryArgs
            ISetOperationResultFactory<
                BooleanNonFictionalAttributeComponent,
                IEnumerable<bool>,
                BooleanAttributeComponentFactoryArgs,
                AttributeComponent<bool>>
            .CreateFactoryArgs(
                IEnumerable<bool> resultElements)
        {
            return CreateFactoryArgs_Default(resultElements.Single());
        }

        BooleanAttributeComponentFactoryArgs
            CreateFactoryArgs_Default(
            BooleanNonFictionalAttributeComponent first,
            bool resultElements)
        {
            return CreateFactoryArgs(resultElements);
        }

        BooleanAttributeComponentFactoryArgs
            ISetOperationResultFactory<
                BooleanNonFictionalAttributeComponent,
                bool,
                BooleanAttributeComponentFactoryArgs,
                AttributeComponent<bool>>
            .CreateFactoryArgs(
            BooleanNonFictionalAttributeComponent first,
            bool resultElements)
        {
            return CreateFactoryArgs_Default(first, resultElements);
        }

        BooleanAttributeComponentFactoryArgs
            ISetOperationResultFactory<
                BooleanNonFictionalAttributeComponent,
                IEnumerable<bool>,
                BooleanAttributeComponentFactoryArgs,
                AttributeComponent<bool>>
            .CreateFactoryArgs(
            BooleanNonFictionalAttributeComponent first,
            IEnumerable<bool> resultElements)
        {
            return CreateFactoryArgs_Default(first, resultElements.First());
        }

        NonFictionalAttributeComponent<bool>
            CreateSpecificNonFictional_Default(
                BooleanAttributeComponentFactoryArgs args)
        {
            bool bValue = args.Value;

            return new BooleanNonFictionalAttributeComponent(
                args.Power, bValue);
        }

        NonFictionalAttributeComponent<bool>
            INonFictionalAttributeComponentFactory2<
                bool,
                BooleanAttributeComponentFactoryArgs>
            .CreateSpecificNonFictional(
                BooleanAttributeComponentFactoryArgs args)
        {
            return CreateSpecificNonFictional_Default(args);
        }
    }

    public class BooleanAttributeComponentFactory
        : AttributeComponentFactory<bool, BooleanAttributeDomainFactoryArgs>,
          IBooleanAttributeComponentFactory,
          IFiniteIterableAttributeComponentFactory<bool>
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
            : base(new BooleanAttributeDomainFactoryArgs())
        { }

        #endregion

        AttributeComponent<bool>
            INonFictionalAttributeComponentFactory<
                bool,
                IEnumerable<bool>>
            .CreateNonFictional(IEnumerable<bool> values)
        {
            IBooleanAttributeComponentFactory booleanFactory = this;

            return booleanFactory.ProduceOperationResult(values);
        }
    }
}
