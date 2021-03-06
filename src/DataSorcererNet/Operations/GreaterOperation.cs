﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class GreaterOperation : BinaryOperation
    {
        public GreaterOperation(QueryOperation leftOperand, QueryOperation rightOperand) : base(leftOperand, rightOperand)
        {
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.Greater;
    }
}
