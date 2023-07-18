using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    public interface IAttributeComponentProvider
    {
        IAlgebraicSetObject CreateAttributeComponent<TEntity>(AttributeInfo attribute, IEnumerable<TEntity> entitySource);
    }
}
