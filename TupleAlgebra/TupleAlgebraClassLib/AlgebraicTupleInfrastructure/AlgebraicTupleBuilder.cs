using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;

namespace TupleAlgebraClassLib.AlgebraicTupleInfrastructure
{
    /// <summary>
    /// Построитель кортежа конкретного типа сущности.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class AlgebraicTupleBuilder<TEntity>
    {
        /// <summary>
        /// Паттерн схемы кортежа типа сущности.
        /// </summary>
        private static AlgebraicTupleSchema<TEntity> _schemaPattern;

        /// <summary>
        /// Индивидуальная схема кортежа типа.
        /// </summary>
        public AlgebraicTupleSchema<TEntity> Schema { get; private set; }

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static AlgebraicTupleBuilder()
        {
            _schemaPattern = new AlgebraicTupleSchema<TEntity>();
        }

        /// <summary>
        /// Конструктор без параметров.
        /// </summary>
        public AlgebraicTupleBuilder()
        {
            Schema = _schemaPattern.Clone();
        }

        /// <summary>
        /// Построение паттерна схемы кортежа.
        /// </summary>
        public static void BuildSchemaPattern()
        {
            Type entityType = typeof(TEntity);
            BindingFlags memberFlags = BindingFlags.Public | BindingFlags.Instance;

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
                MethodInfo addAttributeToSchema = typeof(AlgebraicTupleSchema<TEntity>)
                    .GetMethod(nameof(AlgebraicTupleSchema<TEntity>.AddAttribute), memberFlags)
                    .MakeGenericMethod(targetAttributeType);

                return addAttributeToSchema;
            }

            /*
             * Построение примитивного типа.
             */
            void ConstructPrimitiveType()
            {
                BuildAddAttributeMethodInfo(entityType)
                    .Invoke(_schemaPattern, new object[] { entityType.Name, null });

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
                        .Invoke(_schemaPattern, new object[] { propertyInfo.Name, null });
                }

                return;
            }
        }

        public IAlgebraicTupleAttributeSetupWizard<TAttribute> Attribute<TAttribute>(
            Expression<Func<TEntity, TAttribute>> memberAccess)
        {
            return AlgebraicTupleOneToOneAttributeSetupWizard<TAttribute>.Construct(Schema, memberAccess);
        }

        public AlgebraicTupleOneToManyAttributeSetupWizard<TEnumerable, TAttribute> Attribute<TEnumerable, TAttribute>(
            Expression<Func<TEntity, TEnumerable>> memberAccess)
            where TEnumerable : IEnumerable<TAttribute>
        {
            return AlgebraicTupleOneToManyAttributeSetupWizard<TEnumerable, TAttribute>.Construct<TEntity, TEnumerable>(Schema, memberAccess);
        }

        public AlgebraicTupleOneToManyAttributeSetupWizard<TDictionary, KeyValuePair<TKey, TAttribute>> Attribute<TDictionary, TKey, TAttribute>(
            Expression<Func<TEntity, TDictionary>> memberAccess)
            where TDictionary : IDictionary<TKey, TAttribute>
        {
            return AlgebraicTupleOneToManyAttributeSetupWizard<TDictionary, TAttribute>.Construct<TEntity, TDictionary, TKey>(Schema, memberAccess);
        }

        public AlgebraicTupleOneToManyAttributeSetupWizard<List<TAttribute>, TAttribute> Attribute<TAttribute>(
            Expression<Func<TEntity, List<TAttribute>>> memberAccess)
        {
            return Attribute<List<TAttribute>, TAttribute>(memberAccess);
        }

        public AlgebraicTupleOneToManyAttributeSetupWizard<HashSet<TAttribute>, TAttribute> Attribute<TAttribute>(
            Expression<Func<TEntity, HashSet<TAttribute>>> memberAccess)
        {
            return Attribute<HashSet<TAttribute>, TAttribute>(memberAccess);
        }

        public AlgebraicTupleOneToManyAttributeSetupWizard<Dictionary<TKey, TAttribute>, KeyValuePair<TKey, TAttribute>> Attribute<TAttribute, TKey>(
            Expression<Func<TEntity, Dictionary<TKey, TAttribute>>> memberAccess)
        {
            return Attribute<Dictionary<TKey, TAttribute>, TKey, TAttribute>(memberAccess);
        }
    }
}
