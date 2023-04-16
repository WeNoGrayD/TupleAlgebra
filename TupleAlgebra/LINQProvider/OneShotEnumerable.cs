using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider
{
    public class OneShotEnumerable<T> : IEnumerable<T>
    {
        private IEnumerator<T> _enumerator;

        public OneShotEnumerable(IEnumerable<T> source) => _enumerator = new OneShotEnumerator(source);

        public IEnumerator<T> GetEnumerator() => _enumerator;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class OneShotEnumerator : IEnumerator<T>
        {
            private IEnumerator<T> _sourceEnumerator;

            public T Current { get; private set; } = default(T);

            object IEnumerator.Current => Current;

            public OneShotEnumerator(IEnumerable<T> source) => _sourceEnumerator = source.GetEnumerator();

            public bool MoveNext()
            {
                bool moveNext = _sourceEnumerator.MoveNext();
                if (moveNext) Current = _sourceEnumerator.Current;

                return moveNext;
            }

            public void Dispose() => _sourceEnumerator.Dispose();

            public void Reset()
            {
                _sourceEnumerator = Empty();
            }

            private IEnumerator<T> Empty() { yield break; }
        }
    }
}
