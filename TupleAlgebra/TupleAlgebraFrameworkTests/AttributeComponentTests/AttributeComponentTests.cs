using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Buffered;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Streaming;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;
using System.Linq.Expressions;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.OrderedFiniteEnumerable;
using System.Collections;
using UniversalClassLib;

namespace TupleAlgebraFrameworkTests.AttributeComponentTests
{
    [Flags]
    public enum TestCaseInfo : int
    {
        None = 0,
        EmptyComplementionOp1 = 0b1,
        NonEmptyComplementionOp1 = 0b1 << 1,
        FullComplementionOp1 = 0b1 << 2,
        EmptyComplementionOp2 = 0b1 << 3,
        NonEmptyComplementionOp2 = 0b1 << 4,
        FullComplementionOp2 = 0b1 << 5,
        EmptyIntersection = 0b1 << 6,
        NonEmptyIntersection = 0b1 << 7,
        FullIntersection = 0b1 << 8,
        EmptyUnion = 0b1 << 9,
        NonEmptyUnion = 0b1 << 10,
        FullUnion = 0b1 << 11,
        EmptyException = 0b1 << 12,
        NonEmptyException = 0b1 << 13,
        FullException = 0b1 << 14,
        EmptySymmetricException = 0b1 << 15,
        NonEmptySymmetricException = 0b1 << 16,
        FullSymmetricException = 0b1 << 17,
        FirstGreaterThanSecond = 0b1 << 18,
        FirstEqualToSecond = 0b1 << 19,
        FirstLowerThanSecond = 0b1 << 20
    }

