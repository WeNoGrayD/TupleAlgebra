using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraFrameworkTests.AttributeComponentTests.CrossTypeTests
{
    public abstract class CrossTypeFiniteEnumerableAttributeComponentTests<
        Op1Factory,
        Op2Factory>
        : IntegerAttributeComponentTests
        where Op1Factory : IEnumerableNonFictionalAttributeComponentFactory<int>
        where Op2Factory : IEnumerableNonFictionalAttributeComponentFactory<int>
    {
        private Op2Factory _op2Factory;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            _op2Factory = InitOperand2Factory();

            return;
        }

        protected override IEnumerableNonFictionalAttributeComponentFactory<int>
            InitOperandFactory() => InitOperand1Factory();

        protected abstract Op1Factory
            InitOperand1Factory();

        protected abstract Op2Factory
            InitOperand2Factory();

        protected override AttributeComponent<int> CreateNonFictionalOperand2(IEnumerable<int> values)
        {
            return _op2Factory.CreateNonFictional(values);
        }
    }

    [TestClass]
    public class OrderedXIterableAttributeComponentTests
        : CrossTypeFiniteEnumerableAttributeComponentTests<
            OrderedFiniteEnumerableAttributeComponentFactory<int>,
            FiniteIterableAttributeComponentFactory<int>>
    {
        protected override OrderedFiniteEnumerableAttributeComponentFactory<int>
            InitOperand1Factory()
        {
            return new OrderedFiniteEnumerableAttributeComponentFactory<int>(DomainValues);
        }

        protected override FiniteIterableAttributeComponentFactory<int>
            InitOperand2Factory()
        {
            return new FiniteIterableAttributeComponentFactory<int>(DomainValues);
        }
    }

    [TestClass]
    public class OrderedXUnorderedAttributeComponentTests
        : CrossTypeFiniteEnumerableAttributeComponentTests<
            OrderedFiniteEnumerableAttributeComponentFactory<int>,
            UnorderedFiniteEnumerableAttributeComponentFactory<int>>
    {
        protected override OrderedFiniteEnumerableAttributeComponentFactory<int>
            InitOperand1Factory()
        {
            return new OrderedFiniteEnumerableAttributeComponentFactory<int>(DomainValues);
        }

        protected override UnorderedFiniteEnumerableAttributeComponentFactory<int>
            InitOperand2Factory()
        {
            return new UnorderedFiniteEnumerableAttributeComponentFactory<int>(DomainValues.ToHashSet());
        }
    }

    [TestClass]
    public class UnorderedXIterableAttributeComponentTests
        : CrossTypeFiniteEnumerableAttributeComponentTests<
            UnorderedFiniteEnumerableAttributeComponentFactory<int>,
            FiniteIterableAttributeComponentFactory<int>>
    {
        protected override UnorderedFiniteEnumerableAttributeComponentFactory<int>
            InitOperand1Factory()
        {
            return new UnorderedFiniteEnumerableAttributeComponentFactory<int>(DomainValues.ToHashSet());
        }

        protected override FiniteIterableAttributeComponentFactory<int>
            InitOperand2Factory()
        {
            return new FiniteIterableAttributeComponentFactory<int>(DomainValues);
        }
    }
}
