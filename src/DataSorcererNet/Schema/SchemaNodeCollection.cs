using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Schema
{
    public class SchemaNodeCollection : SchemaNode
    {
        public SchemaNodeCollection(SchemaNode elementSchema) : base()
        {
            ElementSchema = elementSchema;
        }
        public override SchemaNodeKind Kind => SchemaNodeKind.Collection;

        public SchemaNode ElementSchema { get; private set; }
    }
}
