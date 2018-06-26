using DataSorcererNet.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet
{
    public interface IElement
    {
        SchemaNodeComplex Schema { get; }
        object Get(string fieldName);
    }
}
