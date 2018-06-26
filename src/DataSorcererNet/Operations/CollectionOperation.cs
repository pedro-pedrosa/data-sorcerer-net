using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public abstract class CollectionOperation : QueryOperation
    {
        public CollectionOperation(QueryOperation source) : base()
        {
            Source = source;
        }

        public QueryOperation Source { get; private set; }
    }
}
