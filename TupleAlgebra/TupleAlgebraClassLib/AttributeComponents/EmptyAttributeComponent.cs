using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework;
using System.Linq.Expressions;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.AttributeComponents
{
    using static AttributeComponentHelper;

    public sealed class EmptyAttributeComponent<TData> 
        : AttributeComponent<TData>
    {
        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static EmptyAttributeComponent()
        {
            Helper.RegisterType<TData, EmptyAttributeComponent<TData>>(
                setOperations: (_) => new EmptyAttributeComponentOperationExecutorsContainer());

            return;
        }

        public EmptyAttributeComponent(
            EmptyAttributeComponentPower power,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(power, queryProvider, queryExpression)
        {
            return;
        }

        #endregion

        #region Instance methods

        public override AttributeComponentFactoryArgs ZipInfoImpl<TReproducedData>(IEnumerable<TReproducedData> populatingData)
        {
            return new EmptyAttributeComponentFactoryArgs();
        }

        public override IEnumerator<TData> GetEnumeratorImpl()
        {
            yield break;
        }

        protected override AttributeComponent<TReproducedData> ReproduceImpl<TReproducedData>(
            AttributeComponentFactoryArgs factoryArgs)
        {
            return null;
            //return Factory.CreateEmpty<TReproducedData>(factoryArgs);
        }

        #endregion

        #region Nested types

        private class EmptyAttributeComponentOperationExecutorsContainer
            : InstantAttributeComponentOperationExecutorsContainer<EmptyAttributeComponent<TData>>
        {
            public EmptyAttributeComponentOperationExecutorsContainer() : base(
                () => new EmptyAttributeComponentComplementationOperator<TData>(),
                () => new EmptyAttributeComponentIntersectionOperator<TData>(),
                () => new EmptyAttributeComponentUnionOperator<TData>(),
                () => new EmptyAttributeComponentExceptionOperator<TData>(),
                () => new EmptyAttributeComponentSymmetricExceptionOperator<TData>(),
                () => new EmptyAttributeComponentInclusionComparer<TData>(),
                () => new EmptyAttributeComponentEqualityComparer<TData>(),
                () => new EmptyAttributeComponentInclusionOrEqualityComparer<TData>())
            { }
        }

        #endregion
    }
}
