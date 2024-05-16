using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure;

namespace TupleAlgebraClassLib.TupleObjects
{
    using static TupleObjectHelper;

    public interface ITupleObjectSystem
    {
        //public ISingleTupleObject this[int tuplePtr] { get; set; }

        public ISingleTupleObject this[int tuplePtr]
        { get; }

        public IAttributeComponent this[int tuplePtr, int attrPtr] 
        { get; set; }

        public IAttributeComponent this[int tuplePtr, AttributeName attrName] 
        { get; set; }

        public IAttributeComponent<TAttribute>
            GetDefaultFictionalAttributeComponent<TAttribute>(
                IAttributeComponentFactory<TAttribute> factory);
    }

    public abstract class TupleObjectSystem<TEntity, TSingleTupleObject> 
        : TupleObject<TEntity>, ITupleObjectSystem
        where TEntity : new()
        where TSingleTupleObject : SingleTupleObject<TEntity>
    {
        //private IEnumerable<SingleTupleObject<TEntity>> _tuples = Enumerable.Empty<SingleTupleObject<TEntity>>();

        protected TSingleTupleObject[] _tuples;

        public TSingleTupleObject[] Tuples
        {
            get => _tuples;
        }

        ISingleTupleObject ITupleObjectSystem.this[int tuplePtr]
        {
            get => this[tuplePtr];
        }

        public TSingleTupleObject this[int tuplePtr]
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

        public int RowLength { get => Schema.PluggedAttributesCount; }

        public int ColumnLength { get => _tuples.Length; }

        public bool IsOrthogonal { get; set; } = false;

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
            _tuples = new TSingleTupleObject[len];

            return;
        }

        protected TupleObjectSystem(
            TupleObjectSchema<TEntity> schema,
            IList<SingleTupleObject<TEntity>> tuples)
            : base(schema)
        {
            _tuples = tuples.OfType<TSingleTupleObject>().ToArray();

            return;
        }

        public override TupleObject<TEntity> AlignWithSchema(
            TupleObjectSchema<TEntity> schema,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
        {
            if (object.ReferenceEquals(Schema, schema)) return this;

            builder = builder ?? factory.GetBuilder<TEntity>();

            return factory.CreateConjunctive(
                AlignTuples(), 
                schema.PassToBuilder, 
                builder);

            IEnumerable<TupleObject<TEntity>> AlignTuples()
            {
                for (int i = 0; i < _tuples.Length; i++)
                {
                    yield return _tuples[i].AlignWithSchema(schema, factory, builder);
                }
            }
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

        public abstract TupleObject<TEntity> Reproduce(
            IEnumerable<TupleObject<TEntity>> tuples,
            TupleObjectFactory factory,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder);

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
