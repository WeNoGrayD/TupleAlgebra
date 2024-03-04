using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable.Buffered;

/// <summary>
/// Мощность упорядоченной конечной перечислимой компонентой аттрибута.
/// </summary>
public class BufferedOrderedFiniteEnumerableAttributeComponentPower<TData>
    : CountableEnumerableAttributeComponentPower<TData, BufferedOrderedFiniteEnumerableAttributeComponent<TData>>
{ }
