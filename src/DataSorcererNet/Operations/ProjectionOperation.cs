using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class ProjectionOperation : CollectionOperation
    {
        public ProjectionOperation(QueryOperation source, QueryOperation projection, string parameterName) : base(source)
        {
            Projection = projection;
            ParameterName = parameterName;
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.Projection;
        public QueryOperation Projection { get; private set; }
        public string ParameterName { get; private set; }
    }
}
