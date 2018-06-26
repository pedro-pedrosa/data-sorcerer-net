using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Schema
{
    public class SchemaNodeCurrency : SchemaNodeDecimalBase
    {
        public SchemaNodeCurrency()
        {

        }
        public SchemaNodeCurrency(decimal? maxValue, decimal? minValue, int? lcid) : base(maxValue, minValue)
        {
            Lcid = lcid;
        }
        public override SchemaNodeKind Kind => SchemaNodeKind.Currency;

        public int? Lcid { get; private set; }
    }
}
