using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjects;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators
{
    using static TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectHelper;
    using static TupleObjectFactoryMethods;
    
    public static class TupleObjectComplementionOperations
    {
        private static TupleObject<TEntity> ComplementTuple<TEntity>(
            SingleTupleObject<TEntity> tuple,
            TupleObjectFactory factory,
            SingleTupleObjectFactoryHandler<TEntity> tupleComplementionFactory)
            where TEntity : new()
        {
            TupleObjectSchema<TEntity> schema = tuple.Schema;
            int len = tuple.RowLength;

            return tupleComplementionFactory(
                GetFactoryArgs(),
                schema.PassToBuilder, 
                factory);

            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> GetFactoryArgs()
            {
                for (int attrLoc = 0; attrLoc < len; attrLoc++)
                {
                    yield return new(attrLoc, schema, tuple[attrLoc].ComplementThe());
                }

                yield break;
            }
        }

        private static TupleObject<TEntity> ComplementTupleSystem<
            TEntity, 
            TSingleTupleObject>(
            TupleObjectSystem<TEntity, TSingleTupleObject> tupleSys,
            TupleObjectFactory factory,
            SingleTupleObjectFactoryHandler<TEntity> tupleComplementionFactory,
            TupleObjectSystemFactoryHandler<TEntity> tupleSysComplementionFactory)
            where TEntity : new()
            where TSingleTupleObject : SingleTupleObject<TEntity>
        {
            TupleObjectSchema<TEntity> schema = tupleSys.Schema;

            return tupleSysComplementionFactory(
                MakeTuples(), 
                schema.PassToBuilder,
                factory);

            IEnumerable<TupleObject<TEntity>>
                MakeTuples()
            {
                for (int i = 0; i < tupleSys.ColumnLength; i++)
                {
                    yield return ComplementTuple(
                        tupleSys[i],
                        factory,
                        tupleComplementionFactory);
                }

                yield break;
            }
        }

        public static TupleObject<TEntity> Complement<TEntity>(
            ConjunctiveTuple<TEntity> tuple,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return ComplementTuple(
                tuple, 
                factory, 
                DisjunctiveTupleFactory<TEntity>);
        }

        public static TupleObject<TEntity> Complement<TEntity>(
            DisjunctiveTuple<TEntity> tuple,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return ComplementTuple(
                tuple,
                factory,
                ConjunctiveTupleFactory<TEntity>);
        }

        public static TupleObject<TEntity> Complement<TEntity>(
            ConjunctiveTupleSystem<TEntity> tupleSys,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return ComplementTupleSystem(
                tupleSys,
                factory,
                DisjunctiveTupleFactory<TEntity>,
                DisjunctiveTupleSystemFactory<TEntity>);
        }

        public static TupleObject<TEntity> Complement<TEntity>(
            DisjunctiveTupleSystem<TEntity> tupleSys,
            TupleObjectFactory factory)
            where TEntity : new()
        {
            return ComplementTupleSystem(
                tupleSys,
                factory,
                ConjunctiveTupleFactory<TEntity>,
                ConjunctiveTupleSystemFactory<TEntity>);
        }
    }
}
