using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.Filtering;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.Default;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Iterable.Finite;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering
{
    using static AttributeComponentHelper;

    public class FilteringAttributeComponent<TData>
        : NonFictionalAttributeComponent<TData>
    {
        #region Instance fields

        private Expression<Func<TData, bool>> _predicateExpression;

        private Lazy<Func<TData, bool>> _predicate;

        #endregion

        #region Instance properties

        public Expression<Func<TData, bool>> PredicateExpression
        { get => _predicateExpression; }

        public Func<TData, bool> Predicate 
        { get => _predicate.Value; }

        #endregion

        #region Constructors

        static FilteringAttributeComponent()
        {
            Helper.RegisterType<TData, FilteringAttributeComponent<TData>>(
                acFactory: (domain) => new FilteringAttributeComponentFactory<TData>(domain),
                setOperations: (factory) => new FilteringAttributeComponentOperationExecutorsContainer((IFilteringAttributeComponentFactory<TData>)factory));

            return;
        }

        public FilteringAttributeComponent(
            AttributeComponentPower power,
            Expression<Func<TData, bool>> predicateExpression,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(
                  power,
                  queryProvider ?? new DefaultAttributeComponentQueryProvider(),
                  queryExpression)
        {
            _predicateExpression = predicateExpression;
            _predicate = new Lazy<Func<TData, bool>>(
                _predicateExpression.Compile);

            return;
        }

        #endregion

        #region Instance methods

        public override AttributeComponentFactoryArgs ZipInfoImpl<TReproducedData>(
            IEnumerable<TReproducedData> populatingData)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<TData> GetEnumeratorImpl()
        {
            return ToIterableAttributeComponent().GetEnumerator();
        }

        public IAttributeComponent<TData> ToIterableAttributeComponent()
        {
            return Domain & this;
        }

        #endregion

        #region Nested types

        private class FilteringAttributeComponentOperationExecutorsContainer
            : NonFictionalAttributeComponentOperationExecutorsContainer<
                FilteringAttributeComponent<TData>,
                FilteringAttributeComponentFactoryArgs<TData>,
                IFilteringAttributeComponentFactory<TData>,
                FilteringAttributeComponentFactoryArgs<TData>>
        {
            #region Constructors

            public FilteringAttributeComponentOperationExecutorsContainer(
                IFilteringAttributeComponentFactory<TData> factory)
                : base(
                      factory,
                      () => new ComplementionOperator<TData>(),
                      () => new IntersectionOperator<TData>(),
                      () => new UnionOperator<TData>(),
                      () => new ExceptionOperator<TData>(),
                      () => new SymmetricExceptionOperator<TData>(),
                      () => new InclusionComparer<TData>(),
                      () => new EqualityComparer<TData>(),
                      () => new InclusionOrEqualityComparer<TData>())
            { }

            #endregion
        }

        #endregion
    }
}
