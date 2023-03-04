using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure
{
    public interface INonFictionalAttributeComponentFactory<TData, TFactoryArgs>
        where TFactoryArgs : AttributeComponentFactoryArgs<TData>
    {
        NonFictionalAttributeComponent<TData> CreateSpecificNonFictional(TFactoryArgs args);
    }
}
