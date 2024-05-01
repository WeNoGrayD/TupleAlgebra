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
    public class TupleObjectFactoryArgs<TEntity>
        where TEntity : new()
    {
        /*
        #region Instance fields

        private Delegate _onTupleBuilding;

        #endregion
        */

        #region Instance properties

        public TupleObjectSchema<TEntity> Schema { get; set; }

        public IQueryProvider QueryProvider { get; set; }

        public Expression QueryExpression { get; set; }

        #endregion

        #region Constructors

        protected TupleObjectFactoryArgs(
            IQueryProvider queryProvider, 
            Expression queryExpression)
        {
            QueryProvider = queryProvider;
            QueryExpression = queryExpression;

            return;
        }

        #endregion

        /*
        #region Instance methods

        protected void SetOnTupleBuildingHandler<TEntity>(TupleObjectBuildingHandler<TEntity> onTupleBuilding)
        {
            _onTupleBuilding = onTupleBuilding;

            return;
        }

        public TupleObjectBuildingHandler<TEntity> GetOnTupleBuildingHandler<TEntity>()
        {
            return _onTupleBuilding as TupleObjectBuildingHandler<TEntity>;
        }

        #endregion
    */
    }

    /*
    public class TupleObjectCopyFactoryArgs<TEntity> : TupleObjectFactoryArgs<TEntity>
        where TEntity : new()
    {
        public TupleObject<TEntity> Source { get; private set; }

        public TupleObjectCopyFactoryArgs(
            TupleObject<TEntity> source,
            object actionAfterCopy = null,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(source.Schema, queryProvider, queryExpression)
        {
            Source = source;

            return;
        }
    }
    */

    /*
    public class SingleTupleObjectFactoryArgs<TEntity> : TupleObjectFactoryArgs<TEntity>
        where TEntity : new()
    {
        public SingleTupleObjectFactoryArgs(
            IQueryProvider queryProvider = null,
            Expression queryExpression = null,
            params (LambdaExpression AttributeGetterExpr, IAttributeComponent AttributeComponent)[] attributes)
            : base(queryProvider, queryExpression)
        { }
    }

    public class TupleSystemObjectFactoryArgs<TEntity> : TupleObjectFactoryArgs<TEntity>
        where TEntity : new()
    {
        public TupleSystemObjectFactoryArgs(
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(queryProvider, queryExpression)
        { }
    }
    */
}
