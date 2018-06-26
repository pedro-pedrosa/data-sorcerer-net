using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class StartsWithOperation : QueryOperation
    {
        public StartsWithOperation(QueryOperation source, QueryOperation fragment) : base()
        {
            Source = source;
            Fragment = fragment;
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.StartsWith;
        public QueryOperation Source { get; private set; }
        public QueryOperation Fragment { get; private set; }
    }
}
