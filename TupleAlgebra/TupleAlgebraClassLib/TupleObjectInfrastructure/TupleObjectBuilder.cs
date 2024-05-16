using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    using static TupleObjectHelper;

    public interface ITupleObjectBuilder
    {
        public ITupleObjectAttributeSetupWizard Attribute(AttributeName name);

        public ITupleObjectAttributeSetupWizard AttributeAt(int i);
    }

    /// <summary>
    /// Построитель кортежа конкретного типа сущности.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial class TupleObjectBuilder<TEntity>
        : ITupleObjectBuilder
    {
        #region Constants

        private const string ENTITY_PARAM_NAME = "entity";

        #endregion

        #region Instance properties

        /// <summary>
        /// Индивидуальная схема кортежа типа.
        /// </summary>
        public TupleObjectSchema<TEntity> Schema { get; set; }

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
             * Создание построителя кортежа сущности и схемы кортежа сущности по умолчанию.
             */
            StaticBuilder = new TupleObjectBuilder<TEntity>(
                new TupleObjectSchema<TEntity>());
            StaticBuilder.BuildSchemaPattern();

            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        public TupleObjectBuilder()
        {
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
            TypeInfo entityType = typeof(TEntity).GetTypeInfo();
            BindingFlags memberFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            MethodInfo addAttributeToSchema = typeof(TupleObjectBuilder<TEntity>)
                    .GetMethod(nameof(TupleObjectBuilder<TEntity>.AddAttribute), memberFlags);

            if (IsEntityTypePrimitive(entityType))
                ConstructPrimitiveType();
            else
                ConstructComplicatedType();

            EndSchemaInitialization();

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
                AddEntityAsAttribute(entityType);

                return;
            }

            /*
             * Построение сложного типа.
             */
            void ConstructComplicatedType()
            {
                foreach (FieldInfo fieldInfo in entityType.DeclaredFields.Where(fi => fi.IsPublic))
                {
                    BuildAddAttributeMethodInfo(fieldInfo.FieldType)
                        .Invoke(this, new object[] { fieldInfo });
                }

                foreach (PropertyInfo propertyInfo in entityType.DeclaredProperties
                    .Where(pi => (pi.SetMethod?.IsPublic ?? false) && (pi.GetMethod?.IsPublic ?? false)))
                {
                    BuildAddAttributeMethodInfo(propertyInfo.PropertyType)
                        .Invoke(this, new object[] { propertyInfo });
                }

                return;
            }
        }

        /// <summary>
        /// Случай, когда у сущности нет свойств. т.е. это примитивный тип.
        /// Сущность проецируется сама на себя и становится своим атрибутом.
        /// </summary>
        private void AddEntityAsAttribute(Type entityType)
        {
            AttributeName attrName = entityType.Name;
            ParameterExpression entityParameterExpr =
                Expression.Parameter(entityType, ENTITY_PARAM_NAME);
            Expression<AttributeGetterHandler<TEntity, TEntity>>
                attributeGetterExpr = Expression
                .Lambda<AttributeGetterHandler<TEntity, TEntity>>(
                    entityParameterExpr,
                    entityParameterExpr);
            Schema.AddAttribute(attributeGetterExpr, attrName);

            return;
        }

        private void AddAttribute<TAttribute>(MemberInfo attributeMember)
        {
            AttributeName attrName = attributeMember.Name;
            ParameterExpression entityParameterExpr =
                Expression.Parameter(typeof(TEntity), ENTITY_PARAM_NAME);
            MemberExpression memberGetterExpr =
                attributeMember.MemberType switch
                {
                    MemberTypes.Field => 
                        Expression.Field(entityParameterExpr, (attributeMember as FieldInfo)!),
                    MemberTypes.Property => 
                        Expression.Property(entityParameterExpr, (attributeMember as PropertyInfo)!)
                };
            Expression<AttributeGetterHandler<TEntity, TAttribute>> 
                attributeGetterExpr = Expression
                .Lambda<AttributeGetterHandler<TEntity, TAttribute>>(
                    memberGetterExpr, 
                    entityParameterExpr);
            Schema.AddAttribute(attributeGetterExpr, attrName);

            return;
        }

        public void InitDefaultSchema()
        { 
            Schema = StaticBuilder.Schema.Clone();
            EndSchemaInitialization();

            return;
        }

        public void EndSchemaInitialization()
        {
            Schema.EndInit();

            return;
        }

        /// <summary>
        /// Создание мастера по настройке атрибута сущности для кортежа.
        /// Тип атрибута наиболее обобщён, перегрузка используется для обычных атрибутов с отношением "один к одному".
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="memberAccess"></param>
        /// <returns></returns>
        public ITupleObjectAttributeSetupWizard Attribute(
            AttributeName attrName)
        {
            return Schema.GetSetupWizard(attrName);
        }

        public ITupleObjectAttributeSetupWizard AttributeAt(
            int attrPtr)
        {
            return Schema.GetSetupWizard(attrPtr);
        }

        /// <summary>
        /// Создание мастера по настройке атрибута сущности для кортежа.
        /// Нужно только для настройки схемы пользователем.
        /// Тип атрибута наиболее обобщён, перегрузка используется для обычных атрибутов с отношением "один к одному".
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="memberAccess"></param>
        /// <returns></returns>
        public ITupleObjectAttributeSetupWizard<TAttribute> Attribute<TAttribute>(
            Expression<AttributeGetterHandler<TEntity, TAttribute>> memberAccess)
        {
            return (Schema[memberAccess] as ITupleObjectAttributeInfo<TAttribute>)!
                .SetupWizardFactory(Schema, memberAccess);
        }

        #endregion
    }
}
