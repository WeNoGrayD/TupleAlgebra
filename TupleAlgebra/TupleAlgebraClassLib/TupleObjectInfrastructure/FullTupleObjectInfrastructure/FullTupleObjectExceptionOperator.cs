﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.FullTupleObjectInfrastructure
{
    public sealed class FullTupleObjectExceptionOperator<TEntity>
        : TupleObjectCrossTypeInstantDefaultBinaryVisitor<TEntity, FullTupleObject<TEntity>, TupleObject<TEntity>>
        where TEntity : new()
    {
        public override TupleObject<TEntity> Visit(
            FullTupleObject<TEntity> first,
            EmptyTupleObject<TEntity> second)
        {
            return second;
        }

        public override TupleObject<TEntity> VisitDefault(
            FullTupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return ~second;
        }

        public override TupleObject<TEntity> Visit(
            FullTupleObject<TEntity> first,
            FullTupleObject<TEntity> second)
        {
            return ~second;
        }
    }
}
