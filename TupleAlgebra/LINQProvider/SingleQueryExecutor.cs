using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure;
using LINQProvider.QueryResultAccumulatorInfrastructure;

namespace LINQProvider
{
    [Flags]
    public enum QuerySourceToResultMiltiplicity : byte
    {
        ManyToOne = 1,
        OneToOne = 2,
        OneToMany = 6
    }

    /// <summary>
    /// Исполнитель запроса.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResult"></typeparam>
    public abstract class SingleQueryExecutor<TData, TQueryResult>
    {
        #region Instance properties

        /// <summary>
        /// Кратность источника данных запроса и его результат.
        /// </summary>
        public abstract QuerySourceToResultMiltiplicity Multiplicity { get; } 

        #endregion

        #region Constructors

        public SingleQueryExecutor()
        {
            return;
        }

        #endregion
    }
}
