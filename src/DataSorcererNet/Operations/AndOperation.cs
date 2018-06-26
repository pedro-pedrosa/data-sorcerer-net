using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class AndOperation : BinaryOperation
    {
        public AndOperation(QueryOperation leftOperand, QueryOperation rightOperand) : base(leftOperand, rightOperand)
        {
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.And;
    }
}
