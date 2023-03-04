using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    public abstract class EveryDataInstanceReader<TData, TQueryResult>
        : ISingleQueryExecutor<TData, TQueryResult>
    {
        public Func<TData, bool> Predicate { get; private set; }

        public void PutData(TData data)
        {

        }

        public abstract TQueryResult Execute();

        public abstract bool IsAlreadyOver();

        public void Accept(ISingleQueryExecutorVisitor visitor)
        {
            visitor.VisitEveryDataInstanceReader(this);

            return;
        }

        public void Accept(ISingleQueryExecutorVisitor<TData> visitor)
        {
            visitor.VisitEveryDataInstanceReader(this);

            return;
        }
    }

    public abstract class EveryDataInstanceReader2<TData, TQueryResult>
        : SingleQueryExecutor<TData, TQueryResult>
    {
        public Func<TData, (bool DidDataPass, bool MustGoOn)> ExecuteOverDataInstanceHandler { get; private set; }

        public event Action<TQueryResult> DataDidNotPass;

        public Predicate<TData> DataPassingCondition { get; private set; }

        public Predicate<TData> ExecuteOverDataInstance { get; private set; }

        public EveryDataInstanceReader2(Predicate<TData> dataPassingCondition) : base()
        {
            DataPassingCondition = dataPassingCondition;
        }

        protected void InitBehavior(Func<TData, (bool DidDataPass, bool MustGoOn)> executeOverDataInstanceHandler)
        {
            ExecuteOverDataInstanceHandler = executeOverDataInstanceHandler;
            InitBehavior((TData data) => executeOverDataInstanceHandler(data).MustGoOn);
        }

        public void InitBehavior(Predicate<TData> executeOverDataInstanceHandler)
        {
            ExecuteOverDataInstance = executeOverDataInstanceHandler;
        }

        public abstract (bool DidDataPass, bool MustGoOn) ExecuteOverDataInstanceHandlerWithPositiveCovering(TData data);

        public abstract (bool DidDataPass, bool MustGoOn) ExecuteOverDataInstanceHandlerWithNegativeCovering(TData data);

        public abstract (bool DidDataPass, bool MustGoOn) ExecuteOverDataInstanceHandlerWithFullCovering(TData data);

        protected abstract (bool DidDataPass, bool MustGoOn) ConsumeData(TData data);

        protected void OnDataDidNotPass(TQueryResult outputData) => DataDidNotPass?.Invoke(outputData);

        protected void OnDataDidNotPass(Func<TData, bool, TQueryResult> outputDataSelector, TData data) => 
            DataDidNotPass?.Invoke(outputDataSelector(data, false));

        public override void Accept(ISingleQueryExecutorVisitor2 queryPipeline)
            => queryPipeline.VisitEveryDataInstanceReader(this);
    }

    public abstract class EveryDataInstanceReader2WithAggregableResult<TData, TQueryResult>
        : EveryDataInstanceReader2<TData, TQueryResult>,
          IAccumulateAnyAggregableQueryResult<TQueryResult>
    {
        protected Func<TData, bool, TQueryResult> Transform;

        public EveryDataInstanceReader2WithAggregableResult(
            Predicate<TData> dataPassingCondition,
            Func<TData, bool, TQueryResult> transform) 
            : base(dataPassingCondition)
        {
            Transform = transform;
        }

        public override (bool DidDataPass, bool MustGoOn) ExecuteOverDataInstanceHandlerWithPositiveCovering(TData data)
        {
            (bool DidDataPass, bool MustGoOn) executionInfo = ConsumeData(data);
            if (executionInfo.DidDataPass) OnDataDidPass(Transform(data, true));

            return executionInfo;
        }

        public override (bool DidDataPass, bool MustGoOn) ExecuteOverDataInstanceHandlerWithNegativeCovering(TData data)
        {
            (bool DidDataPass, bool MustGoOn) executionInfo = ConsumeData(data);
            if (!executionInfo.DidDataPass) OnDataDidNotPass(Transform, data);

            return executionInfo;
        }

        public override (bool DidDataPass, bool MustGoOn) ExecuteOverDataInstanceHandlerWithFullCovering(TData data)
        {
            (bool DidDataPass, bool MustGoOn) executionInfo = ConsumeData(data);
            if (executionInfo.DidDataPass) OnDataDidPass(Transform, data);
            else OnDataDidNotPass(Transform, data);

            return executionInfo;
        }

        public virtual TQueryResult InitAccumulator(TQueryResult initialAccumulatorValue)
        {
            return initialAccumulatorValue;
        }

        public abstract void AccumulateIfDataDidPass(ref TQueryResult accumulator, TQueryResult outputData);

        public virtual void AccumulateIfDataDidNotPass(ref TQueryResult accumulator, TQueryResult outputData) =>
            throw new NotImplementedException();
    }

    public abstract class EveryDataInstanceReader2WithEnumerableResult<TData, TQueryResultData>
        : EveryDataInstanceReader2<TData, IEnumerable<TQueryResultData>>,
          IAccumulateAnyEnumerableQueryResult<TQueryResultData>
    {
        private Func<TData, bool, TQueryResultData> Transform;

        private TQueryResultData[] _intermediateQueryResult = new TQueryResultData[1];

        public IEnumerable<TQueryResultData> IntermediateQueryResult
        {
            get => _intermediateQueryResult;
        }

        public EveryDataInstanceReader2WithEnumerableResult(
            Predicate<TData> dataPassingCondition,
            Func<TData, bool, TQueryResultData> transform) 
            : base(dataPassingCondition)
        {
            Transform = transform;
        }

        public override (bool DidDataPass, bool MustGoOn) ExecuteOverDataInstanceHandlerWithPositiveCovering(TData data)
        {
            (bool DidDataPass, bool MustGoOn) executionInfo = ConsumeData(data);
            ModifyIntermediateQueryResult(data, executionInfo.DidDataPass);
            if (executionInfo.DidDataPass) OnDataDidPass(IntermediateQueryResult);

            return executionInfo;
        }

        public override (bool DidDataPass, bool MustGoOn) ExecuteOverDataInstanceHandlerWithNegativeCovering(TData data)
        {
            (bool DidDataPass, bool MustGoOn) executionInfo = ConsumeData(data);
            ModifyIntermediateQueryResult(data, executionInfo.DidDataPass);
            if (!executionInfo.DidDataPass) OnDataDidNotPass(IntermediateQueryResult);

            return executionInfo;
        }

        public override (bool DidDataPass, bool MustGoOn) ExecuteOverDataInstanceHandlerWithFullCovering(TData data)
        {
            (bool DidDataPass, bool MustGoOn) executionInfo = ConsumeData(data);
            ModifyIntermediateQueryResult(data, executionInfo.DidDataPass);
            if (executionInfo.DidDataPass) OnDataDidPass(IntermediateQueryResult);
            else OnDataDidNotPass(IntermediateQueryResult);

            return executionInfo;
        }

        protected void ModifyIntermediateQueryResult(TData data, bool didDataPass)
        {
            _intermediateQueryResult[0] = Transform(data, didDataPass);
        }

        public void AccumulateIfDataDidPass(
            ref ICollection<TQueryResultData> accumulator, 
            IEnumerable<TQueryResultData> outputData)
            => accumulator.Add(outputData.Last());

        public void AccumulateIfDataDidNotPass(
            ref ICollection<TQueryResultData> accumulator,
            IEnumerable<TQueryResultData> outputData)
            => accumulator.Add(outputData.Last());
    }

    /*
    public class WhereQueryExecutorUUU<TData>
        : EveryDataInstanceReader2WithEnumerableResult<TData, TData>
    {
        protected readonly TData[] _intermediateQueryResult = new TData[1];

        protected List<TData> _queryResult = new List<TData>();

        public override IEnumerable<TData> IntermediateQueryResult => _intermediateQueryResult;

        //public bool IsOnStop { get; protected set; } = false;

        protected override (bool, bool) ConsumeData(TData data)
        {
            return (true, Predicate(data));
        }

        protected override void ModifyIntermediateQueryResult(TData data)
        {
            _intermediateQueryResult[0] = data;
        }

        protected override void ModifyQueryResult(TData data)
        {
            _queryResult.Add(data);
        }
    }
    */
}