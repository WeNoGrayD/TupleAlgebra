using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleSystemInfrastructure
{
    using static UniversalClassLib.CartesianProductHelper;
    using static TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators.TupleObjectFactoryMethods;

    public class ConjunctiveTupleSystemTrueUnionOperator<TEntity>
        where TEntity : new()
    {
        public TupleObject<TEntity> Union(
            ConjunctiveTupleSystem<TEntity> cSys,
            TupleObjectFactory factory)
        {
            return factory.CreateDisjunctiveTupleSystem(
                Union(cSys.Schema, cSys.Tuples, factory),
                cSys.PassSchema,
                null);
        }

        private IEnumerable<DisjunctiveTuple<TEntity>> Union(
            TupleObjectSchema<TEntity> schema,
            ConjunctiveTuple<TEntity>[] tuples,
            TupleObjectFactory factory)
        {
            int schemaLen = schema.PluggedAttributesCount,
                csysLen = tuples.Length,
                attrLoc;
            //TupleObjectBuilder<TEntity> builder = factory.GetBuilder(schema);
            if (csysLen < 2)
                throw new InvalidOperationException("У C-системы не должно быть меньше двух кортежей.");

            IndexedComponentFactoryArgs<IAttributeComponent>[]
                opFactoryArgs,
                op1FactoryArgs =
                new IndexedComponentFactoryArgs<IAttributeComponent>[schemaLen],
                op2FactoryArgs =
                new IndexedComponentFactoryArgs<IAttributeComponent>[schemaLen];
            IndexedComponentFactoryArgs<IAttributeComponent>[][] tupleOpFactoryArgs =
                new IndexedComponentFactoryArgs<IAttributeComponent>[csysLen][];
            for (int i = 0; i < csysLen; i++)
            {
                tupleOpFactoryArgs[i] = opFactoryArgs =
                    new IndexedComponentFactoryArgs<IAttributeComponent>[schemaLen];
                InitDefaultFactoryArgs<TEntity>(opFactoryArgs, schema);
            }

            return GetAccumulatedCartesianProduct(
                (RectangleInfo rect) => factory.CreateDisjunctiveTuple<TEntity>(
                    rect.Components,
                    schema.PassToBuilder,
                    null),
                tuples,
                (stackPtr, tuple) => RectangleInfo
                    .FromTuple(tuple, tupleOpFactoryArgs[stackPtr]),
                RectangleInfo.DefineBranchActionForUnion,
                RectangleInfo.BranchAccumulatorFactoryForUnion,
                tuple => !tuple.IsFull(),
                RectangleInfo.ResetFactoryArgs)
                    .OfType<DisjunctiveTuple<TEntity>>();
        }
    }
}
