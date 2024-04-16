using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    public static class TupleObjectHelper
    {
        #region Delegates

        public delegate TData AttributeGetterHandler<TEntity, TData>(TEntity entity);

        public delegate TEntity EntityFactoryHandler<TEntity>(IEnumerator[] properties);

        #endregion
    }
}
