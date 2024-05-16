using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;

namespace TupleAlgebraClassLib.TupleObjects
{
    using static TupleObjectHelper;

    public class DisjunctiveTuple<TEntity> : SingleTupleObject<TEntity>
        where TEntity : new()
    {
        #region Constructors

        public DisjunctiveTuple(TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(onTupleBuilding)
        { }

        public DisjunctiveTuple(TupleObjectSchema<TEntity> schema)
            : base(schema)
        { }

        #endregion

        #region Instance methods

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
            return factory.CreateDisjunctive(components, onTupleBuilding, builder);
        }

        public override TupleObject<TEntity> ToAlternateDiagonal(
            TupleObjectFactory factory)
        {
            return factory.CreateDiagonalConjunctiveSystem<TEntity>(
                this);
        }

        #endregion

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
    }
}
