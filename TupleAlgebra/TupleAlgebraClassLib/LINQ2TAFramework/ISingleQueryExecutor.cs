using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    public interface ISingleQueryExecutor<TData, TQueryResult>
        : IQueryPipelineExecutorAcceptor, IQueryPipelineExecutorAcceptor<TData>
    {
        Func<TData, bool> Predicate { get; }

        void PutData(TData data);

        TQueryResult Execute();
    }

    public interface ISingleQueryExecutorVisitor2
    {
        IQueryPipelineMiddleware FirstQueryExecutor { get; set; }

        TPipelineQueryResult Execute<TPipelineQueryResult>();

        void SetDataSource<TData>(IEnumerable<TData> dataSource);

        void VisitWholeDataSourceReader<TData, TQueryResult>(
            WholeDataSourceReader2<TData, TQueryResult> queryExecutor);

        void VisitEveryDataInstanceReader<TData, TQueryResult>(
            EveryDataInstanceReader2<TData, TQueryResult> queryExecutor);
    }

    public interface IQueryPipelineAcceptor
    {
        void Accept(ISingleQueryExecutorVisitor2 queryPipeline);
    }

    public interface IQueryPipelineMiddleware : IQueryPipelineAcceptor
    {
        IQueryPipelineMiddleware ContinueWith(IQueryPipelineMiddleware continuingExecutor) => default(IQueryPipelineMiddleware);

        TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor2 pipelineQueryExecutor);
    }

    public interface IQueryPipelineMiddleware<TData, TQueryResult> : IQueryPipelineMiddleware
    {
        IQueryPipelineMiddleware<TData, TQueryResult> ContinueWith<TContinuingQueryData, TContinuingQueryResult>(
                IQueryPipelineMiddleware<TContinuingQueryData, TContinuingQueryResult> continuingExecutor);
    }

    public interface IAccumulatePositiveQueryResult<TQueryResult, TAccumulator> //: IQueryPipelineMiddleware
        where TAccumulator : TQueryResult
    {
        event Action<TQueryResult> DataDidPass;

        TAccumulator InitAccumulator();

        TAccumulator InitAccumulator(TQueryResult initialAccumulatorValue);

        void AccumulateIfDataDidPass(ref TAccumulator accumulator, TQueryResult outputData);
    }

    public interface IAccumulateNegativeQueryResult<TQueryResult, TAccumulator> //: IQueryPipelineMiddleware
        where TAccumulator : TQueryResult
    {
        event Action<TQueryResult> DataDidNotPass;

        void AccumulateIfDataDidNotPass(ref TAccumulator accumulator, TQueryResult outputData);
    }

    public interface IAccumulatePositiveAggregableQueryResult<TQueryResult>
        : IAccumulatePositiveQueryResult<TQueryResult, TQueryResult>
    {
        TQueryResult IAccumulatePositiveQueryResult<TQueryResult, TQueryResult>.InitAccumulator() 
            => InitAccumulator(default(TQueryResult));
    }

    public interface IAccumulateAnyAggregableQueryResult<TQueryResult>
        : IAccumulatePositiveAggregableQueryResult<TQueryResult>, IAccumulateNegativeQueryResult<TQueryResult, TQueryResult>
    { }

    public interface IAccumulatePositiveEnumerableQueryResult<TQueryResultData>
        : IAccumulatePositiveQueryResult<IEnumerable<TQueryResultData>, ICollection<TQueryResultData>>
    {
        ICollection<TQueryResultData>  IAccumulatePositiveQueryResult<IEnumerable<TQueryResultData>, ICollection<TQueryResultData>>
            .InitAccumulator()
            => new List<TQueryResultData>();

        ICollection<TQueryResultData> IAccumulatePositiveQueryResult<IEnumerable<TQueryResultData>, ICollection<TQueryResultData>>
            .InitAccumulator(IEnumerable<TQueryResultData> initialAccumulatorValue)
            => new List<TQueryResultData>(initialAccumulatorValue);
    }

    public interface IAccumulateAnyEnumerableQueryResult<TQueryResultData>
        : IAccumulatePositiveEnumerableQueryResult<TQueryResultData>, 
          IAccumulateNegativeQueryResult<IEnumerable<TQueryResultData>, ICollection<TQueryResultData>>
    { }

    public abstract class SingleQueryExecutor<TData, TQueryResult> : IQueryPipelineAcceptor
    {
        protected bool _isResultEnumerable;

        public event Action<TQueryResult> DataDidPass;

        public SingleQueryExecutor()
        {
            _isResultEnumerable = typeof(TQueryResult).GetInterface(nameof(System.Collections.IEnumerable)) is not null;
        }

        public abstract void Accept(ISingleQueryExecutorVisitor2 queryPipeline);

        protected void OnDataDidPass(TQueryResult outputData) => DataDidPass?.Invoke(outputData);

        protected void OnDataDidPass(Func<TData, bool, TQueryResult> outputDataSelector, TData data) => 
            DataDidPass?.Invoke(outputDataSelector(data, true));

        //public virtual TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(
        //    ISingleQueryExecutorVisitor2 pipeline) => (dynamic)GetQueryResult();
    }

    /// <summary>
    /// Фабрика для производства исполнителей запросов с продолжением.
    /// </summary>
    public static class SingleQueryExecutorWithContinuationFactory
    {
        public static SingleQueryExecutor<TData, IEnumerable<TQueryResultData>>
            Create<TData, TQueryResultData, TNextQueryResult>(
            IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> innerExecutor,
            IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> nextExecutor)
        {
            return innerExecutor switch
            {
                EveryDataInstanceReader2WithEnumerableResult<TData, TQueryResultData> edir =>
                    new EveryDataInstanceReaderWithContinuation<TData, TQueryResultData, TNextQueryResult>(edir, nextExecutor),
                WholeDataSourceReader2<TData, IEnumerable<TQueryResultData>> wdsr =>
                    new WholeDataSourceReaderWithContinuation<TData, TQueryResultData, TNextQueryResult>(wdsr, nextExecutor),
                _ => throw new ArgumentException("Обёртка в запрос с продолжением " +
                                                $"не поддерживается для следующих типов: {innerExecutor.GetType().Name}.")
            };
        }
    }

    /// <summary>
    /// Декоратор исполнителя запроса для передачи результата запроса на следующего исполнителя запросов в конвейере.
    /// </summary>
    /// <typeparam name="TInnerQueryExecutor"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResultData"></typeparam>
    /// <typeparam name="TNextQueryResult"></typeparam>
    public abstract class SingleQueryExecutorWithContinuation<TInnerQueryExecutor, TData, TQueryResultData, TNextQueryResult>
        : SingleQueryExecutor<TData, IEnumerable<TQueryResultData>>, 
          IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>>
        where TInnerQueryExecutor : SingleQueryExecutor<TData, IEnumerable<TQueryResultData>>
    {
        public TInnerQueryExecutor InnerExecutor { get; private set; }

        public IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> NextExecutor { get; protected set; }

        protected SingleQueryExecutorWithContinuation(
            TInnerQueryExecutor innerExecutor,
            IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> nextExecutor)
            : base()
        {
            InnerExecutor = innerExecutor;
            InitInnerExecutorBehavior();
            NextExecutor = nextExecutor;
        }

        public void UpdateNextQueryExecutor(IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> newNextExecutor)
            => NextExecutor = newNextExecutor;

        protected abstract void InitInnerExecutorBehavior();

        public abstract TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor2 pipeline);

        public override void Accept(ISingleQueryExecutorVisitor2 pipeline) => InnerExecutor.Accept(pipeline);

        IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>>
            .ContinueWith<TContinuingQueryData, TContinuingQueryResult>(
                IQueryPipelineMiddleware<TContinuingQueryData, TContinuingQueryResult> continuingExecutor)
        {
            NextExecutor = NextExecutor.ContinueWith(continuingExecutor);

            return this;
        }
    }

    public class EveryDataInstanceReaderWithContinuation<TData, TQueryResultData, TNextQueryResult>
        : SingleQueryExecutorWithContinuation<
            EveryDataInstanceReader2WithEnumerableResult<TData, TQueryResultData>, 
            TData, 
            TQueryResultData, 
            TNextQueryResult>
    {
        //private Predicate<TQueryResultData> ExecuteNextQueryOverDataInstance;

        public EveryDataInstanceReaderWithContinuation(
            EveryDataInstanceReader2WithEnumerableResult<TData, TQueryResultData> innerExecutor,
            IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> nextExecutor)
            : base(innerExecutor, nextExecutor)
        {
            Action<IEnumerable<TQueryResultData>> onDataDidPass;  
            switch(NextExecutor)
            {
                case EveryDataInstanceReader2<TQueryResultData, TNextQueryResult> nextEDIR:
                    {
                        onDataDidPass = (intermediateResult) => nextEDIR.ExecuteOverDataInstance(intermediateResult.First());
                        break;
                    }
                case WholeDataSourceReader2<TQueryResultData, TNextQueryResult> nextWDSR:
                    {
                        onDataDidPass = (intermediateResult) => nextWDSR.ExecuteOverDataInstance(intermediateResult.First());
                        break;
                    }
                default: throw new InvalidOperationException();
            };
            InnerExecutor.DataDidPass += onDataDidPass;
        }

        protected override void InitInnerExecutorBehavior()
        {
            InnerExecutor.InitBehavior(ExecuteOverDataInstanceHandler);
        }

        public bool ExecuteOverDataInstanceHandler(TData data)
        {
            (_, bool mustGoOn) = InnerExecutor.ExecuteOverDataInstanceHandler(data);

            return mustGoOn;
            //return didDataPass ? ExecuteNextQueryOverDataInstance(
            //    InnerExecutor.IntermediateQueryResult.First()) : mustGoOn;
        }

        public override TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor2 pipeline)
            => NextExecutor.GetPipelineQueryResult<TPipelineQueryResult>(pipeline);
    }

    public class WholeDataSourceReaderWithContinuation<TData, TQueryResultData, TNextQueryResult>
        : SingleQueryExecutorWithContinuation<
            WholeDataSourceReader2<TData, IEnumerable<TQueryResultData>>,
            TData, 
            TQueryResultData, 
            TNextQueryResult>
    {

        public WholeDataSourceReaderWithContinuation(
            WholeDataSourceReader2<TData, IEnumerable<TQueryResultData>> innerExecutor,
            IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> nextExecutor)
            : base(innerExecutor, nextExecutor)
        { }

        protected override void InitInnerExecutorBehavior()
        {
            InnerExecutor.InitBehavior(ExecuteOverDataInstanceHandler);
        }

        public void ExecuteOverDataInstanceHandler(TData data) => InnerExecutor.ExecuteOverDataInstanceHandler(data);

        public override TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor2 pipeline)
        {
            // Явное уведомление исполнителя запроса о требовании подытоживания запроса.
            IEnumerable<TQueryResultData> queryResult = InnerExecutor.GetQueryResult();
            // Установка текущего источника данных в конвейере значением результата выполненного запроса.
            pipeline.SetDataSource(queryResult);
            pipeline.FirstQueryExecutor = NextExecutor;

            // Переход к следующему исполнителю запроса.
            return pipeline.Execute<TPipelineQueryResult>();
        }
    }

    public static class SingleQueryExecutorWithAccumulationFactory
    {
        public static SingleQueryExecutor<TData, TQueryResult>
            Create<TData, TQueryResult>(SingleQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            return CreateImpl(queryExecutor, (dynamic)queryExecutor);
        }

        private static SingleQueryExecutorWithAccumulation<TData, TQueryResult, TAccumulator>
            CreateImpl<TData, TQueryResult, TAccumulator>(
                SingleQueryExecutor<TData, TQueryResult>  queryExecutor,
                IAccumulatePositiveQueryResult<TQueryResult, TAccumulator> queryExecutorAsAccumulating)
            where TAccumulator : TQueryResult
        {
            return new SingleQueryExecutorWithAccumulation<TData, TQueryResult, TAccumulator>(
                queryExecutor,
                queryExecutorAsAccumulating);
        }
    }

    /// <summary>
    /// Декоратор исполнителя запроса для аккумулирования результатов запроса. Конечная точка конвейера запросов.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResult"></typeparam>
    /// <typeparam name="TAccumulator"></typeparam>
    public class SingleQueryExecutorWithAccumulation<TData, TQueryResult, TAccumulator>
        : SingleQueryExecutor<TData, TQueryResult>, IQueryPipelineMiddleware<TData, TQueryResult>
        where TAccumulator : TQueryResult
    {
        public SingleQueryExecutor<TData, TQueryResult> InnerExecutor { get; private set; }

        protected TAccumulator _accumulator; 

        public SingleQueryExecutorWithAccumulation(
            SingleQueryExecutor<TData, TQueryResult> innerExecutor,
            IAccumulatePositiveQueryResult<TQueryResult, TAccumulator> innerExecutorAsAccumulating)
            : base()
        {
            InnerExecutor = innerExecutor;
            _accumulator = innerExecutorAsAccumulating.InitAccumulator();
            innerExecutor.DataDidPass += 
                (TQueryResult outputData) => innerExecutorAsAccumulating.AccumulateIfDataDidPass(ref _accumulator, outputData);
            if (innerExecutor is IAccumulateNegativeQueryResult<TQueryResult, TAccumulator> fullCoveringReader)
                fullCoveringReader.DataDidNotPass +=
                    (TQueryResult outputData) => fullCoveringReader.AccumulateIfDataDidNotPass(ref _accumulator, outputData);
        }

        public TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor2 pipeline)
        {
            switch (InnerExecutor)
            {
                case WholeDataSourceReader2<TData, TQueryResult> wdsr: 
                    {
                        wdsr.GetQueryResult();

                        break;
                    }
                default: break;
            }

            return (dynamic)_accumulator;//GetQueryResult();//InnerExecutor.GetPipelineQueryResult<TPipelineQueryResult>(pipeline);
        }

        public override void Accept(ISingleQueryExecutorVisitor2 pipeline) => InnerExecutor.Accept(pipeline);

        IQueryPipelineMiddleware<TData, TQueryResult> IQueryPipelineMiddleware<TData, TQueryResult>
            .ContinueWith<TContinuingQueryData, TContinuingQueryResult>(
                IQueryPipelineMiddleware<TContinuingQueryData, TContinuingQueryResult> continuingExecutor)
            => SingleQueryExecutorWithContinuationFactory.Create((dynamic)this, continuingExecutor);
    }
}
