using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Schema
{
    public abstract class SchemaNodeNumber<T> : SchemaNode where T : struct
    {
        public SchemaNodeNumber() : this(null, null)
        {

        }
        public SchemaNodeNumber(T? maxValue, T? minValue)
        {
            MaxValue = maxValue;
            MinValue = minValue;
        }
        public T? MaxValue { get; private set; }
        public T? MinValue { get; private set; }
    }
}
