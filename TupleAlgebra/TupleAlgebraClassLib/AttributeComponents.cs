using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib
{
    public abstract class AttributeComponent<TValue>
        where TValue : IComparable<TValue>
    {
        public IEnumerable<TValue> Values { get { return GetValues(); } }

        protected abstract AttributeComponent<TValue> IntersectWith(dynamic second);

        protected abstract AttributeComponent<TValue> UnionWith(dynamic second);

        public abstract IEnumerable<TValue> GetValues();

        public static AttributeComponent<TValue> operator &(
            AttributeComponent<TValue> first,
            AttributeComponent<TValue> second)
        {
            return first.IntersectWith(second);
        }

        public static AttributeComponent<TValue> operator |(
            AttributeComponent<TValue> first,
            AttributeComponent<TValue> second)
        {
            return first.UnionWith(second);
        }
    }

    public class NonFictionalAttributeComponent<TValue> : AttributeComponent<TValue>
        where TValue : IComparable<TValue>
    {
        private IEnumerable<TValue> _values;

        private static IAttributeComponentAcceptor _intersector = new 
            NonFictionalAttributeComponentIntersector();
        private static IAttributeComponentAcceptor _uniter = new
            NonFictionalAttributeComponentUniter();

        protected override AttributeComponent<TValue> IntersectWith(dynamic second)
        {
            return _intersector.Accept(this, second);
        }

        protected override AttributeComponent<TValue> UnionWith(dynamic second)
        {
            return _uniter.Accept(this, second);
        }

        public override IEnumerable<TValue> GetValues()
        {
            return _values;
        }

        public NonFictionalAttributeComponent(IEnumerable<TValue> values)
        {
            _values = new List<TValue>(values);
            ((List<TValue>)_values).Sort();
        }
    }

    public class EmptyAttributeComponent<TValue> : AttributeComponent<TValue>
        where TValue : IComparable<TValue>
    {
        private static IAttributeComponentAcceptor _intersector = new
            NonFictionalAttributeComponentIntersector();
        private static IAttributeComponentAcceptor _uniter = new
            NonFictionalAttributeComponentIntersector();

        protected override AttributeComponent<TValue> IntersectWith(dynamic second)
        {
            return _intersector.Accept(this, second);
        }

        protected override AttributeComponent<TValue> UnionWith(dynamic second)
        {
            return _intersector.Accept(this, second);
        }

        public override IEnumerable<TValue> GetValues()
        {
            yield break;
        }
    }

    public class FullAttributeComponent<TValue> : AttributeComponent<TValue>
        where TValue : IComparable<TValue>
    {
        private static IAttributeComponentAcceptor _intersector = new
            NonFictionalAttributeComponentIntersector();
        private static IAttributeComponentAcceptor _uniter = new
            NonFictionalAttributeComponentIntersector();

        protected override AttributeComponent<TValue> IntersectWith(dynamic second)
        {
            return _intersector.Accept(this, second);
        }

        protected override AttributeComponent<TValue> UnionWith(dynamic second)
        {
            return _intersector.Accept(this, second);
        }

        public override IEnumerable<TValue> GetValues()
        {
            yield break;
        }
    }
}
