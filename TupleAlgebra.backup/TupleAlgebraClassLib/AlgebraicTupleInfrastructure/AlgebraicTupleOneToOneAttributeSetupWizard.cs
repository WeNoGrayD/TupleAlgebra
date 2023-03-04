using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace TupleAlgebraClassLib.AlgebraicTupleInfrastructure
{
    public class AlgebraicTupleOneToOneAttributeSetupWizard<TAttribute>
        : AlgebraicTupleAttributeSetupWizard<TAttribute>
    {
        private AlgebraicTupleOneToOneAttributeSetupWizard(
            IAlgebraicTupleSchemaProvider schema,
            LambdaExpression memberAccess)
            : base(schema, memberAccess)
        { }

        private AlgebraicTupleOneToOneAttributeSetupWizard(
            IAlgebraicTupleSchemaProvider schema,
            string attributeName)
            : base(schema, attributeName)
        { }

        public static AlgebraicTupleOneToOneAttributeSetupWizard<TAttribute> Construct<TEntity>(
            IAlgebraicTupleSchemaProvider schema,
            Expression<Func<TEntity, TAttribute>> memberAccess)
        {
            return new AlgebraicTupleOneToOneAttributeSetupWizard<TAttribute>(schema, memberAccess);
        }

        public static AlgebraicTupleOneToOneAttributeSetupWizard<TAttribute> Construct(
            IAlgebraicTupleSchemaProvider schema,
            string attributeName)
        {
            return new AlgebraicTupleOneToOneAttributeSetupWizard<TAttribute>(schema, attributeName);
        }
    }
}
