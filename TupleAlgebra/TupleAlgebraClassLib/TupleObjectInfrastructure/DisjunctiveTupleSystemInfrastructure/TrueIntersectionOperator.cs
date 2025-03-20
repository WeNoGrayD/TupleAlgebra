using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.DisjunctiveTupleSystemInfrastructure
{
    using static TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators.TupleObjectFactoryMethods;
    using static TupleObjectConversionToAlternateOperator;

    public class DisjunctiveTupleSystemTrueIntersectionOperator<TEntity>
        where TEntity : new()
    {
        public TupleObject<TEntity> Intersect(
            DisjunctiveTupleSystem<TEntity> dSys,
            TupleObjectFactory factory)
        {
            return ConvertToAlternate(dSys, factory);//TrueIntersect(dSys, factory);
        }
    }
}
