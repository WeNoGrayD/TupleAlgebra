using LINQProvider.QueryPipelineInfrastructure.Buffering;
using LINQProvider.QueryPipelineInfrastructure.Streaming;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryPipelineInfrastructure
{
    /// <summary>
    /// Интерфейс исполнителя конвейера запросов.
    /// </summary>
    public interface IQueryPipelineExecutor
    {
        #region Instance properties

        /// <summary>
        /// Перечислитель предикатов, которые возвращают флаги прохождения данных по конвейеру.
        /// Последний предикат означает, что конечный компонент может предоставить выходные данные.
        /// Промежуточные предикаты, если они имеются, показывают ситуацию в промежуточных компонентах,
        /// если им требуется приостановить пропуск данных по конвейеру.
        /// В целом, промежуточные предикаты добавляются только потоковыми компонентами с 
        /// множественными выходами на один вход.
        /// </summary>
        IEnumerator<Func<bool>> PipelineResultProviders { get; }

        #endregion

        #region Instance methods

        /// <summary>
        /// Добавление предиката прохождения данных по конвейеру.
        /// </summary>
        /// <param name="pipelineResultProvider"></param>
        void PutPipelineResultProvider(Func<bool> pipelineResultProvider);

        /// <summary>
        /// Удаление предиката прохождения данных по конвейеру.
        /// </summary>
        void PullPipelineResultProvider();

        #endregion
    }

    public class QueryPipelineExecutor
        : IQueryPipelineExecutor, ISingleQueryExecutorResultRequester
    {
        #region Instance fields

        /// <summary>
        /// Источник данных конвейера запросов.
        /// </summary>
        private IEnumerable _dataSource;

        /// <summary>
        /// Очередь промежуточных предикатов прохождения данных по конвейеру.
        /// </summary>
        private Queue<Func<bool>> _pipelineResultProviders;

        private MethodInfo _executeWithExpectedEnumerableResultMIPattern;

        #endregion

        #region Instance properties

        public IQueryPipelineMiddleware StartupMiddleware { get; set; }

        public IQueryPipelineEndpoint PipelineEndpoint { get => StartupMiddleware.PipelineEndpoint; }

        public IEnumerator<Func<bool>> PipelineResultProviders { get; private set; }

        #endregion

        #region Constructors

        public QueryPipelineExecutor(IEnumerable dataSource)
        {
            _dataSource = dataSource;
            _pipelineResultProviders = new Queue<Func<bool>>(0);

            return;
        }

        #endregion

        #region implementation

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
                CreateExecuteWithExpectedEnumerableResultMethodPattern();

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
                .AcceptToExecuteWithEnumerableResult<TEndpointOutputData>(_dataSource, this);
            _dataSource = intermediateQueryPipelineResult;

            return intermediateQueryPipelineResult;
        }

        #endregion

        public bool IsRequiredResultEnumerable()
        {
            return (PipelineEndpoint.Multiplicity & QuerySourceToResultMiltiplicity.OneToOne) != 0;
        }

        private void CreateExecuteWithExpectedEnumerableResultMethodPattern()
        {
            _executeWithExpectedEnumerableResultMIPattern = this.GetType()
                .GetMethod(
                    nameof(ExecuteWithExpectedEnumerableResult),
                    BindingFlags.Instance | BindingFlags.NonPublic)!;

            return;
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
            queryExecutor.LoadDataSource((_dataSource as IEnumerable<TData>)!);

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
                IEnumerable dataSource,
                IQueryPipelineMiddleware<TData, TQueryResult> startupMiddleware,
                BufferingQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            queryExecutor.LoadDataSource((dataSource as IEnumerable<TData>)!);

            return startupMiddleware.GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(this);
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
            bool mustGoOn;

            StartupMiddleware.PrepareToAggregableResult<TPipelineQueryResult>(this);
            queryExecutor.PrepareToQueryStart();

            foreach (TData data in GetDataSource<TData>())
            {
                (_, mustGoOn) = queryExecutor.ExecuteOverDataInstance(data);
                if (!mustGoOn) break;
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
                IEnumerable dataSource,
                IStreamingQueryPipelineMiddleware<TFirstData> startupMiddleware,
                StreamingQueryExecutor<TFirstData, TFirstQueryResult> startupExecutor)
        {
            bool resultProvided, mustGoOn;
            Func<bool> pipelineResultProvided = PrepareToExecute();

            foreach (TFirstData data in (dataSource as IEnumerable<TFirstData>)!)
            {
                (resultProvided, mustGoOn) = startupExecutor.ExecuteOverDataInstance(data);
                startupMiddleware.ResultProvided = resultProvided;
                if (pipelineResultProvided())
                {
                    foreach (TPipelineQueryResultData resultData
                         in startupMiddleware.GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(this))
                    {
                        yield return resultData;
                        if (!(mustGoOn && startupMiddleware.MustGoOn)) break;
                    }
                }
                if (!(mustGoOn && startupMiddleware.MustGoOn)) break;
            }

            FinishExecuting();

            yield break;

            /*
             * Подготовка к выполнению запроса.
             */
            Func<bool> PrepareToExecute()
            {
                // Получение конечного компонента текущей задачи конвейера.
                IQueryPipelineEndpoint queryPipelineEndpoint = startupMiddleware.PipelineEndpoint;

                // Очищение очереди предикатов и создание нового перечислителя предикатов.
                ClearPipelineResultProviders();
                PipelineResultProviders = PipelineResultProvidersImpl(queryPipelineEndpoint);

                // Подготовка конвейера к перечислимому результату.
                startupMiddleware.PrepareToEnumerableResult<TPipelineQueryResultData>(this);

                // Получение первого предиката из перечислителя.
                PipelineResultProviders.MoveNext();
                Func<bool> pipelineResultProvided = PipelineResultProviders.Current;
                
                // Указание конвейеру подготовить свои предикаты (если требуется).
                startupMiddleware.PreparePipelineResultProvider();

                return pipelineResultProvided;
            }

            /*
             * Окончание выполнения запроса.
             */
            void FinishExecuting()
            {
                PipelineResultProviders.Dispose();

                return;
            }
        }

        #region IQueryPipelineExecutor implementation

        /// <summary>
        /// Очищение очереди промежуточных предикатов прохождения данных по конвейеру.
        /// </summary>
        private void ClearPipelineResultProviders()
        {
            _pipelineResultProviders.Clear();

            return;
        }

        public void PutPipelineResultProvider(Func<bool> pipelineResultProvider)
        {
            _pipelineResultProviders.Enqueue(pipelineResultProvider);

            return;
        }

        public void PullPipelineResultProvider()
        {
            _pipelineResultProviders.Dequeue();

            return;
        }

        /// <summary>
        /// Метод возвращает перечислитель предикатов прохождения данных по конвейеру.
        /// </summary>
        /// <param name="endpoint">Конечный компонент текущей задачи конвейера.</param>
        /// <returns></returns>
        private IEnumerator<Func<bool>> PipelineResultProvidersImpl(IQueryPipelineEndpoint endpoint)
        {
            /*
             * Сначала возвращаются все промежуточные предикаты из очереди.
             */
            foreach (Func<bool> pipelineResultProvider in _pipelineResultProviders)
            {
                yield return pipelineResultProvider;
            }

            /*
             * Затем передаётся предикат получения результата на конечном компоненте конвейера.
             */
            yield return () => EndpointResultProvider(endpoint);
        }

        /// <summary>
        /// Предикат получения результата на конечном компоненте конвейера.
        /// </summary>
        /// <param name="endpoint">Конечный компонент текущей задачи конвейера.</param>
        /// <returns></returns>
        private bool EndpointResultProvider(IQueryPipelineEndpoint endpoint) => endpoint.ResultProvided;

        #endregion
    }
}
