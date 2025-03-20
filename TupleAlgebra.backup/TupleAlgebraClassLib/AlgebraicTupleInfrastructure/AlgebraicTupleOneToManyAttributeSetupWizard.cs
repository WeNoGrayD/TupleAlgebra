using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace TupleAlgebraClassLib.AlgebraicTupleInfrastructure
{
    public class AlgebraicTupleOneToManyAttributeSetupWizard<TAttributeContainer, TAttribute>
        : AlgebraicTupleAttributeSetupWizard<TAttributeContainer>
    {
        private AlgebraicTupleOneToManyAttributeSetupWizard(
            IAlgebraicTupleSchemaProvider schema,
            LambdaExpression memberAccess)
            : base(schema, memberAccess)
        { }

        public static AlgebraicTupleOneToManyAttributeSetupWizard<TEnumerable, TAttribute> Construct<TEntity, TEnumerable>(
            IAlgebraicTupleSchemaProvider schema,
            Expression<Func<TEntity, TAttributeContainer>> memberAccess)
            where TEnumerable : TAttributeContainer, IEnumerable<TAttribute>
        {
            return new AlgebraicTupleOneToManyAttributeSetupWizard<TEnumerable, TAttribute>(schema, memberAccess);
        }

        public static AlgebraicTupleOneToManyAttributeSetupWizard<TDictionary, KeyValuePair<TKey, TAttribute>> Construct<TEntity, TDictionary, TKey>(
            IAlgebraicTupleSchemaProvider schema,
            Expression<Func<TEntity, TDictionary>> memberAccess)
            where TDictionary : TAttributeContainer, IDictionary<TKey, TAttribute>
        {
            return new AlgebraicTupleOneToManyAttributeSetupWizard<TDictionary, KeyValuePair<TKey, TAttribute>>(schema, memberAccess);
        }

        public AlgebraicTupleOneToOneAttributeSetupWizard<TAttribute> OneToOneRelation()
        {
            return AlgebraicTupleOneToOneAttributeSetupWizard<TAttribute>.Construct(Schema, _attributeName);
        }
    }
}
