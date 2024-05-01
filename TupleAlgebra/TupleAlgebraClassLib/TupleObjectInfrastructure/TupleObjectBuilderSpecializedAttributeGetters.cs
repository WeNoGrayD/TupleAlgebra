using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    using static TupleObjectHelper;

    /// <summary>
    /// Построитель кортежа конкретного типа сущности.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial class TupleObjectBuilder<TEntity>
    {
        public TupleObjectOneToManyAttributeSetupWizard<TEnumerable, TAttribute> Attribute<TEnumerable, TAttribute>(
            Expression<AttributeGetterHandler<TEntity, TEnumerable>> memberAccess)
            where TEnumerable : IEnumerable<TAttribute>
        {
            return null;
            //return TupleObjectOneToManyAttributeSetupWizard<TEnumerable, TAttribute>.Construct<TEntity, TEnumerable>(Schema, memberAccess);
        }

        public TupleObjectOneToManyAttributeSetupWizard<TDictionary, KeyValuePair<TKey, TAttribute>> Attribute<TDictionary, TKey, TAttribute>(
            Expression<AttributeGetterHandler<TEntity, TDictionary>> memberAccess)
            where TDictionary : IDictionary<TKey, TAttribute>
        {
            return TupleObjectOneToManyAttributeSetupWizard<TDictionary, TAttribute>.Construct<TEntity, TDictionary, TKey>(Schema, memberAccess);
        }

        public TupleObjectOneToManyAttributeSetupWizard<List<TAttribute>, TAttribute> Attribute<TAttribute>(
            Expression<AttributeGetterHandler<TEntity, List<TAttribute>>> memberAccess)
        {
            return Attribute<List<TAttribute>, TAttribute>(memberAccess);
        }

        public TupleObjectOneToManyAttributeSetupWizard<HashSet<TAttribute>, TAttribute> Attribute<TAttribute>(
            Expression<AttributeGetterHandler<TEntity, HashSet<TAttribute>>> memberAccess)
        {
            return Attribute<HashSet<TAttribute>, TAttribute>(memberAccess);
        }

        public TupleObjectOneToManyAttributeSetupWizard<Dictionary<TKey, TAttribute>, KeyValuePair<TKey, TAttribute>> Attribute<TAttribute, TKey>(
            Expression<AttributeGetterHandler<TEntity, Dictionary<TKey, TAttribute>>> memberAccess)
        {
            return Attribute<Dictionary<TKey, TAttribute>, TKey, TAttribute>(memberAccess);
        }
    }
}
