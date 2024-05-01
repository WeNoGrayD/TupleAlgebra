using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using System.Linq.Expressions;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    public static class TupleObjectHelper
    {
        #region Constants

        private const string STRING_TYPE_NAME = nameof(String);

        #endregion

        #region Delegates

        public delegate void TupleObjectBuildingHandler<TEntity>(
            TupleObjectBuilder<TEntity> builder);

        public delegate TData AttributeGetterHandler<TEntity, TData>(TEntity entity);

        public delegate TEntity AttributeGetterHandler<TEntity>(TEntity entity);

        public delegate TEntity EntityFactoryHandler<TEntity>(IEnumerator[] properties);

        public delegate TEntity PrimitiveEntityFactoryHandler<TEntity>(IEnumerator<TEntity> property);

        public delegate ITupleObjectAttributeSetupWizard
            AttributeSetupWizardFactoryHandler(
                ITupleObjectSchemaProvider schema,
                AttributeName attrName);

        public delegate ITupleObjectAttributeSetupWizard<TAttribute>
            AttributeSetupWizardFactoryHandler<TAttribute>(
                ITupleObjectSchemaProvider schema,
                AttributeName attrName);

        #endregion

        #region Static properties

        public static AttributeMemberExtractor MemberExtractor { get; } =
            new AttributeMemberExtractor();

        #endregion

        #region Static methods

        public static bool IsEntityTypePrimitive(TypeInfo entityType)
        {
            return entityType.IsPrimitive || entityType.Name == STRING_TYPE_NAME;
        }

        public static SingleTupleObjectFactoryArgs<TEntity, TAttribute>
            SetAC<TEntity, TAttribute>(
                Expression<AttributeGetterHandler<TEntity, TAttribute>> getter,
                NonFictionalAttributeComponentFactoryArgs<TAttribute> componentFactoryArgs)
        {
            return new SingleTupleObjectFactoryArgs<TEntity, TAttribute>(
                getter, componentFactoryArgs);
        }

        #endregion
    }
}
