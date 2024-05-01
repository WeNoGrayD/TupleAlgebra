using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    using static TupleObjectHelper;

    public class TupleObjectOneToOneAttributeSetupWizard<TAttribute>
        : TupleObjectAttributeSetupWizard<TAttribute>
    {
        private TupleObjectOneToOneAttributeSetupWizard(
            ITupleObjectSchemaProvider schema,
            LambdaExpression memberAccess)
            : base(schema, memberAccess)
        { }

        private TupleObjectOneToOneAttributeSetupWizard(
            ITupleObjectSchemaProvider schema,
            string attributeName)
            : base(schema, attributeName)
        { }

        private TupleObjectOneToOneAttributeSetupWizard(
            ITupleObjectSchemaProvider schema,
            AttributeName attributeName)
            : base(schema, attributeName)
        { }

        public static TupleObjectOneToOneAttributeSetupWizard<TAttribute> Construct<TEntity>(
            ITupleObjectSchemaProvider schema,
            Expression<AttributeGetterHandler<TEntity, TAttribute>> memberAccess)
        {
            return new TupleObjectOneToOneAttributeSetupWizard<TAttribute>(schema, memberAccess);
        }

        public static TupleObjectOneToOneAttributeSetupWizard<TAttribute> Construct(
            ITupleObjectSchemaProvider schema,
            AttributeName attributeName)
        {
            return new TupleObjectOneToOneAttributeSetupWizard<TAttribute>(schema, attributeName);
        }
    }
}
