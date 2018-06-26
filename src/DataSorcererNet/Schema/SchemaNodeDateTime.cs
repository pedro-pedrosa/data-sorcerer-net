using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Schema
{
    public class SchemaNodeDateTime : SchemaNode
    {
        public SchemaNodeDateTime() : this(SchemaNodeDateTimeFormat.DateTime, null, null)
        {

        }
        public SchemaNodeDateTime(SchemaNodeDateTimeFormat format, DateTime? maxValue, DateTime? minValue)
        {
            Format = format;
            MaxValue = maxValue;
            MinValue = minValue;
        }
        public override SchemaNodeKind Kind => SchemaNodeKind.DateTime;

        public SchemaNodeDateTimeFormat Format { get; private set; }
        public DateTime? MaxValue { get; private set; }
        public DateTime? MinValue { get; private set; }
    }
    public enum SchemaNodeDateTimeFormat
    {
        DateTime,
        Date,
        Time
    }
}
