using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Schema
{
    public class SchemaNodeChoice : SchemaNode
    {
        public SchemaNodeChoice(IEnumerable<string> choices) : this(choices, null)
        {

        }
        public SchemaNodeChoice(IEnumerable<string> choices, bool? multi)
        {
            Choices = new List<string>(choices).AsReadOnly();
            Multi = multi;
        }
        public override SchemaNodeKind Kind => SchemaNodeKind.Choice;
        
        public IEnumerable<string> Choices { get; private set; }
        public bool? Multi { get; set; }
    }
}
