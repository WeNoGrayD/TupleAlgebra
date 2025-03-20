using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraFrameworkTests.AttributeComponentTests
{
    public abstract class FiniteEnumerableNonFictionalAttributeComponentTests<
        TData,
        TOperandFactory>
        : AttributeComponentTests<
            TData,
            IEnumerable<TData>,
            IEnumerable<TData>,
            TOperandFactory>
        where TOperandFactory : INonFictionalAttributeComponentFactory<
            TData,
            IEnumerable<TData>>
    {
        protected abstract IEnumerable<TData> DomainValues { get; }

        protected override IEnumerable<TData> GetEmptyValues()
        {
            return [];
        }

        protected override IEnumerable<TData> GetFullValues()
        {
            return DomainValues;
        }

        protected override bool OperationResultEquals(
            IEnumerable<TData> resultPattern,
            IAttributeComponent<TData> result)
        {
            return EqualPredefined(resultPattern, result);
        }

        protected override AttributeComponent<TData> 
            CreateNonFictionalOperand2(IEnumerable<TData> values)
        {
            return OperandFactory.CreateNonFictional(values);
        }

        #region Predefined operation methods

        protected override IEnumerable<TData> ComplementionPredefined(
            IEnumerable<TData> first)
        {
            HashSet<TData> domain = DomainValues.ToHashSet();
            domain.ExceptWith(first);

            return domain;
        }

        protected override IEnumerable<TData> IntersectPredefined(
            IEnumerable<TData> first,
            IEnumerable<TData> second)
        {
            HashSet<TData> firstHS = first.ToHashSet();
            firstHS.IntersectWith(second);

            return firstHS;
        }

        protected override IEnumerable<TData> UnionPredefined(
            IEnumerable<TData> first,
            IEnumerable<TData> second)
        {
            HashSet<TData> firstHS = first.ToHashSet();
            firstHS.UnionWith(second);

            return firstHS;
        }

        protected override IEnumerable<TData> ExceptionPredefined(
            IEnumerable<TData> first,
            IEnumerable<TData> second)
        {
            HashSet<TData> firstHS = first.ToHashSet();
            firstHS.ExceptWith(second);

            return firstHS;
        }

        protected override IEnumerable<TData> SymmetricExceptionPredefined(
            IEnumerable<TData> first,
            IEnumerable<TData> second)
        {
            HashSet<TData> firstHS = first.ToHashSet();
            firstHS.SymmetricExceptWith(second);

            return firstHS;
        }

        protected override bool IncludePredefined(
            IEnumerable<TData> first,
            IEnumerable<TData> second)
        {
            return first.ToHashSet().IsProperSupersetOf(second);
        }

        protected override bool EqualPredefined(
            IEnumerable<TData> first,
            IEnumerable<TData> second)
        {
            return first.ToHashSet().SetEquals(second);
        }

        protected override bool IncludeOrEqualPredefined(
            IEnumerable<TData> first,
            IEnumerable<TData> second)
        {
            return first.ToHashSet().IsSupersetOf(second);
        }

        #endregion
    }
}
