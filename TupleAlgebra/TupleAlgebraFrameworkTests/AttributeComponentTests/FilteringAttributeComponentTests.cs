using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.Filtering;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering;

namespace TupleAlgebraFrameworkTests.AttributeComponentTests
{
    [TestClass]
    public class FilteringAttributeComponentTests
        : AttributeComponentTests<
            int,
            FilteringAttributeComponentFactoryArgs<int>,
            FilteringAttributeComponentFactoryArgs<int>,
            TestFilteringAttributeComponentFactory>
    {
        protected IEnumerable<int> _domainValues = Enumerable.Range(0, 10);

        protected IEnumerable<int> DomainValues
        { get => _domainValues; }

        protected override FilteringAttributeComponentFactoryArgs<int>
            GetEmptyValues()
        {
            return new FilteringAttributeComponentFactoryArgs<int>(
                (_) => false);
        }

        protected override FilteringAttributeComponentFactoryArgs<int>
            GetFullValues()
        {
            return new FilteringAttributeComponentFactoryArgs<int>(
                (_) => true);
        }

        protected override TestFilteringAttributeComponentFactory
            InitOperandFactory()
        {
            return new TestFilteringAttributeComponentFactory(
                DomainValues.ToHashSet());
        }
         
        protected override bool OperationResultEquals(
            IEnumerable<int> resultPattern,
            IAttributeComponent<int> result)
        {
            return OperandFactory.CreateConstantNonFictional<
                UnorderedFiniteEnumerableNonFictionalAttributeComponent<int>,
                UnorderedFiniteEnumerableAttributeComponentFactoryArgs<int>>(
                    new UnorderedFiniteEnumerableAttributeComponentFactoryArgs<int>(
                        resultPattern.ToHashSet())) == result;
        }

        protected override AttributeComponent<int>
            CreateNonFictionalOperand2(
                FilteringAttributeComponentFactoryArgs<int> values)
        {
            return OperandFactory.CreateNonFictional(values);
        }

        #region Predefined operation methods

        protected override IEnumerable<int> ComplementionPredefined(
            FilteringAttributeComponentFactoryArgs<int> first)
        {
            HashSet<int> domain = DomainValues.ToHashSet();
            Func<int, bool> pred = first.PredicateExpression.Compile();

            return domain.Where((d) => !pred(d));
        }

        protected override IEnumerable<int> IntersectPredefined(
            FilteringAttributeComponentFactoryArgs<int> first,
            FilteringAttributeComponentFactoryArgs<int> second)
        {
            HashSet<int> domain = DomainValues.ToHashSet();
            Func<int, bool> pred1 = first.PredicateExpression.Compile(),
                pred2 = second.PredicateExpression.Compile(),
                pred = (d) => pred1(d) & pred2(d);

            return domain.Where(pred);
        }

        protected override IEnumerable<int> UnionPredefined(
            FilteringAttributeComponentFactoryArgs<int> first,
            FilteringAttributeComponentFactoryArgs<int> second)
        {
            HashSet<int> domain = DomainValues.ToHashSet();
            Func<int, bool> pred1 = first.PredicateExpression.Compile(),
                pred2 = second.PredicateExpression.Compile(),
                pred = (d) => pred1(d) | pred2(d);

            return domain.Where(pred);
        }

        protected override IEnumerable<int> ExceptionPredefined(
            FilteringAttributeComponentFactoryArgs<int> first,
            FilteringAttributeComponentFactoryArgs<int> second)
        {
            HashSet<int> domain = DomainValues.ToHashSet();
            Func<int, bool> pred1 = first.PredicateExpression.Compile(),
                pred2 = second.PredicateExpression.Compile(),
                pred = (d) => pred1(d) & !pred2(d);

            return domain.Where(pred);
        }

        protected override IEnumerable<int> SymmetricExceptionPredefined(
            FilteringAttributeComponentFactoryArgs<int> first,
            FilteringAttributeComponentFactoryArgs<int> second)
        {
            HashSet<int> domain = DomainValues.ToHashSet();
            Func<int, bool> pred1 = first.PredicateExpression.Compile(),
                pred2 = second.PredicateExpression.Compile(),
                pred = (d) => pred1(d) ^ pred2(d);

            return domain.Where(pred);
        }

        protected override bool IncludePredefined(
            FilteringAttributeComponentFactoryArgs<int> first,
            FilteringAttributeComponentFactoryArgs<int> second)
        {
            HashSet<int> domain = DomainValues.ToHashSet(),
                         firstHS, secondHS;
            Func<int, bool> pred1 = first.PredicateExpression.Compile(),
                pred2 = second.PredicateExpression.Compile();
            (firstHS, secondHS) = (domain.Where(pred1).ToHashSet(), domain.Where(pred2).ToHashSet());

            return firstHS.IsProperSupersetOf(secondHS);
        }

        protected override bool EqualPredefined(
            FilteringAttributeComponentFactoryArgs<int> first,
            FilteringAttributeComponentFactoryArgs<int> second)
        {
            HashSet<int> domain = DomainValues.ToHashSet(),
                         firstHS, secondHS;
            Func<int, bool> pred1 = first.PredicateExpression.Compile(),
                pred2 = second.PredicateExpression.Compile();
            (firstHS, secondHS) = (domain.Where(pred1).ToHashSet(), domain.Where(pred2).ToHashSet());

            return firstHS.SetEquals(secondHS);
        }

        protected override bool IncludeOrEqualPredefined(
            FilteringAttributeComponentFactoryArgs<int> first,
            FilteringAttributeComponentFactoryArgs<int> second)
        {
            HashSet<int> domain = DomainValues.ToHashSet(),
                         firstHS, secondHS;
            Func<int, bool> pred1 = first.PredicateExpression.Compile(),
                pred2 = second.PredicateExpression.Compile();
            (firstHS, secondHS) = (domain.Where(pred1).ToHashSet(), domain.Where(pred2).ToHashSet());

            return firstHS.IsSupersetOf(secondHS);
        }

        #endregion

        #region Operation result getters

        protected override IAttributeComponent<int> ComplementionGetter(
            AttributeComponent<int> first)
        {
            return base.ComplementionGetter(first) switch
            {
                FilteringAttributeComponent<int> f => f.ToIterableAttributeComponent(),
                var ac => ac
            };
        }

        protected override IAttributeComponent<int> IntersectionGetter(
            AttributeComponent<int> first,
            IAttributeComponent<int> second)
        {
            return base.IntersectionGetter(first, second) switch
            {
                FilteringAttributeComponent<int> f => f.ToIterableAttributeComponent(),
                var ac => ac
            };
        }

        protected override IAttributeComponent<int> UnionGetter(
            AttributeComponent<int> first,
            IAttributeComponent<int> second)
        {
            return base.UnionGetter(first, second) switch
            {
                FilteringAttributeComponent<int> f => f.ToIterableAttributeComponent(),
                var ac => ac
            };
        }

        protected override IAttributeComponent<int> ExceptionGetter(
            AttributeComponent<int> first,
            IAttributeComponent<int> second)
        {
            return base.ExceptionGetter(first, second) switch
            {
                FilteringAttributeComponent<int> f => f.ToIterableAttributeComponent(),
                var ac => ac
            };
        }

        protected override IAttributeComponent<int> SymmetricExceptionGetter(
            AttributeComponent<int> first,
            IAttributeComponent<int> second)
        {
            return base.SymmetricExceptionGetter(first, second) switch
            {
                FilteringAttributeComponent<int> f => f.ToIterableAttributeComponent(),
                var ac => ac
            };
        }

        #endregion

        protected override IEnumerable<SingleTestData> CreateTestCaseList()
        {
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.EmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d < 3),
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d >= 3 & d <= 5));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.EmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d >= 3 & d <= 5),
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d < 3));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.EmptyIntersection |
                TestCaseInfo.FullUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.FullSymmetricException,
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => (d & 0b1) == 0),
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => (d & 0b1) == 1));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => (d & 0b1) == 0),
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => new int[] { 0, 3, 5, 7, 9 }. Contains(d)));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => (d & 0b1) == 0),
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => new int[] { 1, 3, 4, 7, 9 }.Contains(d)));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => new int[] { 0, 2, 5, 6, 8 }.Contains(d)),
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => (d & 0b1) == 1));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => new int[] { 0, 2, 4, 6, 9 }.Contains(d)),
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => (d & 0b1) == 1));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => (d & 0b1) == 0),
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => new int[] { 0, 2, 5, 7, 9 }.Contains(d)));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => new int[] { 0, 3, 4, 6, 8 }.Contains(d)),
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => new int[] { 1, 3, 4, 6, 9 }.Contains(d)));
            yield return new SingleTestData(
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.NonEmptySymmetricException,
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => new int[] { 0, 2, 4, 7, 9 }.Contains(d)),
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
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d < 6),
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
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d == 1 || d == 3 || d == 5),
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
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d == 0),
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
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d == 0 || d == 4 || d == 8),
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
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => (d & 0b1) == 0),
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
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d < 9),
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
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d > 0 && d < 9),
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
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => (d > 0 && d <= 3) || (d >= 6 && d < 9)),
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
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d > 0 && d <= 9),
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
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d >= 0 && d <= 9, AttributeComponentContentType.Full),
                new FilteringAttributeComponentFactoryArgs<int>(
                    (d) => d >= 0 && d <= 9, AttributeComponentContentType.Full));

            yield break;
        }
    }

    public class TestFilteringAttributeComponentFactory
        : UnorderedFiniteEnumerableAttributeComponentFactory<int>,
          IFilteringAttributeComponentFactory<int>
    {
        public TestFilteringAttributeComponentFactory(
            HashSet<int> domainValues)
            : base(domainValues)
        {
            return;
        }
    }
}
