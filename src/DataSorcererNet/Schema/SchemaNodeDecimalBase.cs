using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Schema
{
    public abstract class SchemaNodeDecimalBase : SchemaNodeNumber<decimal>
    {
        public SchemaNodeDecimalBase() : this(null, null)
        {

        }
        public SchemaNodeDecimalBase(decimal? maxValue, decimal? minValue) : base(maxValue, minValue)
        {

        }
    }
}
