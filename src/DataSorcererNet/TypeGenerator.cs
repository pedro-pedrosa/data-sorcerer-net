using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace DataSorcererNet
{
    public class TypeGenerator
    {
        private Dictionary<string, Type> _cache;
        public TypeGenerator()
        {
            _cache = new Dictionary<string, Type>();
        }

        private string GetKeyForTypes(IEnumerable<Type> propertyTypes)
        {
            return string.Join(";", propertyTypes
                .GroupBy(t => t.FullName)
                .OrderBy(g => g.Key)
                .Select(g => $"{g.Key}@{g.Count()}"));
        }

        public Type GenerateType(IEnumerable<Type> propertyTypes)
        {
            var key = GetKeyForTypes(propertyTypes);
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }
            var typeGuid = Guid.NewGuid();
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(typeGuid.ToString()), AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("Sorcerer_Generated");
            var typeBuilder = moduleBuilder.DefineType($"Sorcerer.Core.Services.GeneratedTypes.{typeGuid.ToString().Replace("-", "")}", TypeAttributes.Public | TypeAttributes.Class);

            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

            int count = 0;
            foreach (var prop in propertyTypes)
            {
                GenerateProperty(typeBuilder, prop, $"prop{count++}");
            }
            var generatedType = typeBuilder.CreateType();
            _cache.Add(key, generatedType);
            return generatedType;
        }
        private void GenerateProperty(TypeBuilder typeBuilder, Type propertyType, string propertyName)
        {
            FieldBuilder field = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder property = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, propertyType, new Type[] { propertyType });
            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.HideBySig;

            MethodBuilder getBuilder = typeBuilder.DefineMethod("get_" + propertyName, getSetAttr, propertyType, Type.EmptyTypes);
            ILGenerator iLGenerator = getBuilder.GetILGenerator();
            iLGenerator.Emit(OpCodes.Ldarg_0);
            iLGenerator.Emit(OpCodes.Ldfld, field);
            iLGenerator.Emit(OpCodes.Ret);

            MethodBuilder setBuilder = typeBuilder.DefineMethod("set_" + propertyName, getSetAttr, null, new Type[] { propertyType });
            iLGenerator = setBuilder.GetILGenerator();
            iLGenerator.Emit(OpCodes.Ldarg_0);
            iLGenerator.Emit(OpCodes.Ldarg_1);
            iLGenerator.Emit(OpCodes.Stfld, field);
            iLGenerator.Emit(OpCodes.Ret);

            property.SetGetMethod(getBuilder);
            property.SetSetMethod(setBuilder);
        }
    }
}
