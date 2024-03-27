using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraFrameworkTests.AttributeComponentTests
{
    [TestClass]
    public class EmptyAttributeComponentTests
        : IntegerAttributeComponentTests
    {
        /*
        [TestMethod]
        public void EmptyComponentCreating()
        {
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateEmpty();

            Assert.AreNotEqual(component1, component2);
        }
        */

        protected override AttributeComponentMultipleTestData
            TestsWithAttributeComponentResult
        { set => base.TestsWithAttributeComponentResult =
                ModifyTestCasesWithOp1Empty<
                    IEnumerable<int>, 
                    IAttributeComponent<int>, 
                    AttributeComponentMultipleTestData>(value); }

        protected override BooleanMultipleTestData
            TestsWithBooleanResult
        { set => base.TestsWithBooleanResult =
                ModifyTestCasesWithOp1Empty<
                    bool, 
                    bool, 
                    BooleanMultipleTestData>(value); }

        protected IEnumerable<SingleTestData> ModifyTestCasesWithOp1Empty(
            IEnumerable<SingleTestData> original)
        {
            return original.Select((tc) => ModifyTestCase(tc,
                GetEmptyValues(),
                tc.Operand2,

                TestCaseInfo.EmptyComplementionOp1 |
                TestCaseInfo.NonEmptyComplementionOp1 |
                TestCaseInfo.NonEmptyIntersection |
                TestCaseInfo.FullIntersection |
                TestCaseInfo.EmptyUnion |
                TestCaseInfo.NonEmptyUnion |
                TestCaseInfo.FullUnion |
                TestCaseInfo.NonEmptyException |
                TestCaseInfo.FullException |
                TestCaseInfo.EmptySymmetricException |
                TestCaseInfo.NonEmptySymmetricException |
                TestCaseInfo.FullSymmetricException |
                TestCaseInfo.FirstGreaterThanSecond |
                TestCaseInfo.FirstLowerThanSecond |
                TestCaseInfo.FirstEqualToSecond,

                TestCaseInfo.FullComplementionOp1 |
                TestCaseInfo.EmptyIntersection |
                TestCaseInfo.EmptyException |
                ((tc.Info & TestCaseInfo.EmptyComplementionOp2) != TestCaseInfo.None ? TestCaseInfo.FullUnion | TestCaseInfo.FullSymmetricException | TestCaseInfo.FirstLowerThanSecond : TestCaseInfo.None) |
                ((tc.Info & TestCaseInfo.NonEmptyComplementionOp2) != TestCaseInfo.None ? TestCaseInfo.NonEmptyUnion |  TestCaseInfo.NonEmptySymmetricException | TestCaseInfo.FirstLowerThanSecond : TestCaseInfo.None) |
                ((tc.Info & TestCaseInfo.FullComplementionOp2) != TestCaseInfo.None ? TestCaseInfo.EmptyUnion | TestCaseInfo.EmptySymmetricException | TestCaseInfo.FirstEqualToSecond : TestCaseInfo.None)));
        }

        protected TMultipleTestData
            ModifyTestCasesWithOp1Empty<TResultPattern, TResult, TMultipleTestData>(
                TMultipleTestData original)
            where TMultipleTestData : MultipleTestData<TResultPattern, TResult>
        {
            return original with
            {
                TestPatterns = ModifyTestCasesWithOp1Empty(original.TestPatterns)
            };
        }
    }
}
