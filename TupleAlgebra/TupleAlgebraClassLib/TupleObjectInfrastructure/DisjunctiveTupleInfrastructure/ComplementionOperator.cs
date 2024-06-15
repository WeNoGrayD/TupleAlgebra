using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.DisjunctiveTupleInfrastructure
{
    using static TupleObjectComplementionOperations;

    public sealed class DisjunctiveTupleComplementionOperator<TEntity>
        : TupleObjectFactoryUnarySetOperator<TEntity, DisjunctiveTuple<TEntity>>
        where TEntity : new()
    {
        public override TupleObject<TEntity> Visit(
            DisjunctiveTuple<TEntity> first,
            TupleObjectFactory factory)
        {
            return Complement(first, factory);
        }
    }
}
