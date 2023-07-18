using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    /// <summary>
    /// Построитель кортежа конкретного типа сущности.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class TupleObjectBuilder<TEntity>
    {
        public static TupleObjectBuilder<TEntity> StaticBuilder;

        /// <summary>
        /// Паттерн схемы кортежа типа сущности.
        /// </summary>
        //private static TupleObjectSchema<TEntity> _schemaPattern;

        /// <summary>
        /// Индивидуальная схема кортежа типа.
        /// </summary>
        public TupleObjectSchema<TEntity> Schema { get; private set; }

        static TupleObjectBuilder()
        {
            Init();
        }

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        public static void Init()
        {
            //_schemaPattern = new TupleObjectSchema<TEntity>();
            StaticBuilder = new TupleObjectBuilder<TEntity>(new TupleObjectSchema<TEntity>());// _schemaPattern);
            //StaticBuilder.Schema =  _schemaPattern = 
            //    new TupleObjectSchema<TEntity>(StaticBuilder);//TupleObjectSchema<TEntity>.Create(StaticBuilder);
            StaticBuilder.BuildSchemaPattern();
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        public TupleObjectBuilder()
        {
            Schema = StaticBuilder.Schema.Clone(this);
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        public TupleObjectBuilder(TupleObjectSchema<TEntity> schema)
        {
            Schema = schema;
        }

        /// <summary>
        /// Построение паттерна схемы кортежа.
        /// </summary>
        public void BuildSchemaPattern()
        {
            Type entityType = typeof(TEntity);
            BindingFlags memberFlags = BindingFlags.Public | BindingFlags.Instance;
            TupleObjectSchema<TEntity> schemaPattern = StaticBuilder.Schema;

            if (entityType.IsPrimitive)
                ConstructPrimitiveType();
            else
                ConstructComplicatedType();

            return;

            /*
             * Построение метода добавления нового атрибута к схеме кортежа.
             */
            MethodInfo BuildAddAttributeMethodInfo(Type targetAttributeType)
            {
                MethodInfo addAttributeToSchema = typeof(TupleObjectSchema<TEntity>)
                    .GetMethod(nameof(TupleObjectSchema<TEntity>.AddAttribute), memberFlags)
                    .MakeGenericMethod(targetAttributeType);

                return addAttributeToSchema;
            }

            /*
             * Построение примитивного типа.
             */
            void ConstructPrimitiveType()
            {
                BuildAddAttributeMethodInfo(entityType)
                    .Invoke(schemaPattern, new object[] { entityType.Name, null });

                return;
            }

            /*
             * Построение сложного типа.
             */
            void ConstructComplicatedType()
            {
                foreach (PropertyInfo propertyInfo in entityType.GetProperties(memberFlags))
                {
                    BuildAddAttributeMethodInfo(propertyInfo.PropertyType)
                        .Invoke(schemaPattern, new object[] { propertyInfo.Name, null });
                }

                return;
            }
        }

        public ITupleObjectAttributeSetupWizard<TAttribute> Attribute<TAttribute>(
            Expression<Func<TEntity, TAttribute>> memberAccess)
        {
            return TupleObjectOneToOneAttributeSetupWizard<TAttribute>.Construct(Schema, memberAccess);
        }

        public TupleObjectOneToManyAttributeSetupWizard<TEnumerable, TAttribute> Attribute<TEnumerable, TAttribute>(
            Expression<Func<TEntity, TEnumerable>> memberAccess)
            where TEnumerable : IEnumerable<TAttribute>
        {
            return TupleObjectOneToManyAttributeSetupWizard<TEnumerable, TAttribute>.Construct<TEntity, TEnumerable>(Schema, memberAccess);
        }

        public TupleObjectOneToManyAttributeSetupWizard<TDictionary, KeyValuePair<TKey, TAttribute>> Attribute<TDictionary, TKey, TAttribute>(
            Expression<Func<TEntity, TDictionary>> memberAccess)
            where TDictionary : IDictionary<TKey, TAttribute>
        {
            return TupleObjectOneToManyAttributeSetupWizard<TDictionary, TAttribute>.Construct<TEntity, TDictionary, TKey>(Schema, memberAccess);
        }

        public TupleObjectOneToManyAttributeSetupWizard<List<TAttribute>, TAttribute> Attribute<TAttribute>(
            Expression<Func<TEntity, List<TAttribute>>> memberAccess)
        {
            return Attribute<List<TAttribute>, TAttribute>(memberAccess);
        }

        public TupleObjectOneToManyAttributeSetupWizard<HashSet<TAttribute>, TAttribute> Attribute<TAttribute>(
            Expression<Func<TEntity, HashSet<TAttribute>>> memberAccess)
        {
            return Attribute<HashSet<TAttribute>, TAttribute>(memberAccess);
        }

        public TupleObjectOneToManyAttributeSetupWizard<Dictionary<TKey, TAttribute>, KeyValuePair<TKey, TAttribute>> Attribute<TAttribute, TKey>(
            Expression<Func<TEntity, Dictionary<TKey, TAttribute>>> memberAccess)
        {
            return Attribute<Dictionary<TKey, TAttribute>, TKey, TAttribute>(memberAccess);
        }
    }
}
