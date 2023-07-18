using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure
{
    public interface INonFictionalAttributeComponentFactory<TFactoryArgs>
        where TFactoryArgs : AttributeComponentFactoryArgs
    {
        NonFictionalAttributeComponent<TData> CreateSpecificNonFictional<TData>(TFactoryArgs args);
    }
}
