using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class LiteralOperation : QueryOperation
    {
        public LiteralOperation(object value) : base()
        {
            Value = value;
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.Literal;
        public object Value { get; private set; }
    }
}
