﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Boolean;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Buffered;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Streaming;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Specialized.EnumBased
{
    public class EnumBasedAttributeComponentFactory<TData>
        /*
        : AttributeComponentFactory<TData, EnumBasedAttributeDomainFactoryArgs<TData>>,
          IBufferedOrderedFiniteEnumerableAttributeComponentFactory<TData>,
          IFiniteIterableAttributeComponentFactory<TData>
        */
        : OrderedFiniteEnumerableAttributeComponentFactory<TData>
        where TData : struct, Enum
    {
        public static EnumBasedAttributeComponentFactory<TData> Instance { get; private set; }

        static EnumBasedAttributeComponentFactory()
        {
            Instance = new EnumBasedAttributeComponentFactory<TData>();

            return;
        }

        private EnumBasedAttributeComponentFactory()
            : base(new EnumBasedAttributeDomainFactoryArgs<TData>())
        {
            return;
        }

        /*
        AttributeComponent<TData> INonFictionalAttributeComponentFactory<
            TData,
            IEnumerable<TData>>
            .CreateNonFictional(IEnumerable<TData> resultElements)
        {
            return (this as IBufferedOrderedFiniteEnumerableAttributeComponentFactory<TData>)
                .ProduceOperationResult(resultElements);
        }

        NonFictionalAttributeComponentFactoryArgs<TData> 
            INonFictionalAttributeComponentFactory<
            TData,
            IEnumerable<TData>>
            .CreateNonFictionalFactoryArgs(IEnumerable<TData> resultElements)
        {
            return (this as IBufferedOrderedFiniteEnumerableAttributeComponentFactory<TData>)
                .CreateFactoryArgs(resultElements);
        }
        */
    }

    public record EnumBasedAttributeDomainFactoryArgs<TData>
        //: FiniteIterableAttributeComponentFactoryArgs<TData>
        : BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>
        where TData : struct, Enum
    {
        public EnumBasedAttributeDomainFactoryArgs()
            : base(Enum.GetValues<TData>())
        {
            return;
        }
    }
}
