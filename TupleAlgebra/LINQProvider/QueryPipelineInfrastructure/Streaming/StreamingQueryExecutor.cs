using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure;
using LINQProvider.QueryResultAccumulatorInterfaces;

namespace LINQProvider.QueryPipelineInfrastructure.Streaming
{
    public abstract class StreamingQueryExecutor<TData, TQueryResult>
        : SingleQueryExecutor<TData, TQueryResult>
    {
        public Func<TData, (bool ResultProvided, bool MustGoOn)> ExecuteOverDataInstance { get; private set; }

        public Func<TData, bool> DataPassingCondition { get; private set; }

        public event Action<TQueryResult> DataNotPassed;

        public StreamingQueryExecutor(Func<TData, bool> dataPassingCondition)
        {
            DataPassingCondition = dataPassingCondition;
        }

        protected void InitBehavior(Func<TData, (bool ResultProvided, bool MustGoOn)> executeOverDataInstanceHandler)
        {
            ExecuteOverDataInstance = executeOverDataInstanceHandler;
        }

        public virtual void PrepareToQueryStart() { }

        public abstract (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithPositiveCovering(TData data);

        public abstract (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithFullCovering(TData data);

        protected abstract (bool DidDataPass, bool MustGoOn) ConsumeData(TData data);

        protected void OnDataNotPassed(TQueryResult outputData) => DataNotPassed?.Invoke(outputData);

        protected void OnDataNotPassed(Func<TData, TQueryResult> outputDataSelector, TData data) =>
            DataNotPassed?.Invoke(outputDataSelector(data));

        public override TPipelineQueryResult AcceptToExecuteWithAggregableResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor queryPipeline)
        {
            return queryPipeline.VisitStreamingQueryExecutorWithExpectedAggregableResult<
                TData, 
                TQueryResult, 
                TPipelineQueryResult>(this);
        }

        public override IEnumerable<TPipelineQueryResultData> AcceptToExecuteWithEnumerableResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor queryPipeline)
        {
            return queryPipeline.VisitStreamingQueryExecutorWithExpectedEnumerableResult<
                TData,
                TQueryResult,
                TPipelineQueryResultData>(this);
        }
    }

    public abstract class StreamingQueryExecutorWithAggregableResult<TData, TQueryResult>
        : StreamingQueryExecutor<TData, TQueryResult>,
          IAccumulateAnyAggregableQueryResult<TQueryResult>
    {
        #region Instance properties

        public override QuerySourceToResultMiltiplicity Multiplicity
        {
            get => QuerySourceToResultMiltiplicity.ManyToOne;
        }

        #endregion

        public StreamingQueryExecutorWithAggregableResult(Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition)
        {
            return;
        }

        public override (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithPositiveCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) flags = ConsumeData(data);
            if (flags.didDataPass) OnDataPassed(ModifyIntermediateQueryResult(data));

            return flags;
        }

        public override (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithFullCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) flags = ConsumeData(data);
            if (flags.didDataPass) OnDataPassed(ModifyIntermediateQueryResult, data);
            else OnDataNotPassed(ModifyIntermediateQueryResult, data);

