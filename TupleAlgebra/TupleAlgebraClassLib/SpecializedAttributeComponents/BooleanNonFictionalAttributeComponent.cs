using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SetOperationExecutersContainers;
using System.Linq.Expressions;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.LINQ2TAFramework;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.SpecializedAttributeComponents
{
    /*
    public class BooleanNonFictionalAttributeComponent
        : NonFictionalAttributeComponent<bool>
    {
        #region Instance fields

        private bool _value;

        #endregion

        #region Constructors

        public BooleanNonFictionalAttributeComponent(
            bool value,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(queryProvider, queryExpression)
        {
            _value = value;

            return;
        }

        #endregion
    }

    /// <summary>
    /// Мощность компоненты, которая может включать 
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class AtomicNonFictionalAttributeComponentPower<TData> 
        : NonFictionalAttributeComponent<TData>.NonFictionalAttributeComponentPower
    {
        #region Instance methods

        public override bool IsZero()
        {
            return false;
        }

        public override void InitAttributeComponent(AttributeComponent<TData> component)
        {
            return;// _component = component as OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>;
        }

        protected override int CompareToSame(dynamic second)
        {
            if (second is OrderedFiniteEnumerableNonFictionalAttributeComponentPower second2)
                return this.CompareToSame(second);
            else
                throw new InvalidCastException("Непустая булевая компонента сравнивается с непустой компонентой другого вида, в данный момент эта операция не поддерживается.");
        }

        #endregion
    }
    */
}
