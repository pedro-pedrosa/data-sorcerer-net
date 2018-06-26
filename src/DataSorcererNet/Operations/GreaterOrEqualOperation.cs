using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class GreaterOrEqualOperation : BinaryOperation
    {
        public GreaterOrEqualOperation(QueryOperation leftOperand, QueryOperation rightOperand) : base(leftOperand, rightOperand)
        {
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.GreaterOrEqual;
    }
}
