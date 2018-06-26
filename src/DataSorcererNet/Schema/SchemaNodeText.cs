using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataSorcererNet.Schema
{
    public class SchemaNodeText : SchemaNode
    {
        public SchemaNodeText() : this(null, null)
        {

        }
        public SchemaNodeText(uint? maxLength, bool? isUnicode)
        {
            MaxLength = maxLength;
            IsUnicode = isUnicode;
        }
        public uint? MaxLength { get; private set; }
        public bool? IsUnicode { get; private set; }

        public override SchemaNodeKind Kind => SchemaNodeKind.Text;
    }

}
