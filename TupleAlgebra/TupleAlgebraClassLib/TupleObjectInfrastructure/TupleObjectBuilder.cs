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
    public partial class TupleObjectBuilder<TEntity>
    {
        #region Instance properties

        /// <summary>
        /// Индивидуальная схема кортежа типа.
        /// </summary>
        public TupleObjectSchema<TEntity> Schema { get; private set; }

        #endregion

        #region Static fields

        /// <summary>
        /// Построитель кортежа данного типа сущности по умолчанию.
        /// Содержит схему кортежа сущности по умолчанию.
        /// </summary>
        public static TupleObjectBuilder<TEntity> StaticBuilder;

        #endregion

        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static TupleObjectBuilder()
        {
            /*
             * Создания построителя кортежа сущности и схемы кортежа сущности по умолчанию.
             */
            StaticBuilder = new TupleObjectBuilder<TEntity>(new TupleObjectSchema<TEntity>());
            StaticBuilder.BuildSchemaPattern();

            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        public TupleObjectBuilder()
        {
            // Копируется схема кортежа сущности по умолчанию.
            Schema = StaticBuilder.Schema.Clone();

            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        public TupleObjectBuilder(TupleObjectSchema<TEntity> schema)
        {
            Schema = schema;

            return;
        }

        #endregion

        #region Instance methods

        /// <summary>
        /// Построение паттерна схемы кортежа.
        /// </summary>
        private void BuildSchemaPattern()
        {
            Type entityType = typeof(TEntity);
            BindingFlags memberFlags = BindingFlags.Public | BindingFlags.Instance;
            TupleObjectSchema<TEntity> schemaPattern = Schema;
            MethodInfo addAttributeToSchema = typeof(TupleObjectSchema<TEntity>)
                    .GetMethod(nameof(TupleObjectSchema<TEntity>.AddAttribute), memberFlags);

            if (TupleObjectSchema<TEntity>.IsEntityPrimitive)
                ConstructPrimitiveType();
            else
                ConstructComplicatedType();

            return;

            /*
             * Построение метода добавления нового атрибута к схеме кортежа.
             */
            MethodInfo BuildAddAttributeMethodInfo(Type targetAttributeType)
            {
                return addAttributeToSchema.MakeGenericMethod(targetAttributeType);
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

        /// <summary>
        /// Создания мастера по настройке атрибута сущности для кортежа.
        /// Тип атрибута наиболее обобщён, перегрузка используется для обычных атрибутов с отношением "один к одному".
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="memberAccess"></param>
        /// <returns></returns>
        public ITupleObjectAttributeSetupWizard<TAttribute> Attribute<TAttribute>(
            Expression<Func<TEntity, TAttribute>> memberAccess)
        {
            return TupleObjectOneToOneAttributeSetupWizard<TAttribute>.Construct(Schema, memberAccess);
        }

        #endregion
    }
}
