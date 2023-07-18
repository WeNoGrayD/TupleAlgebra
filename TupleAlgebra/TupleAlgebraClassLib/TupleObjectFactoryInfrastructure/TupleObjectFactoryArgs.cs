using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjects;
using TupleAlgebraClassLib.TupleObjectInfrastructure;

namespace TupleAlgebraClassLib.TupleObjectFactoryInfrastructure
{
    public class TupleObjectFactoryArgs
    {
        /*
        #region Instance fields

        private Delegate _onTupleBuilding;

        #endregion
        */

        #region Instance properties

        public readonly IQueryProvider QueryProvider;

        public Expression QueryExpression { get; set; }

        #endregion

        #region Constructors

        protected TupleObjectFactoryArgs(IQueryProvider queryProvider, Expression queryExpression)
        {
            QueryProvider = queryProvider;
            QueryExpression = queryExpression;

            return;
        }

        #endregion

        /*
        #region Instance methods

        protected void SetOnTupleBuildingHandler<TEntity>(Action<TupleObjectBuilder<TEntity>> onTupleBuilding)
        {
            _onTupleBuilding = onTupleBuilding;

            return;
        }

        public Action<TupleObjectBuilder<TEntity>> GetOnTupleBuildingHandler<TEntity>()
        {
            return _onTupleBuilding as Action<TupleObjectBuilder<TEntity>>;
        }

        #endregion
    */
    }

    public class TupleObjectCopyFactoryArgs<TEntity> : TupleObjectFactoryArgs
    {
        public TupleObject<TEntity> Source { get; private set; }

        public TupleObjectCopyFactoryArgs(
            TupleObject<TEntity> source,
            object actionAfterCopy = null,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(queryProvider, queryExpression)
        {
            Source = source;

            return;
        }
    }

    public class SingleTupleObjectFactoryArgs: TupleObjectFactoryArgs
    {
        public SingleTupleObjectFactoryArgs(
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(queryProvider, queryExpression)
        { }
    }

    public class TupleSystemObjectFactoryArgs : TupleObjectFactoryArgs
    {
        public TupleSystemObjectFactoryArgs(
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(queryProvider, queryExpression)
        { }
    }
}
