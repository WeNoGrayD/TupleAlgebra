﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.EmptyTupleObjectInfrastructure
{
    public sealed class EmptyTupleObjectComplementionOperator<TEntity>
        : TupleObjectFactoryUnarySetOperator<TEntity, EmptyTupleObject<TEntity>>
        where TEntity : new()
    {
        public override TupleObject<TEntity> Visit(
            EmptyTupleObject<TEntity> first, 
            TupleObjectFactory factory)
        {
            return factory.CreateFull<TEntity>(first.PassSchema);
        }
    }
}
