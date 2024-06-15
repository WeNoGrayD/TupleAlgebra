using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleSystemInfrastructure
{
    using static UniversalClassLib.CartesianProductHelper;
    using static TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators.TupleObjectFactoryMethods;
    using static TupleObjectConversionToAlternateOperator;

    public class ConjunctiveTupleSystemTrueUnionOperator<TEntity>
        where TEntity : new()
    {
        public TupleObject<TEntity> Union(
            ConjunctiveTupleSystem<TEntity> cSys,
            TupleObjectFactory factory)
        {
            return ConvertToAlternate(cSys, factory);//TrueUnion(cSys, factory);
        }
    }
}
