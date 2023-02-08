using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure
{
    public interface INonFictionalAttributeComponentFactory<TValue, TFactoryArgs>
        where TFactoryArgs : AttributeComponentFactoryArgs<TValue>
    {
        NonFictionalAttributeComponent<TValue> CreateSpecificNonFictional(TFactoryArgs args);
    }
}
