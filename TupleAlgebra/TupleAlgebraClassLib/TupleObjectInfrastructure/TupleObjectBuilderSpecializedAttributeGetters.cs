using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    /// <summary>
    /// Построитель кортежа конкретного типа сущности.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public partial class TupleObjectBuilder<TEntity>
    {
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
