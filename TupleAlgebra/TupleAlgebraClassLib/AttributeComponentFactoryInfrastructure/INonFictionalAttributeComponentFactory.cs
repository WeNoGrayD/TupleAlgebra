using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure
{
    using static AttributeComponentFactoryHelper;

    public interface INonFictionalAttributeComponentFactory<
        TData,
        in TFactoryValues>
        : IAttributeComponentFactory<TData>
    {
        AttributeComponent<TData> CreateNonFictional(
            TFactoryValues resultElements);
    }

    public interface INonFictionalAttributeComponentFactory2<
        TData,
        in CTFactoryArgs>
        where CTFactoryArgs : AttributeComponentFactoryArgs
    {
        abstract NonFictionalAttributeComponent<TData>
            CreateSpecificNonFictional(CTFactoryArgs args);
    }

    public interface INonFictionalAttributeComponentFactory<
        TData,
        in TFactoryValues,
        in CTFactoryArgs>
        : INonFictionalAttributeComponentFactory<TData, TFactoryValues>,
          INonFictionalAttributeComponentFactory2<TData, CTFactoryArgs>
        where CTFactoryArgs : AttributeComponentFactoryArgs
    {
    }

    public interface INonFictionalAttributeComponentFactory<
        TData,
        in TFactoryValues,
        in CTNonFictionalAttributeComponent,
        CTFactoryArgs>
        : INonFictionalAttributeComponentFactory<
            TData,
            TFactoryValues, 
            CTFactoryArgs>,
          ISetOperationResultFactory<
              CTNonFictionalAttributeComponent,
              TFactoryValues,
              CTFactoryArgs,
              AttributeComponent<TData>>
        where CTNonFictionalAttributeComponent : NonFictionalAttributeComponent<TData>
        where CTFactoryArgs : AttributeComponentFactoryArgs
    {
        protected AttributeComponent<TData> 
            ProduceOperationResult_DefaultImpl(
                CTNonFictionalAttributeComponent first,
                TFactoryValues resultElements)
        {
            CTFactoryArgs factoryArgs = CreateFactoryArgs(
                first,
                resultElements);
            AttributeComponent<TData> resultComponent =
                CreateNonFictional(factoryArgs);

            return resultComponent;
        }

        AttributeComponent<TData>
            ISetOperationResultFactory<
                CTNonFictionalAttributeComponent,
                TFactoryValues,
                CTFactoryArgs,
                AttributeComponent<TData>>
            .ProduceOperationResult(
                CTNonFictionalAttributeComponent first,
                TFactoryValues resultElements)
        {
            return ProduceOperationResult_DefaultImpl(first, resultElements);
        }

        AttributeComponent<TData>
            ISetOperationResultFactory<
                CTNonFictionalAttributeComponent,
                TFactoryValues,
                CTFactoryArgs,
                AttributeComponent<TData>>
            .ProduceOperationResult(
                TFactoryValues resultElements)
        {
            CTFactoryArgs factoryArgs = CreateFactoryArgs(
                resultElements);
            AttributeComponent<TData> resultComponent =
                CreateNonFictional(factoryArgs);

            return resultComponent;
        }

        sealed AttributeComponent<TData> CreateNonFictional(
            CTNonFictionalAttributeComponent first,
            TFactoryValues resultElements)
        {
            return ProduceOperationResult(first, resultElements);
        }

        AttributeComponent<TData>
            INonFictionalAttributeComponentFactory<TData, TFactoryValues>
            .CreateNonFictional(
                TFactoryValues resultElements)
        {
            return ProduceOperationResult(resultElements);
        }
    }
}
