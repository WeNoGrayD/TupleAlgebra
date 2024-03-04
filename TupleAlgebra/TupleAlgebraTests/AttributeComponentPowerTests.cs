using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable.Streaming;

namespace TupleAlgebraTests
{
    /// <summary>
    /// Тесты мощностей компонент атрибутов.
    /// </summary>
    [TestClass]
    public class AttributeComponentPowerTests
    {
        /// <summary>
        /// Тест переводимости мощности домена универсума в мощность компоненты универсума.
        /// </summary>
        /*
        [TestMethod]
        public void AttributeUniversePowerConversionTest()
        {
            var ofePower = new OrderedFiniteEnumerableNonFictionalAttributeComponentPower<int>();
            var attributeUniversePower = new AttributeDomain<int>.AttributeUniversePower(ofePower);

            AttributeComponentPower acPower = attributeUniversePower;
            NonFictionalAttributeComponentPower<int> nfPower = acPower
                .As<NonFictionalAttributeComponentPower<int>>();
            OrderedFiniteEnumerableNonFictionalAttributeComponentPower<int> ofePowerConverted = acPower
                .As<OrderedFiniteEnumerableNonFictionalAttributeComponentPower<int>>();
        }
        */
    }
}
