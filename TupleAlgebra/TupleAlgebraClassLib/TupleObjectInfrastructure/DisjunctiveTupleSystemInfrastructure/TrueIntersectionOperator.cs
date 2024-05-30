using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.DisjunctiveTupleSystemInfrastructure
{
    using static UniversalClassLib.CartesianProductHelper;
    using static TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators.TupleObjectFactoryMethods;

    public class DisjunctiveTupleSystemTrueIntersectionOperator<TEntity>
        where TEntity : new()
    {
        public TupleObject<TEntity> Intersect(
            DisjunctiveTupleSystem<TEntity> dSys,
            TupleObjectFactory factory)
        {
            return factory.CreateConjunctiveTupleSystem( 
                Intersect(dSys.Schema, dSys.Tuples, factory),
                dSys.PassSchema,
                null);
        }

        private IEnumerable<ConjunctiveTuple<TEntity>> Intersect(
            TupleObjectSchema<TEntity> schema,
            DisjunctiveTuple<TEntity>[] tuples,
            TupleObjectFactory factory)
        {
            int schemaLen = schema.PluggedAttributesCount,
                dsysLen = tuples.Length,
                attrLoc;
            //TupleObjectBuilder<TEntity> builder = factory.GetBuilder(schema);
            if (dsysLen < 2)
                throw new InvalidOperationException("У D-системы не должно быть меньше двух кортежей.");

            IndexedComponentFactoryArgs<IAttributeComponent>[]
                opFactoryArgs,
                op1FactoryArgs =
                new IndexedComponentFactoryArgs<IAttributeComponent>[schemaLen],
                op2FactoryArgs =
                new IndexedComponentFactoryArgs<IAttributeComponent>[schemaLen];
            IndexedComponentFactoryArgs<IAttributeComponent>[][] tupleOpFactoryArgs =
                new IndexedComponentFactoryArgs<IAttributeComponent>[dsysLen][];
            for (int i = 0; i < dsysLen; i++)
            {
                tupleOpFactoryArgs[i] = opFactoryArgs =
                    new IndexedComponentFactoryArgs<IAttributeComponent>[schemaLen];
                InitDefaultFactoryArgs<TEntity>(opFactoryArgs, schema);
            }

            return GetAccumulatedCartesianProduct(
                (RectangleInfo rect) => factory.CreateConjunctiveTuple<TEntity>(
                    rect.Components,
                    schema.PassToBuilder,
                    null),
                tuples,
                (stackPtr, tuple) => RectangleInfo
                    .FromTuple(tuple, tupleOpFactoryArgs[stackPtr]),
                RectangleInfo.DefineBranchActionForIntersection,
                RectangleInfo.BranchAccumulatorFactoryForIntersection,
                tuple => !tuple.IsEmpty(),
                RectangleInfo.ResetFactoryArgs)
                    .OfType<ConjunctiveTuple<TEntity>>();
        }
    }
}
