﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class EqualOperation : BinaryOperation
    {
        public EqualOperation(QueryOperation leftOperand, QueryOperation rightOperand) : base(leftOperand, rightOperand)
        {
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.Equal;
    }
}
