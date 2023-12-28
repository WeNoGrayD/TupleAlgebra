using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.TupleObjects
{
    public class DisjunctiveTuple<TEntity> : SingleTupleObject<TEntity>
        where TEntity : new()
    {
        #region Constructors

        public DisjunctiveTuple(Action<TupleObjectBuilder<TEntity>> onTupleBuilding = null)
            : base(onTupleBuilding)
        { }

        #endregion

        #region Instance methods

        protected override AttributeComponent<TData> GetDefaultFictionalAttributeComponentImpl<TData>(AttributeInfo attribute)
        {
            //var factoryArgs = new AttributeComponentFactoryInfrastructure.AttributeComponentFactoryArgs();
            //factoryArgs.SetAttributeDomainGetter(() => attribute.GetDomain<TData>());
            return null;
            //return AttributeComponent<TData>.FictionalAttributeComponentFactory.CreateEmpty<TData>(factoryArgs);
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

        protected override TupleObject<TEntity> ComplementThe()
        {
            return null;
        }

        protected override TupleObject<TEntity> IntersectWith(TupleObject<TEntity> second)
        {
            return null;
        }

        protected override TupleObject<TEntity> UnionWith(TupleObject<TEntity> second)
        {
            return null;
        }

        protected override TupleObject<TEntity> ExceptWith(TupleObject<TEntity> second)
        {
            return null;
        }

        protected override TupleObject<TEntity> SymmetricExceptWith(TupleObject<TEntity> second)
        {
            return null;
        }

        public override TupleObject<TEntity> Diagonal()
        {
            throw new NotImplementedException();
        }
    }
}
