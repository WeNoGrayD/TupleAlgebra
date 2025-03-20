using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Iterable
{
    using static AttributeComponentHelper;

    public class IterableAttributeComponent<TData>
        : SequenceBasedNonFictionalAttributeComponent<TData, IEnumerable<TData>>
    {
        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="values"></param>
        /// <param name="queryExpression"></param>
        public IterableAttributeComponent(
            AttributeComponentPower power,
            IEnumerable<TData> values,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(power,
                   queryProvider ?? new UnorderedFiniteEnumerableAttributeComponentQueryProvider(),
                   queryExpression)
        {
            Values = values;

            return;
        }

        #endregion

        #region Instance methods

        public override AttributeComponentFactoryArgs ZipInfoImpl<TReproducedData>(
            IEnumerable<TReproducedData> populatingData)
        {
            return new UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TReproducedData>(
                populatingData as HashSet<TReproducedData>);//,
                                                            //this.Provider as OrderedFiniteEnumerableAttributeComponentQueryProvider);
        }

        /*
        protected override sealed AttributeComponent<TReproducedData> ReproduceImpl<TReproducedData>(
            AttributeComponentFactoryArgs factoryArgs)
        {
            return _setOperations.Produce<TReproducedData>(factoryArgs);
        }
        */

        /// <summary>
        /// Перечисление значений компоненты.
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<TData> GetEnumeratorImpl()
        {
            return Values.GetEnumerator();
        }

        #endregion
    }
}
