using LINQProvider;
using LINQProvider.QueryPipelineInfrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.LINQ2TAFramework.TupleObjectInfrastructure
{
    using static TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectHelper;
    using static TupleObjectStaticDataStorage;

    public class TupleObjectQueryProvider
        : QueryProvider
    {
        #region Constructors

        public TupleObjectQueryProvider()
            : base()
        {
            return;
        }

        #endregion

        #region Istance methods

        /*
        protected virtual QueryContext CreateQueryContext()
        {
            return new QueryContext();
        }
        */

        protected override IDataSourceExtractor<IQueryable> 
            CreateDataSourceExtractor<TQueryEntity>()
        {
            return new DataSourceExtractor<ITupleObject>()
                as IDataSourceExtractor<IQueryable>;
        }

        protected override QueryContext CreateQueryContext()
        {
            return new TupleObjectQueryContext();
        }

        /// <summary>
        /// Оборачивание перечислимого нефиктивного результата запроса в компоненту атрибута.
        /// </summary>
        /// <typeparam name="TQueryResultData"></typeparam>
        /// <param name="queryableAC"></param>
        /// <param name="queryResult"></param>
        /// <returns></returns>
        /*
        protected object WrapEnumerableResultWithTupleObject<TQueryEntity>(
            TupleObject<TQueryEntity> queryableTO,
            IEnumerable<TQueryEntity> queryResult)
            where TQueryEntity : new()
        {
            return queryableTO.Factory.CreateQueried<TQueryEntity>(factoryArgs);
        }
        */

        #endregion

        #region IQueryProvider implementation

        /// <summary>
        /// Имплементированное обобщённое создание запроса.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="queryExpression"></param>
        /// <param name="queryableAC"></param>
        /// <returns></returns>
        protected override IQueryable<TQueryEntity> CreateQueryImpl<TQueryEntity>(
            Expression queryExpression,
            IQueryable dataSource)
        {
            IQueryable<TQueryEntity> res;

            CreateQueryImplRestricted(
                queryExpression,
                (dynamic)dataSource,
                out res);

            return res;
        }

        private void CreateQueryImplRestricted<
            TQueryEntity>(
            Expression queryExpression,
            ITupleObject queryableTO,
            out IQueryable<TQueryEntity> res)
            where TQueryEntity : new()
        {
            res = queryableTO.Factory.CreateQueried<TQueryEntity>(
                queryExpression,
                queryableTO.PassSchema<TQueryEntity>);

            return;
        }

        /// <summary>
        /// Обобщённое выполнение запроса.
        /// </summary>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <param name="queryExpression"></param>
        /// <returns></returns>
        public override TQueryResult Execute<TQueryResult>(Expression queryExpression)
        {
            TQueryResult queryResult = base.Execute<TQueryResult>(queryExpression);
            /*
            if (_isResultEnumerable && !_queryIsFiction)
                return (TQueryResult)WrapEnumerableResultWithTupleObject(
                    (dynamic)_queryDataSource, 
                    queryResult);
            */
            return queryResult;
        }

        #endregion
    }

    public interface IFactoryProvider<TFactory>
        : IEnumerable
    {
        public TFactory Factory { get; }
    }
}
