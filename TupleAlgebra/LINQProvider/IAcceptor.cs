using LINQProvider.QueryPipelineInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider
{
    public interface IAcceptor<TVisitor>
    {
        void Accept(TVisitor visitor);
    }
}
