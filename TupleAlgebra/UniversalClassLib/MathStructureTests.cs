using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UniversalClassLib
{
    [TestClass]
    public abstract class MathStructureTests<
        TData,
        TOperand1Values,
        TOperand1,
        TOperand2Values,
        TOperand2,
        TTestCaseInfo,
        TOperandFactory>
        where TTestCaseInfo : Enum
    {
        #region Instance fields

        protected IDictionary<string, IOperationTestCollection> GlobalTests
        { get; private set; } =
            new Dictionary<string, IOperationTestCollection>();

        #endregion

        #region Instance properties

        public TOperandFactory OperandFactory { get; private set; }

        #endregion

        #region Delegates

        public delegate void TestAssertionHandler<TResultPattern, TResult>(
            TOperand1 op1,
            TOperand2 op2,
            TResultPattern opResPattern,
            TResult opRes);

        public delegate TRes GetResultHandler<TOp1, TRes>(
            TOp1 op1);

        public delegate TRes GetResultHandler<TOp1, TOp2, TRes>(
            TOp1 op1,
            TOp2 op2);

        public delegate TOp Factory<TValues, TOp>(TValues values);

        #endregion

        #region Instance methods

        [TestInitialize]
        public virtual void Setup()
        {
            InitOperationsTest();
            OperandFactory = InitOperandFactory();

            return;
        }

        protected abstract void InitOperationsTest();

        protected abstract TOperandFactory InitOperandFactory();

        protected void RunManyUnaryOperationTests(
            [CallerMemberName] string callerName = null!)
        {
            IOperationTestCollection testList =
                GlobalTests[callerName];
            testList.RunUnaryOperationTests();

            return;
        }

        protected void RunManyBinaryOperationTests(
            [CallerMemberName] string callerName = null!)
        {
            IOperationTestCollection testList =
                GlobalTests[callerName];
            testList.RunBinaryOperationTests();

            return;
        }

        #endregion

        #region Nested types

        protected interface IOperationTestCollection
        {
            void RunUnaryOperationTests();

            void RunBinaryOperationTests();
        }

        protected record SingleTestData
        {
            public TTestCaseInfo Info { get; init; }

            public TOperand1Values Operand1 { get; init; }

            public TOperand2Values Operand2 { get; init; }

            public SingleTestData(
                TTestCaseInfo info,
                TOperand1Values op1,
                TOperand2Values op2 = default)
            {
                Info = info;
                Operand1 = op1;
                Operand2 = op2;

                return;
            }
        }

        protected record SingleTestData<TResultPattern, TResult>
            : SingleTestData
        {
            public TestAssertionHandler<TResultPattern, TResult> Assert
            { get; }

            public SingleTestData(
                TTestCaseInfo info,
                TOperand1Values op1,
                TOperand2Values op2 = default,
                TestAssertionHandler<TResultPattern, TResult> assert = null)
                : base(info, op1, op2)
            {
                Assert = assert;

                return;
            }
        }

        protected record MultipleTestData<TResultPattern, TResult>
            : IOperationTestCollection
        {
            public IEnumerable<SingleTestData> TestPatterns { get; init; }

            public IEnumerable<TestAssertionHandler<TResultPattern, TResult>>
                Asserts
            {
                init
                {
                    Tests = TestPatterns.Zip(value)
                        .Select(((SingleTestData Test, TestAssertionHandler<TResultPattern, TResult> Assert) t) =>
                            new SingleTestData<TResultPattern, TResult>(
                                t.Test.Info,
                                t.Test.Operand1,
                                t.Test.Operand2,
                                t.Assert));
                }
            }

            public IEnumerable<SingleTestData<TResultPattern, TResult>>
                Tests
            { get; init; }

            public Factory<TOperand1Values, TOperand1> Operand1Factory
            { get; set; }

            public Factory<TOperand2Values, TOperand2> Operand2Factory
            { get; set; }

            public GetResultHandler<TOperand1Values, TOperand2Values, TResultPattern>
                ResultPatternGetter
            { get; set; }

            public GetResultHandler<TOperand1, TOperand2, TResult>
                ResultGetter
            { get; set; }

            public MultipleTestData(
                Factory<TOperand1Values, TOperand1>
                op1Factory = null!,
                Factory<TOperand2Values, TOperand2>
                op2Factory = null!,
                GetResultHandler<TOperand1Values, TOperand2Values, TResultPattern>
                resultPatternGetter = null!,
                GetResultHandler<TOperand1, TOperand2, TResult>
                resultGetter = null!)
            {
                Operand1Factory = op1Factory;
                Operand2Factory = op2Factory;
                ResultPatternGetter = resultPatternGetter;
                ResultGetter = resultGetter;

                return;
            }

            public MultipleTestData(
                IEnumerable<SingleTestData<TResultPattern, TResult>>
                tests,
                Factory<TOperand1Values, TOperand1>
                op1Factory = null!,
                Factory<TOperand2Values, TOperand2>
                op2Factory = null!,
                GetResultHandler<TOperand1Values, TOperand2Values, TResultPattern>
                resultPatternGetter = null!,
                GetResultHandler<TOperand1, TOperand2, TResult>
                resultGetter = null!)
                : this(
                      op1Factory,
                      op2Factory,
                      resultPatternGetter,
                      resultGetter)
            {
                Tests = tests;

                return;
            }

            public MultipleTestData(
                IEnumerable<SingleTestData>
                tests,
                IEnumerable<TestAssertionHandler<TResultPattern, TResult>>
                asserts = null!,
                Factory<TOperand1Values, TOperand1>
                op1Factory = null!,
                Factory<TOperand2Values, TOperand2>
                op2Factory = null!,
                GetResultHandler<TOperand1Values, TOperand2Values, TResultPattern>
                resultPatternGetter = null!,
                GetResultHandler<TOperand1, TOperand2, TResult>
                resultGetter = null!)
                : this(
                      op1Factory,
                      op2Factory,
                      resultPatternGetter,
                      resultGetter)
            {
                TestPatterns = tests;
                if (asserts is not null)
                    Asserts = asserts;

                return;
            }

            public void RunUnaryOperationTests()
            {
                TOperand1Values op1Values;
                TOperand1 op1;
                TResultPattern opResPattern;
                TResult opRes;

                foreach (var test in Tests)
                {
                    op1Values = test.Operand1;
                    op1 = Operand1Factory(op1Values);
                    opResPattern = ResultPatternGetter(op1Values, default);
                    opRes = ResultGetter(op1, default);
                    test.Assert(op1, default, opResPattern, opRes);
                }

                return;
            }

            public void RunBinaryOperationTests()
            {
                TOperand1Values op1Values;
                TOperand1 op1;
                TOperand2Values op2Values;
                TOperand2 op2;
                TResultPattern opResPattern;
                TResult opRes;

                foreach (var test in Tests)
                {
                    op1Values = test.Operand1;
                    op2Values = test.Operand2;
                    op1 = Operand1Factory(op1Values);
                    op2 = Operand2Factory(op2Values);
                    opResPattern = ResultPatternGetter(op1Values, op2Values);
                    opRes = ResultGetter(op1, op2);
                    test.Assert(op1, op2, opResPattern, opRes);
                }

                return;
            }
        }

        protected record BooleanMultipleTestData
            : MultipleTestData<bool, bool>
        {
            public BooleanMultipleTestData(
                IEnumerable<SingleTestData>
                tests)
                : base(tests)
            { }

            public BooleanMultipleTestData(
                IEnumerable<SingleTestData<bool, bool>>
                tests)
                : base(tests)
            { }
        }

        #endregion
    }
}
