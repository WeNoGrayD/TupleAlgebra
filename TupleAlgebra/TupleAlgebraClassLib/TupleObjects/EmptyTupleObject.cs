﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.EmptyTupleObjectInfrastructure;

namespace TupleAlgebraClassLib.TupleObjects
{
    public class EmptyTupleObject<TEntity> : TupleObject<TEntity>
        where TEntity : new()
    {
        public EmptyTupleObject(Action<TupleObjectBuilder<TEntity>> onTupleBuilding)
            : base(onTupleBuilding)
        {

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

        protected override TupleObject<TEntity> ComplementThe()
        {
            return null;
        }

        protected override TupleObject<TEntity> IntersectWith(TupleObject<TEntity> second)
        {
            EmptyTupleObjectIntersectionOperator<TEntity> i = new EmptyTupleObjectIntersectionOperator<TEntity>();
            

            return i.Accept(this, second);
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
