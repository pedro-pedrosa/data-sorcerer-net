using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class ElementLiteralOperation : QueryOperation
    {
        public ElementLiteralOperation(IEnumerable<ElementLiteralOperationField> fields) : base()
        {
            Fields = fields;
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.ElementLiteral;
        public IEnumerable<ElementLiteralOperationField> Fields { get; private set; }
    }
    public class ElementLiteralOperationField
    {
        public ElementLiteralOperationField(string name, QueryOperation value)
        {
            Name = name;
            Value = value;
        }
        public string Name{ get; private set; }
        public QueryOperation Value { get; private set; }
    }
}
