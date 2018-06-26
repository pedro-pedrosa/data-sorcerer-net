using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class NegateOperation : UnaryOperation
    {
        public NegateOperation(QueryOperation operand) : base(operand)
        {
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.Negate;
    }
}
