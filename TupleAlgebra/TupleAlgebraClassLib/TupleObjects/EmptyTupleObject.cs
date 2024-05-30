using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.EmptyTupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.FullTupleObjectInfrastructure;

namespace TupleAlgebraClassLib.TupleObjects
{
    using static TupleObjectHelper;
    using static TupleObjectStaticDataStorage;

    public class EmptyTupleObject<TEntity> : TupleObject<TEntity>
        where TEntity : new()
    {
        static EmptyTupleObject()
        {
            Storage.RegisterType<TEntity, EmptyTupleObject<TEntity>>(
                (factory) => new EmptyTupleObjectOperationExecutorsContainer(factory));

            return;
        }

        public EmptyTupleObject(TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            : base(onTupleBuilding)
        {

        }

        public EmptyTupleObject(TupleObjectSchema<TEntity> schema)
            : base(schema)
        { }

        public override TupleObject<TEntity> AlignWithSchema(
            TupleObjectSchema<TEntity> schema,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
        {
            return factory.CreateEmpty<TEntity>(schema.PassToBuilder);
        }

        public override bool IsEmpty()
        {
            return true;
        }

        public override bool IsFull()
        {
            return false;
        }

        protected override void DisposeImpl()
        {
            return;
        }

        protected override IEnumerator<TEntity> GetEnumeratorImpl()
        {
            return Enumerable.Empty<TEntity>().GetEnumerator();
        }

        #region Nested types

        public class EmptyTupleObjectOperationExecutorsContainer
            : FictionalTupleObjectOperationExecutorsContainer<EmptyTupleObject<TEntity>>
        {
            #region Constructors

            public EmptyTupleObjectOperationExecutorsContainer(
                TupleObjectFactory factory)
                : base(factory,
                       () => new EmptyTupleObjectComplementionOperator<TEntity>(),
                       () => new EmptyTupleObjectIntersectionOperator<TEntity>(),
                       () => new EmptyTupleObjectUnionOperator<TEntity>(),
                       () => new EmptyTupleObjectExceptionOperator<TEntity>(),
                       () => new EmptyTupleObjectSymmetricExceptionOperator<TEntity>(),
                       () => new EmptyTupleObjectInclusionComparer<TEntity>(),
                       () => new EmptyTupleObjectEqualityComparer<TEntity>(),
                       () => new EmptyTupleObjectInclusionOrEqualityComparer<TEntity>())
            {
                return;
            }

            #endregion
        }

        #endregion
    }
}
