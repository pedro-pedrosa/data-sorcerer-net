using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Schema
{
    public class SchemaNodeDecimal : SchemaNodeDecimalBase
    {
        public SchemaNodeDecimal() : this(null, null, null)
        {

        }
        public SchemaNodeDecimal(decimal? maxValue, decimal? minValue, bool? showAsPercent) : base(maxValue, minValue)
        {
            ShowAsPercent = showAsPercent;
        }
        public override SchemaNodeKind Kind => SchemaNodeKind.Decimal;

        public bool? ShowAsPercent { get; private set; }
    }
}
