using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    public class AttributeComponentQueryPipelineExecutor
        : QueryPipelineExecutor
    {
        private Type _dataSourceType;

        private IEnumerable<object> _originalDataSource;
        private IEnumerable<object> _dataSource;

        private bool _isResultEnumerable;

        private List<IQueryPipelineExecutorAcceptor> _pipeline;

        public AttributeComponentQueryPipelineExecutor(
            Type dataSourceType,
            IEnumerable<object> dataSource,
            bool isResultEnumerable)
        {
            _dataSourceType = dataSourceType;
            _originalDataSource = dataSource;
            _dataSource = dataSource.ToArray();
            _isResultEnumerable = isResultEnumerable;
            _pipeline = new List<IQueryPipelineExecutorAcceptor>();
        }

        private IEnumerable<TData> DataSource<TData>()
        {
            return _dataSource.Cast<TData>();
        }

        public override void AddSingleQueryExecutor(
            IQueryPipelineExecutorAcceptor queryExecutor)
        {
            _pipeline.Add(queryExecutor);
        }

        public override TQueryPipelineResult ExecutePipeline<TQueryPipelineResult>()
        {
            foreach (IQueryPipelineExecutorAcceptor singleQueryExecutor in _pipeline)
                singleQueryExecutor.Accept(this);

            return _isResultEnumerable ? 
                (TQueryPipelineResult)OnGetQueryPipelineResult(_originalDataSource, _dataSource) : 
                (TQueryPipelineResult)_dataSource.SingleOrDefault();
        }

        public override void VisitWholeDataSourceReader<TData, TQueryResult>(
            WholeDataSourceReader<TData, TQueryResult> reader)
        {
            foreach (TData data in DataSource<TData>())
            {
                if (reader.Predicate(data))
                    reader.PutData(data);
            }

            TQueryResult queryResult = reader.GetResult();
            _dataSource = queryResult as IEnumerable<object> ?? 
                new TQueryResult[] { queryResult } as IEnumerable<object>;

            return;
        }

        public override void VisitEveryDataInstanceReader<TData, TQueryResult>(
            EveryDataInstanceReader<TData, TQueryResult> reader)
        {
            foreach (TData data in DataSource<TData>())
            {
                if (reader.Predicate(data))
                {
                    reader.PutData(data);
                    if (reader.IsAlreadyOver())
                        break;
                }
            }

            TQueryResult queryResult = reader.GetResult();
            _dataSource = queryResult as IEnumerable<object> ?? 
                new TQueryResult[] { queryResult } as IEnumerable<object>;

            return;
        }
    }
}
