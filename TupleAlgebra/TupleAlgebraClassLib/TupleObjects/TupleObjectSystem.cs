using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure;

namespace TupleAlgebraClassLib.TupleObjects
{
    public abstract class TupleObjectSystem<TEntity> : TupleObject<TEntity>
        where TEntity : new()
    {
        //private IEnumerable<SingleTupleObject<TEntity>> _tuples = Enumerable.Empty<SingleTupleObject<TEntity>>();

        protected SingleTupleObject<TEntity>[] _tuples;

        public TupleObjectSystem(Action<TupleObjectBuilder<TEntity>> onTupleBuilding)
            : base(onTupleBuilding)
        {

            return;
        }

        /// <summary>
        /// Освобождение ресурсов 
        /// </summary>
        protected override void DisposeImpl()
        {
            //foreach (SingleTupleObject<TEntity> tuple in _tuples)
            //    tuple.Dispose();

            for (int i = 0; i < _tuples.Length; i++)
            {
                _tuples[i].Dispose();
            }

            return;
        }
    }
}
