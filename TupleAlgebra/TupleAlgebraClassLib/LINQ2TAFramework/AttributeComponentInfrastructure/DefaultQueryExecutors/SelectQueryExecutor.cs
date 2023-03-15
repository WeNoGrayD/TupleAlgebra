﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.DefaultQueryExecutors
{
    public class SelectBufferingQueryExecutor<TData, TQueryResultData> 
        : BufferingQueryExecutorWithEnumerableResult<TData, TQueryResultData>
    {
        private Func<TData, TQueryResultData> _transform;

        public SelectBufferingQueryExecutor(Func<TData, TQueryResultData> transform)
            : base((_) => true)
        {
            _transform = transform;
        }

        protected override IEnumerable<TQueryResultData> TraverseOverDataSource()
        {
            foreach (TData data in DataSource)
                yield return _transform(data);
        }
    }
    public class SelectStreamingQueryExecutor<TData, TQueryResultData> 
        : StreamingQueryExecutorWithEnumerableResult<TData, TQueryResultData>
    {
        public SelectStreamingQueryExecutor(Func<TData, TQueryResultData> transform)
            : base(null, (TData data, bool didDataPass) => transform(data))
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data) => (true, true);
    }
}
