using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Complex;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Specialized.TextBased.ASCII;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using TupleAlgebraFrameworkTests.DataModels;

namespace TupleAlgebraFrameworkTests.AttributeComponentTests
{
    public class AlphabetTests 
        : FiniteEnumerableNonFictionalAttributeComponentTests<
            CharAlphabet,
            ComplexAttributeComponentFactory<CharAlphabet>>
    {
        private static bool _alphabetWasConfigured = false;

        protected override IEnumerable<CharAlphabet> DomainValues 
        { get => OperandFactory.Domain; }

        protected override ComplexAttributeComponentFactory<CharAlphabet> InitOperandFactory()
        {
            TupleObject<CharAlphabet>.Configure(CustomAlphabet);

            return new ComplexAttributeComponentFactory<CharAlphabet>();
        }

        protected override IEnumerable<SingleTestData> CreateTestCaseList()
        {
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.EmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                [0, 1, 2], 
                [3, 4, 5]);
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.EmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                [3, 4, 5], [0, 1, 2]);
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.EmptyIntersection |
                TestCaseInfo.FullUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.FullSymmetricException,
                [0, 2, 4, 6, 8], [1, 3, 5, 7, 9]);
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                [0, 2, 4, 6, 8], [0, 3, 5, 7, 9]);
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                [0, 2, 4, 6, 8], [1, 3, 4, 7, 9]);
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                [0, 2, 5, 6, 8], [1, 3, 5, 7, 9]);
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                [0, 2, 4, 6, 9], [1, 3, 5, 7, 9]);
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                [0, 2, 4, 6, 8], [0, 2, 5, 7, 9]);
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                [0, 3, 4, 6, 8], [1, 3, 4, 6, 9]);
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                [0, 2, 4, 7, 9], [1, 3, 5, 7, 9]);
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException |
                TestCaseInfo.FirstGreaterThanSecond,
                [0, 1, 2, 3, 4, 5], [3, 5]);
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.EmptyException |
                TestCaseInfo.NonEmptySymmetricException |
                TestCaseInfo.FirstLowerThanSecond,
                [1, 3, 5], [0, 1, 2, 3, 4, 5]);
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.EmptyException |
                TestCaseInfo.EmptySymmetricException |
                TestCaseInfo.FirstEqualToSecond,
                [0], [0]);
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.EmptyException |
                TestCaseInfo.EmptySymmetricException |
                TestCaseInfo.FirstEqualToSecond,
                [0, 4, 8], [0, 4, 8]);
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.EmptyException |
                TestCaseInfo.EmptySymmetricException |
                TestCaseInfo.FirstEqualToSecond,
                [0, 2, 4, 6, 8], [0, 2, 4, 6, 8]);
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.EmptyException |
                TestCaseInfo.EmptySymmetricException |
                TestCaseInfo.FirstEqualToSecond,
                [0, 1, 2, 3, 4, 5, 6, 7, 8], [0, 1, 2, 3, 4, 5, 6, 7, 8]);
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.EmptyException |
                TestCaseInfo.EmptySymmetricException |
                TestCaseInfo.FirstEqualToSecond,
                [1, 2, 3, 4, 5, 6, 7, 8], [1, 2, 3, 4, 5, 6, 7, 8]);
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.EmptyException |
                TestCaseInfo.EmptySymmetricException |
                TestCaseInfo.FirstEqualToSecond,
                [1, 2, 3, 6, 7, 8], [1, 2, 3, 6, 7, 8]);
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.EmptyException |
                TestCaseInfo.EmptySymmetricException |
                TestCaseInfo.FirstEqualToSecond,
                [1, 2, 3, 4, 5, 6, 7, 8, 9], [1, 2, 3, 4, 5, 6, 7, 8, 9]);
            yield return new SingleTestData(
                TestCaseInfo.EmptyComplementionOp1 |
                TestCaseInfo.EmptyComplementionOp2 |
                TestCaseInfo.FullIntersection |
                TestCaseInfo.FullUnion |
                TestCaseInfo.EmptyException |
                TestCaseInfo.EmptySymmetricException |
                TestCaseInfo.FirstEqualToSecond,
                [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);

            yield break;
        }

        public void CustomAlphabet(
            TupleObjectBuilder<CharAlphabet> builder)
        {
            if (_alphabetWasConfigured) return;
            _alphabetWasConfigured = true;

            IAttributeComponentFactory<char> charFactory =
                AsciiCharacterAttributeComponentFactory.Lower();

            builder.Attribute(abc => abc.A).SetFactory(charFactory).Attach();
            builder.Attribute(abc => abc.B).SetFactory(charFactory).Attach();
            builder.Attribute(abc => abc.C).SetFactory(charFactory).Attach();

            return;
        }
    }
}
