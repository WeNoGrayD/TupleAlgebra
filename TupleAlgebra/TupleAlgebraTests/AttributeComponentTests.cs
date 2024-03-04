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

namespace TupleAlgebraTests
{
    [TestClass]
    public class AttributeComponentTests
    {
        protected AttributeComponent<int> component1, component2;
        protected MockAttributeComponentFactory<int> intFactory;
        protected MockAttributeComponentFactory<string> stringFactory;
        protected Dictionary<Type, object> factories;

        protected class MockAttributeComponentFactory<TData> 
            : OrderedFiniteEnumerableAttributeComponentFactory<TData>//,
              //INonFictionalAttributeComponentFactory<TData, MockAttributeComponentFactory<TData>.MockAttributeComponentFactoryArgs<TData>>
            where TData : IComparable<TData>
        {
            public MockAttributeComponentFactory(IEnumerable<TData> universeData)
                : base(universeData)
            { }

            public AttributeComponent<TData> CreateNonFictional(
                IEnumerable<TData> values)
            {
                StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData> factoryArgs =
                    new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>(values);

                return CreateNonFictional(factoryArgs);
            }

            /*
            public NonFictionalAttributeComponent<TData> CreateSpecificNonFictional(
                BufferedMockAttributeComponentFactoryArgs<TData> args)
            {
                return new MockAttributeComponent(
                    args.Power,
                    args.Values as IEnumerable<TData>);
            }

            public NonFictionalAttributeComponent<TData> CreateSpecificNonFictional(
                StreamingMockAttributeComponentFactoryArgs<TData> args)
            {
                return new MockAttributeComponent(
                    args.Power,
                    args.Values as IEnumerable<TData>);
            }
            */

            public AttributeComponent<TData> CreateMock(
                IEnumerable<TData> values = null)
            {
                StreamingMockAttributeComponentFactoryArgs<TData> factoryArgs =
                    new StreamingMockAttributeComponentFactoryArgs<TData>(values);

                return CreateNonFictional(factoryArgs);
            }

            public class BufferedMockAttributeComponentFactoryArgs<TData>
                : BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>
            {
                public BufferedMockAttributeComponentFactoryArgs(
                    IEnumerable<TData> values = null)
                    : base(values: values)
                { return; }
            }

            public class StreamingMockAttributeComponentFactoryArgs<TData>
                : StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>
            {
                public StreamingMockAttributeComponentFactoryArgs(
                    IEnumerable<TData> values = null)
                    : base(values: values)
                { return; }
            }

            /*
            private class MockAttributeComponent
                : OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>
            {
                static MockAttributeComponent()
                {
                    AttributeComponentHelper.Helper.RegisterType<TData, MockAttributeComponent>(
                        setOperations: null);// new OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutorsContainer());

                    return;
                }

                public MockAttributeComponent(
                    AttributeComponentPower power,
                    IEnumerable<TData> values)
                    : base(power, values)
                {
                    return;
                }

                private class MockSetOperationContainer
                    : OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutorsContainer
                {
                    public MockSetOperationContainer(
                        IOrderedFiniteEnumerableAttributeComponentFactory<TData> 
                        factory) 
                        : base(factory) { }
                }
            }
            */
        }

        [TestInitialize]
        public void SetUp()
        {
            intFactory = new MockAttributeComponentFactory<int>(
                Enumerable.Range(0, 10));
            stringFactory = new MockAttributeComponentFactory<string>(
                new[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" });
            factories = new Dictionary<Type, object>()
            { { typeof(int), intFactory }, { typeof(string), stringFactory } };
        }

        protected IEnumerable<T> SortedComplement<T>(
            HashSet<T> first)
            where T : IComparable<T>
        {
            MockAttributeComponentFactory<T> factory =
                (factories[typeof(T)] as MockAttributeComponentFactory<T>)!;
            first.SymmetricExceptWith(factory.Domain);
            List<T> resultValuesList = new List<T>(first);
            resultValuesList.Sort();
            return resultValuesList;
        }

        protected IEnumerable<T> SortedBinarySetOperation<T>(
            HashSet<T> first, 
            HashSet<T> second,
            Action<HashSet<T>, HashSet<T>> opAction)
        {
            opAction(first, second);
            List<T> resultValuesList = new List<T>(first);
            resultValuesList.Sort();
            return resultValuesList;
        }

        protected IEnumerable<T> SortedIntersect<T>(HashSet<T> first, HashSet<T> second)
        {
            return SortedBinarySetOperation(first, second, 
                (set1, set2) => set1.IntersectWith(set2));
        }

        protected IEnumerable<T> SortedUnion<T>(HashSet<T> first, HashSet<T> second)
        {
            return SortedBinarySetOperation(first, second,
                (set1, set2) => set1.UnionWith(set2));
        }

        protected IEnumerable<T> SortedExcept<T>(HashSet<T> first, HashSet<T> second)
        {
            return SortedBinarySetOperation(first, second,
                (set1, set2) => set1.ExceptWith(set2));
        }

        protected IEnumerable<T> SortedSymmetricExcept<T>(HashSet<T> first, HashSet<T> second)
        {
            return SortedBinarySetOperation(first, second,
                (set1, set2) => set1.SymmetricExceptWith(set2));
        }
    }
}
