using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    public interface ITupleObjectOperationExecutorsContainer<TEntity>
        : ISetOperationExecutorsContainer<TupleObject<TEntity>>
        where TEntity : new()
    {
        public TupleObject<TEntity> ConvertToAlternate(TupleObject<TEntity> first);
    }
}
