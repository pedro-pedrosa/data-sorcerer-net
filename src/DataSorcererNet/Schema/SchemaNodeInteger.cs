using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Schema
{
    public class SchemaNodeInteger : SchemaNodeNumber<int>
    {
        public SchemaNodeInteger() : this(null, null)
        {

        }
        public SchemaNodeInteger(int? maxValue, int? minValue) : base(maxValue, minValue)
        {

        }
        public override SchemaNodeKind Kind => SchemaNodeKind.Integer;
    }
}
