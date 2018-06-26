using DataSorcererNet.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataSorcererNet
{
    public class ObjectElementService
    {
        private TypeGenerator _typeGenerator;
        private Dictionary<SchemaNodeComplex, (Type, ObjectElementPropertyResolver)> _cache;
        public ObjectElementService(TypeGenerator typeGenerator)
        {
            _typeGenerator = typeGenerator;
            _cache = new Dictionary<SchemaNodeComplex, (Type, ObjectElementPropertyResolver)>();
        }

        public Type GetTypeForElement(SchemaNodeComplex schema, out ObjectElementPropertyResolver namesResolver)
        {
            if (_cache.ContainsKey(schema))
            {
                var cached = _cache[schema];
                namesResolver = cached.Item2;
                return cached.Item1;
            }
            var fields = new List<(string, Type)>();
            foreach (var field in schema.Fields)
            {
                switch (field.Schema.Kind)
                {
                    case SchemaNodeKind.Boolean:
                        fields.Add((field.Name, typeof(bool)));
                        break;
                    case SchemaNodeKind.Text:
                    case SchemaNodeKind.Choice:
                        fields.Add((field.Name, typeof(string)));
                        break;
                    case SchemaNodeKind.Decimal:
                    case SchemaNodeKind.Currency:
                        fields.Add((field.Name, typeof(decimal?)));
                        break;
                    case SchemaNodeKind.Integer:
                        fields.Add((field.Name, typeof(int?)));
                        break;
                    case SchemaNodeKind.DateTime:
                        fields.Add((field.Name, typeof(DateTime?)));
                        break;
                    case SchemaNodeKind.Complex:
                        fields.Add((field.Name, GetTypeForElement((SchemaNodeComplex)field.Schema, out _)));
                        break;
                    case SchemaNodeKind.Collection:
                        fields.Add((field.Name, typeof(IEnumerable<>).MakeGenericType(GetTypeForElement((SchemaNodeComplex)field.Schema, out _))));
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            Type result = _typeGenerator.GenerateType(fields.Select(t => t.Item2));
            namesResolver = new ObjectElementPropertyResolver();
            var availableProperties = result.GetProperties()
                .GroupBy(p => p.PropertyType.FullName)
                .ToDictionary(g => g.Key, g => g.ToList());
            foreach (var (name, type) in fields)
            {
                var group = availableProperties[type.FullName];
                var property = group.First();
                group.RemoveAt(0);
                namesResolver.Add(name, property.Name);
            }
            return result;
        }
        public void RegisterTypeForDataPoint(SchemaNodeComplex schemaType, Type type, ObjectElementPropertyResolver namesResolver)
        {
            _cache[schemaType] = (type, namesResolver);
        }
    }
    public class ObjectElementPropertyResolver
    {
        private Dictionary<string, string> _names;
        public ObjectElementPropertyResolver()
        {
            _names = new Dictionary<string, string>();
        }

        private string MakeKey(string field)
        {
            return field;
        }
        private string MakeKey(string field, string subField)
        {
            return $"{field}\\{subField}";
        }

        private void AddAux(string key, string propertyName)
        {
            _names[key] = propertyName;
        }
        public void Add(string field, string propertyName)
        {
            Add(MakeKey(field), propertyName);
        }
        public void Add(string field, string subField, string propertyName)
        {
            Add(MakeKey(field, subField), propertyName);
        }

        private string GetAux(string key)
        {
            return _names[key];
        }
        public string Get(string field)
        {
            return GetAux(MakeKey(field));
        }
        public string Get(string field, string subField)
        {
            return GetAux(MakeKey(field, subField));
        }
    }
}
