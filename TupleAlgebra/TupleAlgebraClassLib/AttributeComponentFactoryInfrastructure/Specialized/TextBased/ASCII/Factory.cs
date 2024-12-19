using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Buffered;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Specialized.EnumBased;
using System.Runtime.InteropServices;
using System.Globalization;
using System.ComponentModel;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Specialized.TextBased.ASCII
{
    public class AsciiCharacterAttributeComponentFactory
        : OrderedFiniteEnumerableAttributeComponentFactory<char>
    {
        public AsciiCharacterAttributeComponentFactory(
            IEnumerable<char> domainValues)
            : base(
                  new BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<char>(domainValues))
        {
            return;
        }

        public static AsciiCharacterAttributeComponentFactory Digits()
        {
            return new("0123456789");
        }

        public static AsciiCharacterAttributeComponentFactory Lower()
        {
            return new("abcdefghijklmnopqrvwustxyz");
        }

        public static AsciiCharacterAttributeComponentFactory Upper()
        {
            return new("ABCDEFGHIJKLMNOPQRVWUSTXYZ");
        }
    }
}
