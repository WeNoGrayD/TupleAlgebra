using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraFrameworkTests.AttributeComponentTests
{
    [TestClass]
    public class FiniteIterableAttributeComponentTests
        : IntegerAttributeComponentTests
    {
        private static Lazy<INonFictionalAttributeComponentFactory<int, IEnumerable<int>>>
            _op2Factory;

        private static INonFictionalAttributeComponentFactory<int, IEnumerable<int>>
            Op2Factory
        { get => _op2Factory.Value; }

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            if (_op2Factory is null)
            {
                _op2Factory = new Lazy<INonFictionalAttributeComponentFactory<int, IEnumerable<int>>>(() =>
                    new UnorderedFiniteEnumerableAttributeComponentFactory<int>(DomainValues.ToHashSet()));
            }   

            return;
        }

        protected override IEnumerableNonFictionalAttributeComponentFactory<int> InitOperandFactory()
        {
            return new FiniteIterableAttributeComponentFactory<int>(DomainValues);
        }

        protected override AttributeComponent<int> CreateNonFictionalOperand2(IEnumerable<int> values)
        {
            return Op2Factory.CreateNonFictional(values);
        }
    }
}
