using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    public abstract class BufferingQueryExecutor<TData, TQueryResult> : SingleQueryExecutor<TData, TQueryResult>
    {
        public Func<TData, bool> DataPassingCondition { get; private set; }

        private List<TData> _dataSource;

        protected IEnumerable<TData> DataSource { get; private set; }

        public Action<TData> ExecuteOverDataInstance { get; private set; }

        public BufferingQueryExecutor(Func<TData, bool> dataPassingCondition)
            : base()
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

        public override TPipelineQueryResult Accept<TPipelineQueryResultParam, TPipelineQueryResult>(
            bool isResultEnumerable, ISingleQueryExecutorVisitor queryPipeline) =>
            queryPipeline.VisitBufferingQueryExecutor<TData, TQueryResult, TPipelineQueryResultParam, TPipelineQueryResult>(
                isResultEnumerable, this);
    }

    public abstract class BufferingQueryExecutorWithAggregableResult<TData, TQueryResult>
        : BufferingQueryExecutor<TData, TQueryResult>, 
          IAccumulatePositiveAggregableQueryResult<TQueryResult>
    {
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
