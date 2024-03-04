using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure
{
    public interface INonFictionalAttributeComponentFactory<
        TData,
        in CTFactoryArgs>
        : IAttributeComponentFactory<TData>
        where CTFactoryArgs : AttributeComponentFactoryArgs
    {
        abstract NonFictionalAttributeComponent<TData>
            CreateSpecificNonFictional(CTFactoryArgs args);
    }

    public interface INonFictionalAttributeComponentFactory<
        TData,
        in CTNonFictionalAttributeComponent,
        CTFactoryArgs>
        : INonFictionalAttributeComponentFactory<TData, CTFactoryArgs>,
          ISetOperationResultFactory<
              CTNonFictionalAttributeComponent,
              IEnumerable<TData>,
              CTFactoryArgs,
              AttributeComponent<TData>>
        where CTNonFictionalAttributeComponent : NonFictionalAttributeComponent<TData>
        where CTFactoryArgs : AttributeComponentFactoryArgs
    {
        protected AttributeComponent<TData> 
            ProduceOperationResult_DefaultImpl(
                CTNonFictionalAttributeComponent first,
                IEnumerable<TData> resultElements)
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
                IEnumerable<TData>,
                CTFactoryArgs,
                AttributeComponent<TData>>
            .ProduceOperationResult(
                CTNonFictionalAttributeComponent first,
                IEnumerable<TData> resultElements)
        {
            return ProduceOperationResult_DefaultImpl(first, resultElements);
        }

        protected static CTFactoryArgs CreateFactoryArgs_DefaultImpl(
            CTNonFictionalAttributeComponent first,
            IEnumerable<TData> resultElements)
        {
            return (first.ZipInfo(resultElements, true) as CTFactoryArgs)!;
        }

        CTFactoryArgs
            ISetOperationResultFactory<
                CTNonFictionalAttributeComponent,
                IEnumerable<TData>,
                CTFactoryArgs,
                AttributeComponent<TData>>
            .CreateFactoryArgs(
            CTNonFictionalAttributeComponent first,
            IEnumerable<TData> resultElements)
        {
            return CreateFactoryArgs_DefaultImpl(first, resultElements);
        }

        sealed AttributeComponent<TData> CreateNonFictional(
            CTNonFictionalAttributeComponent first,
            IEnumerable<TData> resultElements)
        {
            return ProduceOperationResult(first, resultElements);
        }
    }
}
