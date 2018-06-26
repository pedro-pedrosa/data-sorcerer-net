using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public class ParameterOperation : QueryOperation
    {
        public ParameterOperation(string name) : base()
        {
            Name = name;
        }

        public override QueryOperationNodeType Operation => QueryOperationNodeType.Parameter;
        public string Name { get; private set; }
    }
}
