using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib
{
    public interface IReproducingQueryable<TData> : IQueryable<TData>
    {
        IReproducingQueryable<TReproducedData> Reproduce<TReproducedData>(
            IEnumerable<TReproducedData> reproducedData,
            bool includeDomain = false);
    }

    public interface IReproducingQueryable<TFactoryArgs, TData> : IQueryable<TData>
    {
        IReproducingQueryable<TReproducedData> Reproduce<TReproducedData>(
            TFactoryArgs reproducedData);
    }
}
