using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class FieldReferenceOperation : QueryOperation
    {
        public FieldReferenceOperation(QueryOperation element, string fieldName) : base()
        {
            Element = element;
            FieldName = fieldName;
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.FieldReference;
        public QueryOperation Element { get; private set; }
        public string FieldName { get; private set; }
    }
}
