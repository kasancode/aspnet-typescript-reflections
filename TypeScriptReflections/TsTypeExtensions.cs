using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TypeScriptReflections
{
    public static class TsTypeExtensions
    {
        internal static string ToTsNameCommon(this Type type, Func<Type, string> getName)
        {
            if (type.Namespace == "System")
            {
                switch (type.Name)
                {
                    case "Boolean":
                        return "boolean";
                    case "String":
                    case "Char":
                        return "string";
                    case "Byte":
                    case "SByte":
                    case "Int16":
                    case "Int32":
                    case "Int64":
                    case "UInt16":
                    case "UInt32":
                    case "UInt64":
                    case "Single":
                    case "Double":
                    case "Decimal":
                    case "IntPtr":
                    case "UIntPtr":
                        return "number";
                    case "DateTime":
                    case "DateTimeOffset":
                        return "Date"; // Date is not builtin type
                    case "Object":
                    case "ValueType":
                    case "Enum":
                        return "any";
                    case "Void":
                        return "void";
                }
            }

            if (type.IsArray)
            {
                var sb = new StringBuilder();
                var rank = type.GetArrayRank();

                return sb
                    .Append(type.GetElementType().ToTsNameCommon(getName))
                    .Append('[', rank)
                    .Append(']', rank)
                    .ToString();
            }

            var interfaces = type.GetInterfaces();

            if (type.IsTsDictionary())
            {
                if (type.GenericTypeArguments.Length >= 2)
                {
                    var innerTypes = type.GenericTypeArguments.Select(g => g.ToTsNameCommon(getName)).ToArray();

                    return $"{{[index:{innerTypes[0]}]:{innerTypes[1]};}}";
                }
                else
                {
                    return "{[index:any]:any;}";
                }
            }

            if (type.IsTsCollection())
            {
                if (type.GenericTypeArguments.Length >= 1)
                {
                    var innerTypes = type.GenericTypeArguments[0].ToTsNameCommon(getName);

                    return $"{innerTypes}[]";
                }
                else
                {
                    return "any[]";
                }
            }

            if (type.IsGenericType)
            {
                var sb = new StringBuilder();

                var innerTypes = type.GetGenericArguments().Select(g => g.ToTsNameCommon(getName));

                var name = getName(type);

                var pos = name.LastIndexOf('`');

                if(pos > 0)
                    name = name.Substring(0, pos);

                return sb
                    .Append(name)
                    .Append("<")
                    .Append(string.Join(",", innerTypes))
                    .Append(">")
                    .ToString();
            }

            return getName(type);

        }

        public static string ToTsName(this Type type)
        {
            return ToTsNameCommon(type, (t) => t.Name);
        }

        public static string ToTsFullName(this Type type)
        {
            return ToTsNameCommon(type, (t) => t.FullName == null ? t.Name : $"{t.FullName.Replace("+",".")}");
        }

        public static string GetTsNamespace(this Type type)
        {
            var fullname = type.ToTsFullName();
            var i =fullname.LastIndexOf(".");

            return i > 0 ? fullname.Substring(0, i) : "";
        }

        public static bool IsTsBuiltinType(this Type type)
        {
            if (type.Namespace != "System")
                return false;

            switch (type.Name)
            {
                case "Boolean":
                case "String":
                case "Char":
                case "Byte":
                case "SByte":
                case "Int16":
                case "Int32":
                case "Int64":
                case "UInt16":
                case "UInt32":
                case "UInt64":
                case "Single":
                case "Double":
                case "Decimal":
                case "IntPtr":
                case "UIntPtr":
                // Date is not builtin type
                case "DateTime":
                case "DateTimeOffset":
                case "Object":
                case "ValueType":
                case "Enum":
                case "Void":
                    return true;
            }
            return false;
        }

        public static List<Type> ListUpDependeceTypes(this Type type, TsReflectionFilters filters = null)
            => ListUpDependeceTypes(new[] { type }, filters);

        public static List<Type> ListUpDependeceTypes(Type[] types, TsReflectionFilters filters = null)
        {
            if (types == null)
                throw new ArgumentNullException("types must be not null.");

            if (types.Length < 1)
                throw new ArgumentOutOfRangeException("types.Length must be > 0.");

            var typeList = new List<Type>();
            var stack = new List<Type>();
            stack.AddRange(types);
            var index = 0;

            if (filters == null)
                filters = TsReflectionFilters.DefaultFilters;


            while (stack.Count > index)
            {
                var target = stack[index];
                index++;

                if (typeList.Contains(target))
                    continue;

                if (typeList.Any(t => t.ToTsFullName() == target.ToTsFullName()))
                    continue;

                if (target.IsNotPublic)
                    continue;

                if (target.IsTsBuiltinType())
                    continue;

                if (target.IsGenericParameter)
                    continue;

                if(target.IsEnum)
                {
                    if(!stack.Contains(target.BaseType))
                        stack.Add(target.BaseType);

                    if(filters.EnumFilter(target))
                        typeList.Add(target);

                    continue;
                }

                if (target.IsArray)
                {
                    if(!stack.Contains(target.GetElementType()))
                        stack.Add(target.GetElementType());
                    continue;
                }

                if(target.IsTsCollection() || target.IsTsDictionary())
                {
                    if (target.IsGenericType)
                    {
                        foreach (var t in target.GenericTypeArguments)
                            if (!stack.Contains(t))
                                stack.Add(t);
                    }
                    continue;
                }

                if (target.BaseType != null)
                    if (!stack.Contains(target.BaseType))
                        stack.Add(target.BaseType);

                var interfaces = target.GetInterfaces();
                foreach (var i in interfaces)
                    if (!stack.Contains(i))
                        stack.Add(i);





                var properties = target.GetProperties();
                foreach(var p in properties)
                {
                    if(filters.PropertyFilter(target, p))
                    {
                        if (!stack.Contains(p.PropertyType))
                            stack.Add(p.PropertyType);
                    }
                }

                var fields = target.GetFields();
                foreach(var f in fields)
                {
                    if (filters.FieldFilter(target,f))
                    {
                        if (!stack.Contains(f.FieldType))
                            stack.Add(f.FieldType);
                    }
                }

                var methods = target.GetMethods();
                foreach(var m in methods)
                {
                    if (filters.MethodFilter(target,m))
                    {
                        if (!stack.Contains(m.ReturnType))
                            stack.Add(m.ReturnType);
                        foreach(var p in m.GetParameters())
                        {
                            if (!stack.Contains(p.ParameterType))
                                stack.Add(p.ParameterType);
                        }
                    }
                }

                if (target.IsGenericType)
                {
                    foreach (var t in target.GenericTypeArguments)
                        if (!stack.Contains(t))
                            stack.Add(t);

                    var gt = target.GetGenericTypeDefinition();

                    if (gt != null && gt != target)
                    {
                        if (!stack.Contains(gt))
                            stack.Add(gt);
                        continue;
                    }
                }

                if (filters.TypeFilter(target))
                    typeList.Add(target);

            }

            return typeList;
        }
        
        public static bool IsTsCollection(this Type type)
        {
            if (type.IsInterface)
            {
                if (type.IsGenericType)
                {
                    var gt = type.GetGenericTypeDefinition();

                    return
                        gt == typeof(IEnumerable<>) ||
                        gt == typeof(IDictionary<,>) ||
                        gt == typeof(IList<>) ||
                        gt == typeof(ICollection<>);

                }

                return
                    type == typeof(IEnumerable) ||
                    type == typeof(IDictionary) ||
                    type == typeof(IList) ||
                    type == typeof(ICollection)
                    ;
            }
            else if(type.IsClass || type.IsValueType)
            {
                return type.GetInterfaces().Any(i => i.IsTsCollection());
            }

            return true;
        }

        public static bool IsTsDictionary(this Type type)
        {
            if (type.IsInterface)
            {
                if (type.IsGenericType)
                {
                    var gt = type.GetGenericTypeDefinition();

                    return
                        gt == typeof(IDictionary<,>);

                }

                return
                    type == typeof(IDictionary);
            }
            else if (type.IsClass || type.IsValueType)
            {
                return type.GetInterfaces().Any(i => i.IsTsDictionary());
            }

            return true;
        }
        

        public static bool IsPropertyAccessor(this MethodInfo info)
        {
            return
                info.DeclaringType.GetProperties().Any(p => p.GetMethod == info) ||
                info.DeclaringType.GetProperties().Any(p => p.SetMethod == info);
        }

    }


}
