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
        private Type _dataSourceType;

        private object _originalDataSource;
        protected object _dataSource;

        private bool _isResultEnumerable;

        private List<IQueryPipelineExecutorAcceptor> _pipeline;

        protected QueryPipelineExecutor(
            Type dataSourceType,
            object dataSource,
            bool isResultEnumerable)
        {
            _dataSourceType = dataSourceType;
            _originalDataSource = dataSource;
            _dataSource = dataSource;
            _isResultEnumerable = isResultEnumerable;
            _pipeline = new List<IQueryPipelineExecutorAcceptor>();
        }

        private IEnumerable<TData> GetDataSource<TData>()
        {
            return _dataSource as IEnumerable<TData>;
        }

        private void SetDataSource<TData>(IEnumerable<TData> dataSource)
        {
            _dataSource = dataSource;

            return;
        }

        public void AddSingleQueryExecutor(
            IQueryPipelineExecutorAcceptor queryExecutor)
        {
            _pipeline.Add(queryExecutor);
        }

        protected abstract IQueryable ProduceQueryResult<TData, TQueryResultData>(
                IEnumerable<TData> dataSource,
                IEnumerable<TQueryResultData> resultData);

        public TQueryPipelineResult ExecutePipeline<TQueryPipelineResult>()
        {
            foreach (IQueryPipelineExecutorAcceptor singleQueryExecutor in _pipeline)
                singleQueryExecutor.Accept(this);

            return _isResultEnumerable ?
                (TQueryPipelineResult)ProduceQueryResult(
                    (dynamic)_originalDataSource, (dynamic)_dataSource) :
                (_dataSource as TQueryPipelineResult[]).SingleOrDefault();
        }

        public void VisitWholeDataSourceReader<TData, TQueryResult>(
            WholeDataSourceReader<TData, TQueryResult> reader)
        {
            foreach (TData data in GetDataSource<TData>())
            {
                if (reader.Predicate(data))
                    reader.PutData(data);
            }

            TQueryResult queryResult = reader.GetResult();
            SetDataSource(queryResult as IEnumerable<TData>);
            if (_dataSource is null)
                _dataSource = new TQueryResult[] { queryResult };

            return;
        }

        public void VisitEveryDataInstanceReader<TData, TQueryResult>(
            EveryDataInstanceReader<TData, TQueryResult> reader)
        {
            foreach (TData data in GetDataSource<TData>())
            {
                if (reader.Predicate(data))
                {
                    reader.PutData(data);
                    if (reader.IsAlreadyOver())
                        break;
                }
            }

            TQueryResult queryResult = reader.GetResult();
            SetDataSource(queryResult as IEnumerable<TData>);
            if (_dataSource is null)
                _dataSource = new TQueryResult[] { queryResult };

            return;
        }
    }

    public abstract class QueryPipelineExecutor<TData>
        : ISingleQueryExecutorVisitor<TData>
    {
        public abstract void AddSingleQueryExecutor(
            IQueryPipelineExecutorAcceptor<TData> queryExecutor);

        public abstract TQueryPipelineResult ExecutePipeline<TQueryPipelineResult>();

        public abstract void VisitWholeDataSourceReader<TQueryResult>(
            WholeDataSourceReader<TData, TQueryResult> reader);

        public abstract void VisitEveryDataInstanceReader<TQueryResult>(
            EveryDataInstanceReader<TData, TQueryResult> reader);
    }
}
