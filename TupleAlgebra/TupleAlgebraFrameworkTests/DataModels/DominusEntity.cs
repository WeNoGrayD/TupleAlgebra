using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TupleAlgebraFrameworkTests.Entity2EntityRelationshipTests;

namespace TupleAlgebraFrameworkTests.DataModels
{
    public abstract class DominusEntity<TRelation, TServantEntity> : IEnumerable<TServantEntity>
    {
        public TRelation Relation { get; set; }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public abstract IEnumerator<TServantEntity> GetEnumerator();
    }

    public class DominusListEntity<TServantEntity> : DominusEntity<IList<TServantEntity>, TServantEntity>
    {
        public override IEnumerator<TServantEntity> GetEnumerator()
        {
            return Relation.GetEnumerator();
        }
    }

    public class DominusHashSetEntity<TServantEntity> : DominusEntity<HashSet<TServantEntity>, TServantEntity>
    {
        public override IEnumerator<TServantEntity> GetEnumerator()
        {
            return Relation.GetEnumerator();
        }
    }
    public class DominusDictionayEntity<TServantKey, TServantEntity> :
        DominusEntity<IDictionary<TServantKey, TServantEntity>, TServantEntity>
        where TServantKey : IComparable<TServantKey>
    {
        public override IEnumerator<TServantEntity> GetEnumerator()
        {
            return Relation.Values.GetEnumerator();
        }
    }

}
