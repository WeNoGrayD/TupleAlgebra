using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjects;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleInfrastructure
{
    using static TupleObjectComplementionOperations;

    public sealed class ConjunctiveTupleComplementionOperator<TEntity>
        : TupleObjectFactoryUnarySetOperator<TEntity, ConjunctiveTuple<TEntity>>
        where TEntity : new ()
    {
        public override TupleObject<TEntity> Visit(
            ConjunctiveTuple<TEntity> first,
            TupleObjectFactory factory)
        {
            return Complement(first, factory);
        }
    }
}
