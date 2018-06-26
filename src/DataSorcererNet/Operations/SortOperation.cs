using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class SortOperation : CollectionOperation
    {
        public SortOperation(QueryOperation source, IEnumerable<SortOperationStep> steps, string parameterName) : base(source)
        {
            Steps = steps;
            ParameterName = parameterName;
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.Sort;
        public IEnumerable<SortOperationStep> Steps { get; private set; }
        public string ParameterName { get; private set; }
    }
    public class SortOperationStep
    {
        public SortOperationStep(QueryOperation sortBy, bool ascending)
        {
            SortBy = sortBy;
            Ascending = ascending;
        }
        public QueryOperation SortBy { get; private set; }
        public bool Ascending { get; private set; }
    }
}
