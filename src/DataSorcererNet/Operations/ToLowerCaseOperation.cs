using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class ToLowerCaseOperation : QueryOperation
    {
        public ToLowerCaseOperation(QueryOperation text) : base()
        {
            Text = text;
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.ToLowerCase;
        public QueryOperation Text { get; private set; }
    }
}
