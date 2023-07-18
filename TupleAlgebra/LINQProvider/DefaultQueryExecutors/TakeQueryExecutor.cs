﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Streaming;

namespace LINQProvider.DefaultQueryExecutors
{
    public class TakeStreamingQueryExecutor<TData> : StreamingQueryExecutorWithEnumerableOneToOneResult<TData, TData>
    {
        private int _tookCount;

        public TakeStreamingQueryExecutor(int takingCount)
            : base(null, (TData data) => data)
        {
            InitBehavior(ExecuteOverDataInstanceHandlerWithPositiveCovering);
            _tookCount = takingCount;
        }

        protected override (bool DidDataPass, bool MustGoOn) ConsumeData(TData data)
        {
            return (_tookCount > 0, --_tookCount > 0);
        }
    }
}
