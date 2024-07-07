using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using TupleAlgebraClassLib.TupleObjectInfrastructure.Annotations;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    using static TupleObjectHelper;

    public interface ITupleObjectBuilder
    {
        public void SetSchema<TEntity>(TupleObjectSchema<TEntity> schema);

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
            string objTypeName = typeof(object).Name;
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
                do
                {
                    foreach (FieldInfo fieldInfo in entityType.DeclaredFields
                        .Where(fi => fi.IsPublic && !fi.IsStatic))
                    {
                        BuildAddAttributeMethodInfo(fieldInfo.FieldType)
                            .Invoke(this, new object[] { fieldInfo });
                    }

                    // удаляем статические свойства
                    foreach (PropertyInfo propertyInfo in entityType.DeclaredProperties
                        .Where(PropertyIsValid))
                    {
                        BuildAddAttributeMethodInfo(propertyInfo.PropertyType)
                            .Invoke(this, new object[] { propertyInfo });
                    }
                }
                while ((entityType = entityType.BaseType.GetTypeInfo())
                       .Name != objTypeName);

                return;

                bool PropertyIsValid(PropertyInfo pi)
                {
                    return !pi.GetAccessors(false)[0].IsStatic &&
                        ((pi.GetMethod?.IsPublic ?? false) && (pi.SetMethod?.IsPublic ?? false)) ||
                        (pi.GetCustomAttribute<CalculatedPropertyAttribute>() is not null);
                }
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
            Expression<Func<TEntity, TEntity>>
                attributeGetterExpr = Expression
                .Lambda<Func<TEntity, TEntity>>(
                    entityParameterExpr,
                    entityParameterExpr);
            Schema.AddAttribute(attrName, attributeGetterExpr);

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
                    MemberTypes.Property => MakePropertyExpr()

                };
            Expression<Func<TEntity, TAttribute>> 
                attributeGetterExpr = Expression
                .Lambda<Func<TEntity, TAttribute>>(
                    memberGetterExpr, 
                    entityParameterExpr);
            Schema.AddAttribute(attrName, attributeGetterExpr);

            return;

            MemberExpression MakePropertyExpr()
            {
                var propertyInfo = (attributeMember as PropertyInfo)!;
                CalculatedPropertyAttribute calcPropAttr =
                    propertyInfo.GetCustomAttribute<CalculatedPropertyAttribute>();

                //propertyInfo.SetMethod.

                return Expression.Property(entityParameterExpr, propertyInfo);
            }
        }

        internal void AddNavigationalAttribute<TKey, TNavigationalAttribute>(
            Expression<Func<TEntity, TKey>>
                simpleKeyAttrGetterExpr,
            Expression<Func<TEntity, TNavigationalAttribute>>
                navigationalAttrGetterExpr,
            TupleObject<TNavigationalAttribute> source,
            Expression<Func<TNavigationalAttribute, TKey>> principalKeySelector)
            where TNavigationalAttribute : new()
        {
            Schema.AddNavigationalAttribute(
                simpleKeyAttrGetterExpr,
                navigationalAttrGetterExpr,
                source,
                principalKeySelector);

            return;
        }

        internal void AddNavigationalAttribute<TKey, TNavigationalAttribute>(
            IEnumerable<AttributeName> complexKeyAttrNames,
            AttributeName navigationalAttrName,
            Expression<Func<TEntity, TNavigationalAttribute>>
                navigationalAttrGetterExpr,
            TupleObject<TNavigationalAttribute> source,
            Expression<Func<TNavigationalAttribute, TKey>> principalKeySelector)
            where TKey : new()
            where TNavigationalAttribute : new()
        {
            Schema.AddNavigationalAttribute(
                complexKeyAttrNames,
                navigationalAttrName,
                navigationalAttrGetterExpr,
                source,
                principalKeySelector);

            return;
        }

        void ITupleObjectBuilder.SetSchema<TBEntity>(
            TupleObjectSchema<TBEntity> schema)
        {
            if (schema is TupleObjectSchema<TEntity> cSchema)
            {
                Schema = cSchema;

                return;
            }

            throw new InvalidOperationException($"Не удаётся назначить схему. Типы {typeof(TEntity).Name} и {typeof(TBEntity).Name} не совпадают");
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
            return Schema[attrName].SetupWizardFactory(Schema, attrName);
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
            Expression<Func<TEntity, TAttribute>> memberAccess)
        {
            return (Schema[memberAccess] as ITupleObjectAttributeInfo<TAttribute>)!
                .SetupWizardFactory(Schema, memberAccess);
        }

        public INavigationalMemberSetupWizard<TEntity, TAttribute>
            HasOne<TAttribute>(Expression<Func<TEntity, TAttribute>> memberAccess)
            where TAttribute : new()
        {
            return new OneToOneNavigationalMemberSetupWizard<TEntity, TAttribute>(
                this,
                memberAccess);
        }

        #endregion
    }

    public interface INavigationalMemberSetupWizard<TEntity, TAttribute>
        where TAttribute : new()
    {
        public INavigationalMemberSetupWizard<TEntity, TAttribute>
            HasForeignKey<TKey>(Expression<Func<TEntity, TKey>> keySelector);

        public ITupleObjectAttributeSetupWizard<TAttribute>
            HasPrincipalKey<TKey>(
            TupleObject<TAttribute> source,
            Expression<Func<TAttribute, TKey>> principalKeySelector);
    }

    public class OneToOneNavigationalMemberSetupWizard<TEntity, TAttribute>
        : INavigationalMemberSetupWizard<TEntity, TAttribute>
        where TAttribute : new()
    {
        private TupleObjectBuilder<TEntity> _builder;

        private LambdaExpression _foreignKeySelector;

        private Expression<Func<TEntity, TAttribute>> _navigationalMemberAccess;

        AttributeName[] _keyAttributeNames;

        public OneToOneNavigationalMemberSetupWizard(
            TupleObjectBuilder<TEntity> builder,
            Expression<Func<TEntity, TAttribute>> navigationalMemberAccess)
        {
            _builder = builder;
            _navigationalMemberAccess = navigationalMemberAccess;
            _builder.Attribute(e => navigationalMemberAccess).Ignore();

            return;
        }

        public INavigationalMemberSetupWizard<TEntity, TAttribute>
            HasForeignKey<TKey>(Expression<Func<TEntity, TKey>> foreignKeySelector)
        {
            /*
            AttributeMemberExtractor memberExtractor = new();
            _keyAttributeNames = memberExtractor
                .ExtractManyFrom(foreignKeySelector)
                .Select<MemberInfo, AttributeName>(mi => mi.Name)
                .ToArray();
            */
            _foreignKeySelector = foreignKeySelector;

            return this;
        }

        public ITupleObjectAttributeSetupWizard<TAttribute>
            HasPrincipalKey<TKey>(
            TupleObject<TAttribute> source, 
            Expression<Func<TAttribute, TKey>> principalKeySelector)
        {
            switch (_keyAttributeNames.Length)
            {
                case 0:
                    {
                        throw new Exception();
                    }
                case 1:
                    {
                        _builder.AddNavigationalAttribute(
                            _foreignKeySelector as Expression<Func<TEntity, TKey>>,
                            _navigationalMemberAccess,
                            source,
                            principalKeySelector);

                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return _builder.Attribute(_navigationalMemberAccess);
        }

        private bool DefineKeyComplexity<TData, TKey>(
            Expression<Func<TData, TKey>> keySelector)
        {
            return keySelector.Body switch
            {
                MemberExpression => false,
                NewExpression => true,
                _ => throw new Exception()
            };
        }
    }
}
