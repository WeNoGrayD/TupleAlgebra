using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using TupleAlgebraClassLib.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.FullTupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;

namespace TupleAlgebraClassLib.TupleObjects
{
    using static TupleObjectHelper;
    using static TupleObjectStaticDataStorage;

    public class FullTupleObject<TEntity> : TupleObject<TEntity>
        where TEntity : new()
    {
        private ConjunctiveTuple<TEntity> _inner;

        static FullTupleObject()
        {
            Storage.RegisterType<TEntity, FullTupleObject<TEntity>>(
                (factory) => new FullTupleObjectOperationExecutorsContainer(factory));

            return;
        }

        public FullTupleObject(
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            ConjunctiveTuple<TEntity> inner)
            : base(onTupleBuilding)
        {
            _inner = inner;

            return;
        }

        public FullTupleObject(
            TupleObjectSchema<TEntity> schema,
            ConjunctiveTuple<TEntity> inner)
            : base(schema)
        {
            _inner = inner;

            return;
        }

        public override TupleObject<TEntity> AlignWithSchema(
            TupleObjectSchema<TEntity> schema,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
        {
            return factory.CreateFull<TEntity>(schema.PassToBuilder);
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override bool IsFull()
        {
            return true;
        }

        public override bool IsFalse()
        {
            return false;
        }

        public override bool IsTrue()
        {
            return true;
        }

        protected override void DisposeImpl()
        {
            return;
        }

        protected override IEnumerator<TEntity> GetEnumeratorImpl()
        {
            return _inner.GetEnumerator();
        }

        #region Nested types

        public class FullTupleObjectOperationExecutorsContainer
            : FictionalTupleObjectOperationExecutorsContainer<FullTupleObject<TEntity>>
        {
            #region Constructors

            public FullTupleObjectOperationExecutorsContainer(
                TupleObjectFactory factory)
                : base(factory,
                       () => new FullTupleObjectComplementionOperator<TEntity>(),
                       () => new FullTupleObjectIntersectionOperator<TEntity>(),
                       () => new FullTupleObjectUnionOperator<TEntity>(),
                       () => new FullTupleObjectExceptionOperator<TEntity>(),
                       () => new FullTupleObjectSymmetricExceptionOperator<TEntity>(),
                       () => new FullTupleObjectInclusionComparer<TEntity>(),
                       () => new FullTupleObjectEqualityComparer<TEntity>(),
                       () => new FullTupleObjectInclusionOrEqualityComparer<TEntity>())
            {
                return;
            }

            #endregion
        }

        #endregion
    }
}
