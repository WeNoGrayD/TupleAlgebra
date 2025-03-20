using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraFrameworkTests.AttributeComponentTests
{
    [TestClass]
    public class FullAttributeComponentComponentTests
        : IntegerAttributeComponentTests
    {
        /*
        [TestMethod]
        public void FullComponentCreating()
        {
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateFull();

            Assert.AreNotEqual(component1, component2);
        }
        */

        protected override AttributeComponentMultipleTestData
            TestsWithAttributeComponentResult
        {
            set => base.TestsWithAttributeComponentResult =
                ModifyTestCasesWithOp1Full<
                    IEnumerable<int>,
                    IAttributeComponent<int>,
                    AttributeComponentMultipleTestData>(value);
        }

        protected override BooleanMultipleTestData
            TestsWithBooleanResult
        {
            set => base.TestsWithBooleanResult =
                ModifyTestCasesWithOp1Full<
                    bool,
                    bool,
                    BooleanMultipleTestData>(value);
        }

        protected IEnumerable<SingleTestData> ModifyTestCasesWithOp1Full(
            IEnumerable<SingleTestData> original)
        {
            return original.Select((tc) => ModifyTestCase(tc,
                GetFullValues(),
                tc.Operand2,

                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.FullComplementionOp1 |
                TestCaseInfo.EmptyIntersection |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.FullIntersection |
                TestCaseInfo.EmptyUnion |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.EmptyException |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.EmptySymmetricException |
                TestCaseInfo.NonEmptySymmetricException |
                TestCaseInfo.FullSymmetricException |
                TestCaseInfo.FirstGreaterThanSecond |
                TestCaseInfo.FirstLowerThanSecond |
                TestCaseInfo.FirstEqualToSecond,

                TestCaseInfo.EmptyComplementionOp1 |
                TestCaseInfo.FullUnion |
                ((tc.Info & TestCaseInfo.EmptyComplementionOp2) != TestCaseInfo.None ? TestCaseInfo.FullIntersection | TestCaseInfo.EmptyException | TestCaseInfo.EmptySymmetricException | TestCaseInfo.FirstEqualToSecond : TestCaseInfo.None) |
                ((tc.Info & TestCaseInfo.NonEmptyComplementionOp2) != TestCaseInfo.None ? TestCaseInfo.NonEmptyIntersection | TestCaseInfo.NonEmptyException | TestCaseInfo.NonEmptySymmetricException | TestCaseInfo.FirstGreaterThanSecond : TestCaseInfo.None) |
                ((tc.Info & TestCaseInfo.FullComplementionOp2) != TestCaseInfo.None ? TestCaseInfo.EmptyIntersection | TestCaseInfo.FullException | TestCaseInfo.FullSymmetricException | TestCaseInfo.FirstGreaterThanSecond : TestCaseInfo.None)));
        }

        protected TMultipleTestData
            ModifyTestCasesWithOp1Full<TResultPattern, TResult, TMultipleTestData>(
                TMultipleTestData original)
            where TMultipleTestData : MultipleTestData<TResultPattern, TResult>
        {
            return original with
            {
                TestPatterns = ModifyTestCasesWithOp1Full(original.TestPatterns)
            };
        }
    }
}
