using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet.Operations
{
    public abstract class QueryOperation
    {
        public QueryOperation()
        {
        }
        public abstract QueryOperationNodeType Operation { get; }
    }
    public enum QueryOperationNodeType
    {
        //Leaves
        Parameter,
        DataSourceReference,
        Literal,

        //Collections
        Count,
        Filter,
        Projection,
        Sort,
        TakePage,

        //Data Points
        ElementLiteral,
        FieldReference,

        //Arithmetic
        Add,
        Subtract,
        Divide,
        Multiply,
        Negate,

        //Boolean
        Equal,
        NotEqual,
        Greater,
        GreaterOrEqual,
        Less,
        LessOrEqual,
        And,
        Or,
        Not,
        If,

        //Misc
        Contains,
        StartsWith,
        EndsWith,
        ToUpperCase,
        ToLowerCase
    }
}
