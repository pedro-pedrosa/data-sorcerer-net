﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class LessOperation : BinaryOperation
    {
        public LessOperation(QueryOperation leftOperand, QueryOperation rightOperand) : base(leftOperand, rightOperand)
        {
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.Less;
    }
}
