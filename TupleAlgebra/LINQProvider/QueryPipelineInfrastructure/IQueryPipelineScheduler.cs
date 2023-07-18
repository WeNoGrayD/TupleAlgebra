using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryPipelineInfrastructure
{
    public interface IQueryPipelineScheduler
    {
        #region Properties

        /// <summary>
        /// Фабрика компонентов конвейера с аккумуляцией результата.
        /// </summary>
        public QueryPipelineMiddlewareWithAccumulationFactory MiddlewareWithAccumulationFactory { get; }

        /// <summary>
        /// Фабрика компонентов конвейера с продолжением.
        /// </summary>
        public QueryPipelineMiddlewareWithContinuationFactory MiddlewareWithContinuationFactory { get; }

        #endregion

        #region Methods

        void GotoPreviousPipelineTask();

        void GotoNextPipelineTask();

        /// <summary>
        /// Получение обобщённого источника данных.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <returns></returns>
        //IEnumerable<TData> GetDataSource<TData>();

        /// <summary>
        /// Обобщённая установка источника данных.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="dataSource"></param>
        //void SetDataSource<TData>(IEnumerable<TData> dataSource);

        /// <summary>
        /// Вставка наверх "списка воспроизведения" нового компонента конвейера
        /// в качестве стартового.
        /// </summary>
        /// <param name="startupMiddlewareMiddleware"></param>
        void PushMiddleware(IQueryPipelineMiddleware startupMiddlewareMiddleware);

        void ContinuePipelineWith(IQueryPipelineMiddleware middleware);

        #endregion
    }
}
