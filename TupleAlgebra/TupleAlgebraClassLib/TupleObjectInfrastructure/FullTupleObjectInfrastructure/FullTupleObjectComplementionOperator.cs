﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.FullTupleObjectInfrastructure
{
    public sealed class FullTupleObjectComplementionOperator<TEntity>
        : TupleObjectFactoryUnarySetOperator<TEntity, FullTupleObject<TEntity>>
        where TEntity : new()
    {
        public override TupleObject<TEntity> Visit(
            FullTupleObject<TEntity> first,
            TupleObjectFactory factory)
        {
            return factory.CreateEmpty<TEntity>(first.PassSchema);
        }
    }
}
