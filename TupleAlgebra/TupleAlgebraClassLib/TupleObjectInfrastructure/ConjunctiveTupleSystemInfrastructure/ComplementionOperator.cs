using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleSystemInfrastructure
{
    using static TupleObjectComplementionOperations;

    public sealed class ConjunctiveTupleSystemComplementionOperator<TEntity>
        : TupleObjectFactoryUnarySetOperator<TEntity, ConjunctiveTupleSystem<TEntity>>
        where TEntity : new()
    {
        public override TupleObject<TEntity> Visit(
            ConjunctiveTupleSystem<TEntity> first,
            TupleObjectFactory factory)
        {
            return Complement(first, factory);
        }
    }
}
