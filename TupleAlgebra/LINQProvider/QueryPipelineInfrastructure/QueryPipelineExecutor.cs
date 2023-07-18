using LINQProvider.QueryPipelineInfrastructure.Buffering;
using LINQProvider.QueryPipelineInfrastructure.Streaming;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryPipelineInfrastructure
{
    public class QueryPipelineExecutor : ISingleQueryExecutorVisitor
    {
        #region Instance fields

        /// <summary>
        /// Источник данных конвейера запросов.
        /// </summary>
        private IEnumerable _dataSource;

        private MethodInfo _executeWithExpectedEnumerableResultMIPattern;

        #endregion

        #region Instance properties

        public IQueryPipelineMiddleware StartupMiddleware { get; set; }

        public IQueryPipelineEndpoint PipelineEndpoint { get => StartupMiddleware.PipelineEndpoint; }

        public bool MustGoOn { get; protected set; }

        #endregion

        #region Constructors

        public QueryPipelineExecutor(IEnumerable dataSource)
        {
            _dataSource = dataSource;
            MustGoOn = true;

            return;
        }

        #endregion

        #region implemention

        public IEnumerable<TData> GetDataSource<TData>()
        {
            return (_dataSource as IEnumerable<TData>)!;
        }

        /// <summary>
        /// Обобщённая установка источника данных.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="dataSource"></param>
        public void SetDataSource<TData>(IEnumerable<TData> dataSource)
        {
            _dataSource = dataSource;

            return;
        }

        public TPipelineQueryResult ExecuteWithExpectedAggregableResult<TPipelineQueryResult>()
        {
            return StartupMiddleware.AcceptToExecuteWithAggregableResult<TPipelineQueryResult>(this);
        }

        public IEnumerable ExecuteWithExpectedEnumerableResult()
        {
            if (_executeWithExpectedEnumerableResultMIPattern is null)
                CreateExecuteWithExecptedEnumerableResultMethodPattern();

            Type[] endpointInterfaceGenericArguments = GetGenericQueryableMiddlewareInterfaceArguments();
            Type endpointOutputDataType = GetEndpointOutputDataType(endpointInterfaceGenericArguments);

            return (_executeWithExpectedEnumerableResultMIPattern!
                .MakeGenericMethod(endpointOutputDataType)
                .Invoke(this, null) as IEnumerable)!;

        }

        private Type[] GetGenericQueryableMiddlewareInterfaceArguments()
        {
            Type[] queryPipelineEndpointInterfaces = PipelineEndpoint.GetType().GetInterfaces(),
                   endpointInterfaceGenericArguments = null!;
            Type endpointInterface;

            for (int i = 0; i < queryPipelineEndpointInterfaces.Length; i++)
            {
                endpointInterface = queryPipelineEndpointInterfaces[i];
                if (endpointInterface.Name.StartsWith("IQueryPipelineMiddleware") &&
                    (endpointInterfaceGenericArguments = endpointInterface.GetGenericArguments()).Length == 2)
                    break;
            }

            return endpointInterfaceGenericArguments;
        }

        private Type GetEndpointOutputDataType(Type[] endpointInterfaceGenericArguments)
        {
            Type endpointOutputType = endpointInterfaceGenericArguments[1];

            return endpointOutputType.GetGenericArguments().SingleOrDefault() ?? typeof(object);
        }

        protected IEnumerable<TEndpointOutputData> ExecuteWithExpectedEnumerableResult<TEndpointOutputData>()
        {
            IEnumerable<TEndpointOutputData> intermediateQueryPipelineResult = StartupMiddleware
                .AcceptToExecuteWithEnumerableResult<TEndpointOutputData>(this);
            //_dataSource = intermediateQueryPipelineResult;

            //OnEndedUpWithEnumerableResult(intermediateQueryPipelineResult);

            return intermediateQueryPipelineResult;
        }

        #endregion

        public bool IsRequiredResultEnumerable()
        {
            return (PipelineEndpoint.Multiplicity & QuerySourceToResultMiltiplicity.OneToOne) != 0;// queryPipelineResultType.IsAssignableTo(typeof(IEnumerable));
            //return (StartupPipelineTask.GetPipelineEndpoint().Multiplicity & QuerySourceToResultMiltiplicity.OneToOne) != 0;
        }

        private void CreateExecuteWithExecptedEnumerableResultMethodPattern()
        {
            _executeWithExpectedEnumerableResultMIPattern = this.GetType()
                .GetMethod(
                    nameof(ExecuteWithExpectedEnumerableResult),
                    BindingFlags.Instance | BindingFlags.NonPublic)!;

            return;
        }

        /// <summary>
        /// Посещение буферизирующего исполнителя запросов.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <typeparam name="TPipelineQueryResultParam"></typeparam>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="isResultEnumerable"></param>
        /// <param name="queryExecutor"></param>
        /// <returns></returns>
        public TPipelineQueryResult VisitBufferingQueryExecutor<TData, TQueryResult, TPipelineQueryResultParam, TPipelineQueryResult>(
            bool isResultEnumerable,
            BufferingQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            queryExecutor.LoadDataSource(GetDataSource<TData>());

            return isResultEnumerable ?
                (TPipelineQueryResult)VisitBufferingQueryExecutorWithExpectedEnumerableResult<TData, TQueryResult, TPipelineQueryResultParam>(queryExecutor) :
                VisitBufferingQueryExecutorWithExpectedAggregableResult<TData, TQueryResult, TPipelineQueryResult>(queryExecutor);
        }

        /// <summary>
        /// Посещение буферизирующего исполнителя запросов с ожидаемым агрегируемым результатом.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="queryExecutor"></param>
        /// <returns></returns>
        public TPipelineQueryResult
            VisitBufferingQueryExecutorWithExpectedAggregableResult<TData, TQueryResult, TPipelineQueryResult>(
            BufferingQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            return StartupMiddleware.GetAggregablePipelineQueryResult<TPipelineQueryResult>(this);
        }

        /// <summary>
        /// Посещение буферизирующего исполнителя запросов с ожидаемым перечислимым результатом.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="queryExecutor"></param>
        /// <returns></returns>
        public IEnumerable<TPipelineQueryResultData>
            VisitBufferingQueryExecutorWithExpectedEnumerableResult<TData, TQueryResult, TPipelineQueryResultData>(
            BufferingQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            return StartupMiddleware.GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(this);
        }

        /// <summary>
        /// Посещение потокового исполнителя запросов с ожидаемым агрегируемым результатом.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="queryExecutor"></param>
        /// <returns></returns>
        public TPipelineQueryResult
            VisitStreamingQueryExecutorWithExpectedAggregableResult<TData, TQueryResult, TPipelineQueryResult>(
            StreamingQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            StartupMiddleware.PrepareToAggregableResult<TPipelineQueryResult>(this);
            queryExecutor.PrepareToQueryStart();
            if (MustGoOn)
            {
                foreach (TData data in GetDataSource<TData>())
                {
                    (_, MustGoOn) = queryExecutor.ExecuteOverDataInstance(data);
                    if (!MustGoOn) break;
                }
            }

            return StartupMiddleware.GetAggregablePipelineQueryResult<TPipelineQueryResult>(this);
        }

        /// <summary>
        /// Посещение потокового исполнителя запросов с ожидаемым перечислимым результатом.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="queryExecutor"></param>
        /// <returns></returns>
        public IEnumerable<TPipelineQueryResultData>
            VisitStreamingQueryExecutorWithExpectedEnumerableResult<TFirstData, TFirstQueryResult, TPipelineQueryResultData>(
            StreamingQueryExecutor<TFirstData, TFirstQueryResult> firstQueryExecutor)
        {
            if (!MustGoOn)
                yield break;

            StartupMiddleware.PrepareToEnumerableResult<TPipelineQueryResultData>(this);
            firstQueryExecutor.PrepareToQueryStart();
            bool resultsProvided;
            IQueryPipelineEndpoint queryPipelineEndpoint = StartupMiddleware.PipelineEndpoint;

            foreach (TFirstData data in GetDataSource<TFirstData>())
            {
                (resultsProvided, MustGoOn) = firstQueryExecutor.ExecuteOverDataInstance(data);
                StartupMiddleware.ResultProvided = resultsProvided;
                if (queryPipelineEndpoint.ResultProvided)
                {
                    foreach (TPipelineQueryResultData resultData
                         in StartupMiddleware.GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(this))
                    {
                        yield return resultData;
                        if (!StartupMiddleware.MustGoOn) break;
                    }
                }
                if (!(MustGoOn && StartupMiddleware.MustGoOn)) break;
            }

            yield break;
        }
    }
}
