using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using static TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators.TupleObjectFactoryMethods;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators
{
    using static TupleObjectFactoryMethods;

    public static class TupleObjectConversionToAlternateOperator
    {
        private static TupleObject<TEntity> ConvertTupleToAlternateTupleSystem<TEntity>(
            SingleTupleObject<TEntity> tuple,
            TupleObjectFactory factory,
            SingleTupleObjectFactoryHandler<TEntity> singleTupleFactory,
            TupleObjectSystemFactoryHandler<TEntity> tupleSysFactory)
            where TEntity : new()
        {
            TupleObjectSchema<TEntity> schema = tuple.Schema;
            int len = tuple.RowLength;
            IndexedComponentFactoryArgs<IAttributeComponent>[] tupleFactoryArgs = 
                new IndexedComponentFactoryArgs<IAttributeComponent>[len];
            InitDefaultFactoryArgs<TEntity>(tupleFactoryArgs, schema);

            return tupleSysFactory(
                MakeTuples(),
                schema.PassToBuilder,
                factory);

            IEnumerable<TupleObject<TEntity>>
                MakeTuples()
            {
                for (int attrLoc = 0; attrLoc < tuple.RowLength; attrLoc++)
                {
                    yield return singleTupleFactory(
                        tupleFactoryArgs,
                        schema.PassToBuilder,
                        factory);
                    tupleFactoryArgs[attrLoc] = CreateDefaultFactoryArg(
                        attrLoc, tupleFactoryArgs);
                }

                yield break;
            }
        }

        private static TupleObject<TEntity> ConvertTupleToAlternateTupleSystem<
            TEntity,
            TSingleTupleObject>(
            TSingleTupleObject tuple,
            TupleObjectFactory factory,
            Func<TupleObjectFactory, TSingleTupleObject, TupleObject<TEntity>> tupleSysFactory)
            where TEntity : new()
            where TSingleTupleObject : SingleTupleObject<TEntity>
        {
            return tupleSysFactory(
                factory,
                tuple);
        }

        private static TupleObject<TEntity> ConvertTupleSystemToAlternateTuple<
            TEntity,
            TSingleTupleObject>(
            TupleObjectSystem<TEntity, TSingleTupleObject> tupleSys,
            TupleObjectFactory factory,
            SingleTupleObjectFactoryHandler<TEntity> singleTupleFactory)
            where TEntity : new()
            where TSingleTupleObject : SingleTupleObject<TEntity>
        {
            if (tupleSys.IsDiagonal)
            {
                TupleObjectSchema<TEntity> schema = tupleSys.Schema;
                IndexedComponentFactoryArgs<IAttributeComponent>[] components =
                    new IndexedComponentFactoryArgs<IAttributeComponent>[tupleSys.RowLength];
                InitDefaultFactoryArgs<TEntity>(components, schema);

                /*
                 * Диагональность подразумевает следующую структуру системы кортежей:
                 *  A | * | *
                 *  _ | B | *
                 *  _ | _ | C
                 *  При изменении порядка столбцов потребуется другой алгоритм.
                 */
                /*
                int schemaLen = tupleSys.RowLength,
                    attrLoc = 0,
                    lastNonEmptyAttrLoc;
                for (int tuplePtr = 0; tuplePtr < tupleSys.ColumnLength; tuplePtr++)
                {
                    for ( ; attrLoc < schemaLen; attrLoc++)
                    {
                        if (!tupleSys[tuplePtr].IsDefault(attrLoc)) continue;

                        lastNonEmptyAttrLoc = attrLoc - 1;
                        components[lastNonEmptyAttrLoc] = new(
                            lastNonEmptyAttrLoc,
                            tupleSys.Schema,
                            tupleSys[tuplePtr][lastNonEmptyAttrLoc]);

                        break;
                    }
                }
                */

                int schemaLen = tupleSys.RowLength,
                    tuplePtr;
                for (int attrLoc = 0; attrLoc < schemaLen; attrLoc++)
                {
                    for (tuplePtr = 0; tuplePtr < tupleSys.ColumnLength; tuplePtr++)
                    {
                        if (tupleSys[tuplePtr].IsDefault(attrLoc)) continue;

                        components[attrLoc] = new(
                            attrLoc,
                            tupleSys.Schema,
                            tupleSys[tuplePtr][attrLoc]);

                        break;
                    }
                }

                return singleTupleFactory(
                    components,
                    schema.PassToBuilder, 
                    factory);
            }

            return null;

            //throw new InvalidOperationException(
            //    "Перевод недиагональных систем кортежей в альтернативные формы отображения не поддерживается.");
        }

        public static TupleObject<TEntity> ConvertToAlternate<TEntity>(
            ConjunctiveTuple<TEntity> tuple,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return factory.CreateDiagonalDisjunctiveTupleSystem(tuple);
        }

        public static TupleObject<TEntity> ConvertToAlternate<TEntity>(
            DisjunctiveTuple<TEntity> tuple,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return factory.CreateDiagonalConjunctiveTupleSystem(tuple);
        }

        public static TupleObject<TEntity> ConvertToAlternate<TEntity>(
            ConjunctiveTupleSystem<TEntity> tupleSys,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return ConvertTupleSystemToAlternateTuple(
                tupleSys,
                factory,
                DisjunctiveTupleFactory<TEntity>) ??
                tupleSys.TrueUnion();
        }

        public static TupleObject<TEntity> ConvertToAlternate<TEntity>(
            DisjunctiveTupleSystem<TEntity> tupleSys,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return ConvertTupleSystemToAlternateTuple(
                tupleSys,
                factory,
                ConjunctiveTupleFactory<TEntity>) ??
                tupleSys.TrueIntersect();
        }
    }
}
