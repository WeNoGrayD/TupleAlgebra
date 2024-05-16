using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using TupleAlgebraClassLib.TupleObjectAcceptors;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.FullTupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectAcceptors;

namespace TupleAlgebraClassLib.TupleObjects
{
    using static TupleObjectHelper;
    using static TupleObjectStaticDataStorage;

    public class FullTupleObject<TEntity> : TupleObject<TEntity>
        where TEntity : new()
    {
        static FullTupleObject()
        {
            Storage.RegisterType<TEntity, FullTupleObject<TEntity>>(
                (factory) => new FullTupleObjectOperationExecutorsContainer(factory));

            return;
        }

        public FullTupleObject(TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            : base(onTupleBuilding)
        {

        }

        public FullTupleObject(TupleObjectSchema<TEntity> schema)
            : base(schema)
        { }

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

        protected override void DisposeImpl()
        {
            return;
        }

        protected override IEnumerator<TEntity> GetEnumeratorImpl()
        {
            return null;
        }

        public override TupleObject<TEntity> Convert(TupleObject<TEntity> diagonal)
        {
            throw new NotImplementedException();
        }

        public override TupleObject<TEntity> Diagonal()
        {
            throw new NotImplementedException();
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
