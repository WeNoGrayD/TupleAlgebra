using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure;

namespace TupleAlgebraClassLib.TupleObjects
{
    public abstract class TupleObjectSystem<TEntity> : TupleObject<TEntity>
    {
        private IEnumerable<SingleTupleObject<TEntity>> _tuples = Enumerable.Empty<SingleTupleObject<TEntity>>();

        public TupleObjectSystem(Action<TupleObjectBuilder<TEntity>> onTupleBuilding)
            : base(onTupleBuilding)
        { }

        /// <summary>
        /// Освобождение ресурсов 
        /// </summary>
        protected override void DisposeImpl()
        {
            foreach (SingleTupleObject<TEntity> tuple in _tuples)
                tuple.Dispose();
        }
    }
}
