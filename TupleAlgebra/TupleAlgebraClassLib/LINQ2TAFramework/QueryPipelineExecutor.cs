using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    public abstract class QueryPipelineExecutor : ISingleQueryExecutorVisitor
    {
        [DllImport("kernel32.dll")]
        extern static IntPtr ConvertThreadToFiber(int fiberData);

        [DllImport("kernel32.dll")]
        extern static IntPtr CreateFiber(int size, System.Delegate function, int handle);

        [DllImport("kernel32.dll")]
        extern static IntPtr SwitchToFiber(IntPtr fiberAddress);

        [DllImport("kernel32.dll")]
        extern static void DeleteFiber(IntPtr fiberAddress);

        [DllImport("kernel32.dll")]
        extern static int GetLastError();

        private object _dataSource;

        private MethodInfo _pipelineQueryExecutionMethodInfo;

        private IQueryPipelineEndpoint _endpoint;

        public IQueryPipelineMiddleware FirstPipelineMiddleware { get; set; }

        protected QueryPipelineExecutor(object dataSource, IQueryPipelineMiddleware firstQueryExecutor)
        {
            _dataSource = dataSource;
            FirstPipelineMiddleware = firstQueryExecutor;
            _endpoint = firstQueryExecutor.GetPipelineEndpoint();

            _endpoint.InitializeAsQueryPipelineEndpoint();
        }

        private IEnumerable<TData> GetDataSource<TData>()
        {
            return _dataSource as IEnumerable<TData>;
        }

        public void SetDataSource<TData>(IEnumerable<TData> dataSource)
        {
            _dataSource = dataSource;

            return;
        }

        public TPipelineQueryResult ContinueQueryExecuting<TQueryResultData, TPipelineQueryResult>(
            IQueryPipelineMiddleware nextMiddleware,
            IEnumerable<TQueryResultData> dataSource)
        {
            // Установка текущего источника данных в конвейере значением результата выполненного запроса.
            SetDataSource(dataSource);
            FirstPipelineMiddleware = nextMiddleware;
            return (TPipelineQueryResult)_pipelineQueryExecutionMethodInfo.Invoke(this, null);
        }

        public TPipelineQueryResult ExecuteWithExpectedAggregableResult<TPipelineQueryResult>()
        {
            _pipelineQueryExecutionMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return FirstPipelineMiddleware.Accept<TPipelineQueryResult, TPipelineQueryResult>(false, this);
        }

        public IEnumerable<TPipelineQueryResultData> ExecuteWithExpectedEnumerableResult<TPipelineQueryResultData>()
        {
            _pipelineQueryExecutionMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return FirstPipelineMiddleware.Accept<TPipelineQueryResultData, IEnumerable<TPipelineQueryResultData>>(true, this);
        }

        public TPipelineQueryResult VisitStreamingQueryExecutor<TData, TQueryResult, TPipelineQueryResultParam, TPipelineQueryResult>(
            bool isResultEnumerable,
            StreamingQueryExecutor<TData, TQueryResult> queryExecutor) =>
            isResultEnumerable ?
            (TPipelineQueryResult)VisitStreamingQueryExecutorWithExpectedEnumerableResult<TData, TQueryResult, TPipelineQueryResultParam>(queryExecutor) :
            VisitStreamingQueryExecutorWithExpectedAggregableResult<TData, TQueryResult, TPipelineQueryResult>(queryExecutor);

        public TPipelineQueryResult VisitBufferingQueryExecutor<TData, TQueryResult, TPipelineQueryResultParam, TPipelineQueryResult>(
            bool isResultEnumerable, 
            BufferingQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            queryExecutor.LoadDataSource(GetDataSource<TData>());

            return FirstPipelineMiddleware.GetPipelineQueryResult<TPipelineQueryResult>(this);
        }

        public TPipelineQueryResult 
            VisitBufferingQueryExecutorWithExpectedAggregableResult<TData, TQueryResult, TPipelineQueryResult>(
            BufferingQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            queryExecutor.LoadDataSource(GetDataSource<TData>());

            return FirstPipelineMiddleware.GetPipelineQueryResult<TPipelineQueryResult>(this);
        }

        public IEnumerable<TPipelineQueryResultData> 
            VisitBufferingQueryExecutorWithExpectedEnumerableResult<TData, TQueryResult, TPipelineQueryResultData>(
            BufferingQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            queryExecutor.LoadDataSource(GetDataSource<TData>());
            
            return FirstPipelineMiddleware.GetPipelineQueryResult<IEnumerable<TPipelineQueryResultData>>(this);
        }

        public TPipelineQueryResult
            VisitStreamingQueryExecutorWithExpectedAggregableResult<TData, TQueryResult, TPipelineQueryResult>(
            StreamingQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            FirstPipelineMiddleware.PrepareToAggregableResult<TPipelineQueryResult>(this);
            queryExecutor.PrepareToQueryStart();
            foreach (TData data in GetDataSource<TData>())
                if (!queryExecutor.ExecuteOverDataInstance(data)) break;

            return FirstPipelineMiddleware.GetAggregablePipelineQueryResult<TPipelineQueryResult>(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="queryExecutor"></param>
        /// <returns></returns>
        public IEnumerable<TPipelineQueryResultData>
            VisitStreamingQueryExecutorWithExpectedEnumerableResult<TData, TQueryResult, TPipelineQueryResultData>(
            StreamingQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            FirstPipelineMiddleware.PrepareToEnumerableResult<TPipelineQueryResultData>(this);

            /* 
             * Динамическое приведение безопасно: ожидаемый результат конвейера по типу совпадает с результатом
             * конечного middleware. 
             */
            //return VisitStreamingQueryExecutorWithExpectedEnumerableResultImpl4(
            //    queryExecutor,
            //    (dynamic)_endpoint);

            return VisitStreamingQueryExecutorWithExpectedEnumerableResultImpl6<TData, TQueryResult, TPipelineQueryResultData>(
                queryExecutor);
        }

        private IEnumerable<TPipelineQueryResultData>
            VisitStreamingQueryExecutorWithExpectedEnumerableResultImpl<TFirstQueryData, TFirstQueryResult, TLastQueryData, TPipelineQueryResultData>(
            StreamingQueryExecutor<TFirstQueryData, TFirstQueryResult> firstQueryExecutor,
            IQueryPipelineEndpoint<TLastQueryData, IEnumerable<TPipelineQueryResultData>> lastQueryAccumulator)
        {
            firstQueryExecutor.PrepareToQueryStart();
            bool mustGoOn;
            IEnumerable<TPipelineQueryResultData> emptyPipelineAccumulator = Enumerable.Empty<TPipelineQueryResultData>(),
                                                  pipelineAccumulator = emptyPipelineAccumulator;
            lastQueryAccumulator.SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
                (IEnumerable<TPipelineQueryResultData> outputData) => pipelineAccumulator = outputData);

            foreach (TFirstQueryData data in GetDataSource<TFirstQueryData>())
            {
                mustGoOn= firstQueryExecutor.ExecuteOverDataInstance(data);
                foreach (TPipelineQueryResultData resultData in pipelineAccumulator)
                    yield return resultData;
                if (!mustGoOn) yield break;
                pipelineAccumulator = emptyPipelineAccumulator;
            }

            yield break;
        }

        private IEnumerable<TPipelineQueryResultData>
            VisitStreamingQueryExecutorWithExpectedEnumerableResultImpl2<TFirstQueryData, TFirstQueryResult, TLastQueryData, TPipelineQueryResultData>(
            StreamingQueryExecutor<TFirstQueryData, TFirstQueryResult> firstQueryExecutor,
            IQueryPipelineEndpoint<TLastQueryData, IEnumerable<TPipelineQueryResultData>> lastQueryAccumulator)
        {
            firstQueryExecutor.PrepareToQueryStart();
            bool mustGoOn;

            foreach (TFirstQueryData data in GetDataSource<TFirstQueryData>())
            {
                mustGoOn = firstQueryExecutor.ExecuteOverDataInstance(data);
                foreach (TPipelineQueryResultData resultData 
                         in FirstPipelineMiddleware.GetPipelineQueryResult<IEnumerable<TPipelineQueryResultData>>(this))
                    yield return resultData;
                if (!mustGoOn) yield break;
            }

            yield break;
        }

        private IEnumerable<TPipelineQueryResultData>
            VisitStreamingQueryExecutorWithExpectedEnumerableResultImpl3<TFirstQueryData, TFirstQueryResult, TLastQueryData, TPipelineQueryResultData>(
            StreamingQueryExecutor<TFirstQueryData, TFirstQueryResult> firstQueryExecutor,
            IQueryPipelineEndpoint<TLastQueryData, IEnumerable<TPipelineQueryResultData>> lastQueryAccumulator)
        {
            IntPtr mainFiberPtr = ConvertThreadToFiber(System.Threading.Thread.CurrentThread.ManagedThreadId);
            TFirstQueryData currentData = default(TFirstQueryData);
            Action executeOverDataInstance = ExecuteOverDataInstance;
            IntPtr executeOverDataInstanceFiberPtr = CreateFiber(2^32, executeOverDataInstance, 12);
            bool mustGoOn = false, dataPassed = false;
            TPipelineQueryResultData pipelineResultData = default(TPipelineQueryResultData);
            IEnumerable<TPipelineQueryResultData> temp = null;
            firstQueryExecutor.PrepareToQueryStart();

            foreach (TFirstQueryData data in GetDataSource<TFirstQueryData>())
            {
                currentData = data;
                SwitchToFiber(executeOverDataInstanceFiberPtr);

                while (dataPassed)
                {
                    var temp2 = temp;
                    //yield return pipelineResultData;
                    //foreach (var temp3 in temp) yield return temp3;
                    SwitchToFiber(executeOverDataInstanceFiberPtr);
                }

                if (!mustGoOn) yield break;
            }

            yield break;

            void ExecuteOverDataInstance()
            {
                lastQueryAccumulator.SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
                    (IEnumerable<TPipelineQueryResultData> outputData) =>
                    {
                        dataPassed = true;
                        temp = lastQueryAccumulator.Accumulator;//.ToList();
                                                                //pipelineResultData = temp.First();//lastQueryAccumulator.Accumulator.First();
                        SwitchToFiber(mainFiberPtr);
                        dataPassed = false;
                    });
                while (true)
                {
                    mustGoOn = firstQueryExecutor.ExecuteOverDataInstance(currentData);
                    dataPassed = false;
                    SwitchToFiber(mainFiberPtr);
                }
            }
        }

        private IEnumerable<TPipelineQueryResultData>
            VisitStreamingQueryExecutorWithExpectedEnumerableResultImpl4<TFirstQueryData, TFirstQueryResult, TLastQueryData, TPipelineQueryResultData>(
            StreamingQueryExecutor<TFirstQueryData, TFirstQueryResult> firstQueryExecutor,
            IQueryPipelineEndpoint<TLastQueryData, IEnumerable<TPipelineQueryResultData>> lastQueryAccumulator)
        {
            firstQueryExecutor.PrepareToQueryStart();
            bool mustGoOn;

            foreach (TFirstQueryData data in GetDataSource<TFirstQueryData>())
            {
                mustGoOn = firstQueryExecutor.ExecuteOverDataInstance(data);
                foreach (TPipelineQueryResultData resultData
                         in FirstPipelineMiddleware.GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(this))
                    yield return resultData;
                if (!mustGoOn) yield break;
            }

            yield break;
        }

        private IEnumerable<TPipelineQueryResultData>
            VisitStreamingQueryExecutorWithExpectedEnumerableResultImpl5<TFirstQueryData, TFirstQueryResult, TPipelineQueryResultData>(
            StreamingQueryExecutor<TFirstQueryData, TFirstQueryResult> firstQueryExecutor)
        {
            firstQueryExecutor.PrepareToQueryStart();
            IQueryPipelineMiddleware firstPipelineMiddleware = FirstPipelineMiddleware;
            bool mustGoOn;

            foreach (TFirstQueryData data in GetDataSource<TFirstQueryData>())
            {
                mustGoOn = firstQueryExecutor.ExecuteOverDataInstance(data);
                foreach (TPipelineQueryResultData resultData
                         in firstPipelineMiddleware.GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(this))
                    yield return resultData;
                if (!mustGoOn) yield break;
            }

            yield break;
        }

        private IEnumerable<TPipelineQueryResultData>
            VisitStreamingQueryExecutorWithExpectedEnumerableResultImpl6<TFirstQueryData, TFirstQueryResult, TPipelineQueryResultData>(
            StreamingQueryExecutor<TFirstQueryData, TFirstQueryResult> firstQueryExecutor)
        {
            firstQueryExecutor.PrepareToQueryStart();
            //IStreamingQueryPipelineMiddleware firstPipelineMiddleware = FirstPipelineMiddleware as IStreamingQueryPipelineMiddleware;
            IEnumerable<TPipelineQueryResultData> queryResult;
            //bool mustGoOn, mustGoOnFromNextLayers;
            bool mustGoOn;

            foreach (TFirstQueryData data in GetDataSource<TFirstQueryData>())
            {
                mustGoOn = firstQueryExecutor.ExecuteOverDataInstance(data);
                //(queryResult, mustGoOnFromNextLayers) = firstPipelineMiddleware.GetEnumerablePipelineQueryResult2<TPipelineQueryResultData>(this);
                queryResult = FirstPipelineMiddleware.GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(this);
                foreach (TPipelineQueryResultData resultData in queryResult)
                {
                    yield return resultData;
                    //if (!(mustGoOn & mustGoOnFromNextLayers)) yield break;
                    if (!(mustGoOn &= FirstPipelineMiddleware.MustGoOn)) yield break;
                }
                if (!(mustGoOn & FirstPipelineMiddleware.MustGoOn)) yield break;
            }

            yield break;
        }

        public delegate void LoadPipelineAccumulatorHandler<TQueryPipelineResultData>(
            ref IEnumerable<TQueryPipelineResultData> pipelineAccumulator,
            IEnumerable<TQueryPipelineResultData> accumulatorValue);
    }
}
