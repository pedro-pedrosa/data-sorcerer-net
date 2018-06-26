using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public abstract class UnaryOperation : QueryOperation
    {
        public UnaryOperation(QueryOperation operand) : base()
        {
            Operand = operand;
        }

        public QueryOperation Operand { get; private set; }
    }
}
