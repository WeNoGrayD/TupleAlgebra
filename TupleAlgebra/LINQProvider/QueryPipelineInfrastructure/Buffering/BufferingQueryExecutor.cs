using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure;
using LINQProvider.QueryResultAccumulatorInterfaces;

namespace LINQProvider.QueryPipelineInfrastructure.Buffering
{
    public abstract class BufferingQueryExecutor<TData, TQueryResult> : SingleQueryExecutor<TData, TQueryResult>
    {
        public Func<TData, bool> DataPassingCondition { get; private set; }

        private List<TData> _dataSource;

        protected IEnumerable<TData> DataSource { get; private set; }

        public Action<TData> ExecuteOverDataInstance { get; private set; }

        public BufferingQueryExecutor(Func<TData, bool> dataPassingCondition)
        {
            DataPassingCondition = dataPassingCondition;
            InitBehavior();
        }

        protected void InitBehavior() => InitBehavior(ExecuteOverDataInstanceHandler);

        public void InitBehavior(Action<TData> executeOnDataInstanceHandler)
        {
            ExecuteOverDataInstance = executeOnDataInstanceHandler;
        }

        public void ExecuteOverDataInstanceHandler(TData data) => _dataSource.Add(data);

        public void InitDataSource() => DataSource = _dataSource = new List<TData>();

        public void LoadDataSource(IEnumerable<TData> preparedDataSource) => DataSource = preparedDataSource;

        public TQueryResult GetQueryResult()
        {
            TQueryResult queryResult = TraverseOverDataSource();
            OnDataPassed(queryResult);

            return queryResult;
        }

        protected abstract TQueryResult TraverseOverDataSource();

        public override TPipelineQueryResult AcceptToExecuteWithAggregableResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor queryPipeline)
        {
            return queryPipeline.VisitBufferingQueryExecutorWithExpectedAggregableResult<
                TData,
                TQueryResult,
                TPipelineQueryResult>(this);
        }

        public override IEnumerable<TPipelineQueryResultData> AcceptToExecuteWithEnumerableResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor queryPipeline)
        {
            return queryPipeline.VisitBufferingQueryExecutorWithExpectedEnumerableResult<
                TData,
                TQueryResult,
                TPipelineQueryResultData>(this);
        }
    }

    public abstract class BufferingQueryExecutorWithAggregableResult<TData, TQueryResult>
        : BufferingQueryExecutor<TData, TQueryResult>,
          IAccumulatePositiveAggregableQueryResult<TQueryResult>
    {
        #region Instance properties

        public override QuerySourceToResultMiltiplicity Multiplicity
        {
            get => QuerySourceToResultMiltiplicity.ManyToOne;
        }

        #endregion

        public BufferingQueryExecutorWithAggregableResult(Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition)
        { }

        public TQueryResult InitAccumulator(TQueryResult initialAccumulatorValue) => initialAccumulatorValue;

        /// <summary>
        /// "Аккумулирование" результата запроса у BufferingQueryExecutor происходит в конце выполнения запроса,
        /// поэтому резонно просто перекопировать результаты в аккумулятор.
        /// </summary>
        /// <param name="accumulator"></param>
        /// <param name="outputData"></param>
        public virtual void AccumulateIfDataPassed(ref TQueryResult accumulator, TQueryResult outputData)
        {
            accumulator = outputData;
        }
    }

    public abstract class BufferingQueryExecutorWithEnumerableResult<TData, TQueryResultData>
        : BufferingQueryExecutor<TData, IEnumerable<TQueryResultData>>,
          IAccumulatePositiveEnumerableQueryResult<TQueryResultData>
    {
        #region Instance properties

        public override QuerySourceToResultMiltiplicity Multiplicity
        {
            get => QuerySourceToResultMiltiplicity.OneToOne;
        }

        #endregion

        public BufferingQueryExecutorWithEnumerableResult(Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition)
        { }

        /// <summary>
        /// "Аккумулирование" результата запроса у BufferingQueryExecutor происходит в конце выполнения запроса,
        /// поэтому резонно просто перекопировать результаты в аккумулятор.
        /// </summary>
        /// <param name="accumulator"></param>
        /// <param name="outputData"></param>
        public void AccumulateIfDataPassed(ref IEnumerable<TQueryResultData> accumulator, IEnumerable<TQueryResultData> outputData)
        {
            accumulator = (this as IAccumulatePositiveEnumerableQueryResult<TQueryResultData>).InitAccumulator(outputData);
        }
    }
}
