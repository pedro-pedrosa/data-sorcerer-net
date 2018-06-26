using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class ContainsOperation : QueryOperation
    {
        public ContainsOperation(QueryOperation source, QueryOperation search) : base()
        {
            Source = source;
            Search = search;
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.Contains;
        public QueryOperation Source { get; private set; }
        public QueryOperation Search { get; private set; }
    }
}
