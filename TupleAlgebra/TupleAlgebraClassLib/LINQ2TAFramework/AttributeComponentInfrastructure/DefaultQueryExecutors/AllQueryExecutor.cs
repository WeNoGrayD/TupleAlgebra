using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.DefaultQueryExecutors
{
    public class AllQueryExecutor<TData> : EveryDataInstanceReader2WithAggregableResult<TData, bool>
    {
        private bool _fault = false;

        public AllQueryExecutor(Predicate<TData> dataPassingCondition) 
            : base(dataPassingCondition, null)
        {
            Transform = (TData data, bool didDataPass) =>
            {
                _fault |= !didDataPass;

                return _fault;
            };
            InitBehavior(ExecuteOverDataInstanceHandlerWithFullCovering);
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            bool didDataPass = DataPassingCondition(data);

            return (didDataPass, didDataPass);
        }

        public override void AccumulateIfDataDidPass(ref bool accumulator, bool outputData)
        {
            accumulator = !_fault;
        }

        public override void AccumulateIfDataDidNotPass(ref bool accumulator, bool outputData)
        {
            accumulator = !_fault;
            //_fault = true;
            //accumulator = false;
        }
    }

    public class AnyQueryExecutor<TData> : EveryDataInstanceReader2WithAggregableResult<TData, bool>
    {
        public AnyQueryExecutor(Predicate<TData> dataPassingCondition) 
            : base(dataPassingCondition, (TData data, bool didDataPass) => didDataPass)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            bool didDataPass = DataPassingCondition(data);

            return (didDataPass, !didDataPass);
        }

        public override void AccumulateIfDataDidPass(ref bool accumulator, bool outputData)
        {
            accumulator = true;
        }
    }

    public class TakeWhileQueryExecutor<TData> : EveryDataInstanceReader2WithEnumerableResult<TData, TData>
    {
        public TakeWhileQueryExecutor(Predicate<TData> dataPassingCondition)
            : base(dataPassingCondition, (TData data, bool didDataPass) => data)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            bool didDataPass = DataPassingCondition(data);

            return (didDataPass, didDataPass);
        }
    }

    public class SelectQueryExecutor<TData, TQueryResultData> : WholeDataSourceReader2WithEnumerableResult<TData, TQueryResultData>
    {
        private Func<TData, TQueryResultData> _transform;

        public SelectQueryExecutor(Func<TData, TQueryResultData> transform)
            : base((_) => true)
        {
            _transform = transform;
        }

        protected override IEnumerable<TQueryResultData> TraverseOverDataSource()
        {
            foreach (TData data in DataSource)
                yield return _transform(data);
        }
    }
}
