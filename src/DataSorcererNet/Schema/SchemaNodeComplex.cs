using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Schema
{
    public class SchemaNodeComplex : SchemaNode
    {
        public SchemaNodeComplex(IEnumerable<SchemaNodeComplexField> fields) : base()
        {
            Fields = fields;
        }
        public override SchemaNodeKind Kind => SchemaNodeKind.Complex;

        public IEnumerable<SchemaNodeComplexField> Fields { get; private set; }
    }
}
