using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class NotEqualOperation : BinaryOperation
    {
        public NotEqualOperation(QueryOperation leftOperand, QueryOperation rightOperand) : base(leftOperand, rightOperand)
        {
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.NotEqual;
    }
}
