using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite;

namespace TupleAlgebraFrameworkTests.AttributeComponentTests
{
    public interface IOrderedFiniteEnumerableNonFictionalAttributeComponentTests<
        TData,
        TFactory>
        where TData : IComparable<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, IEnumerable<TData>>
    {
        TFactory OperandFactory { get; }

        #region Sorted operations methods

        public IEnumerable<TData> SortedComplement(
            HashSet<TData> first)
        {
            first.SymmetricExceptWith(OperandFactory.Domain);
            List<TData> resultValuesList = new List<TData>(first);
            resultValuesList.Sort();
            return resultValuesList;
        }

        public IEnumerable<TData> SortedBinarySetOperation(
            HashSet<TData> first,
            IEnumerable<TData> second,
            Action<HashSet<TData>, IEnumerable<TData>> opAction)
        {
            opAction(first, second);
            List<TData> resultValuesList = new List<TData>(first);
            resultValuesList.Sort();
            return resultValuesList;
        }

        public IEnumerable<TData> SortedIntersect(
            HashSet<TData> first,
            IEnumerable<TData> second)
        {
            return SortedBinarySetOperation(first, second,
                (set1, set2) => set1.IntersectWith(set2));
        }

        public IEnumerable<TData> SortedUnion(
            HashSet<TData> first,
            IEnumerable<TData> second)
        {
            return SortedBinarySetOperation(first, second,
                (set1, set2) => set1.UnionWith(set2));
        }

        public IEnumerable<TData> SortedExcept(
            HashSet<TData> first,
            IEnumerable<TData> second)
        {
            return SortedBinarySetOperation(first, second,
                (set1, set2) => set1.ExceptWith(set2));
        }

        public IEnumerable<TData> SortedSymmetricExcept(
            HashSet<TData> first,
            IEnumerable<TData> second)
        {
            return SortedBinarySetOperation(first, second,
                (set1, set2) => set1.SymmetricExceptWith(set2));
        }

        #endregion
    }

    [TestClass]
    public class OrderedFiniteEnumerableNonFictionalAttributeComponentTests
        : IntegerAttributeComponentTests,
          IOrderedFiniteEnumerableNonFictionalAttributeComponentTests<
            int,
            IEnumerableNonFictionalAttributeComponentFactory<int>>
    {
        private IOrderedFiniteEnumerableNonFictionalAttributeComponentTests<
            int,
            IEnumerableNonFictionalAttributeComponentFactory<int>>
            _orderedInterface;

        protected override bool OperationResultEquals(
            IEnumerable<int> resultPattern,
            IAttributeComponent<int> result)
        {
            return resultPattern.SequenceEqual(result);
        }

        /*
        [TestMethod]
        public void NonFictionalComponentCreating()
        {
            HashSet<int> component1Values = new HashSet<int>();
            component1 = intFactory.CreateNonFictional(component1Values);

            Assert.IsInstanceOfType(component1, typeof(EmptyAttributeComponent<int>));

            component1Values = new HashSet<int>() { 0, 1, 2 };
            List<int> component1ValuesSorted = new List<int>(component1Values);
            component1ValuesSorted.Sort();
            component1 = intFactory.CreateNonFictional(component1Values);

            Assert.IsInstanceOfType(component1, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(component1, component1ValuesSorted));

            component1Values = new HashSet<int>(intFactory.Domain);
            component1 = intFactory.CreateNonFictional(component1Values);

            Assert.IsInstanceOfType(component1, typeof(FullAttributeComponent<int>));
        }
        */

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            _orderedInterface = this;

            return;
        }

        protected override IEnumerableNonFictionalAttributeComponentFactory<int>
            InitOperandFactory()
        {
            return new OrderedFiniteEnumerableAttributeComponentFactory<int>(
                DomainValues);
        }

        #region Predefined operation methods

        protected override IEnumerable<int> ComplementionPredefined(
            IEnumerable<int> first)
        {
            return _orderedInterface.SortedComplement(first.ToHashSet());
        }

        protected override IEnumerable<int> IntersectPredefined(
            IEnumerable<int> first,
            IEnumerable<int> second)
        {
            return _orderedInterface
                .SortedIntersect(first.ToHashSet(), second);
        }

        protected override IEnumerable<int> UnionPredefined(
            IEnumerable<int> first,
            IEnumerable<int> second)
        {
            return _orderedInterface.
                SortedUnion(first.ToHashSet(), second);
        }

        protected override IEnumerable<int> ExceptionPredefined(
            IEnumerable<int> first,
            IEnumerable<int> second)
        {
            return _orderedInterface.
                SortedExcept(first.ToHashSet(), second);
        }

        protected override IEnumerable<int> SymmetricExceptionPredefined(
            IEnumerable<int> first,
            IEnumerable<int> second)
        {
            return _orderedInterface.
                SortedSymmetricExcept(first.ToHashSet(), second);
        }

        #endregion
    }
}
