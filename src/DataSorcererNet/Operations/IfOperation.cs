using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class IfOperation : QueryOperation
    {
        public IfOperation(QueryOperation condition, QueryOperation trueOperation, QueryOperation falseOperation) : base()
        {
            Condition = condition;
            TrueOperation = trueOperation;
            FalseOperation = falseOperation;
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.If;
        public QueryOperation Condition { get; private set; }
        public QueryOperation TrueOperation { get; private set; }
        public QueryOperation FalseOperation { get; private set; }
    }
}
