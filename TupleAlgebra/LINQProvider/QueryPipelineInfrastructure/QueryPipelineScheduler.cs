﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using LINQProvider.QueryPipelineInfrastructure.Buffering;
using LINQProvider.QueryPipelineInfrastructure.Streaming;
using System.Linq.Expressions;

namespace LINQProvider.QueryPipelineInfrastructure
{
    /// <summary>
    /// Исполнитель конвейера запросов.
    /// </summary>
    public class QueryPipelineScheduler : IQueryPipelineScheduler
    {
        #region Instance fields

        /// <summary>
        /// Источник данных конвейера запросов.
        /// </summary>
        private QueryPipelineExecutor _executor;

        /// <summary>
        /// Ссылка на первый компонент конвейера запросов.
        /// </summary>
        public LinkedListNode<IQueryPipelineMiddleware> _currentPipelineTask;

        /// <summary>
        /// Флаг перечислимости результата запроса.
        /// </summary>
        protected bool _isResultEnumerable;

        #endregion

        #region Instance properties

        /// <summary>
        /// Расписание задач конвейра.
        /// Одна задача - связанный список компонентов, которые могут передавать информацию друг другу.
        /// </summary>
        public LinkedList<IQueryPipelineMiddleware> PipelineTaskSchedule { get; set; }

        public IQueryPipelineMiddleware StartupPipelineTask 
        { 
            get => _currentPipelineTask.Value;
        }

        protected IQueryPipelineMiddleware LastPipelineTask
        {
            get => PipelineTaskSchedule.Last!.Value;
            set => PipelineTaskSchedule.Last!.Value = value;
        }

        public QueryPipelineMiddlewareWithAccumulationFactory MiddlewareWithAccumulationFactory { get; private set; }

        public QueryPipelineMiddlewareWithContinuationFactory MiddlewareWithContinuationFactory { get; private set; }

        public bool IsResultEnumerable { get => _isResultEnumerable; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="dataSource"></param>
        /// <param name="firstPipelineMiddleware"></param>
        public QueryPipelineScheduler(
            QueryContext queryContext,
            IEnumerable<MethodCallExpression> methodCallChain,
            IEnumerable dataSource)
        {
            _executor = new QueryPipelineExecutor(dataSource);
            PipelineTaskSchedule = new LinkedList<IQueryPipelineMiddleware>();

            MiddlewareWithAccumulationFactory = CreateMiddlewareWithAccumulationFactory();
            MiddlewareWithContinuationFactory = CreateMiddlewareWithContinuationFactory();
            InitPipeline(queryContext, methodCallChain);
        }

        #endregion

        #region Instance methods

        private void InitPipeline(
            QueryContext queryContext, 
            IEnumerable<MethodCallExpression> methodCallChain)
        {
            object singleQueryExecutor;
            IQueryPipelineEndpoint middleware;

            /*
             * 1) Создаётся исполнитель запроса.
             * 2) Создаётся компонент конвейера, содержащий исполнитель запроса.
             * 3) В конец конвейера (в виде первого звена новой задачи или последнего звена текущей)
             * ставится этот компонент конвейера.
             */
            foreach (MethodCallExpression queryMethod in methodCallChain)
            {
                singleQueryExecutor = queryContext.BuildSingleQueryExecutor(queryMethod);
                middleware = CoverQueryExecutorWithMiddleware(singleQueryExecutor);
                ContinuePipelineWith(middleware);
            }

            // Конечная точка последней задачи инициализируется как аккумулятор.
            LastPipelineTask.PipelineEndpoint.InitializeAsQueryPipelineEndpoint();

            return;
        }

        protected IQueryPipelineEndpoint CoverQueryExecutorWithMiddleware(
            object singleQueryExecutor)
        {
            // Создаётся аккумулирующий компонент конвейера.
            return MiddlewareWithAccumulationFactory.Create(singleQueryExecutor);
        }

        public void ContinuePipelineWith(IQueryPipelineEndpoint middleware)
        {
            LinkedListNode<IQueryPipelineMiddleware> lastPipelineTask =
                PipelineTaskSchedule.Last!;

            /*
             * Если расписание задач вообще не содержит ни одной задачи, то
             * добавляется новая.
             * Иначе последняя задача обрабатывает добавление нового компонента конвейера.
             */
            if (PipelineTaskSchedule.Count == 0)
                PushMiddleware(middleware);
            else
                lastPipelineTask.Value = LastPipelineTask.ContinueWith(middleware, this);

            return;
        }

        protected virtual QueryPipelineMiddlewareWithAccumulationFactory
            CreateMiddlewareWithAccumulationFactory()
        {
            return new QueryPipelineMiddlewareWithAccumulationFactory();
        }

        protected virtual QueryPipelineMiddlewareWithContinuationFactory
            CreateMiddlewareWithContinuationFactory()
        {
            return new QueryPipelineMiddlewareWithContinuationFactory();
        }

        #endregion

        #region IQueryPipelineScheduler implementation

        public TPipelineQueryResult Execute<TPipelineQueryResult>()
        {
            IEnumerable enumerableIntermediateResult = null!;
            TPipelineQueryResult result = default(TPipelineQueryResult)!;

            _currentPipelineTask = PipelineTaskSchedule.First!;
            do
            {
                _executor.StartupMiddleware = StartupPipelineTask;
                _isResultEnumerable = _executor.IsRequiredResultEnumerable();
                if (_isResultEnumerable)
                {
                    enumerableIntermediateResult = _executor.ExecuteWithExpectedEnumerableResult();
                }
                else
                {
                    result = _executor.ExecuteWithExpectedAggregableResult<TPipelineQueryResult>();
                    break;
                }

                GotoNextPipelineTask();
            }
            while (_currentPipelineTask != null);

            return _isResultEnumerable ? (TPipelineQueryResult)enumerableIntermediateResult : result;
        }

        public void GotoPreviousPipelineTask()
        {
            _currentPipelineTask = _currentPipelineTask.Previous!;

            return;
        }

        public void GotoNextPipelineTask()
        {
            _currentPipelineTask = _currentPipelineTask.Next!;

            return;
        }

        /// <summary>
        /// Вставка нового компонента конвейера как начала новой задачи.
        /// </summary>
        /// <param name="startupMiddleware"></param>
        public void PushMiddleware(IQueryPipelineEndpoint startupMiddleware)
        {
            if (PipelineTaskSchedule.Count > 0)
            {
                // Конечная точка последней задачи инициализируется как локальный аккумулятор этой задачи.
                LastPipelineTask.PipelineEndpoint.InitializeAsQueryPipelineEndpoint();
            }

            /*
             * Компонент инициализируется как начало задачи, в конвейер добавляется новая задача.
             */
            startupMiddleware.InitializeAsQueryPipelineStartupMiddleware();
            PipelineTaskSchedule.AddLast(startupMiddleware);

            return;
        }

        #endregion
    }
}
