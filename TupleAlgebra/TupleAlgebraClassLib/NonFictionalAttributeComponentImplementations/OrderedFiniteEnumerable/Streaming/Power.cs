﻿using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable.Streaming;

/// <summary>
/// Мощность упорядоченной конечной перечислимой компонентой аттрибута.
/// </summary>
public class StreamingOrderedFiniteEnumerableAttributeComponentPower<TData>
    : CountableEnumerableAttributeComponentPower<TData, StreamingOrderedFiniteEnumerableAttributeComponent<TData>>
{ }