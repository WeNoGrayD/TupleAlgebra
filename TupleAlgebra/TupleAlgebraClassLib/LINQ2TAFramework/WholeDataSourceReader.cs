using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    public abstract class WholeDataSourceReader<TData, TQueryResult>
        : ISingleQueryExecutor<TData, TQueryResult>
    {
        public Func<TData, bool> Predicate { get; private set; }

        protected List<TData> _dataSource;

        public WholeDataSourceReader(Func<TData, bool> predicate)
        {
            Predicate = predicate;
            _dataSource = new List<TData>();
        }

        public void PutData(TData data)
        {
            _dataSource.Add(data);
        }

        public abstract TQueryResult Execute();

        public void Accept(ISingleQueryExecutorVisitor visitor)
        {
            visitor.VisitWholeDataSourceReader(this);

            return;
        }

        public void Accept(ISingleQueryExecutorVisitor<TData> visitor)
        {
            visitor.VisitWholeDataSourceReader(this);

            return;
        }
    }

    public abstract class WholeDataSourceReader2<TData, TQueryResult> : SingleQueryExecutor<TData, TQueryResult>
    {
        public Predicate<TData> DataPassingCondition { get; private set; }

        private List<TData> _dataSource;

        protected IReadOnlyList<TData> DataSource { get; private set; }

        public Action<TData> ExecuteOverDataInstance { get; private set; }

        public WholeDataSourceReader2(Predicate<TData> dataPassingCondition)
            : base()
        {
            DataPassingCondition = dataPassingCondition;
            _dataSource = new List<TData>();
            InitBehavior();
        }

        protected void InitBehavior() => InitBehavior(ExecuteOverDataInstanceHandler);

        public void InitBehavior(Action<TData> executeOnDataInstanceHandler)
        {
            ExecuteOverDataInstance = executeOnDataInstanceHandler;
        }

        public void ExecuteOverDataInstanceHandler(TData data) => _dataSource.Add(data);

        public void LoadDataSource(IEnumerable<TData> preparedDataSource) => _dataSource = new List<TData>(preparedDataSource);

        public TQueryResult GetQueryResult()
        {
            DataSource = new ReadOnlyCollection<TData>(_dataSource);
            TQueryResult queryResult = TraverseOverDataSource();

            OnDataDidPass(queryResult);

            return queryResult;
        }

        protected abstract TQueryResult TraverseOverDataSource();

        public override void Accept(ISingleQueryExecutorVisitor2 queryPipeline)
            => queryPipeline.VisitWholeDataSourceReader(this);
    }

    public abstract class WholeDataSourceReader2WithAggregableResult<TData, TQueryResult>
        : WholeDataSourceReader2<TData, TQueryResult>, IAccumulatePositiveAggregableQueryResult<TQueryResult>
    {
        public WholeDataSourceReader2WithAggregableResult(Predicate<TData> dataPassingCondition) 
            : base(dataPassingCondition)
        { }

        public TQueryResult InitAccumulator(TQueryResult initialAccumulatorValue) => initialAccumulatorValue;

        /// <summary>
        /// "Аккумулирование" результата запроса у WholeDataSourceReader происходит в конце выполнения запроса,
        /// поэтому резонно просто перекопировать результаты в аккумулятор.
        /// </summary>
        /// <param name="accumulator"></param>
        /// <param name="outputData"></param>
        public abstract void AccumulateIfDataDidPass(ref TQueryResult accumulator, TQueryResult outputData);
    }

    public abstract class WholeDataSourceReader2WithEnumerableResult<TData, TQueryResultData>
        : WholeDataSourceReader2<TData, IEnumerable<TQueryResultData>>, IAccumulatePositiveEnumerableQueryResult<TQueryResultData>
    {
        public WholeDataSourceReader2WithEnumerableResult(Predicate<TData> dataPassingCondition)
            : base(dataPassingCondition)
        { }

        /// <summary>
        /// "Аккумулирование" результата запроса у WholeDataSourceReader происходит в конце выполнения запроса,
        /// поэтому резонно просто перекопировать результаты в аккумулятор.
        /// </summary>
        /// <param name="accumulator"></param>
        /// <param name="outputData"></param>
        public void AccumulateIfDataDidPass(ref ICollection<TQueryResultData> accumulator, IEnumerable<TQueryResultData> outputData)
        {
            accumulator = (this as IAccumulatePositiveEnumerableQueryResult<TQueryResultData>).InitAccumulator(outputData);
        }
    }
}
