using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class DataSourceReferenceOperation : QueryOperation
    {
        public DataSourceReferenceOperation() : base()
        {
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.DataSourceReference;
    }
}
