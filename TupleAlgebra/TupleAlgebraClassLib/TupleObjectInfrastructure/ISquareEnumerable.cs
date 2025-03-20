using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    public interface ISquareEnumerable<T>
        : IEnumerable<IEnumerable<T>>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class SquareEnumerable<T> : ISquareEnumerable<T>
    {
        private IEnumerable<IEnumerable<T>> _source;

        public SquareEnumerable(IEnumerable<IEnumerable<T>> source)
        {
            _source = source;

            return;
        }

        public IEnumerator<IEnumerable<T>> GetEnumerator()
        {
            return _source.GetEnumerator();
        }
    }
}
