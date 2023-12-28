using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using TupleAlgebraClassLib.AttributeComponents;

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
            where TEntity : new()
        {
            TupleObject<TEntity> empty = null;
            SubscribeOnContextDisposing(empty);

            return null;
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            TEntity singleEntity,
            Action<TupleObjectBuilder<TEntity>> onTupleBuilding = null)
            where TEntity : new()
        {
            if (singleEntity is not null)
            {
                ConjunctiveTuple<TEntity> cTuple = new ConjunctiveTuple<TEntity>(onTupleBuilding);
                IDictionary<AttributeName, IAlgebraicSetObject> components = cTuple.Schema.InitAttributes();
                AttributeInfo attribute;
                foreach (AttributeName attributeName in components.Keys)
                {
                    attribute = cTuple.Schema[attributeName];
                    //cTuple[attributeName] =
                    //    attribute.GetDomain().CreateAttributeComponent(attribute, new[] { singleEntity });
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
            where TEntity : new()
        {
            if (dataSource.Count() == 0)
            {
                return CreateEmpty<TEntity>();
            }

            return null;
        }

        /// <summary>
        /// Проверка перечня выражений доступа к свойствам-атрибутам на соответствие структуре вида:
        /// (TEntity entity) => entity.SomeProperty;
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="attributeInfoCollector"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        protected bool CheckAttributes<TEntity>(
            AttributeInfoCollector<TEntity> attributeInfoCollector,
            (LambdaExpression AttributeGetterExpr, IAttributeComponent AttributeComponent)[] attributes,
            out int errorOn)
        {
            errorOn = -1;

            for (int i = 0; i < attributes.Length; i++)
            {
                if (!attributeInfoCollector.CheckAndCollect(
                    attributes[i].AttributeGetterExpr))
                {
                    errorOn = i;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Создание самостоятельного одиночного C-кортежа.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="onTupleBuilding"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            Action<TupleObjectBuilder<TEntity>> onTupleBuilding = null,
            params (LambdaExpression AttributeGetterExpr, IAttributeComponent AttributeComponent)[] attributes)
            where TEntity : new()
        {
            TupleObjectBuilder<TEntity> builder = new TupleObjectBuilder<TEntity>();
            if (onTupleBuilding is not null) onTupleBuilding(builder);
            AttributeInfoCollector<TEntity> attributeInfoCollector =
                new AttributeInfoCollector<TEntity>(builder);
            TupleObjectSchema<TEntity> schema = attributeInfoCollector.Schema;
            
            if (!CheckAttributes(attributeInfoCollector, attributes, out int errorOn))
                throw new ArgumentException($"Атрибут {errorOn} не распознан:{attributes[errorOn].AttributeGetterExpr}");

            schema.EndInitializingAttributes();

            return null;
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>(
            Action<TupleObjectBuilder<TEntity>> onTupleBuilding = null,
            AttributeInfoCollector<TEntity> attributeInfoCollector = null,
            params (LambdaExpression AttributeGetterExpr, IAttributeComponent AttributeComponent)[] attributes)
            where TEntity : new()
        {
            bool singleMode = attributeInfoCollector is null;
            if (singleMode)
            {
                TupleObjectBuilder<TEntity> builder = new TupleObjectBuilder<TEntity>();
                if (onTupleBuilding is not null) onTupleBuilding(builder);
                attributeInfoCollector =
                    new AttributeInfoCollector<TEntity>(builder);
            }

            TupleObjectSchema<TEntity> schema = attributeInfoCollector.Schema;

            for (int i = 0; i < attributes.Length; i++)
            {
                if (!attributeInfoCollector.CheckAndCollect(attributes[i].AttributeGetterExpr))
                    throw new ArgumentException("Какой-то атрибут не атрибут.");
            }

            if (singleMode)
            {
                schema.EndInitializingAttributes();
            }

            return null;
        }

        public TupleObject<TEntity> CreateConjunctive<TEntity>()
            where TEntity : new()
        {
            return null;
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>(
            TEntity singleData,
            Action<TupleObjectBuilder<TEntity>> onTupleBuilding = null)
            where TEntity : new()
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
            where TEntity : new()
        {
            if (dataSource.Count() == 0)
            {
                return CreateEmpty<TEntity>();
            }

            return null;
        }

        public TupleObject<TEntity> CreateDisjunctive<TEntity>()
            where TEntity : new()
        {
            return null;
        }

        public TupleObject<TEntity> CreateFull<TEntity>()
            where TEntity : new()
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

        /// <summary>
        /// Посетитель деревьев выражений, которые предположительно представляют собой
        /// функции получения свойств из объекта типа TEntity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        public class AttributeInfoCollector<TEntity>
        {
            public TupleObjectSchema<TEntity> Schema { get; init; }

            public AttributeInfoCollector(TupleObjectBuilder<TEntity> builder)
            {
                Schema = builder.Schema;

                return;
            }

            public bool CheckAndCollect(LambdaExpression attributeGetterExpr)
            {
                if (attributeGetterExpr.Parameters.Count != 1) return false;

                ParameterExpression entityParamExpr = attributeGetterExpr.Parameters[0];
                if (entityParamExpr.Type != typeof(TEntity)) return false;

                MemberExpression propertyGetterExpr = attributeGetterExpr.Body as MemberExpression;
                if (propertyGetterExpr is null ||
                    !propertyGetterExpr.Expression.Equals(entityParamExpr)) 
                    return false;

                PropertyInfo attributePropertyInfo = propertyGetterExpr.Member as PropertyInfo;
                if (attributePropertyInfo is null) return false;

                Schema.AttachAttribute(attributePropertyInfo.Name);

                return true;
            }
        }
    }
}
