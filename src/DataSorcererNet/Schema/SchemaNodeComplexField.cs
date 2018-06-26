using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Schema
{
    public class SchemaNodeComplexField
    {
        public SchemaNodeComplexField(SchemaNode schema, string name, string title, string description, bool isNullable)
        {
            Schema = schema;
            Name = name;
            Title = title;
            Description = description;
            IsNullable = isNullable;
        }
        public SchemaNode Schema { get; private set; }
        public string Name { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsNullable { get; set; }
    }
}
