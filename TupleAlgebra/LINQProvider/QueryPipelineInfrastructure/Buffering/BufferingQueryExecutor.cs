using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure;
using LINQProvider.QueryResultAccumulatorInfrastructure;

namespace LINQProvider.QueryPipelineInfrastructure.Buffering
{
    public abstract class BufferingQueryExecutor<TData, TQueryResult> : SingleQueryExecutor<TData, TQueryResult>
    {
        #region Instance fields

        protected IEnumerable<TData> _dataSource;

        #endregion

        #region Constructors

        public BufferingQueryExecutor()
        {
            return;
        }

        #endregion

        #region Instance methods

        public void LoadDataSource(IEnumerable<TData> preparedDataSource) => _dataSource = preparedDataSource;

        public abstract TQueryResult Execute();

        #endregion
    }

    public abstract class BufferingQueryExecutorWithAggregableResult<TData, TQueryResult>
        : BufferingQueryExecutor<TData, TQueryResult>
    {
        #region Instance properties

        public override QuerySourceToResultMiltiplicity Multiplicity
        {
            get => QuerySourceToResultMiltiplicity.ManyToOne;
        }

        #endregion

        #region Constructors

        public BufferingQueryExecutorWithAggregableResult()
            : base()
        { }

        #endregion
    }

    public abstract class BufferingQueryExecutorWithEnumerableResult<TData, TQueryResultData>
        : BufferingQueryExecutor<TData, IEnumerable<TQueryResultData>>
    {
        #region Instance properties

        public override QuerySourceToResultMiltiplicity Multiplicity
        {
            get => QuerySourceToResultMiltiplicity.OneToOne;
        }

        #endregion

        #region Constructors

        public BufferingQueryExecutorWithEnumerableResult()
            : base()
        { }

        #endregion
    }
}
