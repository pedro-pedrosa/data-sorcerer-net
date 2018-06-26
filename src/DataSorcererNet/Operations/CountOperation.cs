using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class CountOperation : CollectionOperation
    {
        public CountOperation(QueryOperation source) : base(source)
        {
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.Count;
    }
}
