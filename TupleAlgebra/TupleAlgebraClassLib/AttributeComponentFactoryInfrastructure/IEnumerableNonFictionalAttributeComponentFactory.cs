using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Variable;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure
{
    public interface IEnumerableNonFictionalAttributeComponentFactory<
        TData>
        : INonFictionalAttributeComponentFactory<
            TData,
            IEnumerable<TData>>
    { }

    public interface IEnumerableNonFictionalAttributeComponentFactory<
        TData,
        CTFactoryArgs>
        : INonFictionalAttributeComponentFactory<
            TData,
            IEnumerable<TData>,
            CTFactoryArgs>,
          IEnumerableNonFictionalAttributeComponentFactory<TData>
        where CTFactoryArgs : NonFictionalAttributeComponentFactoryArgs<TData>
    { }

    public interface IEnumerableNonFictionalAttributeComponentFactory<
        TData,
        in CTNonFictionalAttributeComponent,
        CTFactoryArgs>
        : INonFictionalAttributeComponentFactory<
            TData,
            IEnumerable<TData>,
            CTNonFictionalAttributeComponent,
            CTFactoryArgs>,
          IEnumerableNonFictionalAttributeComponentFactory<
            TData,
            CTFactoryArgs>
        where CTNonFictionalAttributeComponent : NonFictionalAttributeComponent<TData>
        where CTFactoryArgs : NonFictionalAttributeComponentFactoryArgs<TData>
    {
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
    }
}
