using DataSorcererNet.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataSorcererNet
{
    public abstract class ElementBase : IElement
    {
        public ElementBase(SchemaNodeComplex schema)
        {
            Schema = schema;
        }

        public SchemaNodeComplex Schema { get; private set; }
        public abstract object Get(string fieldName);
    }
}
