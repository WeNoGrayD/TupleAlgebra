using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib
{
    /*
    interface AttributeComponentVisitor
    {
        AttributeComponent IntersectWith<TValue>(NonFictionalAttributeComponent<TValue> nonFictional)
            where TValue : IComparable<TValue>;

        AttributeComponent IntersectWith(EmptyAttributeComponent empty);

        AttributeComponent IntersectWith<TValue>(FullAttributeComponent full);
    }
    */

    public interface IAttributeComponentAcceptor
    {
        AttributeComponent<TValue> Accept<TValue>(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> nonFictional)
            where TValue : IComparable<TValue>;

        AttributeComponent<TValue> Accept<TValue>(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> empty)
            where TValue : IComparable<TValue>;

        AttributeComponent<TValue> Accept<TValue>(AttributeComponent<TValue> first, FullAttributeComponent<TValue> full)
            where TValue : IComparable<TValue>;
    }

    public class NonFictionalAttributeComponentIntersector : IAttributeComponentAcceptor
    {
        public AttributeComponent<TValue> Accept<TValue>(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> nonFictional)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentIntersectionRules.Intersect((NonFictionalAttributeComponent<TValue>)first, nonFictional);
        }

        public AttributeComponent<TValue> Accept<TValue>(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> empty)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentIntersectionRules.Intersect(empty, (NonFictionalAttributeComponent<TValue>)first);
        }

        public AttributeComponent<TValue> Accept<TValue>(AttributeComponent<TValue> first, FullAttributeComponent<TValue> full)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentIntersectionRules.Intersect((NonFictionalAttributeComponent<TValue>)first, full);
        }
    }

    public class NonFictionalAttributeComponentUniter : IAttributeComponentAcceptor
    {
        public AttributeComponent<TValue> Accept<TValue>(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> nonFictional)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentUnionRules.Union((NonFictionalAttributeComponent<TValue>)first, nonFictional);
        }

        public AttributeComponent<TValue> Accept<TValue>(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> empty)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentUnionRules.Union(empty, (NonFictionalAttributeComponent<TValue>)first);
        }

        public AttributeComponent<TValue> Accept<TValue>(AttributeComponent<TValue> first, FullAttributeComponent<TValue> full)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentUnionRules.Union((NonFictionalAttributeComponent<TValue>)first, full);
        }
    }
}
