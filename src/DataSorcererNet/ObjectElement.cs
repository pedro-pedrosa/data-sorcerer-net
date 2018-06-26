using DataSorcererNet.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataSorcererNet
{
    public class ObjectElement : ElementBase
    {
        private object _value;
        private ObjectElementPropertyResolver _namesResolver;
        public ObjectElement(object value, SchemaNodeComplex type, ObjectElementPropertyResolver namesResolver) : base(type)
        {
            _value = value;
            _namesResolver = namesResolver;
        }

        public override object Get(string fieldName)
        {
            return _value.GetType().GetProperty(_namesResolver.Get(fieldName)).GetMethod.Invoke(_value, new object[] { });
        }
    }
}
