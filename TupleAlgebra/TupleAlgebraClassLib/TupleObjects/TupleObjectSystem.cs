using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectInfrastructure;

namespace TupleAlgebraClassLib.TupleObjects
{
    using static TupleObjectHelper;

    public interface ITupleObjectSystem
    {
        //public ISingleTupleObject this[int tuplePtr] { get; set; }

        public IAttributeComponent this[int tuplePtr, int attrPtr] 
        { get; set; }

        public IAttributeComponent this[int tuplePtr, AttributeName attrName] 
        { get; set; }

        public IAttributeComponent<TAttribute>
            GetDefaultFictionalAttributeComponent<TAttribute>(
                IAttributeComponentFactory<TAttribute> factory);
    }

    public abstract class TupleObjectSystem<TEntity> 
        : TupleObject<TEntity>, ITupleObjectSystem
        where TEntity : new()
    {
        //private IEnumerable<SingleTupleObject<TEntity>> _tuples = Enumerable.Empty<SingleTupleObject<TEntity>>();

        protected SingleTupleObject<TEntity>[] _tuples;

        public SingleTupleObject<TEntity>[] Tuples
        {
            get => _tuples;
        }

        public SingleTupleObject<TEntity> this[int tuplePtr]
        {
            get => _tuples[tuplePtr];
            set => _tuples[tuplePtr] = value;
        }

        public IAttributeComponent this[int tuplePtr, int attrPtr]
        {
            get => _tuples[tuplePtr][attrPtr];
            set => _tuples[tuplePtr][attrPtr] = value;
        }

        public IAttributeComponent this[int tuplePtr, AttributeName attrName]
        {
            get => _tuples[tuplePtr][attrName]; 
            set => _tuples[tuplePtr][attrName] = value;
        }

        public TupleObjectSystem(TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            : base(onTupleBuilding)
        {

            return;
        }

        protected TupleObjectSystem(
            TupleObjectSchema<TEntity> schema,
            int len)
            : base(schema)
        {
            _tuples = new SingleTupleObject<TEntity>[len];

            return;
        }

        protected TupleObjectSystem(
            TupleObjectSchema<TEntity> schema,
            IList<SingleTupleObject<TEntity>> tuples)
            : base(schema)
        {
            _tuples = tuples.ToArray();

            return;
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override bool IsFull()
        {
            return false;
        }

        public void TrimRedundantRows(int len)
        {
            if (len < _tuples.Length)
                _tuples = _tuples[..len];

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

        public abstract IAttributeComponent<TAttribute>
            GetDefaultFictionalAttributeComponent<TAttribute>(
                IAttributeComponentFactory<TAttribute> factory);
    }
}
