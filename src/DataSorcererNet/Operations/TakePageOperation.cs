using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class TakePageOperation : CollectionOperation
    {
        public TakePageOperation(QueryOperation source, int start, int count) : base(source)
        {
            Start = start;
            Count = count;
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.TakePage;
        public int Start { get; private set; }
        public int Count { get; private set; }
    }
}
