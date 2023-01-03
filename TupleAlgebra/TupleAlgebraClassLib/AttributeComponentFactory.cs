using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib
{
    public interface IAttributeComponentFactory<TValue>
        where TValue : IComparable<TValue>
    {
        NonFictionalAttributeComponent<TValue> CreateNonFictional(IEnumerable<TValue> values);

        EmptyAttributeComponent<TValue> CreateEmpty();

        FullAttributeComponent<TValue> CreateFull();
    }

    /*
    public class AttributeComponentFactory<TValue> : IAttributeComponentFactory<TValue>
        where TValue : IComparable<TValue>
    {
        public NonFictionalAttributeComponent<TValue> CreateNonFictional(IEnumerable<TValue> values)
        {
            return new NonFictionalAttributeComponent<TValue>(values);
        }

        public EmptyAttributeComponent<TValue> CreateEmpty()
        {
            throw new NotImplementedException();
        }

        public FullAttributeComponent<TValue> CreateFull()
        {
            throw new NotImplementedException();
        }
    }
    */

    public static class AttributeComponentFactory<TValue>
        where TValue : IComparable<TValue>
    {
        public static NonFictionalAttributeComponent<TValue> CreateNonFictional(IEnumerable<TValue> values)
        {
            return new NonFictionalAttributeComponent<TValue>(values);
        }

        public static EmptyAttributeComponent<TValue> CreateEmpty()
        {
            throw new NotImplementedException();
        }

        public static FullAttributeComponent<TValue> CreateFull()
        {
            throw new NotImplementedException();
        }
    }
}
