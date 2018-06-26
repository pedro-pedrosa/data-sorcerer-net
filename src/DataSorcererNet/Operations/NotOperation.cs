using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class NotOperation : UnaryOperation
    {
        public NotOperation(QueryOperation operand) : base(operand)
        {
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.Not;
    }
}
