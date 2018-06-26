using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public abstract class ToUpperCaseOperation : QueryOperation
    {
        public ToUpperCaseOperation(QueryOperation text) : base()
        {
            Text = text;
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.ToUpperCase;
        public QueryOperation Text { get; private set; }
    }
}
