using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    public class TAContext : IDisposable
    {
        public readonly TupleObjectFactory Factory;

        public event Action Disposing;

        public TAContext()
        {
            Factory = new TupleObjectFactory(this);
        }

        protected void OnDisposing()
        {
            Disposing?.Invoke();

            return;
        }

        public void Dispose()
        {
            OnDisposing();

            return;
        }
    }
}
