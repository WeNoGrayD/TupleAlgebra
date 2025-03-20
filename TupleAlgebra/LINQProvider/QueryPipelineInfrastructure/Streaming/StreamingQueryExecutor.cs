using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure;
using LINQProvider.QueryResultAccumulatorInfrastructure;

namespace LINQProvider.QueryPipelineInfrastructure.Streaming
{
    public abstract class StreamingQueryExecutor<TData, TQueryResult>
        : SingleQueryExecutor<TData, TQueryResult>,
          IStreamingQueryExecutor<TData>
    {
        #region Instance properties

        public abstract TQueryResult Accumulator { get; }

        public Action InitAccumulator { get; protected set; } = null!;

        public Func<TData, (bool ResultProvided, bool MustGoOn)> ExecuteOverDataInstance { get; private set; }

        #endregion

        #region Instance events

        /// <summary>
        /// Событие пропуска данных на возможный следующий компонент конвейера запросов. 
        /// </summary>
        public event Action<TQueryResult> DataPassed;

        public event Action<TQueryResult> DataNotPassed;

        #endregion

        public StreamingQueryExecutor()
        {
            return;
        }

        protected void InitBehavior(Func<TData, (bool ResultProvided, bool MustGoOn)> executeOverDataInstanceHandler)
        {
            ExecuteOverDataInstance = executeOverDataInstanceHandler;
        }

        public virtual void PrepareToQueryStart() { }

        public abstract (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithPositiveCovering(TData data);

        public abstract (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithFullCovering(TData data);

        protected abstract (bool DidDataPass, bool MustGoOn) ConsumeData(TData data);

        /// <summary>
        /// Вызов события пропуска данных с готовым промежуточным результатом.
        /// </summary>
        /// <param name="outputData"></param>
        protected void OnDataPassed(TQueryResult outputData)
        {
            DataPassed?.Invoke(outputData);

            return;
        }

        /// <summary>
        /// Вызов события пропуска данных с отложенным преобразованим данных в промежуточный результат.
        /// </summary>
        /// <param name="outputDataSelector"></param>
        /// <param name="data"></param>
        /*
         * Отложенное преобразование данных используется для того, чтобы оно не выполнялось в том случае,
         * если данные не прошли.
         */
        protected void OnDataPassed(Func<TData, TQueryResult> outputDataSelector, TData data)
        {
            DataPassed?.Invoke(outputDataSelector(data));

            return;
        }

        protected void OnDataNotPassed(TQueryResult outputData) => DataNotPassed?.Invoke(outputData);

        protected void OnDataNotPassed(Func<TData, TQueryResult> outputDataSelector, TData data) =>
            DataNotPassed?.Invoke(outputDataSelector(data));

        public virtual void AccumulateQueryResult()
        {
            return;
        }
    }

    public abstract class StreamingQueryExecutorWithAggregableResult<TData, TQueryResult>
        : StreamingQueryExecutor<TData, TQueryResult>
    {
        #region Instance properties

        public override QuerySourceToResultMiltiplicity Multiplicity
        {
            get => QuerySourceToResultMiltiplicity.ManyToOne;
        }

        #endregion

        #region Constructors

        public StreamingQueryExecutorWithAggregableResult()
            : base()
        {
            return;
        }

        #endregion

        #region Instance methods

        public override (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithPositiveCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) flags = ConsumeData(data);
            if (flags.didDataPass) OnDataPassed(Accumulator);

            return flags;
        }

        public override (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithFullCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) flags = ConsumeData(data);
            if (flags.didDataPass) OnDataPassed(Accumulator);
            else OnDataNotPassed(Accumulator);

            return flags;
        }

        #endregion
    }

    public abstract class StreamingQueryExecutorWithEnumerableOneToOneResult<TData, TQueryResultData>
        : StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>>
    {
        protected TQueryResultData[] _queryResult;

        #region Instance properties

        public override QuerySourceToResultMiltiplicity Multiplicity
        {
            get => QuerySourceToResultMiltiplicity.OneToOne;
        }

        public override IEnumerable<TQueryResultData> Accumulator { get => _queryResult; }

        #endregion

        #region Constructors

        public StreamingQueryExecutorWithEnumerableOneToOneResult()
            : base()
        {
            _queryResult = new TQueryResultData[1];
        }

        #endregion
    }

    public abstract class StreamingQueryExecutorWithEnumerableOneToManyResult<TData, TQueryResultData>
        : StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>>
    {
        #region Instance fields

        private IEnumerable<TQueryResultData> _queryResult;

        #endregion

        #region Instance properties

        public override QuerySourceToResultMiltiplicity Multiplicity
        {
            get => QuerySourceToResultMiltiplicity.OneToMany;
        }

        public override IEnumerable<TQueryResultData> Accumulator { get => _queryResult; }

        #endregion

        public StreamingQueryExecutorWithEnumerableOneToManyResult()
            : base()
        {
            _queryResult = Enumerable.Empty<TQueryResultData>();

            return;
        }

        public override void AccumulateQueryResult()
        {
            DataPassed += (queryResult) => _queryResult = queryResult;

            return;
        }
    }
}