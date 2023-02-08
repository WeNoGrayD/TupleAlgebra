using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    public abstract class QueryPipelineExecutor
        : ISingleQueryExecutorVisitor
    {
        public delegate IQueryable
            ProduceNonFictionalAttributeComponent(
                IEnumerable<object> dataSource,
                IEnumerable<object> resultData);

        public event ProduceNonFictionalAttributeComponent ProduceNonFictionalAttributeComponentEvent;

        public abstract void AddSingleQueryExecutor(
            IQueryPipelineExecutorAcceptor queryExecutor);

        public abstract TQueryPipelineResult ExecutePipeline<TQueryPipelineResult>();

        public abstract void VisitWholeDataSourceReader<TData, TQueryResult>(
            WholeDataSourceReader<TData, TQueryResult> reader);

        public abstract void VisitEveryDataInstanceReader<TData, TQueryResult>(
            EveryDataInstanceReader<TData, TQueryResult> reader);

        protected IQueryable OnGetQueryPipelineResult(
                IEnumerable<object> dataSource,
                IEnumerable<object> resultData)
        {
            return ProduceNonFictionalAttributeComponentEvent(dataSource, resultData);
        }
    }
}