            return flags;
        }

        protected abstract TQueryResult ModifyIntermediateQueryResult(TData data);

        public virtual TQueryResult InitAccumulator(TQueryResult initialAccumulatorValue)
        {
            return initialAccumulatorValue;
        }

        public abstract void AccumulateIfDataPassed(ref TQueryResult accumulator, TQueryResult outputData);

        public virtual void AccumulateIfDataNotPassed(ref TQueryResult accumulator, TQueryResult outputData) =>
            throw new NotImplementedException();
    }

    public abstract class StreamingQueryExecutorWithEnumerableOneToOneResult<TData, TQueryResultData>
        : StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>>,
          IAccumulateAnyEnumerableQueryResult<TQueryResultData>
    {
        protected Func<TData, TQueryResultData> _transform;

        private TQueryResultData[] _intermediateQueryResult = new TQueryResultData[1];

        #region Instance properties

        public override QuerySourceToResultMiltiplicity Multiplicity
        {
            get => QuerySourceToResultMiltiplicity.OneToOne;
        }

        public IEnumerable<TQueryResultData> IntermediateQueryResult
        {
            get => _intermediateQueryResult;
        }

        #endregion

        public StreamingQueryExecutorWithEnumerableOneToOneResult(
            Func<TData, bool> dataPassingCondition,
            Func<TData, TQueryResultData> transform)
            : base(dataPassingCondition)
        {
            _transform = transform;
        }

        public override (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithPositiveCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) flags = ConsumeData(data);
            if (flags.didDataPass)
            {
                ModifyIntermediateQueryResult(data);
                OnDataPassed(IntermediateQueryResult);
            }

            return flags;
        }

        public override (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithFullCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) flags = ConsumeData(data);
            ModifyIntermediateQueryResult(data);
            if (flags.didDataPass) OnDataPassed(IntermediateQueryResult);
            else OnDataNotPassed(IntermediateQueryResult);

            return flags;
        }

        protected void ModifyIntermediateQueryResult(TData data)
        {
            _intermediateQueryResult[0] = _transform(data);
        }

        void IAccumulatePositiveQueryResult<IEnumerable<TQueryResultData>, IEnumerable<TQueryResultData>>.AccumulateIfDataPassed(
            ref IEnumerable<TQueryResultData> accumulator,
            IEnumerable<TQueryResultData> outputData)
            => accumulator = (this as IAccumulatePositiveEnumerableQueryResult<TQueryResultData>).InitAccumulator(outputData);

        void IAccumulateNegativeQueryResult<IEnumerable<TQueryResultData>, IEnumerable<TQueryResultData>>.AccumulateIfDataNotPassed(
            ref IEnumerable<TQueryResultData> accumulator,
            IEnumerable<TQueryResultData> outputData)
            => accumulator = (this as IAccumulatePositiveEnumerableQueryResult<TQueryResultData>).InitAccumulator(outputData);
    }

    public abstract class StreamingQueryExecutorWithEnumerableOneToManyResult<TData, TQueryResultData>
        : StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>>,
          IAccumulateAnyEnumerableQueryResult<TQueryResultData>
    {
        #region Instance properties

        public override QuerySourceToResultMiltiplicity Multiplicity
        {
            get => QuerySourceToResultMiltiplicity.OneToMany;
        }

        #endregion

        public StreamingQueryExecutorWithEnumerableOneToManyResult(
            Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition)
        { }

        public override (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithPositiveCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) flags = ConsumeData(data);
            if (flags.didDataPass) OnDataPassed(Match(data));

            return flags;
        }

        public override (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithFullCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) flags = ConsumeData(data);
            if (flags.didDataPass) OnDataPassed(Match(data));
            else OnDataNotPassed(Match(data));

            return flags;
        }

        protected abstract IEnumerable<TQueryResultData> Match(TData data);

        void IAccumulatePositiveQueryResult<IEnumerable<TQueryResultData>, IEnumerable<TQueryResultData>>.AccumulateIfDataPassed(
            ref IEnumerable<TQueryResultData> accumulator,
            IEnumerable<TQueryResultData> outputData)
            => accumulator = (this as IAccumulatePositiveEnumerableQueryResult<TQueryResultData>).InitAccumulator(outputData);//accumulator.Add(outputData.Last());

        void IAccumulateNegativeQueryResult<IEnumerable<TQueryResultData>, IEnumerable<TQueryResultData>>.AccumulateIfDataNotPassed(
            ref IEnumerable<TQueryResultData> accumulator,
            IEnumerable<TQueryResultData> outputData)
            => accumulator = (this as IAccumulatePositiveEnumerableQueryResult<TQueryResultData>).InitAccumulator(outputData);//accumulator.Add(outputData.Last());
    }
}