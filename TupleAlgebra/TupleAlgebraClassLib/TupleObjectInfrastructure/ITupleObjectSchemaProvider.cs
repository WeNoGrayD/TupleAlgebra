﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    public interface ITupleObjectAttributeInfo
    {
        bool IsPlugged { get; }
    }

    public interface ITupleObjectSchemaProvider
    {
        ITupleObjectAttributeInfo this[string attributeName] { get; set; }
    }
}
