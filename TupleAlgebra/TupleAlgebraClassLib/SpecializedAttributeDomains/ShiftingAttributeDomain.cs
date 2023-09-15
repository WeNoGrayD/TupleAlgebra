using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.SpecializedAttributeDomains
{
    /// <summary>
    /// Домен атрибута с поддержкой операторов сдвига.
    /// Возможно, будут добавлены опции для более удобной работы с перетасовкой доменов.
    /// Пока что в долгом ящике.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TShiftedData"></typeparam>
    public class ShiftingAttributeDomain<TData, TShiftedData> : AttributeDomain<TData>
    {
        public ShiftingAttributeDomain(AttributeDomain<TData> content)
            : base()
        {
            Universum = content.Universum;

            return;
        }

        public static ShiftingAttributeDomain<TShiftedData, TData> operator >>(
            ShiftingAttributeDomain<TData, TShiftedData> domain,
            Expression<Func<TData, TShiftedData>> itemSelector)
        {
            return new ShiftingAttributeDomain<TShiftedData, TData>(domain.Shift(itemSelector));
        }

        public static ShiftingAttributeDomain<TShiftedData, TData> operator >>(
            ShiftingAttributeDomain<TData, TShiftedData> domain,
            Expression<Func<TData, IEnumerable<TShiftedData>>> itemsSelector)
        {
            return new ShiftingAttributeDomain<TShiftedData, TData>(domain.ShiftMany(itemsSelector));
        }
    }
}
