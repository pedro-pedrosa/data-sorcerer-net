using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public abstract class BinaryOperation : QueryOperation
    {
        public BinaryOperation(QueryOperation leftOperand, QueryOperation rightOperand) : base()
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
        }

        public QueryOperation LeftOperand { get; private set; }
        public QueryOperation RightOperand { get; private set; }
    }
}
