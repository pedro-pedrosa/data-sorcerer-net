using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Schema
{
    public class SchemaNodeBoolean : SchemaNode
    {
        public SchemaNodeBoolean() : this(SchemaNodeBooleanFormat.Checkbox)
        {

        }
        public SchemaNodeBoolean(SchemaNodeBooleanFormat format)
        {
            Format = format;
        }
        public override SchemaNodeKind Kind => SchemaNodeKind.Boolean;

        public SchemaNodeBooleanFormat Format { get; private set; }
    }
    public enum SchemaNodeBooleanFormat
    {
        Checkbox,
        YesNo
    }
}