    [TestClass]
    public abstract class AttributeComponentTests<
        TData,
        TOperand1Values,
        TOperand2Values,
        TOperandFactory>
        : MathStructureTests<
            TData,
            TOperand1Values,
            AttributeComponent<TData>,
            TOperand2Values,
            AttributeComponent<TData>,
            TestCaseInfo,
            TOperandFactory>
        where TOperandFactory : INonFictionalAttributeComponentFactory<
            TData,
            TOperand1Values>
    {
        #region Constants

        protected const TestCaseInfo COMPLEMENTION_CASE =
            TestCaseInfo.EmptyComplementionOp1 |
            TestCaseInfo.NonEmptyComplementionOp1 |
            TestCaseInfo.FullComplementionOp1;

        protected const TestCaseInfo INTERSECTION_CASE =
            TestCaseInfo.EmptyIntersection |
            TestCaseInfo.NonEmptyIntersection |
            TestCaseInfo.FullIntersection;

        protected const TestCaseInfo UNION_CASE =
            TestCaseInfo.EmptyUnion |
            TestCaseInfo.NonEmptyUnion |
            TestCaseInfo.FullUnion;

        protected const TestCaseInfo EXCEPTION_CASE =
            TestCaseInfo.EmptyException |
            TestCaseInfo.NonEmptyException |
            TestCaseInfo.FullException;

        protected const TestCaseInfo SYMMETRIC_EXCEPTION_CASE =
            TestCaseInfo.EmptySymmetricException |
            TestCaseInfo.NonEmptySymmetricException |
            TestCaseInfo.FullSymmetricException;

        protected const TestCaseInfo COMPARATION_CASE =
            TestCaseInfo.FirstLowerThanSecond |
            TestCaseInfo.FirstEqualToSecond |
            TestCaseInfo.FirstGreaterThanSecond;

        protected const TestCaseInfo EMPTY_RESULT_CASE =
            TestCaseInfo.EmptyComplementionOp1 |
            TestCaseInfo.EmptyIntersection |
            TestCaseInfo.EmptyUnion |
            TestCaseInfo.EmptyException |
            TestCaseInfo.EmptySymmetricException;

        protected const TestCaseInfo NONEMPTY_RESULT_CASE =
            TestCaseInfo.NonEmptyComplementionOp1 |
            TestCaseInfo.NonEmptyIntersection |
            TestCaseInfo.NonEmptyUnion |
            TestCaseInfo.NonEmptyException |
            TestCaseInfo.NonEmptySymmetricException;

        protected const TestCaseInfo FULL_RESULT_CASE =
            TestCaseInfo.FullComplementionOp1 |
            TestCaseInfo.FullIntersection |
            TestCaseInfo.FullUnion |
            TestCaseInfo.FullException |
            TestCaseInfo.FullSymmetricException;

        protected const TestCaseInfo FIRST_GREATER_THAN_OR_EQUAL_TO_SECOND_CASE =
            TestCaseInfo.FirstGreaterThanSecond |
            TestCaseInfo.FirstEqualToSecond;

        #endregion

        #region Instance fields

        protected static IDictionary<TestCaseInfo, Type>
            _resultTypeAssertionSource =
            new Dictionary<TestCaseInfo, Type>
                {
                    {
                        EMPTY_RESULT_CASE,
                        typeof(EmptyAttributeComponent<TData>)
                    },
                    {
                        NONEMPTY_RESULT_CASE,
                        typeof(NonFictionalAttributeComponent<TData>)
                    },
                    {
                        FULL_RESULT_CASE,
                        typeof(FullAttributeComponent<TData>)
                    }
                };

        private AttributeComponentMultipleTestData
            _testsWithAttributeComponentResult;

        private BooleanMultipleTestData
            _testsWithBooleanResult;

        #endregion

        #region Instance properties

        protected virtual AttributeComponentMultipleTestData
            TestsWithAttributeComponentResult
        { set => _testsWithAttributeComponentResult = value; }

        protected virtual BooleanMultipleTestData
            TestsWithBooleanResult
        { set => _testsWithBooleanResult = value; }

        #endregion

        #region Instance methods

        protected AttributeComponent<TData>
            CreateOperand1(TOperand1Values values)
        {
            return OperandFactory.CreateNonFictional(values);
        }

        protected abstract AttributeComponent<TData>
            CreateNonFictionalOperand2(TOperand2Values values);

        protected override void InitOperationsTest()
        {
            IEnumerable<SingleTestData> testList = CreateTestCaseList();

            TestsWithAttributeComponentResult =
                new AttributeComponentMultipleTestData(testList);
            TestsWithBooleanResult =
                new BooleanMultipleTestData(testList);

            return;
        }

        protected TestCaseInfo GetOperationResultTestCase(TestCaseInfo testCase)
        {
            return GetMidResult(EMPTY_RESULT_CASE) |
                GetMidResult(NONEMPTY_RESULT_CASE) |
                GetMidResult(FULL_RESULT_CASE);

            TestCaseInfo GetMidResult(TestCaseInfo general)
            {
                return (general & testCase) != TestCaseInfo.None ?
                    general :
                    TestCaseInfo.None;
            }
        }

        #region Building test collections methods

        protected abstract IEnumerable<SingleTestData> CreateTestCaseList();

        private TMultipleTestData
            BuildTestCollection<TResultPattern, TResult, TMultipleTestData>(
                TMultipleTestData
                original,
                Func<TestCaseInfo, TestAssertionHandler<TResultPattern, TResult>>
                assertSelector,
                Factory<TOperand1Values, AttributeComponent<TData>>
                op1Factory,
                GetResultHandler<TOperand1Values, TResultPattern>
                resultPatternGetter,
                GetResultHandler<AttributeComponent<TData>, TResult>
                resultGetter)
            where TMultipleTestData : MultipleTestData<TResultPattern, TResult>
        {
            var newTestCollection = FillWithAssertionsTestCases(
                original, assertSelector);
            newTestCollection.Operand1Factory = op1Factory;
            newTestCollection.ResultPatternGetter = (op1, _) => resultPatternGetter(op1);
            newTestCollection.ResultGetter = (op1, _) => resultGetter(op1);

            return newTestCollection;
        }

        private TMultipleTestData
            BuildTestCollection<TResultPattern, TResult, TMultipleTestData>(
                TMultipleTestData
                original,
                TestCaseInfo op2Modifier,
                Func<TestCaseInfo, TestAssertionHandler<TResultPattern, TResult>>
                assertSelector,
                Factory<TOperand1Values, AttributeComponent<TData>>
                op1Factory,
                Factory<TOperand2Values, AttributeComponent<TData>>
                op2Factory,
                GetResultHandler<TOperand1Values, TOperand2Values, TResultPattern>
                resultPatternGetter,
                GetResultHandler<AttributeComponent<TData>, AttributeComponent<TData>, TResult>
                resultGetter)
            where TMultipleTestData : MultipleTestData<TResultPattern, TResult>
        {
            original = op2Modifier switch
            {
                EMPTY_RESULT_CASE => ModifyTestCasesWithOp2Empty<TResultPattern, TResult, TMultipleTestData>(original),
                FULL_RESULT_CASE => ModifyTestCasesWithOp2Full<TResultPattern, TResult, TMultipleTestData>(original),
                _ => original,
            };

            var newTestCollection = FillWithAssertionsTestCases(
                original, assertSelector);
            newTestCollection.Operand1Factory = op1Factory;
            newTestCollection.Operand2Factory = op2Factory;
            newTestCollection.ResultPatternGetter = resultPatternGetter;
            newTestCollection.ResultGetter = resultGetter;

            return newTestCollection;
        }

        protected TMultipleTestData
            FillWithAssertionsTestCases<TResultPattern, TResult, TMultipleTestData>(
                TMultipleTestData original,
                Func<TestCaseInfo, TestAssertionHandler<TResultPattern, TResult>> assertSelector)
            where TMultipleTestData : MultipleTestData<TResultPattern, TResult>
        {
            return original with
            {
                Tests = original.TestPatterns.Select(tp =>
                    new SingleTestData<TResultPattern, TResult>(
                        tp.Info,
                        tp.Operand1,
                        tp.Operand2,
                        assertSelector(tp.Info)))
            };
        }

        protected SingleTestData ModifyTestCase(
            SingleTestData original,
            TOperand1Values op1Values,
            TOperand2Values op2Values,
            TestCaseInfo toReduce,
            TestCaseInfo toIncrease)
        {
            TestCaseInfo origInfo = original.Info;

            return original with
            { 
                Operand1 = op1Values,
                Operand2 = op2Values,
                Info = origInfo & (origInfo ^ toReduce) | toIncrease
            };
        }

        protected abstract TOperand2Values GetEmptyValues();

        protected abstract TOperand2Values GetFullValues();

        protected abstract bool OperationResultEquals(
            IEnumerable<TData> resultPattern,
            IAttributeComponent<TData> result);

        protected IEnumerable<SingleTestData> ModifyTestCasesWithOp2Empty(
            IEnumerable<SingleTestData> original)
        {
            return original.Select((tc) => ModifyTestCase(tc,
                tc.Operand1,
                GetEmptyValues(),

                TestCaseInfo.EmptyComplementionOp2 |
                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.FullIntersection |
                TestCaseInfo.EmptyUnion |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.FullUnion |
                TestCaseInfo.EmptyException |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.FullException |
                TestCaseInfo.EmptySymmetricException |
                TestCaseInfo.NonEmptySymmetricException |
                TestCaseInfo.FullSymmetricException |
                TestCaseInfo.FirstGreaterThanSecond |
                TestCaseInfo.FirstLowerThanSecond |
                TestCaseInfo.FirstEqualToSecond,

                TestCaseInfo.EmptyIntersection |
                ((tc.Info & TestCaseInfo.EmptyComplementionOp1) != TestCaseInfo.None ? TestCaseInfo.FullUnion | TestCaseInfo.FullException | TestCaseInfo.FullSymmetricException | TestCaseInfo.FirstGreaterThanSecond : TestCaseInfo.None) |
                ((tc.Info & TestCaseInfo.NonEmptyComplementionOp1) != TestCaseInfo.None ? TestCaseInfo.NonEmptyUnion | TestCaseInfo.NonEmptyException | TestCaseInfo.NonEmptySymmetricException | TestCaseInfo.FirstGreaterThanSecond : TestCaseInfo.None) |
                ((tc.Info & TestCaseInfo.FullComplementionOp1) != TestCaseInfo.None ? TestCaseInfo.EmptyUnion | TestCaseInfo.EmptyException | TestCaseInfo.EmptySymmetricException | TestCaseInfo.FirstEqualToSecond : TestCaseInfo.None)));
        }

        protected TMultipleTestData
            ModifyTestCasesWithOp2Empty<TResultPattern, TResult, TMultipleTestData>(
                TMultipleTestData original)
            where TMultipleTestData : MultipleTestData<TResultPattern, TResult>
        {
            return original with
            {
                TestPatterns = ModifyTestCasesWithOp2Empty(original.TestPatterns)
            };
        }

        protected IEnumerable<SingleTestData> ModifyTestCasesWithOp2Full(
            IEnumerable<SingleTestData> original)
        {
            return original.Select((tc) => ModifyTestCase(tc,
                tc.Operand1,
                GetFullValues(),

                TestCaseInfo.NonEmptyComplementionOp2 |
                TestCaseInfo.FullComplementionOp2 |
                TestCaseInfo.EmptyIntersection |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.FullIntersection |
                TestCaseInfo.EmptyUnion |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.FullException |
                TestCaseInfo.EmptySymmetricException |
                TestCaseInfo.NonEmptySymmetricException |
                TestCaseInfo.FullSymmetricException |
                TestCaseInfo.FirstGreaterThanSecond |
                TestCaseInfo.FirstLowerThanSecond |
                TestCaseInfo.FirstEqualToSecond,

                TestCaseInfo.FullUnion |
                TestCaseInfo.EmptyException |
                ((tc.Info & TestCaseInfo.EmptyComplementionOp1) != TestCaseInfo.None ? TestCaseInfo.FullIntersection | TestCaseInfo.EmptySymmetricException | TestCaseInfo.FirstEqualToSecond : TestCaseInfo.None) |
                ((tc.Info & TestCaseInfo.NonEmptyComplementionOp1) != TestCaseInfo.None ? TestCaseInfo.NonEmptyIntersection | TestCaseInfo.NonEmptySymmetricException | TestCaseInfo.FirstLowerThanSecond : TestCaseInfo.None) |
                ((tc.Info & TestCaseInfo.FullComplementionOp1) != TestCaseInfo.None ? TestCaseInfo.EmptyIntersection | TestCaseInfo.FullSymmetricException | TestCaseInfo.FirstLowerThanSecond : TestCaseInfo.None)));
        }

        protected TMultipleTestData
            ModifyTestCasesWithOp2Full<TResultPattern, TResult, TMultipleTestData>(
                TMultipleTestData original)
            where TMultipleTestData : MultipleTestData<TResultPattern, TResult>
        {
            return original with
            {
                TestPatterns = ModifyTestCasesWithOp2Full(original.TestPatterns)
            };
        }

        #endregion

        #region Predefined operation methods

        protected abstract IEnumerable<TData> ComplementionPredefined(
            TOperand1Values first);

        protected abstract IEnumerable<TData> IntersectPredefined(
            TOperand1Values first,
            TOperand2Values second);

        protected abstract IEnumerable<TData> UnionPredefined(
            TOperand1Values first,
            TOperand2Values second);

        protected abstract IEnumerable<TData> ExceptionPredefined(
            TOperand1Values first,
            TOperand2Values second);

        protected abstract IEnumerable<TData> SymmetricExceptionPredefined(
            TOperand1Values first,
            TOperand2Values second);

        protected abstract bool IncludePredefined(
            TOperand1Values first,
            TOperand2Values second);

        protected abstract bool EqualPredefined(
            TOperand1Values first,
            TOperand2Values second);

        protected abstract bool IncludeOrEqualPredefined(
            TOperand1Values first,
            TOperand2Values second);

        #endregion

        #region Operation result getters

        protected virtual IAttributeComponent<TData> ComplementionGetter(
            AttributeComponent<TData> first)
        {
            return ~first;
        }

        protected virtual IAttributeComponent<TData> IntersectionGetter(
            AttributeComponent<TData> first,
            IAttributeComponent<TData> second)
        {
            return first & second;
        }

        protected virtual IAttributeComponent<TData> UnionGetter(
            AttributeComponent<TData> first,
            IAttributeComponent<TData> second)
        {
            return first | second;
        }

        protected virtual IAttributeComponent<TData> ExceptionGetter(
            AttributeComponent<TData> first,
            IAttributeComponent<TData> second)
        {
            return first / second;
        }

        protected virtual IAttributeComponent<TData> SymmetricExceptionGetter(
            AttributeComponent<TData> first,
            IAttributeComponent<TData> second)
        {
            return first ^ second;
        }

        #endregion

        #region Operation assertions

        private TestAssertionHandler<IEnumerable<TData>, IAttributeComponent<TData>>
            BuildACOperationTestAssertion(TestCaseInfo testCase)
        {
            TestCaseInfo resultCase = GetOperationResultTestCase(testCase);

            return _assert;

            void _assert(
                IAttributeComponent<TData> first,
                IAttributeComponent<TData> second,
                IEnumerable<TData> resultPattern,
                IAttributeComponent<TData> result)
            {
                Assert.IsInstanceOfType(
                    result,
                    _resultTypeAssertionSource[resultCase]);
                if (resultCase == NONEMPTY_RESULT_CASE)
                    Assert.IsTrue(OperationResultEquals(resultPattern, result));

                return;
            }
        }

        private TestAssertionHandler<bool, bool>
            BuildBooleanOperationTestAssertion()
        {
            return _assert;

            void _assert(
                IAttributeComponent<TData> first,
                IAttributeComponent<TData> second,
                bool resultPattern,
                bool result)
            {
                Assert.AreEqual(resultPattern, result);

                return;
            }
        }

        protected TestAssertionHandler<IEnumerable<TData>, IAttributeComponent<TData>>
            ComplementionAssert(TestCaseInfo testCase)
        {
            return BuildACOperationTestAssertion(
                testCase & COMPLEMENTION_CASE);
        }

        protected TestAssertionHandler<IEnumerable<TData>, IAttributeComponent<TData>>
            IntersectionAssert(TestCaseInfo testCase)
        {
            return BuildACOperationTestAssertion(
                testCase & INTERSECTION_CASE);
        }

        protected TestAssertionHandler<IEnumerable<TData>, IAttributeComponent<TData>>
            UnionAssert(TestCaseInfo testCase)
        {
            return BuildACOperationTestAssertion(
                testCase & UNION_CASE);
        }

        protected TestAssertionHandler<IEnumerable<TData>, IAttributeComponent<TData>>
            ExceptionAssert(TestCaseInfo testCase)
        {
            return BuildACOperationTestAssertion(
                testCase & EXCEPTION_CASE);
        }

        protected TestAssertionHandler<IEnumerable<TData>, IAttributeComponent<TData>>
            SymmetricExceptionAssert(TestCaseInfo testCase)
        {
            return BuildACOperationTestAssertion(
                testCase & SYMMETRIC_EXCEPTION_CASE);
        }

        protected TestAssertionHandler<bool, bool>
            BooleanAssert()
        {
            return BuildBooleanOperationTestAssertion();
        }

        #endregion

        #region Operation test methods

        #region Complemention

        [TestMethod]
        public virtual void ComplementionTest()
        {
            var testCollection = BuildTestCollection(
                _testsWithAttributeComponentResult,
                ComplementionAssert,
                CreateOperand1,
                ComplementionPredefined,
                ComplementionGetter);
            testCollection.RunUnaryOperationTests();

            return;
        }

        #endregion

        #region Intersection

        protected AttributeComponentMultipleTestData
            BuildIntersectionTestCollection(
                Factory<TOperand2Values, AttributeComponent<TData>>
                op2Factory,
                TestCaseInfo op2Modifier)
        {
            return BuildTestCollection(
                _testsWithAttributeComponentResult,
                op2Modifier,
                IntersectionAssert,
                CreateOperand1,
                op2Factory,
                IntersectPredefined,
                IntersectionGetter);
        }

        [TestMethod]
        public virtual void IntersectionWithEmptyAttributeComponentTest()
        {
            var testCollection = BuildIntersectionTestCollection(
                (_) => OperandFactory.CreateEmpty(),
                EMPTY_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        [TestMethod]
        public virtual void IntersectionWithNonFictionalAttributeComponentTest()
        {
            var testCollection = BuildIntersectionTestCollection(
                CreateNonFictionalOperand2,
                NONEMPTY_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        [TestMethod]
        public virtual void IntersectionWithFullAttributeComponentTest()
        {
            var testCollection = BuildIntersectionTestCollection(
                (_) => OperandFactory.CreateFull(),
                FULL_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        #endregion

        #region Union

        protected AttributeComponentMultipleTestData
            BuildUnionTestCollection(
                Factory<TOperand2Values, AttributeComponent<TData>>
                op2Factory,
                TestCaseInfo op2Modifier)
        {
            return BuildTestCollection(
                _testsWithAttributeComponentResult,
                op2Modifier,
                UnionAssert,
                CreateOperand1,
                op2Factory,
                UnionPredefined,
                UnionGetter);
        }

        [TestMethod]
        public virtual void UnionWithEmptyAttributeComponentTest()
        {
            var testCollection = BuildUnionTestCollection(
                (_) => OperandFactory.CreateEmpty(),
                EMPTY_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        [TestMethod]
        public virtual void UnionWithNonFictionalAttributeComponentTest()
        {
            var testCollection = BuildUnionTestCollection(
                CreateNonFictionalOperand2,
                NONEMPTY_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        [TestMethod]
        public virtual void UnionWithFullAttributeComponentTest()
        {
            var testCollection = BuildUnionTestCollection(
                (_) => OperandFactory.CreateFull(),
                FULL_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        #endregion

        #region Exception

        protected AttributeComponentMultipleTestData
            BuildExceptionTestCollection(
                Factory<TOperand2Values, AttributeComponent<TData>>
                op2Factory,
                TestCaseInfo op2Modifier)
        {
            return BuildTestCollection(
                _testsWithAttributeComponentResult,
                op2Modifier,
                ExceptionAssert,
                CreateOperand1,
                op2Factory,
                ExceptionPredefined,
                ExceptionGetter);
        }

        [TestMethod]
        public virtual void ExceptionWithEmptyAttributeComponentTest()
        {
            var testCollection = BuildExceptionTestCollection(
                (_) => OperandFactory.CreateEmpty(),
                EMPTY_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        [TestMethod]
        public virtual void ExceptionWithNonFictionalAttributeComponentTest()
        {
            var testCollection = BuildExceptionTestCollection(
                CreateNonFictionalOperand2,
                NONEMPTY_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        [TestMethod]
        public virtual void ExceptionWithFullAttributeComponentTest()
        {
            var testCollection = BuildExceptionTestCollection(
                (_) => OperandFactory.CreateFull(),
                FULL_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        #endregion

        #region Symmetric exception

        protected AttributeComponentMultipleTestData
            BuildSymmetricExceptionTestCollection(
                Factory<TOperand2Values, AttributeComponent<TData>>
                op2Factory,
                TestCaseInfo op2Modifier)
        {
            return BuildTestCollection(
                _testsWithAttributeComponentResult,
                op2Modifier,
                SymmetricExceptionAssert,
                CreateOperand1,
                op2Factory,
                SymmetricExceptionPredefined,
                SymmetricExceptionGetter);
        }

        [TestMethod]
        public virtual void SymmetricExceptionWithEmptyAttributeComponentTest()
        {
            var testCollection = BuildSymmetricExceptionTestCollection(
                (_) => OperandFactory.CreateEmpty(),
                EMPTY_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        [TestMethod]
        public virtual void SymmetricExceptionWithNonFictionalAttributeComponentTest()
        {
            var testCollection = BuildSymmetricExceptionTestCollection(
                CreateNonFictionalOperand2,
                NONEMPTY_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        [TestMethod]
        public virtual void SymmetricExceptionWithFullAttributeComponentTest()
        {
            var testCollection = BuildSymmetricExceptionTestCollection(
                (_) => OperandFactory.CreateFull(),
                FULL_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        #endregion

        #region Inclusion

        protected BooleanMultipleTestData
            BuildInclusionTestCollection(
                Factory<TOperand2Values, AttributeComponent<TData>>
                op2Factory,
                TestCaseInfo op2Modifier)
        {
            return BuildTestCollection(
                _testsWithBooleanResult,
                op2Modifier,
                (tc) => BooleanAssert(),
                CreateOperand1,
                op2Factory,
                IncludePredefined,
                (op1, op2) => op1 > op2);
        }

        [TestMethod]
        public virtual void InclusionOfEmptyAttributeComponentTest()
        {
            var testCollection = BuildInclusionTestCollection(
                (_) => OperandFactory.CreateEmpty(),
                EMPTY_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        [TestMethod]
        public virtual void InclusionOfNonFictionalAttributeComponentTest()
        {
            var testCollection = BuildInclusionTestCollection(
                CreateNonFictionalOperand2,
                NONEMPTY_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        [TestMethod]
        public virtual void InclusionOfFullAttributeComponentTest()
        {
            var testCollection = BuildInclusionTestCollection(
                (_) => OperandFactory.CreateFull(),
                FULL_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        #endregion

        #region Equality

        protected BooleanMultipleTestData
            BuildEqualityTestCollection(
                Factory<TOperand2Values, AttributeComponent<TData>>
                op2Factory,
                TestCaseInfo op2Modifier)
        {
            return BuildTestCollection(
                _testsWithBooleanResult,
                op2Modifier,
                (tc) => BooleanAssert(),
                CreateOperand1,
                op2Factory,
                EqualPredefined,
                (op1, op2) => op1 == op2);
        }

        [TestMethod]
        public virtual void EqualityToEmptyAttributeComponentTest()
        {
            var testCollection = BuildEqualityTestCollection(
                (_) => OperandFactory.CreateEmpty(),
                EMPTY_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        [TestMethod]
        public virtual void EqualityToNonFictionalAttributeComponentTest()
        {
            var testCollection = BuildEqualityTestCollection(
                CreateNonFictionalOperand2,
                NONEMPTY_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        [TestMethod]
        public virtual void EqualityToFullAttributeComponentTest()
        {
            var testCollection = BuildEqualityTestCollection(
                (_) => OperandFactory.CreateFull(),
                FULL_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        #endregion

        #region Inclusion or equality

        protected BooleanMultipleTestData
            BuildInclusionOrEqualityTestCollection(
                Factory<TOperand2Values, AttributeComponent<TData>>
                op2Factory,
                TestCaseInfo op2Modifier)
        {
            return BuildTestCollection(
                _testsWithBooleanResult,
                op2Modifier,
                (tc) => BooleanAssert(),
                CreateOperand1,
                op2Factory,
                IncludeOrEqualPredefined,
                (op1, op2) => op1 >= op2);
        }

        [TestMethod]
        public virtual void InclusionOfOrEqualityToEmptyAttributeComponentTest()
        {
            var testCollection = BuildInclusionOrEqualityTestCollection(
                (_) => OperandFactory.CreateEmpty(),
                EMPTY_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        [TestMethod]
        public virtual void InclusionOfOrEqualityToNonFictionalAttributeComponentTest()
        {
            var testCollection = BuildInclusionOrEqualityTestCollection(
                CreateNonFictionalOperand2,
                NONEMPTY_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        [TestMethod]
        public virtual void InclusionOfOrEqualityToFullAttributeComponentTest()
        {
            var testCollection = BuildInclusionOrEqualityTestCollection(
                (_) => OperandFactory.CreateFull(),
                FULL_RESULT_CASE);
            testCollection.RunBinaryOperationTests();

            return;
        }

        #endregion

        #endregion

        #endregion

        #region Nested types

        protected record AttributeComponentMultipleTestData
            : MultipleTestData<IEnumerable<TData>, IAttributeComponent<TData>>
        {
            public AttributeComponentMultipleTestData(
                IEnumerable<SingleTestData>
                tests)
                : base(tests)
            { }

            public AttributeComponentMultipleTestData(
                IEnumerable<SingleTestData<IEnumerable<TData>, IAttributeComponent<TData>>>
                tests)
                : base(tests)
            { }
        }

        #endregion
    }
}
