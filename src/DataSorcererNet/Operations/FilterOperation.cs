using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class FilterOperation : CollectionOperation
    {
        public FilterOperation(QueryOperation source, QueryOperation predicate, string parameterName) : base(source)
        {
            Predicate = predicate;
            ParameterName = parameterName;
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.Filter;
        public QueryOperation Predicate { get; private set; }
        public string ParameterName { get; private set; }
    }
}
