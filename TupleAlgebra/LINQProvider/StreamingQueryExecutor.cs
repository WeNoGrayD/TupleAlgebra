using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider
{
    public abstract class StreamingQueryExecutor<TData, TQueryResult>
        : SingleQueryExecutor<TData, TQueryResult>
    {
        public Func<TData, bool> ExecuteOverDataInstance { get; private set; }
        
        public Func<TData, bool> DataPassingCondition { get; private set; }

        public event Action<TQueryResult> DataNotPassed;

        public StreamingQueryExecutor(Func<TData, bool> dataPassingCondition)
        {
            DataPassingCondition = dataPassingCondition;
        }

        protected void InitBehavior(Func<TData, bool> executeOverDataInstanceHandler)
        {
            ExecuteOverDataInstance = executeOverDataInstanceHandler;
        }

        public virtual void PrepareToQueryStart() { }

        public abstract bool ExecuteOverDataInstanceHandlerWithPositiveCovering(TData data);

        public abstract bool ExecuteOverDataInstanceHandlerWithNegativeCovering(TData data);

        public abstract bool ExecuteOverDataInstanceHandlerWithFullCovering(TData data);

        protected abstract (bool DidDataPass, bool MustGoOn) ConsumeData(TData data);

        protected void OnDataNotPassed(TQueryResult outputData) => DataNotPassed?.Invoke(outputData);

        protected void OnDataNotPassed(Func<TData, TQueryResult> outputDataSelector, TData data) => 
            DataNotPassed?.Invoke(outputDataSelector(data));

        public override TPipelineQueryResult Accept<TPipelineQueryResultParam, TPipelineQueryResult>(
            bool isResultEnumerable, ISingleQueryExecutorVisitor queryPipeline) =>
            queryPipeline.VisitStreamingQueryExecutor<TData, TQueryResult, TPipelineQueryResultParam, TPipelineQueryResult>(
                isResultEnumerable, this);
    }

    public abstract class StreamingQueryExecutorWithAggregableResult<TData, TQueryResult>
        : StreamingQueryExecutor<TData, TQueryResult>,
          IAccumulateAnyAggregableQueryResult<TQueryResult>
    {
        public StreamingQueryExecutorWithAggregableResult(Func<TData, bool> dataPassingCondition) 
            : base(dataPassingCondition)
        {
            return;
        }

        public override bool ExecuteOverDataInstanceHandlerWithPositiveCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) = ConsumeData(data);
            if (didDataPass) OnDataPassed(ModifyIntermediateQueryResult(data));

            return mustGoOn;
        }

        public override bool ExecuteOverDataInstanceHandlerWithNegativeCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) = ConsumeData(data);
            if (!didDataPass) OnDataNotPassed(ModifyIntermediateQueryResult, data);

            return mustGoOn;
        }

        public override bool ExecuteOverDataInstanceHandlerWithFullCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) = ConsumeData(data);
            if (didDataPass) OnDataPassed(ModifyIntermediateQueryResult, data);
            else OnDataNotPassed(ModifyIntermediateQueryResult, data);

            return mustGoOn;
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

        public IEnumerable<TQueryResultData> IntermediateQueryResult
        {
            get => _intermediateQueryResult;
        }

        public StreamingQueryExecutorWithEnumerableOneToOneResult(
            Func<TData, bool> dataPassingCondition,
            Func<TData, TQueryResultData> transform) 
            : base(dataPassingCondition)
        {
            _transform = transform;
        }

        public override bool ExecuteOverDataInstanceHandlerWithPositiveCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) = ConsumeData(data);
            if (didDataPass)
            {
                ModifyIntermediateQueryResult(data);
                OnDataPassed(IntermediateQueryResult);
            }

            return mustGoOn;
        }

        public override bool ExecuteOverDataInstanceHandlerWithNegativeCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) = ConsumeData(data);
            if (!didDataPass)
            {
                ModifyIntermediateQueryResult(data);
                OnDataNotPassed(IntermediateQueryResult);
            }

            return mustGoOn;
        }

        public override bool ExecuteOverDataInstanceHandlerWithFullCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) = ConsumeData(data);
            ModifyIntermediateQueryResult(data);
            if (didDataPass) OnDataPassed(IntermediateQueryResult);
            else OnDataNotPassed(IntermediateQueryResult);

            return mustGoOn;
        }

        protected void ModifyIntermediateQueryResult(TData data)
        {
            _intermediateQueryResult[0] = _transform(data);
        }

        void IAccumulatePositiveQueryResult<IEnumerable<TQueryResultData>, IEnumerable<TQueryResultData>>.AccumulateIfDataPassed(
            ref IEnumerable<TQueryResultData> accumulator,
            IEnumerable<TQueryResultData> outputData)
            => accumulator = (this as IAccumulatePositiveEnumerableQueryResult<TQueryResultData>).InitAccumulator(outputData);//accumulator.Add(outputData.Last());

        void IAccumulateNegativeQueryResult<IEnumerable<TQueryResultData>, IEnumerable<TQueryResultData>>.AccumulateIfDataNotPassed(
            ref IEnumerable<TQueryResultData> accumulator,
            IEnumerable<TQueryResultData> outputData)
            => accumulator = (this as IAccumulatePositiveEnumerableQueryResult<TQueryResultData>).InitAccumulator(outputData);//accumulator.Add(outputData.Last());
    }

    public abstract class StreamingQueryExecutorWithEnumerableOneToManyResult<TData, TQueryResultData>
        : StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>>,
          IAccumulateAnyEnumerableQueryResult<TQueryResultData>
    {
        public StreamingQueryExecutorWithEnumerableOneToManyResult(
            Func<TData, bool> dataPassingCondition)
            : base(dataPassingCondition)
        { }

        public override bool ExecuteOverDataInstanceHandlerWithPositiveCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) = ConsumeData(data);
            if (didDataPass) OnDataPassed(Match(data));

            return mustGoOn;
        }

        public override bool ExecuteOverDataInstanceHandlerWithNegativeCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) = ConsumeData(data);
            if (!didDataPass) OnDataNotPassed(Match(data));

            return mustGoOn;
        }

        public override bool ExecuteOverDataInstanceHandlerWithFullCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) = ConsumeData(data);
            if (didDataPass) OnDataPassed(Match(data));
            else OnDataNotPassed(Match(data));

            return mustGoOn;
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