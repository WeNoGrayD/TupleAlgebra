using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.DisjunctiveTupleSystemInfrastructure
{
    using static TupleObjectConversionToAlternateOperator;

    public class DisjunctiveTupleSystemConversionToAlternateOperator<TEntity>
        : TupleObjectFactoryUnarySetOperator<TEntity, DisjunctiveTupleSystem<TEntity>>
        where TEntity : new()
    {
        public override TupleObject<TEntity> Visit(
            DisjunctiveTupleSystem<TEntity> first,
            TupleObjectFactory factory)
        {
            return ConvertToAlternate(first, factory);
        }
    }
}
