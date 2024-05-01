using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.Filtering;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraFrameworkTests.AttributeComponentTests.CrossTypeTests
{
    public abstract class CrossTypeFiniteEnumerableXFilteringAttributeComponentTests<
        TData,
        TOperandFactory>
        : AttributeComponentTests<
            TData,
            IEnumerable<TData>,
            FilteringAttributeComponentFactoryArgs<TData>,
            TOperandFactory>
        where TOperandFactory : INonFictionalAttributeComponentFactory<
            TData,
            IEnumerable<TData>>
    {
        private IFilteringAttributeComponentFactory<TData> _op2Factory;

        protected abstract IEnumerable<TData> DomainValues { get; }

        protected override FilteringAttributeComponentFactoryArgs<TData>
            GetEmptyValues()
        {
            return new FilteringAttributeComponentFactoryArgs<TData>(
                (_) => false);
        }

        protected override FilteringAttributeComponentFactoryArgs<TData>
            GetFullValues()
        {
            return new FilteringAttributeComponentFactoryArgs<TData>(
                (_) => true);
        }

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            _op2Factory = InitOperand2Factory();

            return;
        }

        protected virtual IFilteringAttributeComponentFactory<TData>
            InitOperand2Factory()
        {
            return new FilteringAttributeComponentFactory<TData>(OperandFactory.Domain);
        }

        protected override AttributeComponent<TData>
            CreateNonFictionalOperand2(FilteringAttributeComponentFactoryArgs<TData> values)
        {
            return _op2Factory.CreateNonFictional(values);
        }

        protected override bool OperationResultEquals(
            IEnumerable<TData> resultPattern,
            IAttributeComponent<TData> result)
        {
            return resultPattern.ToHashSet().SetEquals(result.AsEnumerable().ToHashSet());
        }

        #region Predefined operation methods

        protected override IEnumerable<TData> ComplementionPredefined(
            IEnumerable<TData> first)
        {
            HashSet<TData> domain = DomainValues.ToHashSet();
            domain.ExceptWith(first);

            return domain;
        }

        protected override IEnumerable<TData> IntersectPredefined(
            IEnumerable<TData> first,
            FilteringAttributeComponentFactoryArgs<TData> second)
        {
            HashSet<TData> firstHS = first.ToHashSet(),
                           domain = DomainValues.ToHashSet(),
                           secondHS = domain.Where(second.PredicateExpression.Compile()).ToHashSet();
            firstHS.IntersectWith(secondHS);

            return firstHS;
        }

        protected override IEnumerable<TData> UnionPredefined(
            IEnumerable<TData> first,
            FilteringAttributeComponentFactoryArgs<TData> second)
        {
            HashSet<TData> firstHS = first.ToHashSet(),
                           domain = DomainValues.ToHashSet(),
                           secondHS = domain.Where(second.PredicateExpression.Compile()).ToHashSet();
            firstHS.UnionWith(secondHS);

            return firstHS;
        }

        protected override IEnumerable<TData> ExceptionPredefined(
            IEnumerable<TData> first,
            FilteringAttributeComponentFactoryArgs<TData> second)
        {
            HashSet<TData> firstHS = first.ToHashSet(),
                           domain = DomainValues.ToHashSet(),
                           secondHS = domain.Where(second.PredicateExpression.Compile()).ToHashSet();
            firstHS.ExceptWith(secondHS);

            return firstHS;
        }

        protected override IEnumerable<TData> SymmetricExceptionPredefined(
            IEnumerable<TData> first,
            FilteringAttributeComponentFactoryArgs<TData> second)
        {
            HashSet<TData> firstHS = first.ToHashSet(),
                           domain = DomainValues.ToHashSet(),
                           secondHS = domain.Where(second.PredicateExpression.Compile()).ToHashSet();
            firstHS.SymmetricExceptWith(secondHS);

            return firstHS;
        }

        protected override bool IncludePredefined(
            IEnumerable<TData> first,
            FilteringAttributeComponentFactoryArgs<TData> second)
        {
            HashSet<TData> firstHS = first.ToHashSet(),
                           domain = DomainValues.ToHashSet(),
                           secondHS = domain.Where(second.PredicateExpression.Compile()).ToHashSet();
            
            return firstHS.IsProperSupersetOf(secondHS);
        }

        protected override bool EqualPredefined(
            IEnumerable<TData> first,
            FilteringAttributeComponentFactoryArgs<TData> second)
        {
            HashSet<TData> firstHS = first.ToHashSet(),
                           domain = DomainValues.ToHashSet(),
                           secondHS = domain.Where(second.PredicateExpression.Compile()).ToHashSet();
            
            return firstHS.SetEquals(secondHS);
        }

        protected override bool IncludeOrEqualPredefined(
            IEnumerable<TData> first,
            FilteringAttributeComponentFactoryArgs<TData> second)
        {
            HashSet<TData> firstHS = first.ToHashSet(),
                           domain = DomainValues.ToHashSet(),
                           secondHS = domain.Where(second.PredicateExpression.Compile()).ToHashSet();

            return firstHS.IsSupersetOf(secondHS);
        }

        #endregion
    }

    public abstract class IntegerCrossTypeFiniteEnumerableXFilteringAttributeComponentTests<
        TOperandFactory>
        : CrossTypeFiniteEnumerableXFilteringAttributeComponentTests<
            int,
            TOperandFactory>
        where TOperandFactory : INonFictionalAttributeComponentFactory<
            int,
            IEnumerable<int>>
    {
        protected IEnumerable<int> _domainValues = Enumerable.Range(0, 10);

        protected override IEnumerable<int> DomainValues
        { get => _domainValues; }

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
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d >= 3 & d <= 5));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.EmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                [3, 4, 5],
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d < 3));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.EmptyIntersection |
                TestCaseInfo.FullUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.FullSymmetricException,
                [0, 2, 4, 6, 8],
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => (d & 0b1) == 1));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                [0, 2, 4, 6, 8],
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => new int[] { 0, 3, 5, 7, 9 }.Contains(d)));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                [0, 2, 4, 6, 8],
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => new int[] { 1, 3, 4, 7, 9 }.Contains(d)));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                [0, 2, 5, 6, 8],
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => (d & 0b1) == 1));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                [0, 2, 4, 6, 9],
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => (d & 0b1) == 1));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                [0, 2, 4, 6, 8],
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => new int[] { 0, 2, 5, 7, 9 }.Contains(d)));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                [0, 3, 4, 6, 8],
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => new int[] { 1, 3, 4, 6, 9 }.Contains(d)));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                [0, 2, 4, 7, 9],
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => (d & 0b1) == 1));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException |
                TestCaseInfo.FirstGreaterThanSecond,
                [0, 1, 2, 3, 4, 5],
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d == 3 || d == 5));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.EmptyException |
                TestCaseInfo.NonEmptySymmetricException |
                TestCaseInfo.FirstLowerThanSecond,
                [1, 3, 5],
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d < 6));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.EmptyException |
                TestCaseInfo.EmptySymmetricException |
                TestCaseInfo.FirstEqualToSecond,
                [0],
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d == 0));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.EmptyException |
                TestCaseInfo.EmptySymmetricException |
                TestCaseInfo.FirstEqualToSecond,
                [0, 4, 8],
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d == 0 || d == 4 || d == 8));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.EmptyException |
                TestCaseInfo.EmptySymmetricException |
                TestCaseInfo.FirstEqualToSecond,
                [0, 2, 4, 6, 8],
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => (d & 0b1) == 0));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.EmptyException |
                TestCaseInfo.EmptySymmetricException |
                TestCaseInfo.FirstEqualToSecond,
                [0, 1, 2, 3, 4, 5, 6, 7, 8],
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d < 9));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.EmptyException |
                TestCaseInfo.EmptySymmetricException |
                TestCaseInfo.FirstEqualToSecond,
                [1, 2, 3, 4, 5, 6, 7, 8],
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d > 0 && d < 9));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.EmptyException |
                TestCaseInfo.EmptySymmetricException |
                TestCaseInfo.FirstEqualToSecond,
                [1, 2, 3, 6, 7, 8],
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => (d > 0 && d <= 3) || (d >= 6 && d < 9)));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.EmptyException |
                TestCaseInfo.EmptySymmetricException |
                TestCaseInfo.FirstEqualToSecond,
                [1, 2, 3, 4, 5, 6, 7, 8, 9],
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d > 0 && d <= 9));
            yield return new SingleTestData(
                TestCaseInfo.EmptyComplementionOp1 |
                TestCaseInfo.EmptyComplementionOp2 |
                TestCaseInfo.FullIntersection |
                TestCaseInfo.FullUnion |
                TestCaseInfo.EmptyException |
                TestCaseInfo.EmptySymmetricException |
                TestCaseInfo.FirstEqualToSecond,
                [0, 1, 2, 3, 4, 5, 6, 7, 8, 9],
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d >= 0 && d <= 9, AttributeComponentContentType.Full));

            yield break;
        }
    }

    [TestClass]
    public class OrderedXFilteringAttributeComponentTests
        : IntegerCrossTypeFiniteEnumerableXFilteringAttributeComponentTests<
            OrderedFiniteEnumerableAttributeComponentFactory<int>>
    {
        protected override OrderedFiniteEnumerableAttributeComponentFactory<int>
            InitOperandFactory()
        {
            return new OrderedFiniteEnumerableAttributeComponentFactory<int>(DomainValues);
        }
    }

    [TestClass]
    public class IterableXFilteringAttributeComponentTests
        : IntegerCrossTypeFiniteEnumerableXFilteringAttributeComponentTests<
            FiniteIterableAttributeComponentFactory<int>>
    {
        protected override FiniteIterableAttributeComponentFactory<int>
            InitOperandFactory()
        {
            return new FiniteIterableAttributeComponentFactory<int>(DomainValues);
        }
    }

    [TestClass]
    public class UnorderedXFilteringAttributeComponentTests
        : IntegerCrossTypeFiniteEnumerableXFilteringAttributeComponentTests<
            UnorderedFiniteEnumerableAttributeComponentFactory<int>>
    {
        protected override UnorderedFiniteEnumerableAttributeComponentFactory<int>
            InitOperandFactory()
        {
            return new UnorderedFiniteEnumerableAttributeComponentFactory<int>(DomainValues.ToHashSet());
        }
    }
}
