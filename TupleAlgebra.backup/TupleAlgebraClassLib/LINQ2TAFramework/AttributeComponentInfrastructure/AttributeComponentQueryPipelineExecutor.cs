using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    public class AttributeComponentQueryPipelineExecutor2 : QueryPipelineExecutor
    {
        private IReproducingQueryable<TQueryResult> ProduceQueryResultImpl<TData, TQueryResult>(
                AttributeComponent<TData> dataSource,
                IEnumerable<TQueryResult> resultData)
        {
            return dataSource.Reproduce(resultData);
        }

        protected override IQueryable ProduceQueryResult<TData, TQueryResultData>(
                IEnumerable<TData> dataSource,
                IEnumerable<TQueryResultData> resultData)
        {
            //if (!(resultData is AttributeComponent<TData>))
            //    throw new ArgumentException("");

            return ProduceQueryResultImpl(
                dataSource as AttributeComponent<TData>,
                resultData);
        }

        protected AttributeComponentQueryPipelineExecutor2(
            Type dataSourceType,
            object dataSource,
            bool isResultEnumerable)
            : base(dataSourceType, dataSource, isResultEnumerable)
        { }
        
        public static QueryPipelineExecutor Construct<TData>(
            Type dataSourceType,
            IEnumerable<TData> dataSource,
            bool isResultEnumerable)
        {
            if (!(dataSource is AttributeComponent<TData>))
                throw new ArgumentException("Источник данных не является компонентой атрибута.");

            return new AttributeComponentQueryPipelineExecutor2(
                dataSourceType, 
                dataSource, 
                isResultEnumerable);
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
    }

    public class AttributeComponentQueryPipelineExecutor<TData>
        : QueryPipelineExecutor<TData>
    {
        private Type _dataSourceType;

        private IEnumerable<TData> _originalDataSource;
        private object _dataSource;
        private IEnumerable<TData> DataSource
        {
            get => _dataSource as IEnumerable<TData>;
            set => _dataSource = value;
        }

        private bool _isResultEnumerable;

        private List<IQueryPipelineExecutorAcceptor<TData>> _pipeline;

        public delegate IQueryable
            ProduceNonFictionalAttributeComponent(
                IEnumerable<TData> dataSource,
                IEnumerable<TData> resultData);

        public event ProduceNonFictionalAttributeComponent ProduceNonFictionalAttributeComponentEvent;

        public AttributeComponentQueryPipelineExecutor(
            Type dataSourceType,
            AttributeComponent<TData> dataSource,
            bool isResultEnumerable)
        {
            _dataSourceType = dataSourceType;
            _originalDataSource = dataSource;
            _dataSource = dataSource;
            _isResultEnumerable = isResultEnumerable;
            _pipeline = new List<IQueryPipelineExecutorAcceptor<TData>>();
        }

        /*
        private IEnumerable<TData> DataSource<TData>()
        {
            return _dataSource.Cast<TData>();
        }
        */

        public override void AddSingleQueryExecutor(
            IQueryPipelineExecutorAcceptor<TData> queryExecutor)
        {
            _pipeline.Add(queryExecutor);
        }

        public override TQueryPipelineResult ExecutePipeline<TQueryPipelineResult>()
        {
            foreach (IQueryPipelineExecutorAcceptor<TData> singleQueryExecutor in _pipeline)
                singleQueryExecutor.Accept(this);

            return _isResultEnumerable ? 
                (TQueryPipelineResult)ProduceNonFictionalAttributeComponentEvent(
                    _originalDataSource, 
                    DataSource) : 
                (_dataSource as TQueryPipelineResult[]).SingleOrDefault();
        }

        public override void VisitWholeDataSourceReader<TQueryResult>(
            WholeDataSourceReader<TData, TQueryResult> reader)
        {
            foreach (TData data in DataSource)
            {
                if (reader.Predicate(data))
                    reader.PutData(data);
            }

            TQueryResult queryResult = reader.GetResult();
            DataSource = queryResult as IEnumerable<TData>;
            if (_dataSource is null)
                _dataSource = new TQueryResult[] { queryResult };

            return;
        }

        public override void VisitEveryDataInstanceReader<TQueryResult>(
            EveryDataInstanceReader<TData, TQueryResult> reader)
        {
            foreach (TData data in DataSource)
            {
                if (reader.Predicate(data))
                {
                    reader.PutData(data);
                    if (reader.IsAlreadyOver())
                        break;
                }
            }

            TQueryResult queryResult = reader.GetResult();
            DataSource = queryResult as IEnumerable<TData>;
            if (_dataSource is null)
                _dataSource = new TQueryResult[] { queryResult };

            return;
        }
    }
}
