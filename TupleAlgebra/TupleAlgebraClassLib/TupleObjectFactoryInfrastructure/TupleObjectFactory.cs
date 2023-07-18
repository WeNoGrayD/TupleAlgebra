using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectFactoryInfrastructure
{
    public class TupleObjectFactory
    {
        private TAContext _context;

        public TupleObjectFactory(TAContext context)
        {
            _context = context;
        }

        public TupleObject<TEntity> CreateEmpty<TEntity>()
        {
            TupleObject<TEntity> empty = null;
            SubscribeOnContextDisposing(empty);

            return null;
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            TEntity singleEntity,
            Action<TupleObjectBuilder<TEntity>> onTupleBuilding = null)
        {
            if (singleEntity is not null)
            {
                ConjunctiveTuple<TEntity> cTuple = new ConjunctiveTuple<TEntity>(onTupleBuilding);
                IDictionary<AttributeName, IAlgebraicSetObject> components = cTuple.Schema.InitAttributes();
                AttributeInfo attribute;
                foreach (AttributeName attributeName in components.Keys)
                {
                    attribute = cTuple.Schema[attributeName];
                    cTuple[attributeName] =
                        attribute.GetDomain().CreateAttributeComponent(attribute, new[] { singleEntity });
                }

                if (!cTuple.ContainsEmptyAttributeComponent())
                {
                    SubscribeOnContextDisposing(cTuple);

                    return cTuple;
                }
            }

            return CreateEmpty<TEntity>();
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            IEnumerable<TEntity> dataSource,
            Action<TupleObjectBuilder<TEntity>> onTupleBuilding = null)
        {
            if (dataSource.Count() == 0)
            {
                return CreateEmpty<TEntity>();
            }

            return null;
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>()
        {
            return null;
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            TEntity singleData,
            Action<TupleObjectBuilder<TEntity>> onTupleBuilding = null)
        {
            if (singleData is null)
            {
                return CreateEmpty<TEntity>();
            }



            return null;
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            IEnumerable<TEntity> dataSource,
            Action<TupleObjectBuilder<TEntity>> onTupleBuilding = null)
        {
            if (dataSource.Count() == 0)
            {
                return CreateEmpty<TEntity>();
            }

            return null;
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>()
        {
            return null;
        }

        public TupleObject<TEntity> CreateFull<TEntity>()
        {
            TupleObject<TEntity> full= null;
            SubscribeOnContextDisposing(full);

            return null;
        }

        protected void SubscribeOnContextDisposing(IDisposable tupleObject)
        {
            if (tupleObject is not null) _context.Disposing += () => tupleObject.Dispose();

            return;
        }
    }
}
