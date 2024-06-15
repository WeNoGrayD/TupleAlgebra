using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using System.Collections;
using TupleAlgebraClassLib.TupleObjectInfrastructure.DisjunctiveTupleInfrastructure;

namespace TupleAlgebraClassLib.TupleObjects
{
    using static TupleObjectHelper;
    using static TupleObjectStaticDataStorage;

    public class ArrayEnumerator<T>
        : IEnumerator<T>
    {
        private T[] _source;

        public int Index { get; private set; }

        object IEnumerator.Current => Current;

        public T Current { get => _source[Index]; }

        public ArrayEnumerator(T[] source)
        {
            _source = source;
            Reset();

            return;
        }

        public bool MoveNext()
        {
            return ++Index < _source.Length;
        }

        public void Reset()
        {
            Index = 0;
        }

        public void Dispose()
        {
            return;
        }
    }

    public class AttributeComponentEnumerator
        : ArrayEnumerator<IAttributeComponent>
    {
        public AttributeComponentEnumerator(IAttributeComponent[] source)
            : base(source)
        { return; }
    }

    public class DisjunctiveTuple<TEntity> : SingleTupleObject<TEntity>
        where TEntity : new()
    {
        #region Constructors

        static DisjunctiveTuple()
        {
            Storage.RegisterType<TEntity, DisjunctiveTuple<TEntity>>(
                (factory) => new DisjunctiveTupleOperationExecutorsContainer(factory));

            return;
        }


        public DisjunctiveTuple(TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(onTupleBuilding)
        { }

        public DisjunctiveTuple(TupleObjectSchema<TEntity> schema)
            : base(schema)
        { }

        #endregion

        #region Instance methods

        public override bool IsDefault(IAttributeComponent component)
        {
            return component.IsEmpty;
        }

        public override IAttributeComponent<TAttribute>
            GetDefaultFictionalAttributeComponent<TAttribute>(
                IAttributeComponentFactory<TAttribute> factory)
        {
            return factory.CreateEmpty();
        }

        public override TupleObject<TEntity> Reproduce(
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> components,
            TupleObjectFactory factory,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
        {
            return factory.CreateDisjunctiveTuple(
                components, 
                onTupleBuilding,
                builder);
        }

        #endregion

        protected override IEnumerator<TEntity> GetEnumeratorImpl()
        {
            return (!this).GetEnumerator();
        }

        public AttributeComponentEnumerator GetAttributeComponentEnumerator()
        {
            return new AttributeComponentEnumerator(_components);
        }

        #region Nested types

        public class DisjunctiveTupleOperationExecutorsContainer
            : NonFictionalTupleObjectOperationExecutorsContainer<DisjunctiveTuple<TEntity>>
        {
            #region Constructors

            public DisjunctiveTupleOperationExecutorsContainer(
                TupleObjectFactory factory)
                : base(factory,
                       () => new DisjunctiveTupleComplementionOperator<TEntity>(),
                       () => new DisjunctiveTupleConversionToAlternateOperator<TEntity>(),
                       () => new DisjunctiveTupleIntersectionOperator<TEntity>(),
                       () => new DisjunctiveTupleUnionOperator<TEntity>(),
                       () => new DisjunctiveTupleExceptionOperator<TEntity>(),
                       () => new DisjunctiveTupleSymmetricExceptionOperator<TEntity>(),
                       () => new DisjunctiveTupleInclusionComparer<TEntity>(),
                       () => new DisjunctiveTupleEqualityComparer<TEntity>(),
                       () => new DisjunctiveTupleInclusionOrEqualityComparer<TEntity>())
            {
                return;
            }

            #endregion
        }

        #endregion
    }
}
