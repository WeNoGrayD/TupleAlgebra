using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider.QueryPipelineInfrastructure.Streaming
{
    public abstract class StreamingQueryExecutorWithDirectEnumerableOneToOneResult<TData>
        : StreamingQueryExecutorWithEnumerableOneToOneResult<TData, TData>
    {
        #region Constructors

        public StreamingQueryExecutorWithDirectEnumerableOneToOneResult()
            : base()
        {
            return;
        }

        #endregion

        #region Instance methods

        public override (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithPositiveCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) flags = ConsumeData(data);
            if (flags.didDataPass)
            {
                _queryResult[0] = data;
                OnDataPassed(_queryResult);
            }

            return flags;
        }

        public override (bool ResultProvided, bool MustGoOn) ExecuteOverDataInstanceHandlerWithFullCovering(TData data)
        {
            (bool didDataPass, bool mustGoOn) flags = ConsumeData(data);
            _queryResult[0] = data;
            if (flags.didDataPass) OnDataPassed(_queryResult);
            else OnDataNotPassed(_queryResult);

            return flags;
        }

        #endregion
    }
}
