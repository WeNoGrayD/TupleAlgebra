using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleSystemInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.DisjunctiveTupleSystemInfrastructure;
using UniversalClassLib;

namespace TupleAlgebraClassLib.TupleObjects
{
    using static TupleObjectStaticDataStorage;
    using static TupleObjectHelper;
    using static CartesianProductHelper;
    using static TupleObjectInfrastructure.TupleObjectOperators.TupleObjectFactoryMethods;

    public class DisjunctiveTupleSystem<TEntity> 
        : TupleObjectSystem<TEntity, DisjunctiveTuple<TEntity>>
        where TEntity : new()
    {
        static DisjunctiveTupleSystem()
        {
            Storage.RegisterType<TEntity, DisjunctiveTupleSystem<TEntity>>(
                (factory) => new DisjunctiveTupleSystemOperationExecutorsContainer(factory));

            return;
        }
        public DisjunctiveTupleSystem(TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(onTupleBuilding)
        { }

        public DisjunctiveTupleSystem(
            TupleObjectSchema<TEntity> schema,
            int len)
            : base(schema, len)
        { }

        public DisjunctiveTupleSystem(
            TupleObjectSchema<TEntity> schema,
            IList<ISingleTupleObject> tuples)
            : base(schema, tuples)
        { }

        public override IAttributeComponent<TAttribute>
            GetDefaultFictionalAttributeComponent<TAttribute>(
                IAttributeComponentFactory<TAttribute> factory)
        {
            return factory.CreateEmpty();
        }

        public override TupleObject<TEntity> Reproduce(
            IEnumerable<TupleObject<TEntity>> tuples,
            TupleObjectFactory factory,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
        {
            return factory.CreateDisjunctiveTupleSystem(
                tuples, 
                onTupleBuilding, 
                builder);
        }

        protected override IEnumerator<TEntity> GetEnumeratorImpl()
        {
            return TrueIntersect().GetEnumerator();
        }

        public TupleObject<TEntity> TrueIntersect()
        {
            return (SetOperations as IDisjunctiveTupleSystemOperationExecutorsContainer)!
                .TrueIntersect(this);
        }

        #region Nested types

        public interface IDisjunctiveTupleSystemOperationExecutorsContainer
        {
            public TupleObject<TEntity> TrueIntersect(
                DisjunctiveTupleSystem<TEntity> first);
        }

        public class DisjunctiveTupleSystemOperationExecutorsContainer
            : NonFictionalTupleObjectOperationExecutorsContainer<DisjunctiveTupleSystem<TEntity>>,
              IDisjunctiveTupleSystemOperationExecutorsContainer
        {
            private Lazy<DisjunctiveTupleSystemTrueIntersectionOperator<TEntity>>
                _trueIntersectionOperator;

            public DisjunctiveTupleSystemTrueIntersectionOperator<TEntity>
                TrueIntersectionOperator => _trueIntersectionOperator.Value;

            #region Constructors

            public DisjunctiveTupleSystemOperationExecutorsContainer(
                TupleObjectFactory factory)
                : base(factory,
                       () => new DisjunctiveTupleSystemComplementionOperator<TEntity>(),
                       () => new DisjunctiveTupleSystemConversionToAlternateOperator<TEntity>(),
                       () => new DisjunctiveTupleSystemIntersectionOperator<TEntity>(),
                       () => new DisjunctiveTupleSystemUnionOperator<TEntity>(),
                       () => new DisjunctiveTupleSystemExceptionOperator<TEntity>(),
                       () => new DisjunctiveTupleSystemSymmetricExceptionOperator<TEntity>(),
                       () => new DisjunctiveTupleSystemInclusionComparer<TEntity>(),
                       () => new DisjunctiveTupleSystemEqualityComparer<TEntity>(),
                       () => new DisjunctiveTupleSystemInclusionOrEqualityComparer<TEntity>())
            {
                _trueIntersectionOperator = 
                    new Lazy<DisjunctiveTupleSystemTrueIntersectionOperator<TEntity>>();

                return;
            }

            #endregion

            public TupleObject<TEntity> TrueIntersect(
                DisjunctiveTupleSystem<TEntity> first)
            {
                return TrueIntersectionOperator.Intersect(first, Factory);
            }
        }

        #endregion
    }
}
