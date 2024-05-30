using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectAcceptors;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleSystemInfrastructure
{
    using static TupleObjectConversionToAlternateOperator;

    public class ConjunctiveTupleSystemConversionToAlternateOperator<TEntity>
        : TupleObjectFactoryUnarySetOperator<TEntity, ConjunctiveTupleSystem<TEntity>>
        where TEntity : new()
    {
        public override TupleObject<TEntity> Accept(
            ConjunctiveTupleSystem<TEntity> first,
            TupleObjectFactory factory)
        {
            return ConvertToAlternate(first, factory);
        }
    }
}
